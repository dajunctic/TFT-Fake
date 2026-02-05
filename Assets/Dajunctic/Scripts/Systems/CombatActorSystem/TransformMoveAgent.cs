using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    public class TransformMoveAgent : IMoveAgent
    {
        bool _isInitialized;
        public bool IsInitialized => _isInitialized;
        public bool IsEnabled => IsInitialized && _enable;
        public bool CanMove => true;
        public bool IsMoving { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Forward { get; private set; }
        public Vector3 Velocity { get; }

        bool _enable;

        public void Initialize()
        {
            _isInitialized = true;
            Position = Vector3.zero;
            Forward = Vector3.forward;
        }

        public void Cleanup()
        {
            _isInitialized = false;
        }

        public void SetEnable(bool enable)
        {
            _enable = enable;
        }

        public void SetType(string type) { }

        public void SetOffset(float offset) { }

        public void SetSize(float height, float radius) { }

        public void SetAcceleration(float acceleration) { }

        public void ToggleMoveCollision(bool enable) { }

        public void ChangePriority(int priority) { }

        public void MoveAmount(Vector3 amount)
        {
            Position += amount;
        }

        public void MovePosition(
            Vector3 position,
            float moveSpeed,
            float rotateSpeed,
            float stoppingDistance
        )
        {
            if (!MathUtils.InRange(Position, position, stoppingDistance))
            {
                Position = Vector3.MoveTowards(Position, position, moveSpeed * Time.deltaTime);
            }
            RotateDirection(position - Position, rotateSpeed, Time.deltaTime, false);
        }

        public void MoveDirection(
            Vector3 direction,
            float moveSpeed,
            float rotateSpeed,
            float deltaTime
        )
        {
            Position += deltaTime * moveSpeed * direction;
            RotateDirection(direction, rotateSpeed, deltaTime, false);
        }

        public void RotateDirection(
            Vector3 direction,
            float rotateSpeed,
            float deltaTime,
            bool immediately
        )
        {
            direction.y = 0;
            direction.Normalize();
            if (direction != Vector3.zero)
            {
                if (immediately)
                {
                    Forward = direction;
                }
                else
                {
                    Forward =
                        Quaternion.Slerp(
                            Quaternion.LookRotation(Forward),
                            Quaternion.LookRotation(direction),
                            rotateSpeed * deltaTime
                        ) * Vector3.forward;
                }
                Forward.Normalize();
            }
        }

        public void ForceStop() { }
        public void Warp(Vector3 position)
        {
            Position = position;
        }
    }
}
