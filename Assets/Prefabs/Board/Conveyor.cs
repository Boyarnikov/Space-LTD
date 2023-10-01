using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;


public enum ConveyorType
{
    MonoColor,
    HousePark,
    ThreeColor,
    TrashyHousePark
}


public class Conveyor : MonoBehaviour
{
    [SerializeField] public ConveyorType type;
    [SerializeField] public int minFigSize;
    [SerializeField] public int maxFigSize;
    [SerializeField] public CounterUI cui;
    [SerializeField] Vector3 offset = new Vector3 (-1f, 0, 0);
    [SerializeField] Figure figure;
    [SerializeField] int maxSize = 5;

    public List<Figure> queue = new List<Figure>();

    public List<Figure> figureList = new List<Figure>();
    // Start is called before the first frame update
    void Start()
    {
        UpdateConveyor();
    }

    Figure CreateFigure()
    {
        var f = Instantiate(figure);

        switch (type)
        {
            case ConveyorType.MonoColor:
                f.GenerateRandomBasic(UnityEngine.Random.Range(minFigSize, maxFigSize + 1), 1, 0, 0);
                break;
            case ConveyorType.HousePark:
                f.GenerateRandomBasic(UnityEngine.Random.Range(minFigSize, maxFigSize + 1), 1, 1, 0);
                break;
            case ConveyorType.ThreeColor:
                f.GenerateRandomBasic(UnityEngine.Random.Range(minFigSize, maxFigSize + 1));
                break;
            case ConveyorType.TrashyHousePark:
                Dictionary<TileType, int> d = new Dictionary<TileType, int>();
                int count = UnityEngine.Random.Range(minFigSize, maxFigSize + 1);
                d[TileType.Trash] = UnityEngine.Random.Range(0, 2);
                d[TileType.House] = 0;
                d[TileType.Park] = 0;
                for (int i = 0; i < count; i++)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5) {
                        d[TileType.House] += 1;
                    } else
                    {
                        d[TileType.Park] += 1;
                    }
                }
                f.GenerateRandomBasic(d);
                break;
        }

        return f;
    }

    public void UpdateConveyor()
    {
        for (int i = 0; i < maxSize; i++)
        {
            while (figureList.Count > i && figureList[i] is null)
            {
                figureList.RemoveAt(i);
            }
            while (figureList.Count <= i)
            {
                if (queue.Count > 0)
                {
                    figureList.Add(queue[0]);
                    queue.RemoveAt(0);
                } 
                else
                {
                    var f = CreateFigure();
                    figureList.Add(f);
                }
            }
        }

        for (int i = 0; i < maxSize; i++)
        {
            figureList[i].transform.parent = this.transform;
            figureList[i].gameObject.transform.position = offset * i + transform.position;
            figureList[i].idle = offset * i + transform.position;
        }

        if (cui is not null)
        {
            cui.UpdateCounter(queue.Count);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
