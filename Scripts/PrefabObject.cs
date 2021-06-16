using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabObject : UpdatingObject
{
    [HideInInspector]
    private Transform _transform;
    public virtual Transform transform
    {
        set { _transform = value; }
        get { return _transform; }
    }
}
