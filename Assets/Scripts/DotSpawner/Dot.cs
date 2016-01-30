using UnityEngine;
using System.Collections;

[ExecuteInEditMode] 
public class Dot : MonoBehaviour {

    public float killTime = 5;
    public bool kill = false;

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

    public void SendWorldScaleToShader()
    {
        Vector3 worldScale = GetWorldScale();

        GetComponent<Renderer>().sharedMaterial.SetVector("_Scale",
                                    new Vector4(worldScale.x,
                                    worldScale.y,
                                    worldScale.z, 0f));
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
