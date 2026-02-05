using UnityEngine;

namespace Dajunctic
{
    public interface IMoveAgent: ILifeCycle
    {
        bool CanMove {get;}
        bool IsMoving {get;}
        bool IsEnabled {get;}
        Vector3 Position {get;}
        Vector3 Forward {get;}
        Vector3 Velocity {get;}
        void SetType(string type);
        void SetEnable(bool enable);
        void SetAcceleration(float acceleration);
        void SetSize(float height, float radius);
        void SetOffset(float offset);
        void ToggleMoveCollision(bool enable);
        void MoveAmount(Vector3 amount);
        void MoveDirection(Vector3 direction, float moveSpeed, float rotateSpeed, float deltaTime);
        void MovePosition(Vector3 position, float moveSpeed, float rotateSpeed, float stoppingDistance);
        void RotateDirection(Vector3 direction, float rotateSpeed, float deltaTime, bool immediately);
        void ForceStop();
        void ChangePriority(int priority);
        void Warp(Vector3 position);
    }
}