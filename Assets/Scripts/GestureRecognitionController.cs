using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestureRecognitionController : MonoBehaviour
{
    private static GestureRecognitionController _instance;
    
    public float minRecognitionScore;
    public int NumUnistrokes;
    public int NumPoints;
    public float SquareSize;
    public Vector2 Origin;

    private DollarRecognizer dollarRecognizer;

    protected GestureRecognitionController () { }

    public static GestureRecognitionController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType< GestureRecognitionController>();
            }
            return _instance;
        }
    }

    void Start ()
    {
        
	}

    public void Init (List<Gesture> gestures)
    {
        dollarRecognizer = new DollarRecognizer(NumUnistrokes, NumPoints, SquareSize, Origin);

        for (int i = 0; i < gestures.Count; i++)
        {
            dollarRecognizer.addGesture(i.ToString(), gestures[i].points);
        }
    }
	
	void Update ()
    {
        
	}

    public void addGesture (int id, List<Vector2> points)
    {
        dollarRecognizer.addGesture(id.ToString(), points);
    }

    public bool isSignOk (List<Vector2> points, out Result res)
    {
        res = dollarRecognizer.recognize(points, false);
        if (res.score > minRecognitionScore)
        {
            Debug.Log("Sign recognized : " + res.name);
            return true;
        }
        else
        {
            Debug.Log("Sign not recognized : " + res.score);
            return false;
        }
    }
}
