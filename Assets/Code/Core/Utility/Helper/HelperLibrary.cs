using UnityEngine;

public class HelperLibrary
{
    private static float epsilon = 0.0001f;

    public static bool ApproximateEqual(Vector3 a, Vector3 b)
    {
        return (a - b).sqrMagnitude < epsilon * epsilon;
    }

    public static bool ApproximateEqual(Vector2 a, Vector2 b)
    {
        return (a - b).sqrMagnitude < epsilon * epsilon;
    }
}
