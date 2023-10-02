using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    [SerializeField] string SceneName = "MainMenu";
    [SerializeField] public Sprite Lit;
    [SerializeField] public Sprite Unlit;

    Collider2D coll;
    void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (coll == Physics2D.OverlapPoint(mousePos))
            {
                Application.Quit();
                SoundManager.instance.Place();
            }

        }
        if (coll == Physics2D.OverlapPoint(mousePos))
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
        else
        {
            transform.localScale = Vector3.one * 0.8f;
        }

    }
}
