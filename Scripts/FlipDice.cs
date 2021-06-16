using UnityEngine;

public class FlipDice : RoundDice
{
    public override void Init(params object[] args)
    {
        base.Init(args);
        fRot0 = Quaternion.Euler(0f, 90f, 0f);
        fRot60 = Quaternion.Euler(-60f, 90f, 0f);
        fRot120 = Quaternion.Euler(-120f, 90f, 0f);
        fRot180 = Quaternion.Euler(180f, 90f, 0f);
        fRot240 = Quaternion.Euler(-240f, 90f, 0f);
        fRot300 = Quaternion.Euler(-300f, 90f, 0f);

        bRot0 = Quaternion.Euler(0f, -90f, 0f);
        bRot60 = Quaternion.Euler(-60f, -90f, 0f);
        bRot120 = Quaternion.Euler(-120f, -90f, 0f);
        bRot180 = Quaternion.Euler(-180f, -90f, 0f);
        bRot240 = Quaternion.Euler(-240f, -90f, 0f);
        bRot300 = Quaternion.Euler(-300f, -90f, 0f);
    }

}
