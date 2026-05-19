using UnityEngine;

namespace Dajunctic
{
    public interface IMovable : IEntity
    {
        Vector3 MoveDirectionPerFrame { get; set; }
        Vector3 MovePositionPerFrame { get; set; }
        Vector3 Position { get; }
        void ForceStop();
        void ToggleMoveAgent(bool v);
        void Teleport(Vector3 v);
        void Teleport(Vector3 v, bool b);
        void RotatePosition(Vector3 position, float rotateSpeed, float deltaTime, bool immediately);
        void RotateDirection(Vector3 direction, float rotateSpeed, float deltaTime, bool immediately);
        void MovePosition(Vector3 position, float moveSpeed, float rotateSpeed, float stoppingDistance = 0.1f);
    }
}