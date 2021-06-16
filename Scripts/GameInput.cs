using UnityEngine;
using System;

public class GameInput : UpdatingTransform
{

    GameController controller;

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    private bool touchStarted;

    public override void Start()
    {
        base.Start();
        Main.SubscribeToUpdates(this);
    }

    public override void Init(params object[] args)
    {
        base.Init(args);
        controller = args[0] as GameController;
        controller.MadeMove += OnMadeMove;
    }

    private float haltTime = 0;

    private void OnMadeMove()
    {
        haltTime = 0.5f;
    }

    private int[] dirX = new int[6] { -2, -1, 1, 2, 1, -1 };
    private int[] dirY = new int[6] { 0, -1, -1, 0, 1, 1 };

    public override void UpdateTime(float deltaTime)
    {
        haltTime -= deltaTime;
        if (haltTime > 0f)
            return;
        if (Input.GetKeyDown(KeyCode.X))
            controller.ShiftDices(1, -1);
        else if (Input.GetKeyDown(KeyCode.S))
            controller.ShiftDices(2, 0);
        else if (Input.GetKeyDown(KeyCode.W))
            controller.ShiftDices(1, 1);
        else if (Input.GetKeyDown(KeyCode.Z))
            controller.ShiftDices(-1, -1);
        else if (Input.GetKeyDown(KeyCode.A))
            controller.ShiftDices(-2, 0);
        else if (Input.GetKeyDown(KeyCode.Q))
            controller.ShiftDices(-1, 1);
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
                touchStarted = true;
            }
            if (touchStarted && touch.phase == TouchPhase.Moved)
            {
                fingerUpPosition = touch.position;
                if (Vector2.Distance(fingerDownPosition, fingerUpPosition) > 100f)
                {
                    float angle = 180f * Mathf.Atan2(fingerUpPosition.y - fingerDownPosition.y, fingerUpPosition.x - fingerDownPosition.x) / Mathf.PI + 180f;
                    int dir = Mathf.RoundToInt(angle / 60f) % 6;
                    fingerUpPosition = fingerDownPosition;
                    touchStarted = false;
                    controller.ShiftDices(dirX[dir], dirY[dir]);
                }
            }

        }
    }

}
