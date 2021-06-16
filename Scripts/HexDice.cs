using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexDice : Dice
{
    private TextMeshPro text;

    public override void Init(params object[] args)
    {
        base.Init(args);
        text = transform.GetComponentInChildren<TextMeshPro>();
        SetText(Mathf.Pow(2f, data.Value));
    }

    private int lastVal;
    private int newVal;

    private bool changing = false;

    protected override void Validate()
    {
        if(dataChanged)
        {
            changing = false;
            if (last.X != data.X)
            {
                if (last.Value != data.Value)
                {
                    changing = true;
                    lastVal = (int)Mathf.Pow(2f, last.Value);
                    newVal = (int)Mathf.Pow(2f, data.Value);
                }
            }
        }
        base.Validate();
    }

    public override void UpdateTime(float deltaTime)
    {
        if (state == State.Moving && changing)
        {
            if (dist - passed - deltaTime <= 1f)
            {
                SetText(Mathf.Lerp(lastVal, newVal, 1f + passed + deltaTime - dist));
            }
            else if (passed + deltaTime >= dist)
            {
                SetText(Mathf.Pow(2f, data.Value));
                changing = false;
            }
        }
        base.UpdateTime(deltaTime);
    }

    private void SetText(float value)
    {
        if(value < 10f)
        {
            value *= 100f;
            value = Mathf.Round(value);
            value /= 100f;
            text.SetText(value.ToString("0.00"));
        }
        else if (value < 100f)
        {
            value *= 10f;
            value = Mathf.Round(value);
            value /= 10f;
            text.SetText(value.ToString("0.0"));
        }
        else
        {
            value = Mathf.Round(value);
            text.SetText(value.ToString("0"));
        }
    }


}
