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

    public void Init ()
    {
        dollarRecognizer = new DollarRecognizer(NumUnistrokes, NumPoints, SquareSize, Origin);
    }
	
	void Update ()
    {
        
	}

    public void addGesture (List<Vector2> points)
    {
        int num = dollarRecognizer.addGesture(Random.Range(1, 1000).ToString(), points);
    }

    public bool isSignOk (List<Vector2> points)
    {
        Result res = dollarRecognizer.recognize(points, false);
        if (res.score > minRecognitionScore)
        {
            Debug.Log("Sign recognized : " + res.name);
            return true;
        }
        else
        {
            Debug.Log("Sign not recognized");
            return false;
        }
    }
}
