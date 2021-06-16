using UnityEngine;

public class CellData
{
    public readonly int x;
    public readonly int y;

    public CellData(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public delegate void DataHandler(CellData data);
    public event DataHandler Change;

    private DiceData _dice;
    public DiceData Dice {
        get { return _dice; }
        set
        {
            if (_dice == value)
                return;
            _dice = value;
            Change?.Invoke(this);
            //Utils.SetColor(tmpTransform, _dice == null ? Color.blue : Color.green);
        }
    }

    public bool IsFree()
    {
        return _dice == null;
    }

    public int Hash()
    {
        return CellData.Hash(x, y);
    }

    public static int Hash(int x, int y)
    {
        return y * 1024 + x;
    }


    public Transform debugTransform;

}
