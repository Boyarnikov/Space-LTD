using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<GameObject> list = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive((i <= Globals.levelsUnlocked));
        }
    }
}
