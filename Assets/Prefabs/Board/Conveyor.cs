using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


public enum ConveyorType
{
    Small
}


public class Conveyor : MonoBehaviour
{
    [SerializeField] public CounterUI cui;
    [SerializeField] Vector3 offset = new Vector3 (-1f, 0, 0);
    [SerializeField] Figure figure;
    [SerializeField] int maxSize = 5;
    [SerializeField] GameObject frame;

    public List<Figure> queue = new List<Figure>();

    public List<Figure> figureList = new List<Figure>();
    // Start is called before the first frame update
    void Start()
    {
        float w = frame.transform.localScale.x;
        float h = frame.transform.localScale.y;
        float cam_w = Camera.main.orthographicSize * Camera.main.aspect;
        float cam_h = Camera.main.orthographicSize;
        float cam_x = Camera.main.transform.position.x;
        float cam_y = Camera.main.transform.position.y;
        transform.position = new Vector2(w / 2 + offset.x / 2 + cam_x, cam_h - h / 2 + cam_y - 0.2f);
        UpdateConveyor();
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
                    figureList.Add(Instantiate(figure));
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
