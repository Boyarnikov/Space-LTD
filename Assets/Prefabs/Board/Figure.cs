using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using System.Linq;
using Unity.VisualScripting;





public class Figure : MonoBehaviour
{
    BoardManager board;

    Collider2D coll;
    public Vector3 idle;
    public int conv_pos;

    [SerializeField] public bool reverced;

    List<Vector2> rotation = new List<Vector2>();

    [SerializeField] public Tile cell;
    [SerializeField] private Vector3 x_v = new Vector3(0.866f, -0.5f, 0);
    [SerializeField] private Vector3 y_v = new Vector3(0, 1.0f, 0);

    [SerializeField] GameObject minitature;

    [SerializeField] private int board_size = 1;
    public Dictionary<Vector2, Tile> _grid = null;

    bool canMove;
    bool dragging;

    void Shuffle(List<Vector2> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            Vector2 value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    void PopulateWithHexes()
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
                    tile.gameObject.GetComponent<Collider2D>().enabled = false;
                    _grid[new Vector2(i - board_size, j - board_size)] = tile;
                }
    }


    public void GenerateRandomBasic(int items = 4)
    {
        if (_grid is null) PopulateWithHexes();
        List<Vector2> list = _grid.Keys.ToList();
        list.Remove(new Vector2(0, 0));
        Shuffle(list);
        list.Add(new Vector2(0, 0));

        for (int i = 0; i < list.Count; i++)
        {
            var tile = _grid[list[i]];
            if (i > list.Count - items - 1)
            {
                int randomNum = Random.Range((int)TileType.House, (int)TileType.House + 3);
                tile.Init((TileType)randomNum);
                if (reverced)
                {
                    tile.Init((TileType)((int)tile.type + 6));
                }
            }
            else
            {
                tile.Init(TileType.Empty);
            }
        }    
    }


    public void GenerateRandomBasic(int items = 4, int h = 1, int p = 1, int u = 1)
    {
        float house = (float)h/ (float)(h + p + u);
        float util = 1.0f - (float)u / (float)(h + p + u);
        if (_grid is null) PopulateWithHexes();
        List<Vector2> list = _grid.Keys.ToList();
        list.Remove(new Vector2(0, 0));
        Shuffle(list);
        list.Add(new Vector2(0, 0));

        for (int i = 0; i < list.Count; i++)
        {
            var tile = _grid[list[i]];
            if (i > list.Count - items - 1)
            {
                float randomNum = Random.Range(0.0f, 1.0f);
                if (randomNum < house)
                {
                    tile.Init(TileType.House);
                } else if (randomNum > util)
                {
                    tile.Init(TileType.Utility);
                } else
                {
                    tile.Init(TileType.Park);
                }

                if (reverced)
                {
                    tile.Init((TileType)((int)tile.type + 6));
                }
            }
            else
            {
                tile.Init(TileType.Empty);
            }
        }
    }


    public void GenerateBomb(int items = 1)
    {
        if (_grid is null) PopulateWithHexes();
        List<Vector2> list = _grid.Keys.ToList();
        list.Remove(new Vector2(0, 0));
        Shuffle(list);
        list.Add(new Vector2(0, 0));

        for (int i = 0; i < list.Count; i++)
        {
            var tile = _grid[list[i]];
            if (i > list.Count - items - 1)
            {
                tile.Init(TileType.Bomb);
            }
            else
            {
                tile.Init(TileType.Empty);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        board = FindAnyObjectByType<BoardManager>();
        coll = GetComponent<Collider2D>();

        idle = transform.position;

        if (_grid is null)
            if (!reverced) 
                GenerateRandomBasic(Random.Range(1, 4));
            else
                GenerateRandomBasic(Random.Range(3, 7));

        rotation.Add(new Vector2(0, 1));
        rotation.Add(new Vector2(1, 1));
        rotation.Add(new Vector2(1, 0));
        rotation.Add(new Vector2(0, -1));
        rotation.Add(new Vector2(-1, -1));
        rotation.Add(new Vector2(-1, 0));

        minitature.GetComponent<Miniature>().UpdateGrid();
        foreach (var tile in _grid.Values)
        {
            tile.gameObject.SetActive(false);
        }
    }



    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (coll == Physics2D.OverlapPoint(mousePos))
            {
                canMove = true;
                minitature.SetActive(false);
                foreach (var tile in _grid.Values)
                {
                    tile.gameObject.SetActive(true);
                }
            }
            else { canMove = false; }
            if (canMove) { dragging = true; }
        }

        if (Input.GetMouseButtonDown(1) && (coll == Physics2D.OverlapPoint(mousePos) || canMove))
        {
            Tile boof = _grid[rotation[0]];

            Vector3 pos, next_pos;
            Vector2 coords, next_coords;

            pos = _grid[rotation[0]].transform.position;
            coords = _grid[rotation[0]]._coordinates;

            for (int i = 0; i < 5; i++)
            {
                next_pos = _grid[rotation[i + 1]].transform.position;
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

            minitature.GetComponent<Miniature>().UpdateGrid();
        }

        if (dragging)
        {
            this.transform.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (canMove)
            {
                foreach (var tile in _grid.Values)
                {
                    minitature.SetActive(true);
                    tile.gameObject.SetActive(false);
                }
            }
            canMove = false;
            dragging = false;
            if (transform.position != idle)
            {
                board.PlaceFigure(this);
                transform.position = idle;
                minitature.GetComponent<Miniature>().UpdateGrid();
            }
        }
    }
}
