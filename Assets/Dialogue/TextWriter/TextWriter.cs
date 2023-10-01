using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    [SerializeField] RectTransform text_transform;
    [SerializeField] TextMeshPro text_mesh_pro;
    void Start()
    {
        Vector2 new_position = new Vector2(Camera.main.transform.position.x, -1 * Camera.main.orthographicSize + text_transform.rect.height / 2 + Camera.main.transform.position.y);
        transform.position = new_position;
    }

    public void Disable()
    {
        text_mesh_pro.enabled = false;
    }

    public void Enable()
    {
        text_mesh_pro.enabled = true;
    }

    public void SetText(string text) {
        text_mesh_pro.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
