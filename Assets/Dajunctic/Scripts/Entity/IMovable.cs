using UnityEngine;

namespace Dajunctic
{
    public interface IMovable : IEntity
    {
        void RotatePosition(Vector3 position, float rotateSpeed, float deltaTime, bool immediately);
        void RotateDirection(Vector3 direction, float rotateSpeed, float deltaTime, bool immediately);
        void MovePosition(Vector3 position, float moveSpeed, float rotateSpeed, float stoppingDistance = 0.1f);
        void MoveDirection(Vector3 direction, float moveSpeed, float rotateSpeed, float deltaTime);
    }
}