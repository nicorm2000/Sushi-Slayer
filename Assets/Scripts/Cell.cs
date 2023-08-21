using UnityEngine;

public class Cell
{
    public Vector3 position;
    public Vector2Int gridIndex;

    public Cell(Vector3 _position, Vector2Int _gridIndex)
    {
        position = _position;
        gridIndex = _gridIndex;
    }
}