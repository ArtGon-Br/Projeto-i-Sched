using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void DeleteChildren(this Transform transform)
    {
        foreach (Transform child in transform) Object.Destroy(child.gameObject);
    }

}
