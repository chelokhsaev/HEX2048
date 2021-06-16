using UnityEngine;
using System.Collections.Generic;
using System;

public class GameData
{


    public List<CellData> freeCells = new List<CellData>();
    public Dictionary<int, CellData> cellsByCoords = new Dictionary<int, CellData>();

    public event Action<DiceData> NewDice;
    public event Action<int, int> Shifted;
    public event Action OnGameOver;

    public void AddDice(DiceData dice)
    {
        NewDice?.Invoke(dice);
    }

    public void Shift(int dx, int dy)
    {
        Shifted?.Invoke(dx, dy);
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
    }

}
