using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CoverController : MonoBehaviour
{
    [SerializeField] MainSpriteBehaviour sprite_behaviour;
    [SerializeField] float cover_alpha;
    [SerializeField] GameObject dispeller_prefab;
    [SerializeField] Vector2 dispeller_target_position;
    [SerializeField] Vector2 dispeller_target_size;

    DispellerController dispeller;

    bool open = false;
    void Start()
    {
        InitDispeller();
        transform.localScale = new Vector2(1000, 1000);
        int key = (int)AnimationSpriteParameters.MULTIPLY_ALPHA;
        sprite_behaviour.data[key].Set(cover_alpha);
        sprite_behaviour.SetMainColor(Color.black);
    }

    public void InitDispeller()
    {
        GameObject dispeller_object = Instantiate(dispeller_prefab);
        dispeller = dispeller_object.GetComponent<DispellerController>();
    }

    public void SetDispellerTarget(Vector2 target_position, Vector2 target_size) {
        dispeller.SetTarget(target_position, target_size);
    }

    public void SetDispellerTargetForce(Vector2 target_position, Vector2 target_size) {
        dispeller.SetTargetForce(target_position, target_size);
    }

    public void OpenCover(Func<float> hook) {
        SetDispellerTargetForce(new Vector2(0, 0), new Vector2(0, 0));
        open = true;
        int key = (int)AnimationSpriteParameters.ALPHA;
        sprite_behaviour.data[key].speed = 0.05f;
        sprite_behaviour.data[key].repeat = false;
        sprite_behaviour.data[key].reset = false;
        sprite_behaviour.data[key].Front();
        sprite_behaviour.data[key].hook = hook;
        sprite_behaviour.data[key].Play();
    }

    public void CloseCover(Func<float> hook)
    {
        open = false;
        int key = (int)AnimationSpriteParameters.ALPHA;
        sprite_behaviour.data[key].speed = -0.05f;
        sprite_behaviour.data[key].repeat = false;
        sprite_behaviour.data[key].reset = false;
        sprite_behaviour.data[key].Back();
        sprite_behaviour.data[key].hook = hook;
        sprite_behaviour.data[key].Play();
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown("space"))
        {
            if (open) CloseCover(() => { return 0; });
            else OpenCover(() => { return 0; });
        }
        */

        if (Input.GetKeyDown(KeyCode.M))
        {
            SetDispellerTarget(dispeller_target_position, dispeller_target_size);
        }
    }
}
