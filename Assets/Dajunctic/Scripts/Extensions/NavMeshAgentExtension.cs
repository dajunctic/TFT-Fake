using UnityEngine;
using UnityEngine.AI;

namespace Dajunctic
{
    public static class NavMeshAgentExtension
    {
        public static void ForceStop(this NavMeshAgent agent)
        {
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.ResetPath();
                agent.velocity = Vector3.zero;
            }
        }

        public static bool IsReachDestination(this NavMeshAgent agent)
        {
            return agent.isActiveAndEnabled && agent.isOnNavMesh 
                    && (agent.isStopped || MathUtils.InRange(agent.transform.position, agent.destination, agent.stoppingDistance, false));
        }

        public static void RotateDirection(this NavMeshAgent agent, Vector3 direction, float rotateSpeed, float deltaTime, bool immediately)
        {
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                return;
            }

            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.updateRotation = false;
            }

            if (immediately)
            {
                agent.transform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.LookRotation(direction), deltaTime * rotateSpeed);
            }
        }

        public static void RotateDirection(this NavMeshObstacle agent, Vector3 direction, float rotateSpeed, float deltaTime, bool immediately)
        {
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                return;
            }

            if (immediately)
            {
                agent.transform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.LookRotation(direction), deltaTime * rotateSpeed);
            }
        }

        public static void MoveDirection(this NavMeshAgent agent, Vector3 direction, float moveSpeed, float rotateSpeed, float deltaTime)
        {
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.speed = moveSpeed;
                agent.velocity = direction.normalized * moveSpeed;
            }

            agent.RotateDirection(direction, rotateSpeed, deltaTime, false);
        }

        public static bool MovePosition(this NavMeshAgent agent, Vector3 position, float moveSpeed, float rotateSpeed, float stoppingDistance=0.1f)
        {
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.speed = moveSpeed;
                agent.updateRotation = true;
                agent.angularSpeed = rotateSpeed * 360;
                agent.stoppingDistance = stoppingDistance;
                agent.destination = position;
                return true;
            }
            return false;
        }

        public static void MoveAmount(this NavMeshAgent agent, Vector3 amount)
        {
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.Move(amount);
            }
        }

        public static void SetAgentType(this NavMeshAgent agent, string name)
        {
            for (var i = 0; i < NavMesh.GetSettingsCount(); i++)
            {
                var settings = NavMesh.GetSettingsByIndex(i);
                if (name == NavMesh.GetSettingsNameFromID(settings.agentTypeID))
                {
                    agent.agentTypeID = settings.agentTypeID;
                    return;
                }
            }
        }
    }
}