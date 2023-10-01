using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using System.Linq;
using UnityEngine.UIElements;

public class Miniature : MonoBehaviour
{
    BoardManager board;

    GameObject cam;
    BoardManager grid;
    Collider2D coll;
    [SerializeField] public Vector3 idle;

    [SerializeField] public Tile cell;
    [SerializeField] private Vector3 x_v = new Vector3(0.288f, -0.166f, 0);
    [SerializeField] private Vector3 y_v = new Vector3(0, 0.333f, 0);

    [SerializeField] GameObject figure;

    [SerializeField] private int board_size = 1;
    public Dictionary<Vector2, Tile> _grid;
    // Start is called before the first frame update
    void Start()
    {
        _grid = new Dictionary<Vector2, Tile>();
        idle = transform.position;
        var offset_x = -board_size * x_v;
        var offset_y = -board_size * y_v;
        for (var i = 0; i < board_size * 2 + 1; i++)
            for (var j = 0; j < board_size * 2 + 1; j++)
                if (abs(i - j) <= board_size)
                {
                    var tile = Instantiate(cell, Vector3.zero, Quaternion.identity);
                    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Figure";
                    tile.name = $"Tile {i - board_size} {j - board_size}";
                    tile.transform.parent = this.transform;
                    tile.transform.position = tile.transform.parent.transform.position + offset_x + offset_y + x_v * i + y_v * j;
                    tile.Init(i - board_size, j - board_size, TileType.Empty);
                    _grid[new Vector2(i - board_size, j - board_size)] = tile;
                }
        UpdateGrid();
    }

    // Update is called once per frame
    public void UpdateGrid()
    {
        Dictionary<Vector2, Tile> _update = figure.GetComponent<Figure>()._grid;
        if (_update is null || _grid is null)
        {
        } else
        foreach (var key in _update.Keys)
        {
            if (_grid.ContainsKey(key) && _update.ContainsKey(key))
                _grid[key].Init(_update[key].type);
        }
    }

    public void UpdateMove(Vector3 from, Vector3 to)
    {
        transform.position = from;
        idle = to;
    }

    void FixedUpdate()
    {
        float m = (transform.position - idle).magnitude;
        if (m > 1.0f) 
        { 
            transform.position = transform.position * 0.7f + idle * 0.3f; 
        }
        else
        if (m > 24.0f * Time.deltaTime) {
            transform.position -= (transform.position - idle) / m * 24.0f * Time.deltaTime;
        } else
        {
            transform.position = idle;
        }
    }
}
