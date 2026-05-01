using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class EffectPositions
{
    public static Dictionary<string, Vector3> StatEffectPositions = new Dictionary<string, Vector3>()
    {
        {"AntihealVFX", new Vector3(70f, -60f, 0f) },
        {"AwestruckVFX", new Vector3(0f, 35f, 0f) },
        {"FrostbiteVFX", new Vector3(0f, 0f, 0f) },
        {"OnFireVFX", new Vector3(0f, -15f, 0f) },
        {"PoisonVFX", new Vector3(67f, 67f, 0f) },
        {"RegenVFX", new Vector3(0f, 5f, 0f) },
    };
}