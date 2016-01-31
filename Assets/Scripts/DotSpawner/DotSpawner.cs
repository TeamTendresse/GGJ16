using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Mise a jour
public class DotSpawner : MonoBehaviour {

    public Transform prefabDot;
    Transform lastDot = null;

    public float distance = 1;
    bool started = false;
    
    Color CurrentColor;
    float timeNoMove = 0;
    bool hasMoved = false;

    bool fadeout = true;
    bool invert = false;
    Dot.DotType dotType = Dot.DotType.dotCircle;

    public enum ModeSpawner
    {
        silent,
        locked,
        unlocked
    };
    public ModeSpawner _Mode = ModeSpawner.silent;


    public void setParams(Dot.DotType type, bool fadeout, bool invert)
    {
        this.fadeout = fadeout;
        this.invert = invert;
        this.dotType = type;
    }

    Color getNewColor()
    {
        if (_Mode == ModeSpawner.locked || _Mode == ModeSpawner.silent)
        {
            return new Color(0, 0, 0, 0);
        }

        return new Color(Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), 0);
    }

    void setCamInvertColor(Color color)
    {
        Camera.main.backgroundColor = new Color(1,1,1,1) - color;
    }

    // Use this for initialization
    void Start () {
        CurrentColor = getNewColor();
        setCamInvertColor(CurrentColor);
        if (_Mode != ModeSpawner.silent)
        {
            Transform dot = GameObject.Instantiate(prefabDot, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
            Transform subDot = dot.GetChild(0);
            subDot.GetComponent<Dot>().setParams(Dot.DotType.dotScale, true, false);
            subDot.localScale = new Vector3(0, 0, 1);
            subDot.GetComponent<Renderer>().material.SetColor("_Color", CurrentColor);
            subDot.GetComponent<Dot>().SendWorldScaleToShader();
            subDot.GetComponent<Animator>().SetTrigger("First");
            lastDot = dot;
        }
    }

    public void setMode(ModeSpawner mode)
    {
            _Mode = mode;
            if (mode == ModeSpawner.silent)
            {
                killLastDot(); 
            }
        
            CurrentColor = getNewColor();
    }

    private IEnumerator makeSign(List<Vector2> points)
    {
        bool inverty = true;
        //Max and min
        float minx = Mathf.Infinity;
        float miny = Mathf.Infinity;
        float maxx = 0;
        float maxy = 0;

        for (int i = 0; i < points.Count; i++)
        {
            Vector3 pointPos = Camera.main.ScreenToWorldPoint(new Vector3(points[i].x, Screen.height-points[i].y, 1));

            if (pointPos.x > maxx) maxx = pointPos.x;
            if (pointPos.y > maxy) maxy = pointPos.y;
            if (pointPos.x < minx) minx = pointPos.x;
            if (pointPos.y < miny) miny = pointPos.y;

        }
        Vector3 offset = new Vector3();
        offset.x = (maxx + minx) / 2;
        offset.y = (maxy + miny) / 2;
        Debug.Log(offset);

        if (lastDot)
        {
            lastDot.GetChild(0).GetComponent<Animator>().SetTrigger("Dis");
            lastDot.GetChild(0).GetComponent<Animator>().speed = 10;
            lastDot.GetChild(0).GetComponent<Dot>().kill = true;
            started = true;
        }

        //Save it cause coroutine so changes
        Color currentColor = CurrentColor;
        Dot.DotType lDotType = Dot.DotType.dotScale;
        bool lInvert = this.invert;

        lastDot = null;
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 pointPos = Camera.main.ScreenToWorldPoint(new Vector3(points[i].x, Screen.height - points[i].y, 1));
            pointPos -= offset;

            

            if (lastDot == null || Vector3.Distance(pointPos, lastDot.position) > distance)
            {
                Transform dot = GameObject.Instantiate(prefabDot, pointPos, Quaternion.identity) as Transform;
                Transform subDot = dot.GetChild(0);
                subDot.localScale = new Vector3(0, 0, 1);
                subDot.GetComponent<Dot>().setParams(lDotType, lInvert, invert);
                subDot.GetComponent<Renderer>().material.SetColor("_Color", currentColor);
                subDot.GetComponent<Dot>().direction = lastDot == null ? new Vector3(1, 0, 0) : pointPos - lastDot.position;
                subDot.GetComponent<Dot>().SendWorldScaleToShader();
                subDot.GetComponent<Animator>().SetTrigger("Dis");
                lastDot = dot;
                yield return new WaitForSeconds(0.01f);
            }

            

        }

        //Au cas ou
        setCamInvertColor(CurrentColor);

    }

    public void showSign(List<Vector2> points)
    {
        StartCoroutine("makeSign", points);
    }

    void killLastDot()
    {
        if(lastDot)
        {
            lastDot.GetChild(0).GetComponent<Animator>().SetTrigger("Dis");
            lastDot.GetChild(0).GetComponent<Animator>().speed = 10;
            lastDot.GetChild(0).GetComponent<Dot>().kill = true;
        }
    }

    void  Update () {

        if (_Mode == ModeSpawner.silent)
        {
            killLastDot();
            return;
        }
            

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
   
        timeNoMove += Time.deltaTime;
        if((timeNoMove >= 2 && hasMoved) || lastDot == null)
        {
            hasMoved = false;
            CurrentColor = getNewColor();
            setCamInvertColor(CurrentColor);
            Transform dot = GameObject.Instantiate(prefabDot, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
            Transform subDot = dot.GetChild(0);
            subDot.localScale = new Vector3(0, 0, 1);
            subDot.GetComponent<Dot>().setParams(Dot.DotType.dotScale   , true, false);
            subDot.GetComponent<Renderer>().material.SetColor("_Color", CurrentColor);
            subDot.GetComponent<Dot>().direction = new Vector3(1,0,0);
            subDot.GetComponent<Dot>().SendWorldScaleToShader();
            subDot.GetComponent<Animator>().SetTrigger("First");
            lastDot = dot;
        }

        if ((Input.GetButton("Fire1") || Input.touchCount > 0) && 
            (lastDot == null || 
             Vector3.Distance(lastDot.position, pos) > distance * lastDot.GetChild(0).localScale.x))
        {
            if (!hasMoved && lastDot)
            {
                killLastDot();
                started = true;
            }

            hasMoved = true;
            timeNoMove = 0;

            Vector3 prevPos = new Vector3(0, 0, 0);
            if (lastDot)
                prevPos = lastDot.position;

            setCamInvertColor(CurrentColor);
            Transform dot = GameObject.Instantiate(prefabDot, pos,Quaternion.identity) as Transform;
            Transform subDot = dot.GetChild(0);
            subDot.GetComponent<Dot>().setParams(dotType, fadeout, invert);
            subDot.GetComponent<Animator>().SetTrigger("Dis");
            subDot.GetComponent<Renderer>().material.SetColor("_Color",CurrentColor);
            subDot.GetComponent<Dot>().direction = dot.position - prevPos;
            subDot.GetComponent<Dot>().kill = true;
            lastDot = dot;
        }

        if (!Input.GetButton("Fire1"))
        {
            CurrentColor = getNewColor();
            int typeInt = Random.Range(0, 3);
            Dot.DotType type = Dot.DotType.dotCircle;
            if (typeInt == 1) type = Dot.DotType.dotScale;
            if (typeInt == 2) type = Dot.DotType.dotDragon;
            if(_Mode == ModeSpawner.locked)
                setParams(Dot.DotType.dotScale, true, false);
            else
                setParams(type, Random.Range(0, 2) == 0, Random.Range(0, 2) == 0);
        }
    }
}
