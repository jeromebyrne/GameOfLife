using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private const float kTickInterval = 0.5f;
    private float _timer;

    private Dictionary<Vector2Int, int> _sparseGrid = new Dictionary<Vector2Int, int>();
    private Dictionary<Vector2Int, int> _newSparseGrid = new Dictionary<Vector2Int, int>();
    private HashSet<Vector2Int> _cellsToCheck = new HashSet<Vector2Int>();

    [SerializeField] DebugGridVisual _debugGridVisual = null;

    void Start()
    {
        InitSparseGrid();
        _debugGridVisual.Init(25, 25);
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0.0f)
        {
            _timer = kTickInterval;
            UpdateSparseGrid();
            _debugGridVisual.UpdateGridVisual(_sparseGrid);
        }
    }

    void InitSparseGrid()
    {
        _sparseGrid.Clear();

        // Initialize the "Glider" pattern
        AddCell(new Vector2Int(12, 11), 1);
        AddCell(new Vector2Int(13, 12), 1);
        AddCell(new Vector2Int(11, 13), 1);
        AddCell(new Vector2Int(12, 13), 1);
        AddCell(new Vector2Int(13, 13), 1);
    }

    void AddCell(Vector2Int position, int state)
    {
        if (state == 1)
        {
            _sparseGrid[position] = state;
        }
        else if (_sparseGrid.ContainsKey(position))
        {
            _sparseGrid.Remove(position);
        }
    }

    void UpdateSparseGrid()
    {
        // Clear reused collections
        _newSparseGrid.Clear();
        _cellsToCheck.Clear();

        // Add all active cells and their neighbors to the check list
        foreach (var cell in _sparseGrid.Keys)
        {
            _cellsToCheck.Add(cell);

            for (int offsetX = -1; offsetX <= 1; offsetX++)
            {
                for (int offsetY = -1; offsetY <= 1; offsetY++)
                {
                    if (offsetX == 0 && offsetY == 0) continue;
                    _cellsToCheck.Add(cell + new Vector2Int(offsetX, offsetY));
                }
            }
        }

        // Evaluate each cell in the check list
        foreach (var cell in _cellsToCheck)
        {
            int liveNeighbors = CountLiveNeighbors(cell);
            bool isAlive = _sparseGrid.ContainsKey(cell) && _sparseGrid[cell] == 1;

            if (isAlive && (liveNeighbors == 2 || liveNeighbors == 3))
            {
                _newSparseGrid[cell] = 1; // Cell stays alive
            }
            else if (!isAlive && liveNeighbors == 3)
            {
                _newSparseGrid[cell] = 1; // Cell becomes alive
            }
        }

        // Replace the sparse grid with the updated one
        var temp = _sparseGrid;
        _sparseGrid = _newSparseGrid;
        _newSparseGrid = temp;
    }

    int CountLiveNeighbors(Vector2Int cell)
    {
        int liveNeighbors = 0;

        for (int offsetX = -1; offsetX <= 1; offsetX++)
        {
            for (int offsetY = -1; offsetY <= 1; offsetY++)
            {
                if (offsetX == 0 && offsetY == 0) continue;

                Vector2Int neighbor = cell + new Vector2Int(offsetX, offsetY);
                if (_sparseGrid.ContainsKey(neighbor) && _sparseGrid[neighbor] == 1)
                {
                    liveNeighbors++;
                }
            }
        }

        return liveNeighbors;
    }
}