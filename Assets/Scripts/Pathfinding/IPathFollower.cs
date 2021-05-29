using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFollower
{
    Tile Goal { get; }
    List<Tile> Path { get; internal set; }
    float Speed { get; }

    internal void GoalCompleteHandler(Tile completedGoal);
}
