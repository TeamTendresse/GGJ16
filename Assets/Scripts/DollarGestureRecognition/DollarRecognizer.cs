using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DollarRecognizer
{
    private int NumUnistrokes;
    private int NumPoints;
    private float SquareSize;
    private Vector2 Origin;
    private float Diagonal;
    private float HalfDiagonal;
    private float AngleRange;
    private float AnglePrecision;
    private float Phi;

    public List<Unistroke> unistrokes { get; private set; }

	public DollarRecognizer (int NumUnistrokes, int NumPoints, float SquareSize, Vector2 Origin)
    {
        this.NumUnistrokes = NumUnistrokes;
        this.NumPoints = NumPoints;
        this.SquareSize = SquareSize;
        this.Origin = Origin;
        Diagonal = Mathf.Sqrt(SquareSize * SquareSize + SquareSize * SquareSize);
        HalfDiagonal = 0.5f * Diagonal;
        AngleRange = Mathf.Deg2Rad * 45.0f;
        AnglePrecision = Mathf.Deg2Rad * 2.0f;
        Phi = 0.5f * (-1.0f + Mathf.Sqrt(5.0f));

        unistrokes = new List<Unistroke>(NumUnistrokes);
    }

    public Result recognize (List<Vector2> points, bool useProtractor)
    {
        points = resample(points, NumPoints);
        float radians = IndicativeAngle(points);
        points = RotateBy(points, -radians);
        points = ScaleTo(points, SquareSize);
        points = TranslateTo(points, Origin);
        
        List<float> vector = Vectorize(points); // for Protractor

        float b = +Mathf.Infinity;
        int u = -1;
        for (int i = 0; i < unistrokes.Count; i++) // for each unistroke
        {
            float d;
            if (useProtractor) // for Protractor
            {
                d = OptimalCosineDistance(unistrokes[i].vector, vector);
            }
            else // Golden Section Search (original $1)
            {
                d = DistanceAtBestAngle(points, unistrokes[i], -AngleRange, +AngleRange, AnglePrecision);
            }
            if (d < b)
            {
                b = d; // best (least) distance
                u = i; // unistroke
            }
        }
        return (u == -1) ? new Result("No match.", 0.0f) : new Result(unistrokes[u].name, useProtractor ? 1.0f / b : 1.0f - b / HalfDiagonal);
    }

    public int addGesture (string name, List<Vector2> points)
    {
        unistrokes.Add(new Unistroke(name, points, NumPoints, SquareSize, Origin)); // append new unistroke
        int num = 0;
        for (int i = 0; i < unistrokes.Count; i++)
        {
            if (unistrokes[i].name == name)
                num++;
        }
        return num;
    }

    int deleteUserGestures ()
    {
        // clear any beyond the original set
        for (int i = NumUnistrokes; i < unistrokes.Count; i++)
        {
            unistrokes.RemoveAt(i);
        }
        return NumUnistrokes;
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
            else
            {
                D += d;
            }
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

    float OptimalCosineDistance(List<float> v1, List<float> v2) // for Protractor
    {
        float a = 0.0f;
        float b = 0.0f;
        for (int i = 0; i < v1.Count; i += 2)
        {
            a += v1[i] * v2[i] + v1[i + 1] * v2[i + 1];
            b += v1[i] * v2[i + 1] - v1[i + 1] * v2[i];
        }
        float angle = Mathf.Atan(b / a);
        return Mathf.Acos(a * Mathf.Cos(angle) + b * Mathf.Sin(angle));
    }

    float DistanceAtBestAngle(List<Vector2> points, Unistroke T, float a, float b, float threshold)
    {
        float x1 = Phi * a + (1.0f - Phi) * b;
        float f1 = DistanceAtAngle(points, T, x1);
        float x2 = (1.0f - Phi) * a + Phi * b;
        float f2 = DistanceAtAngle(points, T, x2);
        while (Mathf.Abs(b - a) > threshold)
        {
            if (f1 < f2)
            {
                b = x2;
                x2 = x1;
                f2 = f1;
                x1 = Phi * a + (1.0f - Phi) * b;
                f1 = DistanceAtAngle(points, T, x1);
            }
            else {
                a = x1;
                x1 = x2;
                f1 = f2;
                x2 = (1.0f - Phi) * a + Phi * b;
                f2 = DistanceAtAngle(points, T, x2);
            }
        }
        return Mathf.Min(f1, f2);
    }

    float DistanceAtAngle(List<Vector2> points, Unistroke T, float radians)
    {
        List<Vector2> newpoints = RotateBy(points, radians);
        return PathDistance(newpoints, T.points);
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

    float PathDistance(List<Vector2> pts1, List<Vector2> pts2)
    {
        float d = 0.0f;
        for (int i = 0; i < pts1.Count; i++) // assumes pts1.length == pts2.length
            d += Distance(pts1[i], pts2[i]);
        return d / pts1.Count;
    }

    float PathLength(List<Vector2> points)
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
