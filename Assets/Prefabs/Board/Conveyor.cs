using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3 (-1f, 0, 0);
    [SerializeField] Figure figure;
    [SerializeField] int maxSize = 5;

    public List<Figure> figureList = new List<Figure>();
    // Start is called before the first frame update
    void Start()
    {
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
                figureList.Add(Instantiate(figure));
            }
        }

        for (int i = 0; i < maxSize; i++)
        {
            figureList[i].gameObject.transform.position = offset * i + transform.position;
            figureList[i].idle = offset * i + transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
