using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] string SceneName;
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
                SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            }
        }
    }
}
