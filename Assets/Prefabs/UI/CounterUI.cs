using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Mathematics.math;

public class CounterUI : MonoBehaviour
{
    private int count;
    [SerializeField] public GameObject counter_item;
    [SerializeField] public Vector3 start;
    [SerializeField] public Vector3 offset;
    List<GameObject> items = new List<GameObject>();
    [SerializeField] public DialogueDispellerTarget target;
    // Start is called before the first frame update
    void Start()
    {
    }

    public int GetCounter()
    {
        return count;
    }

    // Update is called once per frame
    public void UpdateCounter(int i)
    {
        int old_count = count;
        if (i > 0) count = i; else count = 0;

        if (count < items.Count)
        {
            for (int j = items.Count - 1; j >= count; j--) {
                Destroy(items[j]);
                items.RemoveAt(j);
            }
        }
        else if (count > items.Count) {
            for (int j = items.Count; j < count; j++)
            {
                GameObject c = Instantiate(counter_item) as GameObject;
                c.transform.parent = transform;
                c.transform.position = transform.position + offset * j;
                c.GetComponent<SpriteRenderer>().sortingLayerName = "Board";
                c.GetComponent<SpriteRenderer>().sortingOrder = 0;
                items.Add(c);
            }
        }

        if (old_count != count) {
            for (int j = 0; j < count; j++)
            {
                CounterItemUI c = items[j].GetComponent<CounterItemUI>();
                if (c is not null)
                {
                    c.UpdateCounter(j);
                }
            }
        }
        
    }
}
