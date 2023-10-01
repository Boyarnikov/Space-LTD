using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Mathematics.math;

public class CounterItemUI : MonoBehaviour
{
    Vector3 idle;
    float updateTimestamp;
    float offset = 0;
    Vector3 scale = Vector3.one;

    public void UpdateCounter(int i)
    {
        idle = transform.position;
        updateTimestamp = Time.time;
        offset = i;
    }

    private void Start()
    {
        scale = transform.localScale;
    }

    private void Update()
    {
        if (updateTimestamp + 0.5f > Time.time) {
            //transform.position = idle + Vector3.up * sin((Time.time - updateTimestamp + offset)* 15f) * (updateTimestamp + 0.5f - Time.time) * 0.2f;
            transform.localScale = scale * (1 + sin((Time.time - updateTimestamp + offset) * 15f) * (updateTimestamp + 0.5f - Time.time) * 0.5f);
        }
        else
        {
            transform.position = idle;
        }
    }

}
