using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float minGestureLength;

    private GestureRecognitionController gestureRecognitionController;
    private DotSpawner dotSpawner;

    private List<Vector2> points;
    private bool isDown;
    private Vector2 lastMousePosition;

    void Start ()
    {
        gestureRecognitionController = GestureRecognitionController.instance;
        gestureRecognitionController.Init();
        dotSpawner = GameObject.FindObjectOfType<DotSpawner>();

        points = new List<Vector2>();
        isDown = false;
        lastMousePosition = Vector2.zero;
    }

    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (points.Count >= minGestureLength)
            {
                gestureRecognitionController.addGesture(points);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            points.Clear();
            isDown = true;
        }

        if (isDown)
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.y = Screen.height - mousePosition.y;
            if (Vector2.Distance(mousePosition, lastMousePosition) != 0)
            {
                points.Add(mousePosition);
                lastMousePosition = mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDown)
            {
                isDown = false;
                if (points.Count >= minGestureLength)
                {
                    if (gestureRecognitionController.isSignOk(points))
                    {
                        showSign();
                    }
                }
            }
        }
#endif
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    points.Clear();
                    isDown = true;
                    break;
                case TouchPhase.Moved:
                    if (isDown)
                    {
                        Vector2 mousePosition = Input.mousePosition;
                        mousePosition.y = Screen.height - mousePosition.y;
                        if (Vector2.Distance(mousePosition, lastMousePosition) != 0)
                        {
                            points.Add(mousePosition);
                            lastMousePosition = mousePosition;
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    if (isDown)
                    {
                        isDown = false;
                        if (points.Count >= minGestureLength)
                        {
                            if (gestureRecognitionController.isSignOk(points))
                            {
                                showSign();
                            }
                        }
                    }
                    break;
            }
        }
#endif
    }

    public bool hasDoneSigned ()
    {
        return false;
    }

    private void showSign ()
    {
        
    }
}
