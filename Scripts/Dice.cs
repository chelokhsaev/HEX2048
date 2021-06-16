using UnityEngine;

public class Dice : UpdatingTransform
{

    private static readonly Color[] colors = new Color[16]
    {   new Color(1f, 0f, 0f),
        new Color(0f, 1f, 0f),
        new Color(0f, 0f, 1f),

        new Color(1f, 1f, 0f),
        new Color(0f, 1f, 1f),
        new Color(1f, 0f, 1f),

        new Color(1f, 0.5f, 0f),
        new Color(0f, 1f, 0.5f),
        new Color(0.5f, 0f, 1f),

        new Color(0f, 0.5f, 1f),
        new Color(1f, 0f, 0.5f),
        new Color(0.5f, 1f, 0f),

        new Color(0.5f, 0.5f, 1f),
        new Color(1f, 0.5f, 0.5f),
        new Color(0.5f, 1f, 0.5f),

        new Color(1f, 1f, 1f),
    };

    protected DiceData data;

    protected bool dataChanged;
    private Vector3 targetPosition;
    private Vector3 lastPosition;
    protected float dist;
    protected float passed;
    protected float passedTime;
    protected float totalTime;
    private float downTime = 0f;
    private GameObject colorObject;

    protected enum State { None, Moving, Appearing };
    protected State state = State.None;

    private Color lastColor;

    protected DiceData last;

    public override void Init(params object[] args)
    {
        data = (DiceData)args[0];
        last = data.Clone();
        colorObject = this.transform.Find("Color") ? this.transform.Find("Color").gameObject :
            this.transform.Find("Roll").Find("Color").gameObject;
        data.Change += () => { dataChanged = true; };
        targetPosition = new Vector3(data.X * 0.5f, data.Y * 0.866f, 0.0f);
        lastPosition = transform.localPosition = targetPosition;
        Main.SubscribeToUpdates(this);
        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        lastColor = colors[data.Value];
        Utils.SetColor(colorObject.transform, colors[data.Value]);
        state = State.Appearing;
        totalTime = 1f;
        passedTime = 0f;
    }

    public override void UpdateTime(float deltaTime)
    {
        if (dataChanged)
            Validate();
        if (transform)
        {
            if (state == State.Appearing)
            {
                passedTime += deltaTime;
                if (passedTime < totalTime)
                {
                    if (passedTime > totalTime / 2f)
                    {
                        float scale = (passedTime - totalTime / 2f) / (totalTime / 2f);
                        scale = Easing.Quadratic.Out(scale);
                        transform.localScale = new Vector3(scale, scale, scale);
                    }
                }
                else
                {
                    transform.localScale = Vector3.one;
                    state = State.None;
                }
            }
            if (state == State.Moving)
            {
                passedTime += deltaTime;
                if (passedTime < totalTime)
                {
                    passed = dist * Easing.Quadratic.InOut(passedTime / totalTime);
                    transform.localPosition = Vector3.Lerp(lastPosition, targetPosition, passed / dist);
                    if (dist - passed <= 1f)
                    {
                        if (last.MergingDice != null)
                        {
                            last.MergingDice.Destroyed = true;
                            last.MergingDice = null;
                        }
                        Utils.SetColor(colorObject.transform, Color.Lerp(lastColor, colors[data.Value], Easing.Quadratic.In(passed - dist + 1f)));
                    }
                }
                else
                {
                    transform.localPosition = targetPosition;
                    state = State.None;
                    dist = 0f;
                    passed = 0f;
                    Utils.SetColor(colorObject.transform, colors[data.Value]);
                    lastColor = colors[data.Value];
                    if (data.MergingDice != null)
                    {
                        data.MergingDice.Destroyed = true;
                        data.MergingDice = null;
                    }
                }
            }
        }
        if(downTime > 0f)
        {
            downTime -= deltaTime;
            if (downTime <= 0f)
            {
                data.Change -= OnDataChange;
                GameObject.Destroy(transform.gameObject);
                Main.UnubscribeFromUpdates(this);
            }
            else
            {
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Easing.Quadratic.In(2f * (0.5f - downTime)));
            }
        }
    }



    protected virtual void Validate()
    {
        if (!dataChanged)
            return;
        if (state == State.Appearing)
            transform.localScale = Vector3.one;
        dataChanged = false;
        if (data.Destroyed)
        {
            downTime = 0.5f;
            return;
        }
        if (data.Value != last.Value)
        {
            lastColor = colors[last.Value];
            last.Value = data.Value;
        }
        if (data.X != last.X)
        {
            lastPosition = transform.localPosition;
            targetPosition = new Vector3(data.X * 0.5f, data.Y * 0.866f, data.Replaced ? 0.1f : 0.0f);
            dist = GameController.Distance(data.X - last.X, data.Y - last.Y);
            passed = 0f;
            passedTime = 0;
            totalTime = 0.5f * Mathf.Sqrt(dist);
            last.X = data.X;
            last.Y = data.Y;
            state = State.Moving;
        }
        else
        {
            totalTime = 0f;
        }
        if (data.MergingDice != last.MergingDice)
        {
            if (last.MergingDice != null)
            {
                last.MergingDice.Destroyed = true;
                last.MergingDice = null;
            }
            last.MergingDice = data.MergingDice;
        }
    }

}
