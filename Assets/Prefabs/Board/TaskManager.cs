using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class TaskManager : MonoBehaviour
{
    [SerializeField] public CounterUI cui;
    [SerializeField] public List<Vector3> positions = new List<Vector3>();
    [SerializeField] public int queueSize = 10;
    [SerializeField] public List<Figure> queue = new List<Figure>();
    [SerializeField] public Figure fig;

    public void PopulateQueue()
    {
        queue = new List<Figure>();
        for (int i = 0; i < queueSize; i++)
        {
            queue.Add(Instantiate(fig));
            queue[i].reverced = true;
            queue[i].GenerateRandomBasic(Random.Range(3, 6));
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
        }

        if (cui is not null)
        {
            cui.UpdateCounter(queue.Count - 4);
        }
    }

    void Start()
    {
        PopulateQueue();
        UpdateQueue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
