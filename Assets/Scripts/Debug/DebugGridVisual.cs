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

        for (int x = 0; x < gridDimensionsX; x++)
        {
            for (int y = 0; y < gridDimensionsY; y++)
            {
                GameObject cell = Instantiate(_cellPrefab, transform);
                cell.transform.position = new Vector3(x * 20, y * 20, 1);
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
                Image cellImage = _cellObjects[x, y].GetComponent<Image>();
                cellImage.color = _grid[x, y] == 1 ? Color.white : Color.black;
            }
        }
    }
}