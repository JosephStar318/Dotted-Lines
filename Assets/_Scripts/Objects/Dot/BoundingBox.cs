using UnityEngine;

[System.Serializable]
public class BoundingBox
{
    public Vector2 lowerLimit;
    public Vector2 upperLimit;

    public BoundingBox(Vector2 lowerLimit, Vector2 upperLimit)
    {
        this.lowerLimit = lowerLimit;
        this.upperLimit = upperLimit;
    }

    public Vector2 GetRandomPos()
    {
        float x = UnityEngine.Random.Range(lowerLimit.x, upperLimit.x);
        float y = UnityEngine.Random.Range(lowerLimit.y, upperLimit.y);
        return new Vector2(x, y);
    }
}