using UnityEngine;
using TMPro;
using System;
using System.ComponentModel;

[CreateAssetMenu(menuName = "Setup/Dice Factory")]
public class DiceFactory : ScriptableObject
{
    public GameObject prefab;
    public enum DiceType { Hex, Round, Flip };
    public DiceType diceType;
    public Dice CreateDice(Transform transform, params object[] args)
    {
        Dice dice = null;
        switch (diceType)
        {
            case DiceType.Hex:
                dice = Main.CreateObjFromPrefab<HexDice>(prefab, transform, args);
                break;
            case DiceType.Round:
                dice = Main.CreateObjFromPrefab<RoundDice>(prefab, transform, args);
                break;
            case DiceType.Flip:
                dice = Main.CreateObjFromPrefab<FlipDice>(prefab, transform, args);
                break;
        }
        return dice;
    }
}
