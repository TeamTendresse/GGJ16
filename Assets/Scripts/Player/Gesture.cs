using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Gesture
{
    public List<Vector2> points;

    public Gesture()
    {
        points = new List<Vector2>();
    }

    public Gesture (List<Vector2> points)
    {
        this.points = points;
    }
}
