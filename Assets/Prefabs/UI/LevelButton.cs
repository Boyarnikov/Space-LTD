using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] public CounterUI cui;
    [SerializeField] string SceneName;
    [SerializeField] public Sprite Lit;
    [SerializeField] public Sprite Unlit;
    public int count;
    public bool lit = false;

    Collider2D coll;
    void Start()
    {
        coll = GetComponent<Collider2D>();

    }

    void Update()
    {
        cui.UpdateCounter(Globals.stars[count]);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && (count < Globals.levelsUnlocked))
        {
            if (coll == Physics2D.OverlapPoint(mousePos))
            {
                SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
                SoundManager.instance.Place();
            }
            
        }
        if (coll == Physics2D.OverlapPoint(mousePos))
        {
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
        else
        {
            transform.localScale = Vector3.one;
        }

    }
}
