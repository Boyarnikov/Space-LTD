using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispellerController : MonoBehaviour
{
    Vector2 target_position;
    Vector2 target_size;
    bool need_change = false;
    void Start()
    {
        
    }

    public void SetTargetForce(Vector2 _target_position, Vector2 _target_size) {
        target_position = _target_position;
        target_size = _target_size;
        transform.position = target_position;
        transform.localScale = target_size;
    }

    public void SetTarget(Vector2 _target_position, Vector2 _target_size) {
        target_position = _target_position;
        target_size = _target_size;
        need_change = true;
    }

    bool NeedChange()
    {
        float delta_x = (target_position.x - transform.position.x);
        float delta_y = (target_position.y - transform.position.y);
        if (Mathf.Sqrt(delta_x * delta_x + delta_y * delta_y) > 0.01f)
        {
            return true;
        }
        transform.position = target_position;
        float delta_scale_x = target_size.x - transform.localScale.x;
        float delta_scale_y = target_size.y - transform.localScale.y;
        if (Mathf.Sqrt(delta_scale_x * delta_scale_x + delta_scale_y * delta_scale_y) > 0.01f)
        {
            return true;
        }
        transform.localScale = target_size;
        return false;
    }

    bool Change() {
        float x = transform.position.x + (target_position.x - transform.position.x) / 8;
        float y = transform.position.y + (target_position.y - transform.position.y) / 8;
        transform.position = new Vector2(x, y);
        float scale_x = transform.localScale.x + (target_size.x - transform.localScale.x) / 8;
        float scale_y = transform.localScale.y + (target_size.y - transform.localScale.y) / 8;
        transform.localScale = new Vector2(scale_x, scale_y);
        return NeedChange();
    }
    
    void FixedUpdate()
    {
        if (need_change)
        {
            need_change = Change();
        }
    }
}
