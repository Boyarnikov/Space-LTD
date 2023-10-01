using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class DialogueDispellerTarget : MonoBehaviour
{
    public Vector2 position;
    public Vector2 scale;
    [SerializeField] GameObject frame;
    void Start()
    {

    }

    public void Settings(Vector2 gap) {
        float w = frame.transform.lossyScale.x;
        float h = frame.transform.lossyScale.y;
        position = new Vector2(frame.transform.position.x, frame.transform.position.y);
        scale = new Vector2(w + gap.x, h + gap.y);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
