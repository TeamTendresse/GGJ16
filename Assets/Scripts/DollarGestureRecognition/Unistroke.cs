using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unistroke
{
    private DollarRecognizer dollarRecognizer;

    public string name { get; private set; }
    public List<Vector2> points { get; private set; }
    public List<float> vector { get; private set; }

    public Unistroke (string name, List<Vector2> points, int NumPoints, float SquareSize, Vector2 Origin)
    {
        this.name = name;
        this.points = resample(points, NumPoints);
        var radians = IndicativeAngle(this.points);
        this.points = RotateBy(this.points, -radians);
        this.points = ScaleTo(this.points, SquareSize);
        this.points = TranslateTo(this.points, Origin);
        this.vector = Vectorize(this.points); // for Protractor
    }

    List<Vector2> resample(List<Vector2> points, int n)
    {
        float I = PathLength(points) / (n - 1); // interval length
        float D = 0.0f;
        List<Vector2> newpoints = new List<Vector2>();
        newpoints.Add(points[0]);
        for (int i = 1; i < points.Count; i++)
        {
            float d = Distance(points[i - 1], points[i]);
            if ((D + d) >= I)
            {
                float qx = points[i - 1].x + ((I - D) / d) * (points[i].x - points[i - 1].x);
                float qy = points[i - 1].y + ((I - D) / d) * (points[i].y - points[i - 1].y);
                Vector2 q = new Vector2(qx, qy);
                newpoints.Add(q); // append new point 'q'
                points.Insert(i, q); // insert 'q' at position i in points s.t. 'q' will be the next i
                D = 0.0f;
            }
            else D += d;
        }
        if (newpoints.Count == n - 1) // somtimes we fall a rounding-error short of adding the last point, so add it if so
        {
            newpoints.Add(new Vector2(points[points.Count - 1].x, points[points.Count - 1].y));
        }
        return newpoints;
    }

    float IndicativeAngle(List<Vector2> points)
    {
        Vector2 c = Centroid(points);
        return Mathf.Atan2(c.y - points[0].y, c.x - points[0].x);
    }

    List<Vector2> RotateBy(List<Vector2> points, float radians) // rotates points around centroid
    {
        Vector2 c = Centroid(points);
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        List<Vector2> newpoints = new List<Vector2>();
        for (int i = 0; i < points.Count; i++)
        {
            float qx = (points[i].x - c.x) * cos - (points[i].y - c.y) * sin + c.x;
            float qy = (points[i].x - c.x) * sin + (points[i].y - c.y) * cos + c.y;
            newpoints.Add(new Vector2(qx, qy));
        }
        return newpoints;
    }

    List<Vector2> ScaleTo(List<Vector2> points, float size) // non-uniform scale; assumes 2D gestures (i.e., no lines)
    {
        Rectangle B = BoundingBox(points);
        List<Vector2> newpoints = new List<Vector2>();
        for (int i = 0; i < points.Count; i++)
        {
            float qx = points[i].x * (size / B.width);
            float qy = points[i].y * (size / B.height);
            newpoints.Add(new Vector2(qx, qy));
        }
        return newpoints;
    }

    List<Vector2> TranslateTo(List<Vector2> points, Vector2 pt) // translates points' centroid
    {
        Vector2 c = Centroid(points);
        List<Vector2> newpoints = new List<Vector2>();
        for (int i = 0; i < points.Count; i++)
        {
            float qx = points[i].x + pt.x - c.x;
            float qy = points[i].y + pt.y - c.y;
            newpoints.Add(new Vector2(qx, qy));
        }
        return newpoints;
    }

    List<float> Vectorize(List<Vector2> points) // for Protractor
    {
        float sum = 0.0f;
        List<float> vector = new List<float>();
        for (int i = 0; i < points.Count; i++)
        {
            vector.Add(points[i].x);
            vector.Add(points[i].y);
            sum += points[i].x * points[i].x + points[i].y * points[i].y;
        }
        float magnitude = Mathf.Sqrt(sum);
        for (int i = 0; i < vector.Count; i++)
            vector[i] /= magnitude;
        return vector;
    }

    Vector2 Centroid(List<Vector2> points)
    {
        float x = 0.0f, y = 0.0f;
        for (int i = 0; i < points.Count; i++)
        {
            x += points[i].x;
            y += points[i].y;
        }
        x /= points.Count;
        y /= points.Count;
        return new Vector2(x, y);
    }

    Rectangle BoundingBox(List<Vector2> points)
    {
        float minX = +Mathf.Infinity, maxX = -Mathf.Infinity, minY = +Mathf.Infinity, maxY = -Mathf.Infinity;
        for (int i = 0; i < points.Count; i++)
        {
            minX = Mathf.Min(minX, points[i].x);
            minY = Mathf.Min(minY, points[i].y);
            maxX = Mathf.Max(maxX, points[i].x);
            maxY = Mathf.Max(maxY, points[i].y);
        }
        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }

    float PathDistance (List<Vector2> pts1, List<Vector2> pts2)
    {
        float d = 0.0f;
        for (int i = 0; i < pts1.Count; i++) // assumes pts1.length == pts2.length
            d += Distance(pts1[i], pts2[i]);
        return d / pts1.Count;
    }

    float PathLength (List<Vector2> points)
    {
        float d = 0.0f;
        for (int i = 1; i < points.Count; i++)
            d += Distance(points[i - 1], points[i]);
        return d;
    }

    float Distance(Vector2 p1, Vector2 p2)
    {
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }
}
