using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    private readonly GameData gameData;

    private int moveCount = 0;

    public delegate void ControllerHandle();
    public event ControllerHandle MadeMove;

    public GameController(GameData gameData)
    {
        this.gameData = gameData;
    }

    public void StartGame()
    {
        for (int x = -4; x <= 4; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                if (!IsValidCell(x, y))
                    continue;
                CellData cell = new CellData(x, y);
                cell.Change += OnCellDataChange;
                gameData.freeCells.Add(cell);
                gameData.cellsByCoords.Add(cell.Hash(), cell);
            }
        }
        SpawnDices();
    }

    private void SpawnDices()
    {
        for (var i = 0; i < 3; i++)
        {
            if (gameData.freeCells.Count == 0)
            {
                //GameOver();
                break;
            }
            CellData cell = gameData.freeCells[Random.Range(0, gameData.freeCells.Count)];
            DiceData dice = new DiceData(cell.x, cell.y, Random.Range(0, 2));
            cell.Dice = dice;
            gameData.AddDice(dice);
        }
        if (IsGameOver())
            gameData.GameOver();
    }

    private bool IsGameOver()
    {
        if (gameData.freeCells.Count != 0)
            return false;
        for (int x = -4; x <= 4; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                if (!IsValidCell(x, y))
                    continue;
                gameData.cellsByCoords.TryGetValue(CellData.Hash(x, y), out CellData cell);
                int value = cell.Dice.Value;
                if (IsValidCell(x + 2, y))
                {
                    gameData.cellsByCoords.TryGetValue(CellData.Hash(x + 2, y), out cell);
                    if (cell.Dice.Value == value)
                        return false;
                }
                if (IsValidCell(x + 1, y - 1))
                {
                    gameData.cellsByCoords.TryGetValue(CellData.Hash(x + 1, y - 1), out cell);
                    if (cell.Dice.Value == value)
                        return false;
                }
                if (IsValidCell(x + 1, y + 1))
                {
                    gameData.cellsByCoords.TryGetValue(CellData.Hash(x + 1, y + 1), out cell);
                    if (cell.Dice.Value == value)
                        return false;
                }

            }
        }
        return true;
    }

    public static bool IsValidCell(int x, int y)
    {
        if ((x + y) % 2 != 0)
            return false;
        if (Distance(x, y) > 2)
            return false;
        return true;
    }

    public static int Distance(int x, int y)
    {
        int ax = Mathf.Abs(x);
        int ay = Mathf.Abs(y);
        if (ay >= ax)
            return ay;
        return (ax + ay) / 2;
    }

    private void OnCellDataChange(CellData cell)
    {
        if (cell.IsFree())
        {
            if (!gameData.freeCells.Contains(cell))
                gameData.freeCells.Add(cell);
        }
        else
        {
            if (gameData.freeCells.Contains(cell))
                gameData.freeCells.Remove(cell);
        }
    }

    private bool moved;

    public void ShiftDices(int dx, int dy)
    {
        moved = false;
        moveCount++;
        if (dx == 2 && dy == 0)
            for (int x = 4; x >= -4; x--)
                for (int y = -2; y <= 2; y++)
                    ShiftDice(x, y, dx, dy);
        if (dx == 1 && dy == -1)
            for (int x = 2; x >= -6; x -= 2)
                for (int y = -2; y <= 2; y++)
                    ShiftDice(x + y + 2, y, dx, dy);
        if (dx == 1 && dy == 1)
            for (int x = 6; x >= -2; x -= 2)
                for (int y = -2; y <= 2; y++)
                    ShiftDice(x - y - 2, y, dx, dy);
        if (dx == -2 && dy == 0)
            for (int x = -4; x <= 4; x++)
                for (int y = -2; y <= 2; y++)
                    ShiftDice(x, y, dx, dy);
        if (dx == -1 && dy == 1)
            for (int x = -6; x <= 2; x += 2)
                for (int y = -2; y <= 2; y++)
                    ShiftDice(x + y + 2, y, dx, dy);
        if (dx == -1 && dy == -1)
            for (int x = -2; x <= 6; x += 2)
                for (int y = -2; y <= 2; y++)
                    ShiftDice(x - y - 2, y, dx, dy);
        if (!moved)
        {
            moveCount--;
            return;
        }

        SpawnDices();
        MadeMove?.Invoke();
        gameData.Shift(dx, dy);

    }

    private void ShiftDice(int x, int y, int dx, int dy)
    {
        if (!IsValidCell(x, y))
            return;
        CellData cell = gameData.cellsByCoords[CellData.Hash(x, y)];
        if (cell.IsFree())
            return;
        DiceData dice = cell.Dice;
        int tx = x;
        int ty = y;
        int dt = 1;
        while (IsValidCell(x + dx * dt, y + dy * dt))
        {
            if (gameData.cellsByCoords[CellData.Hash(x + dx * dt, y + dy * dt)].IsFree())
            {
                tx = x + dx * dt;
                ty = y + dy * dt;
                dt++;
                continue;
            }
            if (gameData.cellsByCoords[CellData.Hash(x + dx * dt, y + dy * dt)].Dice.Value == dice.Value
            && gameData.cellsByCoords[CellData.Hash(x + dx * dt, y + dy * dt)].Dice.lastMergedMoveNum != moveCount)
            {
                tx = x + dx * dt;
                ty = y + dy * dt;
                dice.Value++;
            }
            break;
        }
        if (tx == x && ty == y)
            return;
        moved = true;
        dice.X = tx;
        dice.Y = ty;
        cell.Dice = null;
        cell = gameData.cellsByCoords[CellData.Hash(tx, ty)];
        if (!cell.IsFree())
        {
            dice.MergingDice = cell.Dice;
            cell.Dice.Replaced = true;
            dice.lastMergedMoveNum = moveCount;
        }
        cell.Dice = dice;
    }

}
