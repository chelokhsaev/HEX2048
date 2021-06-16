using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void SetColor(Transform transform, Color color)
    {
        MeshRenderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
            renderer.material.color = color;
    }
}
