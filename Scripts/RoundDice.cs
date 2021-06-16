using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundDice : Dice
{

    protected Renderer frontRenderer;
    protected Renderer backRenderer;
    protected Transform roll;
    protected Transform back;
    protected Transform front;

    public override void Init(params object[] args)
    {
        base.Init(args);
        roll = transform.Find("Roll");
        front = roll.Find("Front");
        back = roll.Find("Back");
        frontRenderer = front.gameObject.GetComponent<Renderer>();
        if (frontRenderer == null)
            frontRenderer = front.gameObject.GetComponentInChildren<Renderer>();
        backRenderer = back.gameObject.GetComponent<Renderer>();
        if (backRenderer == null)
            backRenderer = back.gameObject.GetComponentInChildren<Renderer>();
        frontRenderer.material = TexturesRenderer.materials[data.Value];
        backRenderer.material = TexturesRenderer.materials[data.Value];
    }

    protected Quaternion rot60 = Quaternion.Euler(0f, 0f, 60f);
    protected Quaternion rot120 = Quaternion.Euler(0f, 0f, 120f);
    protected Quaternion rot180 = Quaternion.Euler(0f, 0f, 180f);
    protected Quaternion rot240 = Quaternion.Euler(0f, 0f, 240f);
    protected Quaternion rot300 = Quaternion.Euler(0f, 0f, 300f);

    protected Quaternion fRot0 = Quaternion.Euler(0f, -90f, 0f);
    protected Quaternion fRot60 = Quaternion.Euler(60f, -90f, 0f);
    protected Quaternion fRot120 = Quaternion.Euler(120f, -90f, 0f);
    protected Quaternion fRot180 = Quaternion.Euler(180f, -90f, 0f);
    protected Quaternion fRot240 = Quaternion.Euler(240f, -90f, 0f);
    protected Quaternion fRot300 = Quaternion.Euler(300f, -90f, 0f);

    protected Quaternion bRot0 = Quaternion.Euler(0f, 90f, 0f);
    protected Quaternion bRot60 = Quaternion.Euler(60f, 90f, 0f);
    protected Quaternion bRot120 = Quaternion.Euler(120f, 90f, 0f);
    protected Quaternion bRot180 = Quaternion.Euler(180f, 90f, 0f);
    protected Quaternion bRot240 = Quaternion.Euler(240f, 90f, 0f);
    protected Quaternion bRot300 = Quaternion.Euler(300f, 90f, 0f);

    protected override void Validate()
    {
        frontRenderer.material = TexturesRenderer.materials[last.Value];
        backRenderer.material = TexturesRenderer.materials[last.Value];
        if (dataChanged)
        {
            if (data.X < last.X)
            {
                if (last.Y < data.Y)
                {
                    transform.localRotation = rot300;
                    back.localRotation = fRot300;
                    front.localRotation = bRot300;
                }
                else if (last.Y > data.Y)
                {
                    transform.localRotation = rot60;
                    back.localRotation = fRot60;
                    front.localRotation = bRot60;
                }
                else
                {
                    transform.localRotation = Quaternion.identity;
                    back.localRotation = fRot0;
                    front.localRotation = bRot0;
                }
            }
            else if (data.X > last.X)
            {
                if (last.Y < data.Y)
                {
                    transform.localRotation = rot240;
                    back.localRotation = fRot240;
                    front.localRotation = bRot240;
                }
                else if (last.Y > data.Y)
                {
                    transform.localRotation = rot120;
                    back.localRotation = fRot120;
                    front.localRotation = bRot120;
                }
                else
                {
                    transform.localRotation = rot180;
                    back.localRotation = fRot180;
                    front.localRotation = bRot180;
                }
            }
        }
        base.Validate();
    }

    public override void UpdateTime(float time)
    {
        if (state == State.Moving)
        {
            if(dist - passed - time <= 1f)
                backRenderer.material = TexturesRenderer.materials[data.Value];
            if (passed + time >= dist)
            {
                frontRenderer.material = TexturesRenderer.materials[data.Value];
                backRenderer.material = TexturesRenderer.materials[data.Value];
            }
            roll.localRotation = Quaternion.Euler(0f, ((passed + time) % 1) * 180f, 0f);
        } else
        {
            roll.localRotation = Quaternion.identity;
        }
        base.UpdateTime(time);
    }

}
