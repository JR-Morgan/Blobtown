using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour
{
    public TileGrid Grid { get; internal set; }
    public TileType tileType { get; set; }

}

public enum TileType
{
    Default,
    Forest,
    Ore

}
