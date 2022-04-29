using System;
using UnityEngine;

public class CopyCat : MonoBehaviour
{
    private Transform OriginalUniverse;

    public void SetOriginal(Transform originalUniverse)
    {
        OriginalUniverse = originalUniverse;
    }
    private void FixedUpdate()
    {
        transform.position = OriginalUniverse.position;
        transform.rotation = OriginalUniverse.rotation;
    }
}