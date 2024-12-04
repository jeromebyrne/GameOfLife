using UnityEngine;

public class DebugGridVisual : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;

    private GameObject[,] _cellObjects;
    private int _gridDimensionsX;
    private int _gridDimensionsY;

    public void Init(int gridDimensionsX, int gridDimensionsY)
    {
        _gridDimensionsX = gridDimensionsX;
        _gridDimensionsY = gridDimensionsY;
        _cellObjects = new GameObject[gridDimensionsX, gridDimensionsY];

        RectTransform rectTransform = GetComponent<RectTransform>();
        float cellSize = 15f;

        // Calculate the total size of the grid
        float gridWidth = gridDimensionsX * cellSize;
        float gridHeight = gridDimensionsY * cellSize;

        // Center the grid in the middle of the canvas
        rectTransform.sizeDelta = new Vector2(gridWidth, gridHeight);
        rectTransform.anchoredPosition = Vector2.zero;

        for (int x = 0; x < gridDimensionsX; x++)
        {
            for (int y = 0; y < gridDimensionsY; y++)
            {
                GameObject cell = Instantiate(_cellPrefab, transform);
                RectTransform cellRect = cell.GetComponent<RectTransform>();
                cellRect.sizeDelta = new Vector2(cellSize, cellSize);
                cellRect.anchoredPosition = new Vector2((x * cellSize) - gridWidth * 0.5f, (-y * cellSize) + gridHeight * 0.5f); // Negative y to align correctly in UI space
                _cellObjects[x, y] = cell;

                // Initialize all cells as "dead"
                DebugCellVisual cellVisual = cell.GetComponent<DebugCellVisual>();
                cellVisual.SetAlive(false);
            }
        }
    }

    public void UpdateGridVisual(System.Collections.Generic.Dictionary<Vector2Int, int> sparseGrid)
    {
        // Reset all cells to "dead" (optional for visual consistency)
        for (int x = 0; x < _gridDimensionsX; x++)
        {
            for (int y = 0; y < _gridDimensionsY; y++)
            {
                DebugCellVisual cellVisual = _cellObjects[x, y].GetComponent<DebugCellVisual>();
                cellVisual.SetAlive(false); // Reset all cells to dead
            }
        }

        // Update only the cells in the sparse grid
        foreach (var cell in sparseGrid)
        {
            Vector2Int position = cell.Key;
            int state = cell.Value;

            if (position.x >= 0 && position.x < _gridDimensionsX && position.y >= 0 && position.y < _gridDimensionsY)
            {
                DebugCellVisual cellVisual = _cellObjects[position.x, position.y].GetComponent<DebugCellVisual>();
                cellVisual.SetAlive(state == 1); // Set the cell state based on the sparse grid
            }
        }
    }
}