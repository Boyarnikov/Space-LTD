using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicButton : MonoBehaviour
{
    [SerializeField] int type = 0;
    [SerializeField] public Sprite Lit;
    [SerializeField] public Sprite Unlit;
    int state = 0;

    Collider2D colla;

    void Start()
    {
        colla = GetComponent<Collider2D>();
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (colla == Physics2D.OverlapPoint(mousePos))
            {
                state = (state + 1) % 2;
                GetComponent<SpriteRenderer>().sprite = (state == 0) ? Lit : Unlit;
                SoundManager.instance.ToggleMusic();
                SoundManager.instance.Place();
            }
        }
        if (colla == Physics2D.OverlapPoint(mousePos))
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
        else
        {
            transform.localScale = Vector3.one * 0.8f;
        }
    }
}

