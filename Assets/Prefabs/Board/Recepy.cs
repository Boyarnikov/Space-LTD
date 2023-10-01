using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Recepy : MonoBehaviour
{
    [SerializeField] public List<Vector3> positions = new List<Vector3>();
    [SerializeField] public int queueSize = 10;
    [SerializeField] public List<Figure> queue = new List<Figure>();
    [SerializeField] public Figure fig;
    [SerializeField] GameObject frame;
    public Conveyor conv;

    public void PopulateQueue()
    {
        queue = new List<Figure>();
        for (int i = 0; i < queueSize; i++)
        {
            queue.Add(Instantiate(fig));
            queue[i].reverced = true;
            queue[i].GenerateRandomBasic(Random.Range(3, 4));
            queue[i].transform.parent = this.transform;
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
    }

    void Start()
    {
        float w = frame.transform.localScale.x;
        float h = frame.transform.localScale.y;
        float cam_w = Camera.main.orthographicSize * Camera.main.aspect;
        float cam_h = Camera.main.orthographicSize;
        float delta_x = transform.position.x - frame.transform.position.x;
        float cam_x = Camera.main.transform.position.x;
        float cam_y = Camera.main.transform.position.y;
        transform.position = new Vector2(- cam_w + w / 2 + delta_x + 0.2f + cam_x, - cam_h + h / 2 + cam_y + 0.2f);
        conv = FindAnyObjectByType<Conveyor>();
        PopulateQueue();
        UpdateQueue();
    }

    public void TaskDone()
    {
        Figure f = Instantiate(fig);
        f.GenerateBomb(2);
        f.transform.position = new Vector3(100, 100, 0);
        f.idle = f.transform.position;
        conv.queue.Add(f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
