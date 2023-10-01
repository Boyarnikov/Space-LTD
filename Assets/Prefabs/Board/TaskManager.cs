using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    MonoColor,
    HousePark,
    ThreeColor,
    Hotel,
    All
}

public class TaskManager : MonoBehaviour
{
    [SerializeField] public TaskType type;
    [SerializeField] public int minFigSize;
    [SerializeField] public int maxFigSize;

    [SerializeField] public CounterUI cui;
    [SerializeField] public List<Vector3> positions = new List<Vector3>();
    [SerializeField] public int queueSize = 10;
    [SerializeField] public List<Figure> queue = new List<Figure>();
    [SerializeField] public Figure fig;
    [SerializeField] public GameObject frame;
    [SerializeField] DialogueDispellerTarget target;
    [SerializeField] public CounterUI counter;

    public void PopulateQueue()
    {
        queue = new List<Figure>();
        for (int i = 0; i < queueSize; i++)
        {
            var f = CreateFigure();
            queue.Add(f);
        }
    }


    Figure CreateFigure()
    {
        var f = Instantiate(fig);
        f.reverced = true;
        Dictionary<TileType, int> d = new Dictionary<TileType, int>();
        int count = UnityEngine.Random.Range(minFigSize, maxFigSize + 1);
        switch (type)
        {
            case TaskType.MonoColor:
                f.GenerateRandomBasic(UnityEngine.Random.Range(minFigSize, maxFigSize + 1), 1, 0, 0);
                break;
            case TaskType.HousePark:
                f.GenerateRandomBasic(UnityEngine.Random.Range(minFigSize, maxFigSize + 1), 1, 1, 0);
                break;
            case TaskType.ThreeColor:
                f.GenerateRandomBasic(UnityEngine.Random.Range(minFigSize, maxFigSize + 1));
                break;
            case TaskType.Hotel:
                d[TileType.Hotel] = UnityEngine.Random.Range(0, 2);
                d[TileType.House] = 0;
                d[TileType.Park] = 0;
                for (int i = 0; i < count - d[TileType.Hotel]; i++)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5)
                    {
                        d[TileType.House] += 1;
                    }
                    else
                    {
                        d[TileType.Park] += 1;
                    }
                }
                f.GenerateRandomBasic(d);
                break;
            case TaskType.All:
                int super = 0;
                d[TileType.Hotel] = 0;
                d[TileType.Farm] = 0;
                d[TileType.Lab] = 0;
                if (UnityEngine.Random.Range(0f, 1f) > 0.3f)
                {
                    super = 1;
                    d[(UnityEngine.Random.Range(0f, 1f) < 0.33f) ? TileType.Hotel : (UnityEngine.Random.Range(0f, 1f) > 0.5f) ? TileType.Lab : TileType.Farm] += 1;
                }
                d[TileType.House] = 0;
                d[TileType.Park] = 0;
                d[TileType.Utility] = 0;
                for (int i = 0; i < count - super; i++)
                {
                    d[(UnityEngine.Random.Range(0f, 1f) < 0.33f) ? TileType.Utility : (UnityEngine.Random.Range(0f, 1f) > 0.5f) ? TileType.Park : TileType.House] += 1;
                }
                f.GenerateRandomBasic(d);
                break;
        }

        return f;
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
        }

        if (cui is not null)
        {
            cui.UpdateCounter(queue.Count - 4);
        }
    }

    void Start()
    {
        float w = frame.transform.localScale.x;
        float h = frame.transform.localScale.y;
        float cam_w = Camera.main.orthographicSize * Camera.main.aspect;
        float cam_h = Camera.main.orthographicSize;
        float delta_x = transform.position.x - frame.transform.position.x;
        float delta_y = transform.position.y - frame.transform.position.y;
        float cam_x = Camera.main.transform.position.x;
        float cam_y = Camera.main.transform.position.y;
        transform.position = new Vector2(cam_w - w / 2 + delta_x - 0.2f + cam_x, -cam_h + h / 2 + cam_y + delta_y + 0.2f);
        target.Settings(new Vector2(0.25f, 0.25f));
        counter.target.Settings(new Vector2(0.25f, 0.25f));
        PopulateQueue();
        UpdateQueue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
