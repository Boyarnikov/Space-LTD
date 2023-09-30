using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public enum TileType
    {
        Empty,
        Free,
        House,
        Park
    }

    [SerializeField] List<Material> materials;

    GameObject cam;
    BoardManager grid;

    [SerializeField] public Vector2 _coordinates;

    public TileType type = TileType.Empty;

    public Vector2 GetCoordinates() { return _coordinates; }


    public void Select()
    {

    }
    public void Unselect()
    {

    }

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        grid = GameObject.Find("Board").GetComponent<BoardManager>();
    }

    public void Init(int pos_x, int pos_y, TileType t)
    {
        _coordinates = new Vector2(pos_x, pos_y);
        type = t;
        gameObject.GetComponent<Renderer>().material = materials[(int)type];
    }

    public void Init(TileType t)
    {
        type = t;
        gameObject.GetComponent<Renderer>().material = materials[(int)type];
    }

    public void Activate()
    {

    }

    public void Deactivate()
    {
    }

    void UpdateRenderer()
    {
       
    }

    void CalculateLerp()
    {
       
    }

    void Update()
    {

    }
}