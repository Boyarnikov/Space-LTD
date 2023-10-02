using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimaitonManager : MonoBehaviour
{
    [SerializeField] MainSpriteBehaviour sprite_behaviour;
    void Start()
    {
        
    }

    public void UncorrectAnimation()
    {
        sprite_behaviour.SetSubColor(Color.red);
        int key = (int)AnimationSpriteParameters.SUB_COLOR_ALPHA;
        sprite_behaviour.data[key].speed = 0.01f;
        sprite_behaviour.data[key].repeat = false;
        sprite_behaviour.data[key].reset = true;
        sprite_behaviour.data[key].Front();
        sprite_behaviour.data[key].Play();
    }
}
