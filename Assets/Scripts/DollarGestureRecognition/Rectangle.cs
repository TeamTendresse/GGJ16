using UnityEngine;
using System.Collections;

public class Rectangle {

    public float x { get; private set; }
    public float y { get; private set; }
    public float width { get; private set; }
    public float height { get; private set; }

    public Rectangle(float x, float y, float width, float height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
}
