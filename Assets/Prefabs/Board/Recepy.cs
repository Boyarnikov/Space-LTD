using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum RecepyIn
{
    MonoColor,
    HousePark,
    ParkUtil,
    UtilHouse,
    ThreeColor,
}

public enum RecepyOut
{
    Bomb,
    Lab,
    Farm,
    Hotel,
    TrashyLab,
    TrashyHotel,
    TrashyFarm
}

public class Recepy : MonoBehaviour
{
    [SerializeField] List<Material> materials = new List<Material>();
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    [SerializeField] public RecepyIn typeIn;
    [SerializeField] public int minFigSizeIn;
    [SerializeField] public int maxFigSizeIn;

    [SerializeField] public RecepyOut typeOut;
    [SerializeField] public int minFigSizeOut;
    [SerializeField] public int maxFigSizeOut;


    [SerializeField] public List<Vector3> positions = new List<Vector3>();
    [SerializeField] public int queueSize = 10;
    [SerializeField] public List<Figure> queue = new List<Figure>();
    [SerializeField] public Figure fig;
    public Conveyor conv;

    Figure CreateFigureOut()
    {
        var f = Instantiate(fig);
        //f.reverced = true;

        Dictionary<TileType, int> d = new Dictionary<TileType, int>();
        int count = UnityEngine.Random.Range(minFigSizeOut, maxFigSizeOut + 1);
        switch (typeOut)
        {
            case RecepyOut.Bomb:
                d[TileType.Bomb] = count;
                break;
            case RecepyOut.Lab:
                d[TileType.Lab] = count;
                break;
            case RecepyOut.Farm:
                d[TileType.Farm] = count;
                break;
            case RecepyOut.Hotel:
                d[TileType.Hotel] = count;
                break;
        }
        f.GenerateRandomBasic(d);

        return f;
    }

    Figure CreateFigureIn()
    {
        var f = Instantiate(fig);
        f.reverced = true;

        Dictionary<TileType, int> d = new Dictionary<TileType, int>();
        int count = UnityEngine.Random.Range(minFigSizeIn, maxFigSizeIn + 1);
        d[TileType.House] = 0;
        d[TileType.Park] = 0;
        d[TileType.Utility] = 0;
        switch (typeIn)
        {
            case RecepyIn.MonoColor:
                d[TileType.House] = count;
                break;
            case RecepyIn.HousePark:
                d[TileType.House] = 1;
                d[TileType.Park] = 1;
                for (int i = 0; i < count-2; i++)
                    d[(UnityEngine.Random.Range(0f, 1f) > 0.5f) ? TileType.House : TileType.Park] += 1;
                break;
            case RecepyIn.ParkUtil:
                d[TileType.Utility] = 1;
                d[TileType.Park] = 1;
                for (int i = 0; i < count-2; i++)
                    d[(UnityEngine.Random.Range(0f, 1f) > 0.5f) ? TileType.Utility : TileType.Park] += 1;
                break;
            case RecepyIn.UtilHouse:
                d[TileType.House] = 1;
                d[TileType.Utility] = 1;
                for (int i = 0; i < count-2; i++)
                    d[(UnityEngine.Random.Range(0f, 1f) > 0.5f) ? TileType.Utility : TileType.House] += 1;
                break;
            case RecepyIn.ThreeColor:
                for (int i = 0; i < count; i++)
                    d[(UnityEngine.Random.Range(0f, 1f) < 0.33f) ? TileType.Utility : (UnityEngine.Random.Range(0f, 1f) > 0.5f) ? TileType.Park : TileType.House] += 1;
                break;
        }
        f.GenerateRandomBasic(d);

        return f;
    }

    public void PopulateQueue()
    {
        queue = new List<Figure>();
        for (int i = 0; i < positions.Count; i++)
        {
            var f = CreateFigureIn();
            queue.Add(f);
            queue[i].reverced = true;
            queue[i].transform.parent = this.transform;
            queue[i].transform.localPosition = Vector3.zero;
        }
    }

    public void UpdateQueue()
    {
        queue.RemoveAll(item => item == null);

        for (int i = 0; i < queue.Count; i++)
        {
            if (i < positions.Count)
            {
                queue[i].transform.position = positions[i] + transform.position;
            }
            else
            {
                queue[i].transform.position = new Vector3(200, 0, 0);
            }
            queue[i].idle = queue[i].transform.position;
            queue[i].transform.parent = this.transform;
        }

        if (queue.Count == 0) {
            PopulateQueue();
        }
    }

    void Start()
    {
        conv = FindAnyObjectByType<Conveyor>();
        PopulateQueue();
        UpdateQueue();

        List<Figure> list = new List<Figure>();

        List<SpriteRenderer> s = GetComponentsInChildren<SpriteRenderer>().ToList();
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>(); 

        foreach (var f in s)
        {
            if (f.enabled)
            {
                sr = f;
                break;
            }
        }

        switch (typeOut)
        {
            case RecepyOut.Bomb:
                transform.parent.GetComponent<Renderer>().material = materials[0];
                sr.sprite = sprites[0];
                break;
            case RecepyOut.Lab:
                transform.parent.GetComponent<Renderer>().material = materials[1];
                sr.sprite = sprites[1];
                break;
            case RecepyOut.Farm:
                transform.parent.GetComponent<Renderer>().material = materials[2];
                sr.sprite = sprites[2];
                break;
            case RecepyOut.Hotel:
                transform.parent.GetComponent<Renderer>().material = materials[3];
                sr.sprite = sprites[3];
                break;
        }

        transform.parent.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void TaskDone()
    {
        Figure f = CreateFigureOut();
        f.transform.position = new Vector3(0, 100, 0);
        f.idle = f.transform.position;
        conv.queue.Add(f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
