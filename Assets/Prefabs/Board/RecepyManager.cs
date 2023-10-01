using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecepyManager : MonoBehaviour
{
    [SerializeField] GameObject frame;
    // Start is called before the first frame update
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
        transform.position = new Vector2(- cam_w + w / 2 + delta_x + 0.2f + cam_x, - cam_h + h / 2 + cam_y + 0.2f + delta_y);

        DialogueDispellerTarget[] targets = GetComponentsInChildren<DialogueDispellerTarget>();
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].Settings(new Vector2(0.25f, 0.25f));
        }
    }

}
