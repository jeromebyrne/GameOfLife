using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int kGridDimensionsX = 25;
    private const int kGridDimensionsY = 25;
    private const float kTickInterval = 0.5f;

    private int[,] _cellGrid = new int[kGridDimensionsX, kGridDimensionsY];
    private int[,] _nextCellGrid = new int[kGridDimensionsX, kGridDimensionsY];
    private float _timer;

    [SerializeField] DebugGridVisual _debugGridVisual = null;

    void Start()
    {
        InitGrid();

        _debugGridVisual.Init(kGridDimensionsX, kGridDimensionsY);
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0.0f)
        {
            _timer = kTickInterval;
            UpdateGrid();
            _debugGridVisual.UpdateGridVisual(_cellGrid);
        }
    }

    void InitGrid()
    {
        // Clear the grid
        for (int x = 0; x < kGridDimensionsX; x++)
        {
            for (int y = 0; y < kGridDimensionsY; y++)
            {
                _cellGrid[x, y] = 0;
            }
        }

        // Hardcoding the "Glider" pattern for the initial seed
        _cellGrid[12, 11] = 1;
        _cellGrid[13, 12] = 1;
        _cellGrid[11, 13] = 1;
        _cellGrid[12, 13] = 1;
        _cellGrid[13, 13] = 1;
    }

    void UpdateGrid()
    {
        for (int x = 0; x < kGridDimensionsX; x++)
        {
            for (int y = 0; y < kGridDimensionsY; y++)
            {
                int liveNeighbors = GetNumLiveNeighbors(x, y);

                if (_cellGrid[x, y] == 1)
                {
                    // Underpopulation or Overcrowding
                    _nextCellGrid[x, y] = (liveNeighbors < 2 || liveNeighbors > 3) ? 0 : 1;
                }
                else
                {
                    // The cell can reproduce
                    _nextCellGrid[x, y] = (liveNeighbors == 3) ? 1 : 0;
                }
            }
        }

        // Swap the grids 
        var temp = _cellGrid;
        _cellGrid = _nextCellGrid;
        _nextCellGrid = temp;
    }

    int GetNumLiveNeighbors(int cellX, int cellY)
    {
        int liveNeighborCount = 0;

        for (int offsetX = -1; offsetX <= 1; offsetX++)
        {
            for (int offsetY = -1; offsetY <= 1; offsetY++)
            {
                if (offsetX == 0 && offsetY == 0) continue; // Skip the cell itself

                int neighborX = cellX + offsetX;
                int neighborY = cellY + offsetY;

                if (neighborX >= 0 && neighborX < kGridDimensionsX && neighborY >= 0 && neighborY < kGridDimensionsY)
                {
                    liveNeighborCount += _cellGrid[neighborX, neighborY];
                }
            }
        }

        return liveNeighborCount;
    }

    void PrintGridToConsole()
    {
        string gridOutput = "";
        for (int y = 0; y < kGridDimensionsY; y++)
        {
            for (int x = 0; x < kGridDimensionsX; x++)
            {
                gridOutput += (_cellGrid[x, y] == 1 ? "O" : ".");
            }
            gridOutput += "\n";
        }
        Debug.Log(gridOutput);
    }
}
