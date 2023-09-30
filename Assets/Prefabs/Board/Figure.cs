using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using System.Linq;

public class Figure : MonoBehaviour
{
    BoardManager board;

    GameObject cam;
    BoardManager grid;
    Collider2D coll;
    Vector3 idle;

    List<Vector2> rotation = new List<Vector2>();

    [SerializeField] public Tile cell;
    [SerializeField] private Vector3 x_v = new Vector3(0.866f, -0.5f, 0);
    [SerializeField] private Vector3 y_v = new Vector3(0, 1.0f, 0);

    bool canMove;
    bool dragging;

    [SerializeField] private int board_size = 5;
    private Dictionary<Vector2, Tile> _grid;
    // Start is called before the first frame update
    void Start()
    {
        board = FindAnyObjectByType<BoardManager>();
        coll = GetComponent<Collider2D>();
        _grid = new Dictionary<Vector2, Tile>();
        idle = transform.position;
        var offset_x = -board_size * x_v;
        var offset_y = -board_size * y_v;
        for (var i = 0; i < board_size * 2 + 1; i++)
            for (var j = 0; j < board_size * 2 + 1; j++)
                if (abs(i - j) <= board_size)
                {
                    var tile = Instantiate(cell, Vector3.zero, Quaternion.identity);
                    tile.name = $"Tile {i - board_size} {j - board_size}";
                    tile.transform.parent = this.transform;
                    tile.transform.position = tile.transform.parent.transform.position + offset_x + offset_y + x_v * i + y_v * j;
                    tile.Init(i, j, Tile.TileType.Empty);
                    tile.gameObject.GetComponent<Collider2D>().enabled = false;
                    if (Random.Range(0f, 1f) > 0.5)
                    {
                        tile.Init(Tile.TileType.Park);
                    }
                    _grid[new Vector2(i - board_size, j - board_size)] = tile;

                }

        rotation.Add(new Vector2(0, 1));
        rotation.Add(new Vector2(1, 1));
        rotation.Add(new Vector2(1, 0));
        rotation.Add(new Vector2(0, -1));
        rotation.Add(new Vector2(-1, -1));
        rotation.Add(new Vector2(-1, 0));
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (coll == Physics2D.OverlapPoint(mousePos)) { canMove = true; }
            else { canMove = false; }
            if (canMove) { dragging = true; }
        }

        if (Input.GetMouseButtonDown(1) && canMove)
        {
            Tile boof = _grid[rotation[0]];

            Vector3 pos, next_pos;
            Vector2 coords, next_coords;

            pos = _grid[rotation[0]].transform.position;
            coords = _grid[rotation[0]]._coordinates;

            for (int i = 0; i < 5; i++)
            {
                next_pos = _grid[rotation[i+1]].transform.position;
                next_coords = _grid[rotation[i + 1]]._coordinates;
                _grid[rotation[i]] = _grid[rotation[i + 1]];
                _grid[rotation[i]].transform.position = pos;
                _grid[rotation[i]]._coordinates = coords;
                pos = next_pos;
                coords = next_coords; 
            }
            _grid[rotation[5]] = boof;
            _grid[rotation[5]].transform.position = pos;
            _grid[rotation[5]]._coordinates = coords;
        }

        if (dragging)
        {
            this.transform.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            canMove = false;
            dragging = false;
            if (transform.position != idle)
            {
                board.PlaceFigure(this);
                transform.position = idle;
            }
        }
    }
}
