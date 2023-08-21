using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class FlowField
{
    public Cell[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }
    public Cell destinationCell;

    private float cellDiameter;

    public FlowField(float _cellRadius, Vector2Int _gridSize)
    {
        cellRadius = _cellRadius;
        cellDiameter = cellRadius * 2f;
        gridSize = _gridSize;
    }

    public void CreateGrid()
    {
        grid = new Cell[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 worldPos = new Vector3(cellDiameter * x + cellRadius, 0, cellDiameter * y + cellRadius);//The offset will center the cell at index(0,0) on the origin (0,0)
                grid[x, y] = new Cell(worldPos, new Vector2Int(x, y));
            }
        }
    }

    public void CreateCostField()
    {
        Vector3 cellHalfExtents = Vector3.one * cellRadius;

        int terrainMask = LayerMask.GetMask("Impassable", "RoughTerrain");

        foreach (Cell currentCell in grid)
        {
            Collider[] obstacles = Physics.OverlapBox(currentCell.worldPos, cellHalfExtents, Quaternion.identity, terrainMask);

            bool hasIncreasedCost = false;

            foreach (Collider col in obstacles) 
            {
                if (col.gameObject.layer == 8)
                {
                    currentCell.IncreaseCost(255);
                    continue;
                }
                else if (!hasIncreasedCost && col.gameObject.layer == 9)
                {
                    currentCell.IncreaseCost(3);
                    hasIncreasedCost = true;
                }
            }
        }
    }

    public void CreateIntegrationField(Cell _destinationCell)
    {
        destinationCell = _destinationCell;

        destinationCell.cost = 0;
        destinationCell.bestCost = 0;

        Queue<Cell> cellsToCheck = new Queue<Cell> ();

        cellsToCheck.Enqueue (destinationCell);

        while (cellsToCheck.Count > 0)
        {
            Cell currentCell = cellsToCheck.Dequeue ();

            List<Cell> currentNeighbours = GetNeighbourCells(currentCell.gridIndex, GridDirection.CardinalDirections);

            foreach (Cell currentNeighbour in currentNeighbours) 
            {
                if (currentNeighbour.cost == byte.MaxValue)
                {
                    continue;
                }
                if (currentNeighbour.cost + currentCell.bestCost < currentNeighbour.bestCost)
                {
                    currentNeighbour.bestCost = (ushort)(currentNeighbour.cost + currentCell.bestCost);

                    cellsToCheck.Enqueue(currentNeighbour);
                }
            }
        }
    }

    private List<Cell> GetNeighbourCells(Vector2Int nodeIndex, List<GridDirection> directions)
    {
        List<Cell> neighboursCells = new List<Cell>();

        foreach (Vector2Int currentDirection in directions)
        {
            Cell newNeighbour = GetCellAtRelativePos(nodeIndex, currentDirection);

            if (newNeighbour != null)
            {
                neighboursCells.Add(newNeighbour);
            }
        }
        return neighboursCells;
    }

    private Cell GetCellAtRelativePos(Vector2Int originPos, Vector2Int relativePosition)
    {
        Vector2Int finalPos = originPos + relativePosition;

        if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
        {
            return null;
        }
        else
        {
            return grid[finalPos.x, finalPos.y];
        }
    }

    public Cell GetCellFromWorldPos(Vector3 worldPos)
    {
        float percentX = worldPos.x / (gridSize.x * cellDiameter);
        float percentY = worldPos.z / (gridSize.y * cellDiameter);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        return grid[x, y];
    }
}