using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFollowHelper
{
    public static Vector3 CalculateDesiredVelocity(IPathFollower pathFindable)
    {
        if (pathFindable.Goal != null)
        {
            if (pathFindable.Path.Count != 0)
            {
                Vector3 position = ((Component) pathFindable).transform.position;
                Vector3 direction = pathFindable.Path[0].transform.position - position;
                if (pathFindable.Path[0].transform.position == position)
                {
                    pathFindable.Path.RemoveAt(0);

                    if (pathFindable.Path.Count == 0)
                    {
                        pathFindable.GoalCompleteHandler(pathFindable.Goal);
                    }
                }

                float speed = pathFindable.Speed * Time.deltaTime;
                return new Vector3(
                        (direction.x < 0f ? Mathf.Max(-speed, direction.x) : Mathf.Min(+speed, direction.x)),
                        (direction.y < 0f ? Mathf.Max(-speed, direction.y) : Mathf.Min(+speed, direction.y)),
                        (direction.z < 0f ? Mathf.Max(-speed, direction.z) : Mathf.Min(+speed, direction.z))
                       );

            }
            else
            {
                pathFindable.Path = Pathfinder.Find(((Component)pathFindable).transform.position, pathFindable.Goal);
            }
        }
        return Vector3.zero;
    }

}
