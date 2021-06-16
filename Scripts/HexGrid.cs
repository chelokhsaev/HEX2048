using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Setup/Grid")]
public class HexGrid : PrefabObject
{
    public GameObject сellPrefab;
    public DiceFactory diceFactory;

    public Type diceLogic;

    public bool rotateBoard;

    private GameData gameData;

    private float moveTime;

    private Quaternion targetRot;
    private Quaternion startRot;

    private static readonly Quaternion rot0 = Quaternion.Euler(0f, 20f, 0f);
    private static readonly Quaternion rot60 = Quaternion.Euler(-15f, 10f, 0f);
    private static readonly Quaternion rot120 = Quaternion.Euler(15f, -10f, 0f);
    private static readonly Quaternion rot180 = Quaternion.Euler(0f, -20f, 0f);
    private static readonly Quaternion rot240 = Quaternion.Euler(-15f, -10f, 0f);
    private static readonly Quaternion rot300 = Quaternion.Euler(15f, 10f, 0f);

    public override void Init(params object[] args)
    {
        gameData = args[0] as GameData;
    }

    public override void Start()
    {
        Main.SubscribeToUpdates(this);
        if (rotateBoard)
            gameData.Shifted += OnShiftDices;
        StartGame();
    }

    void StartGame() {
        for (int x = -4; x <= 4; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                if (!GameController.IsValideCell(x, y))
                    continue;
                GameObject.Instantiate(сellPrefab, transform, false).transform.localPosition = new Vector3(x * 0.5f, y * 0.866f, 0f);
            }
        }
        gameData.NewDice += OnNewDice;
    }

    private void OnNewDice(DiceData diceData)
    {
        diceFactory.CreateDice(transform, diceData);
    }

    public override void UpdateTime(float deltaTime)
    {
        if (moveTime <= 0.5f)
        {
            moveTime += deltaTime;
            if (moveTime < 0.25f)
                transform.localRotation = Quaternion.Slerp(startRot, targetRot,
                    Easing.Quadratic.Out(moveTime * 4f));
            else if (moveTime < 0.5f)
                transform.localRotation = Quaternion.Slerp(targetRot, Quaternion.identity,
                    Easing.Quadratic.In((moveTime - 0.25f) * 4f));
            else
                transform.localRotation = Quaternion.identity;
            return;
        }
    }

    private void OnShiftDices(int dx, int dy)
    {
        if(dx < 0)
        {
            if (dy > 0)
                targetRot = rot300;
            else if (dy < 0)
                targetRot = rot60;
            else
                targetRot = rot0;
        }
        else
        {
            if (dy < 0)
                targetRot = rot240;
            else if (dy > 0)
                targetRot = rot120;
            else
                targetRot = rot180;
        }
        moveTime = 0f;
        startRot = transform.localRotation;
    }

}
