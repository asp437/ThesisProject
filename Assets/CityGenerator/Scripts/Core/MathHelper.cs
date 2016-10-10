using UnityEngine;
using System.Collections;

public abstract class MathHelper {
    public static float lerp(float a, float b, float t) {
        return a + (b - a) * t;
    }

    // Quintic filter function
    public static float quintic(float t) {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    // Cosine filter function
    public static float cosine(float t) {
        return (1 - Mathf.Cos(t * Mathf.PI)) / 2;
    }

    // Cubic filter function
    public static float cubic(float t) {
        return -2 * t * t * t + 3 * t * t;
    }
}
