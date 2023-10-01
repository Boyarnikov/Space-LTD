using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<GameObject> list;

    private void Awake()
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(i < Globals.levelsUnlocked);
            var s = list[i].GetComponent<LevelButton>();
            list[i].GetComponent<SpriteRenderer>().sprite = (i < Globals.levelsDone) ? s.Lit : s.Unlit;
        }
    }
}
