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

    [SerializeField] public int level;
    [SerializeField] public bool endless;

    string taskText = "";
    public bool task_failed = false;
    public bool task_done = false;

    int endless_mission = 2;
    int endless_step = 2;
    int endless_inc = 2;
    int endless_count = 0;

    TMPro.TextMeshPro taskTextMesh;

    [SerializeField] public CounterUI cui;
    [SerializeField] public List<Vector3> positions = new List<Vector3>();
    [SerializeField] public int queueSize = 10;
    [SerializeField] public List<Figure> queue = new List<Figure>();
    [SerializeField] public Figure fig;
    [SerializeField] public GameObject frame;
    [SerializeField] DialogueDispellerTarget target;
    [SerializeField] public CounterUI counter;

    public int turn = 0;
    public int filled = 1;
    public int types = 0;
    public int done = 0;

    bool startTask = false;

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

        if (endless)
        {
            for (int i = queue.Count; i < queueSize; i++)
            {
                var f = CreateFigure();
                f.transform.position = f.idle;
                queue.Add(f);

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
        }

        if (cui is not null)
        {
            if (endless)
            {
                cui.UpdateCounter(10);
            }
            else
            {
                cui.UpdateCounter(queue.Count);
            }
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

        taskTextMesh = GetComponentInChildren<TMPro.TextMeshPro>();
    }

    
    void CheckTasks()
    {
        switch (level)
        {
            case (0):
                task_done = true;
                if (turn > 30)
                {
                    task_failed = true;
                }
                break;
            case (1):
                task_done = true;
                if (turn > 20)
                {
                    task_failed = true;
                }
                break;
            case (2):
                if (filled == 19)
                {
                    task_done = true;
                }
                break;
            case (3):
                if (filled == 0)
                {
                    task_done = true;
                }
                break;
            case (4):
                if (types == 6)
                {
                    task_done = true;
                }
                break;
            case (5):
                if (endless_mission <= done)
                {
                    endless_mission += endless_step;
                    endless_step += endless_inc;
                    endless_count++;
                    if (endless_count > Globals.stars[5])
                    {
                        Globals.stars[5] = endless_count;
                    }
                }
                break;

        }
    }


    // Update is called once per frame
    void Update()
    {
        CheckTasks();
        switch (level) 
        { 
            case (0):
                taskText = "Finish in\r\n30 turns\r\n[Turn: " + turn + "/30]";
                break;
            case (1):
                taskText = "Finish in\r\n20 turns\r\n[Turn: " + turn + "/20]";
                break;
            case (2):
                taskText = "Have fully\r\nfilled grid\r\n[Tiles: " + filled + "/19]";
                if (task_done) taskText = "Have fully\r\nfilled grid\r\n[Done]";
                break;
            case (3):
                taskText = "Have fully\r\nemptied grid\r\n[Tiles: " + filled + "/0]";
                if (task_done) taskText = "Have fully\r\nemptied grid\r\n[Done]";
                break;
            case (4):
                taskText = "Have all types\r\nof buildings\r\n[Types: " + types + "/6]";
                if (task_done) taskText = "Have all types\r\nof buildings\r\n[Done]";
                break;
            case (5):
                taskText = "Do requests\r\n[Requests: " + done + "/" + endless_mission + "]\r\n[Stars: " + endless_count + "]";
                break;
        }

        if (task_failed)
        {
            taskTextMesh.color = new Color(55f/255f, 0, 0);
        } else if (task_done)
        {
            taskTextMesh.color = new Color(0, 55f / 255f, 0);
        } else
        {
            taskTextMesh.color = Color.black;
        }
        taskTextMesh.text = taskText;
    
    }
}
