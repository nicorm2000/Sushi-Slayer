using UnityEngine;

public class FlowField
{
    public Cell[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }

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
}