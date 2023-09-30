using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    [SerializeField] public Tile cell;
    [SerializeField] private Vector3 x_v = new Vector3(0.866f, -0.5f, 0);
    [SerializeField] private Vector3 y_v = new Vector3(0, 1.0f, 0);

    [SerializeField] private int board_size = 5;
    private Dictionary<Vector2, Tile> _grid;

    void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        _grid = new Dictionary<Vector2, Tile>();
        var offset_x = -board_size * x_v;
        var offset_y = -board_size * y_v;
        for (var i = 0; i < board_size * 2 + 1; i++)
            for (var j = 0; j < board_size * 2 + 1; j++)
                if (abs(i - j) <= board_size)
                {
                    var tile = Instantiate(cell, Vector3.zero, Quaternion.identity);
                    tile.name = $"Tile {i - board_size} {j - board_size}";
                    tile.transform.parent = this.transform;
                    tile.transform.position = offset_x + offset_y + x_v * i + y_v * j;
                    tile.Init(i - board_size, j - board_size, Tile.TileType.Free);
                    _grid[new Vector2(i - board_size, j - board_size)] = tile;
                }
    }

    public List<Tile> GetAllTiles()
    {
        return _grid.Values.ToList(); ;
    }

    public Tile GetTile(Vector2 pos)
    {
        if (_grid.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public Vector3 GetMid()
    {
        return -board_size * x_v / 2 + (x_v + y_v) * board_size;
    }

    void UpdateMousePosition()
    {

    }

    void Start()
    {
        GenerateGrid();
    }

    void Update()
    {
        UpdateMousePosition();
    }

    public void PlaceFigure(Figure f)
    {
        float d = 1000f;
        Vector2 ind = Vector2.zero;
        foreach (var key in _grid.Keys)
        {
            float dist = (_grid[key].transform.position - f.transform.position).magnitude;
            if (d > dist)
            {
                d = dist;
                ind = key;
            }
        }
        Debug.Log(ind);
        Debug.Log(d);
        if (d < 0.5f)
        {

        }
    }
}