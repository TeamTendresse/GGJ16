using UnityEngine;
using System.Collections;

public class DotSpawner : MonoBehaviour {

    public Transform prefabDotDisapear;
    public float distance = 1;
    bool started = false;
    Transform previous = null;
    Color CurrentColor;
    float timeNoMove = 0;
    bool hasMoved = false;
    // Use this for initialization
    void Start () {
        CurrentColor = new Color(Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), 0);
        previous = GameObject.Instantiate(prefabDotDisapear, new Vector3(0,0,0), Quaternion.identity) as Transform;
        previous.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color",
           CurrentColor);
        //previous.GetChild(0).GetComponent<Animator>().enabled = false;
        previous.GetChild(0).transform.localScale = new Vector3(0, 0, 1);
        previous.GetChild(0).GetComponent<Dot>().SendWorldScaleToShader();
        previous.GetChild(0).GetComponent<Animator>().SetTrigger("First");
        //
        //
    }

    // Update is called once per frame
    void   Update () {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        timeNoMove += Time.deltaTime;
        if(timeNoMove >= 2 && hasMoved)
        {
            hasMoved = false;
            CurrentColor = new Color(Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), 0);
            previous = GameObject.Instantiate(prefabDotDisapear, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
            previous.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color",
               CurrentColor);
            previous.GetChild(0).transform.localScale = new Vector3(0, 0, 1);
            previous.GetChild(0).GetComponent<Dot>().SendWorldScaleToShader();
            previous.GetChild(0).GetComponent<Animator>().SetTrigger("First");

            
        }

        if ((Input.GetButton("Fire1") || Input.touchCount > 0) && 
            (previous == null || 
             Vector3.Distance(previous.position, pos) > distance * previous.GetChild(0).localScale.x))

        {
            
            if (!hasMoved)
            {
                previous.GetChild(0).GetComponent<Animator>().SetTrigger("Dis");
                previous.GetChild(0).GetComponent<Animator>().speed = 10;
                previous.GetChild(0).GetComponent<Dot>().kill = true;
                started = true;
            }

            hasMoved = true;
            timeNoMove = 0;

            previous = GameObject.Instantiate(prefabDotDisapear, pos,Quaternion.identity) as Transform;
            previous.GetChild(0).GetComponent<Animator>().SetTrigger("Dis");
            previous.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color",
               CurrentColor);
            previous.GetChild(0).GetComponent<Dot>().kill = true;
        }

        if (!Input.GetButton("Fire1"))
        {

            CurrentColor = new Color(Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f),0);

        }

    }
}
