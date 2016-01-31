using UnityEngine;
using System.Collections;

[ExecuteInEditMode] 
public class Dot : MonoBehaviour {

    public float killTime = 5;
    public bool kill = false;
    public float p1 = 0.5f;
    public float p2 = 0.5f;
    public Vector3 direction = new Vector3(0, 1, 0);
    public bool fadeout;
    public bool invert;

    public enum DotType
    {
        dotScale,
        dotCircle,
        dotDragon
    };
    public DotType dotType;

    public Vector3 GetWorldScale()
    {
        Vector3 worldScale = transform.localScale;
        Transform parent = transform.parent;

        while (parent != null)
        {
            worldScale = Vector3.Scale(worldScale, parent.localScale);
            parent = parent.parent;
        }
        return worldScale;
    }

    public void setParams(DotType type, bool fadeout, bool invert)
    {
        this.fadeout = fadeout;
        this.invert = invert;
        this.dotType = type;  
    }

    public void SendWorldScaleToShader()
    {
        Material m = GetComponent<Renderer>().sharedMaterial;
        Vector3 worldScale = GetWorldScale();
        m.SetVector("_Scale", new Vector4(worldScale.x, worldScale.y, worldScale.z, 0f));
        m.SetFloat("p1", p1);
        m.SetFloat("p2", p2);
        m.SetVector("_DirTrace", new Vector2(direction.x, direction.y));
        m.SetInt("_Fadeout", fadeout ? 1 : 0);
        m.SetInt("_Invert", invert ? 1 : 0);
        switch (dotType)
        {
            case DotType.dotScale: m.SetInt("_Mode", 0); break;
            case DotType.dotCircle: m.SetInt("_Mode", 1); break;
            case DotType.dotDragon: m.SetInt("_Mode", 2); break;
        }


    }

	// Use this for initialization
	void Start () {
        SendWorldScaleToShader();
    }

     // Update is called once per frame
    void OnPreRender() {
        SendWorldScaleToShader();
        
        
    }

    // Update is called once per frame
    void Update() {
        SendWorldScaleToShader();
        if (kill)
        {
            killTime -= Time.deltaTime;
            if (killTime < 0)
                GameObject.Destroy(transform.parent.gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SendWorldScaleToShader();
    }
}
