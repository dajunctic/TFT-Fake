using UnityEngine;

namespace Dajunctic
{
    public interface IMovable: IEntity
    {
        public void RotatePosition(Vector3 position, float rotateSpeed, float deltaTime, bool immediately);
        public void RotateDirection(Vector3 direction, float rotateSpeed,  float deltaTime, bool immediately);
        public void MovePosition(Vector3 position, float moveSpeed, float rotateSpeed, float stoppingDistance = 0.1f);
        public void MoveDirection(Vector3 direction, float moveSpeed, float rotateSpeed, float deltaTime);     
    }
}