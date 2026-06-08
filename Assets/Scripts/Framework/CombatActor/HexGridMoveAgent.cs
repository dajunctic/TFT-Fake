using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class HexGridMoveAgent : MonoBehaviour, IMoveAgent
    {
        private bool _initialized;
        public bool Initialized => _initialized;
        public bool IsEnabled { get; private set; }
        public bool CanMove => true;
        public bool IsMoving => _isMovingDirect || (_currentPath != null && _currentPathIndex < _currentPath.Count);
        public Vector3 Position => transform.position;
        public Vector3 Forward => transform.forward;
        public Vector3 Velocity { get; private set; }

        private HexAreaView _hexArea;
        private List<Vector2Int> _currentPath;
        private int _currentPathIndex;
        private float _moveSpeed;
        private float _rotateSpeed;
        private float _stoppingDistance;
        private Vector3 _targetDestination;

        // Direct movement (fallback when no hex area)
        private bool _isMovingDirect;
        private Vector3 _directTarget;

        /// <summary>
        /// Explicitly set the hex area for A* pathfinding.
        /// When null, MovePosition falls back to direct movement.
        /// </summary>
        public void SetHexArea(HexAreaView area)
        {
            _hexArea = area;
        }

        public void Initialize()
        {
            // Don't auto-find hex area — it will be set explicitly via SetHexArea()
            _initialized = true;
        }

        public void Cleanup()
        {
            _initialized = false;
            _hexArea = null;
        }

        public void SetEnable(bool enable)
        {
            IsEnabled = enable;
            if (!enable) ForceStop();
        }

        public void SetType(string type) { }
        public void SetAcceleration(float acceleration) { }
        public void SetSize(float height, float radius) { }
        public void SetOffset(float offset) { }
        public void ToggleMoveCollision(bool enable) { }
        public void ChangePriority(int priority) { }

        public void Warp(Vector3 position)
        {
            transform.position = position;
            _targetDestination = position;
            _currentPath = null;
            _isMovingDirect = false;
        }

        public void ForceStop()
        {
            _currentPath = null;
            _isMovingDirect = false;
            Velocity = Vector3.zero;
        }

        public void MoveAmount(Vector3 amount)
        {
            transform.position += amount;
        }

        public void MovePosition(Vector3 position, float moveSpeed, float rotateSpeed, float stoppingDistance)
        {
            _moveSpeed = moveSpeed;
            _rotateSpeed = rotateSpeed;
            _stoppingDistance = stoppingDistance;

            if (Vector3.Distance(transform.position, position) <= stoppingDistance)
            {
                ForceStop();
                return;
            }

            if (_hexArea != null && _hexArea.Data != null)
            {
                // Hex A* pathfinding mode
                if (_targetDestination != position)
                {
                    _targetDestination = position;
                    CalculatePath(position);
                    // If A* fails (e.g., target on different hex area), fall back to direct
                    if (_currentPath == null || _currentPath.Count == 0)
                    {
                        _directTarget = position;
                        _isMovingDirect = true;
                    }
                }
            }
            else
            {
                // Direct movement (no hex area — free-roam like tactician)
                _directTarget = position;
                _isMovingDirect = true;
                _currentPath = null;
            }
        }

        private void CalculatePath(Vector3 targetWorldPos)
        {
            if (_hexArea == null || _hexArea.Data == null) return;

            Vector2Int startHex = _hexArea.Data.WorldToHex(transform.position, _hexArea.CachedTransform.position);
            Vector2Int endHex = _hexArea.Data.WorldToHex(targetWorldPos, _hexArea.CachedTransform.position);

            if (startHex == endHex) return;

            _currentPath = AStarSearch(startHex, endHex);
            _currentPathIndex = 0;
        }

        private List<Vector2Int> AStarSearch(Vector2Int start, Vector2Int goal)
        {
            var openSet = new PriorityQueue<Vector2Int, float>();
            openSet.Enqueue(start, 0);

            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            var gScore = new Dictionary<Vector2Int, float>();
            gScore[start] = 0;

            var fScore = new Dictionary<Vector2Int, float>();
            fScore[start] = Heuristic(start, goal);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current == goal)
                {
                    return ReconstructPath(cameFrom, current);
                }

                foreach (var neighbor in _hexArea.Data.GetNeighbors(current))
                {
                    float tentativeGScore = gScore[current] + 1; 
                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Enqueue(neighbor, fScore[neighbor]);
                        }
                    }
                }
            }

            return null;
        }

        private float Heuristic(Vector2Int a, Vector2Int b)
        {
            return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.x + a.y - b.x - b.y) + Mathf.Abs(a.y - b.y)) / 2f;
        }

        private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            var path = new List<Vector2Int> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }

        private void Update()
        {
            if (!IsEnabled) return;

            // Hex pathfinding mode
            if (_currentPath != null && _currentPathIndex < _currentPath.Count)
            {
                if (_hexArea == null || _hexArea.Data == null)
                {
                    ForceStop();
                    return;
                }

                Vector2Int targetHex = _currentPath[_currentPathIndex];
                Vector3 targetPos = _hexArea.Data.HexToWorld(_hexArea.CachedTransform.position, targetHex);
                targetPos.y = transform.position.y;

                float step = _moveSpeed * Time.deltaTime;
                Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, step);
                Velocity = (newPos - transform.position) / Time.deltaTime;
                transform.position = newPos;

                if (Velocity != Vector3.zero)
                {
                    RotateDirection(Velocity.normalized, _rotateSpeed, Time.deltaTime, false);
                }

                if (Vector3.Distance(transform.position, targetPos) < 0.05f)
                {
                    _currentPathIndex++;
                    if (_currentPathIndex >= _currentPath.Count)
                    {
                        _currentPath = null;
                        Velocity = Vector3.zero;
                    }
                }
            }
            // Direct movement mode (fallback — used by tactician or cross-grid combat)
            else if (_isMovingDirect)
            {
                float step = _moveSpeed * Time.deltaTime;
                Vector3 direction = _directTarget - transform.position;
                direction.y = 0;

                if (direction.magnitude <= _stoppingDistance)
                {
                    _isMovingDirect = false;
                    Velocity = Vector3.zero;
                    return;
                }

                Vector3 newPos = Vector3.MoveTowards(transform.position, _directTarget, step);
                newPos.y = transform.position.y; // Keep same height
                Velocity = (newPos - transform.position) / Time.deltaTime;
                transform.position = newPos;

                if (Velocity.sqrMagnitude > 0.001f)
                {
                    RotateDirection(Velocity.normalized, _rotateSpeed, Time.deltaTime, false);
                }
            }
        }

        public void RotateDirection(Vector3 direction, float rotateSpeed, float deltaTime, bool immediately)
        {
            if (direction == Vector3.zero) return;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            if (immediately) transform.rotation = targetRotation;
            else transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * deltaTime);
        }
    }

    public class PriorityQueue<TElement, TPriority>
    {
        private List<(TElement element, TPriority priority)> elements = new List<(TElement, TPriority)>();

        public int Count => elements.Count;

        public void Enqueue(TElement element, TPriority priority)
        {
            elements.Add((element, priority));
            elements.Sort((a, b) => Comparer<TPriority>.Default.Compare(a.priority, b.priority));
        }

        public TElement Dequeue()
        {
            var item = elements[0];
            elements.RemoveAt(0);
            return item.element;
        }

        public bool Contains(TElement element)
        {
            return elements.Any(e => EqualityComparer<TElement>.Default.Equals(e.element, element));
        }
    }
}
