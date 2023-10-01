using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine.Rendering.UI;


public class Tile : MonoBehaviour
{

    [SerializeField] SpriteRenderer sp;

    [SerializeField] List<Material> materials;
    [SerializeField] List<Sprite> sprites;

    GameObject cam;
    BoardManager grid;

    [SerializeField] public Vector2 _coordinates;

    public TileType type = TileType.Empty;

    public Vector2 GetCoordinates() { return _coordinates; }

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        grid = GameObject.Find("Board").GetComponent<BoardManager>();
    }

    public void Init(int pos_x, int pos_y, TileType t)
    {
        _coordinates = new Vector2(pos_x, pos_y);
        Init(t);
    }

    public void Init(TileType t)
    {
        type = t;

        if ((int)type < sprites.Count && (int)type != 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)type];
            gameObject.GetComponent<Renderer>().material = materials[1];
        }
        else { 
        if ((int)type >= 0)
            {
                if ((int) type >= materials.Count)
                    gameObject.GetComponent<Renderer>().material = materials[(int)type - 6];
                else
                    gameObject.GetComponent<Renderer>().material = materials[(int)type];
            }  
            else
                gameObject.GetComponent<Renderer>().material = null;

        }

    }

    void Update()
    {
        sp.sortingOrder =  (int)(- transform.position.y * 10);
    }
}