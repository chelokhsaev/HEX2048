using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatingObject : ScriptableObject, IUpdating
{
    public virtual void Init(params object[] args) { }
    public virtual void Awake() { }
    public virtual void Start() { }
    public virtual void UpdateTime(float deltaTime) { }
}
