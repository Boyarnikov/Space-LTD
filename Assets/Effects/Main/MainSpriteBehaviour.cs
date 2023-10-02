using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum AnimationSpriteParameters
{
    ALPHA = 0,
    MULTIPLY_ALPHA = 1,
    SUB_COLOR_ALPHA = 2,
}

public class AnimationSpriteData {
    public bool play;
    public bool repeat;
    public bool reset;
    public float timer;
    public float speed;
    public float value;
    public AnimationCurve curve;
    public Func<float> hook;
    public Func<float, float> every_step;

    public AnimationSpriteData(AnimationCurve _curve) {
        play = false;
        repeat = false;
        reset = true;
        timer = 0;
        speed = 0;
        curve = _curve;
        hook = null;
        every_step = null;
    }

    public void DebugInfo() {
        Debug.Log("play " + play);
        Debug.Log("repeat " + repeat);
        Debug.Log("reset " + reset);
        Debug.Log("timer " + timer);
        Debug.Log("speed " + speed);
        Debug.Log("value " + value);
    }
    public void CalculateValue() { value = curve.Evaluate(timer); }

    public void MoveTimer() { timer += speed; }
    public void IsTimeOut() {
        if (timer > 1 || timer < 0)
        {
            if (repeat == false)
            {
                Reverse();
                Restart();
                Reverse();
                Pause();
                if (reset)
                {
                    Restart();
                }
            }
            else
            {
                Restart();
            }
            if (hook != null)
            {
                hook();
            }
        }
    }

    public void PlayStep()
    {
        if (play)
        {
            MoveTimer();
            IsTimeOut();
            CalculateValue();
            Step();
        }
    }
    public void Step() {
        if (every_step != null)
        {
            every_step(value);
        }
    }
    public void Set(float _value) { 
        value = _value;
        Step();
    }
    public void Play() { play = true; }
    public void Pause() { play = false; }
    public void Restart() {
        if (speed > 0) timer = 0;
        else timer = 1;
    }
    public void Reverse() { speed *= -1; }
    public void Front() { if (speed < 0) { Reverse(); } }
    public void Back() { if (speed > 0) { Reverse(); } }
}

public class MainSpriteBehaviour : MonoBehaviour
{
    [SerializeField] AnimationCurve[] curves;
    [SerializeField] SpriteRenderer sprite_renderer;
    int count_of_parameters = 2;
    public List<AnimationSpriteData> data;
    public void Start()
    {
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        data = new List<AnimationSpriteData>();
        InitCurves();
    }

    public void InitCurves() {
        AnimationSpriteData data_alpha = new AnimationSpriteData(curves[0]);
        data_alpha.every_step = SetMainAlpha;
        data.Add(data_alpha);
        AnimationSpriteData data_alpha_mult = new AnimationSpriteData(curves[1]);
        data_alpha_mult.every_step = SetMultAlpha;
        data.Add(data_alpha_mult);
        if (curves.Length > 2)
        {
            AnimationSpriteData data_subcolor_alpha = new AnimationSpriteData(curves[2]);
            data_subcolor_alpha.every_step = SetSubColorAlpha;
            data.Add(data_subcolor_alpha);
        }
    }
    public void Set(AnimationSpriteParameters key, float value) { data[(int)key].Set(value); }
    public void Play(AnimationSpriteParameters key) { data[(int)key].Play(); }
    public void Pause(AnimationSpriteParameters key) { data[(int)key].Pause(); }
    public void Restart(AnimationSpriteParameters key) { data[(int)key].Restart(); }

    public void UpdateData() {
        for (int i = 0; i < data.Count; i++)
        {
            data[i].PlayStep();
        }
    }

    public void Enable() { sprite_renderer.enabled = true; }
    public void Disable() { sprite_renderer.enabled = false; }

    public float SetMainAlpha(float alpha)
    {
        sprite_renderer.material.SetFloat("_MainAlpha", alpha);
        return 0;
    }

    public float SetMultAlpha(float alpha)
    {
        sprite_renderer.material.SetFloat("_MultAlpha", alpha);
        return 0;
    }

    public void SetDrawBorder(float draw_border)
    {
        sprite_renderer.material.SetFloat("_DrawBorder", draw_border);
    }

    public void SetOrder(int order)
    {
        sprite_renderer.sortingOrder = order;
    }

    public void SetBorderColor(Color color)
    {
        sprite_renderer.material.SetColor("_BorderColor", color);
    }

    public float SetMainColor(Color color)
    {
        sprite_renderer.material.SetColor("_MainColor", color);
        return 0;
    }

    public float SetSubColor(Color color)
    {
        sprite_renderer.material.SetColor("_SubColor", color);
        return 0;
    }

    public float SetSubColorAlpha(float alpha)
    {
        sprite_renderer.material.SetFloat("_SubColorAlpha", alpha);
        return 0;
    }

    public void FixedUpdate()
    {
        UpdateData();
    }
}
