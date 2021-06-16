public class DiceData
{

    public DiceData(int x, int y, int value)
    {
        _x = x;
        _y = y;
        _value = value;
    }

    public delegate void DataHandler();
    public event DataHandler Change;

    private int _x;
    public int X
    {
        get { return _x; }
        set
        {
            if (value == _x)
                return;
            _x = value;
            Change?.Invoke();
        }
    }

    private int _y;
    public int Y
    {
        get { return _y; }
        set
        {
            if (value == _y)
                return;
            _y = value;
            Change?.Invoke();
        }
    }

    private int _value;
    public int Value
    {
        get { return _value; }
        set
        {
            if (value == _value)
                return;
            _value = value;
            Change?.Invoke();
        }
    }

    private bool _replaced = false;
    public bool Replaced
    {
        get { return _replaced; }
        set
        {
            if (value == _replaced)
                return;
            _replaced = value;
            Change?.Invoke();
        }
    }

    private bool _destroyed = false;
    public bool Destroyed
    {
        get { return _destroyed; }
        set
        {
            if (value == _destroyed)
                return;
            _destroyed = value;
            Change?.Invoke();
        }
    }

    private DiceData _mergingDice;
    public DiceData MergingDice
    {
        get { return _mergingDice; }
        set
        {
            if (value == _mergingDice)
                return;
            if (_mergingDice != null)
                _mergingDice.Destroyed = true;
            _mergingDice = value;
            Change?.Invoke();
        }
    }

    public int lastMergedMoveNum = 0;

    public DiceData Clone()
    {
        return new DiceData(X, Y, Value);
    }


}
