using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFollowHelper
{
    public static Vector3 CalculateDesiredVelocity(IPathFollower pathFollower)
    {
        if (pathFollower.Goal != null)
        {
            if (pathFollower.Path != null && pathFollower.Path.Count > 0)
            {
                Vector3 position = ((Component) pathFollower).transform.position;
                Vector3 direction = pathFollower.Path[0].transform.position - position;
                if (pathFollower.Path[0].transform.position == position)
                {
                    pathFollower.Path.RemoveAt(0);

                    if (pathFollower.Path.Count == 0)
                    {
                        pathFollower.GoalCompleteHandler(pathFollower.Goal);
                    }
                }

                float speed = pathFollower.Speed * Time.deltaTime;
                return new Vector3(
                        (direction.x < 0f ? Mathf.Max(-speed, direction.x) : Mathf.Min(+speed, direction.x)),
                        (direction.y < 0f ? Mathf.Max(-speed, direction.y) : Mathf.Min(+speed, direction.y)),
                        (direction.z < 0f ? Mathf.Max(-speed, direction.z) : Mathf.Min(+speed, direction.z))
                       );

            }
            else
            {
                pathFollower.Path = Pathfinder.Find(((Component)pathFollower).transform.position, pathFollower.Goal, true);
            }
        }
        return Vector3.zero;
    }

}
