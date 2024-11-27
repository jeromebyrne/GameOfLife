using UnityEngine;
using UnityEngine.UI;

public class DebugGridVisual : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;

    private GameObject[,] _cellObjects;
    private int[,] _grid; 

    public void Init(int gridDimensionsX, int gridDimensionsY)
    {
        _cellObjects = new GameObject[gridDimensionsX, gridDimensionsY];

        RectTransform rectTransform = GetComponent<RectTransform>();
        float cellSize = 10f;

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
            }
        }
    }

    public void UpdateGridVisual(int[,] gridState)
    {
        _grid = gridState;

        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                DebugCellVisual cellVisual = _cellObjects[x, y].GetComponent<DebugCellVisual>();
                cellVisual.SetAlive(_grid[x, y] == 1);
            }
        }
    }
}