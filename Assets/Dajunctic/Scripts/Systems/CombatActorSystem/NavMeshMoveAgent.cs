using Dajunctic;
using UnityEngine;
using UnityEngine.AI;

namespace Dajunctic
{
    public class NavMeshMoveAgent : MonoBehaviour, IMoveAgent, IObjectPoolItem<string>
    {
        bool _initialized;
        public bool IsInitialized => _initialized && gameObject;
        public bool IsEnabled => IsInitialized && _navMeshAgent.enabled;
        public bool CanMove => true;
        public bool IsMoving => _navMeshAgent.velocity != Vector3.zero;
        public Vector3 Position => transform.position;
        public Vector3 Forward => transform.forward;
        public Vector3 Velocity => _navMeshAgent.velocity;
        public ObjectPoolMetadata<string> ObjectPoolMetadata { get; set; }

        public static NavMeshMoveAgentPool Pool = new ();
        NavMeshAgent _navMeshAgent;
        Vector3 _lastDestination = Vector3.positiveInfinity;
        
        public void Initialize()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            if (_navMeshAgent == null)
            {
                _navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
                _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                _navMeshAgent.avoidancePriority = 99;
                _navMeshAgent.baseOffset = 0;
                _navMeshAgent.autoTraverseOffMeshLink = false;
                _navMeshAgent.autoRepath = false;
            }
            _initialized = true;
            _lastDestination = Vector3.positiveInfinity;
        }

        public void Cleanup()
        {
            _initialized = false;
            if (gameObject)
            {
                gameObject.SetActive(false);
                Pool.Push(this);
            }
        }

        public void SetEnable(bool enable)
        {
            _navMeshAgent.enabled = enable;
        }
        public void SetType(string type)
        {
            _navMeshAgent.SetAgentType(type);
        }

        public void SetAcceleration(float acceleration)
        {
            _navMeshAgent.acceleration = acceleration;
        }

        public void SetSize(float height, float radius)
        {
            _navMeshAgent.radius = radius;
            _navMeshAgent.height = height;
        }

        public void SetOffset(float offset)
        {
            _navMeshAgent.baseOffset = offset;
        }

        public void ToggleMoveCollision(bool enable)
        {
            _navMeshAgent.obstacleAvoidanceType = enable ? ObstacleAvoidanceType.LowQualityObstacleAvoidance : ObstacleAvoidanceType.NoObstacleAvoidance;
        }

        public void MoveAmount(Vector3 amount)
        {
            _navMeshAgent.MoveAmount(amount);
        }

        public void MoveDirection(Vector3 direction, float moveSpeed, float rotateSpeed, float deltaTime)
        {
           _lastDestination = Vector3.positiveInfinity;
            if (_navMeshAgent.isActiveAndEnabled && _navMeshAgent.isOnNavMesh)
            {
                _navMeshAgent.MoveDirection(direction, moveSpeed, rotateSpeed, deltaTime);
            }
            else
            {
                direction.y = 0;
                direction.Normalize();
                _navMeshAgent.transform.position += direction * moveSpeed * deltaTime;
                RotateDirection(direction, rotateSpeed, Time.deltaTime, false);
            }
        }

        public void MovePosition(Vector3 position, float moveSpeed, float rotateSpeed, float stoppingDistance=0.1f)
        {
             if (_navMeshAgent.isActiveAndEnabled && _navMeshAgent.isOnNavMesh)
            {
                if (_lastDestination != position)
                {
                    if (_navMeshAgent.MovePosition(position, moveSpeed, rotateSpeed, stoppingDistance))
                    {
                        _lastDestination = position;
                    }
                }
            }
            else
            {
                RotateDirection(position - _navMeshAgent.transform.position, rotateSpeed, Time.deltaTime, false);
                _navMeshAgent.transform.position = Vector3.MoveTowards(_navMeshAgent.transform.position, position, moveSpeed * Time.deltaTime);
            }
        }

        public void RotateDirection(Vector3 direction, float rotateSpeed, float deltaTime, bool immediately)
        {
            _navMeshAgent.RotateDirection(direction, rotateSpeed, deltaTime, immediately);
        }

        public void ForceStop()
        {
            _navMeshAgent.ForceStop();
        }

        public void ChangePriority(int priority)
        {
            _navMeshAgent.avoidancePriority = priority;
        }

        public void Warp(Vector3 position)
        {
            _navMeshAgent.transform.position = position;
        }
    


    }

    public class NavMeshMoveAgentPool: BaseObjectPool<string, NavMeshMoveAgent>
    {
        const string key = "key";

        public NavMeshMoveAgent GetOrCreate(string name)
        {
            if (TryPop(key, out var agent))
            {
                agent.gameObject.SetActive(true);
                return agent;
            }

            agent = new GameObject(name).AddComponent<NavMeshMoveAgent>();
            SetupMetadata(key, agent);
            return agent;
        }
    }
}