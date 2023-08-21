using UnityEngine;

public class Cell
{
    public Vector3 worldPos;
    public Vector2Int gridIndex;
    public byte cost;
    public ushort bestCost;

    public Cell(Vector3 _position, Vector2Int _gridIndex)
    {
        worldPos = _position;
        gridIndex = _gridIndex;
        cost = 1;
        bestCost = ushort.MaxValue;
    }

    public void IncreaseCost(int amount)
    {
        if (cost == byte.MaxValue)
            return;
        if (amount + cost >= 255)
            cost = byte.MaxValue;
        else
            cost += (byte)amount;
    }
}