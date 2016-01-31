using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class checkLoudness : MonoBehaviour {
	public Color color_silent = Color.blue;
	public Color color_silent2 = Color.white;
	public Color color_noise = Color.blue;
	public Color color_noise2 = Color.blue;
    private Color color1 ;
    private Color color2 ;
    public float duration = 3.0F;
	Camera camera ;
	public Text count ;
	private Player player;

	float objectiveTime ;

	// Use this for initialization
	void Start () {
	 	camera = GetComponent<Camera>();
    	camera.clearFlags = CameraClearFlags.SolidColor;
    	color1 = color_silent ;
    	color2 = color_silent2 ;
    	objectiveTime = Time.time + 10f*1f + 1f ;
    	player = GameObject.FindObjectOfType<Player>();
	}
	
	int status = 0 ;
	bool toNoise = false ;
	bool toSilent = false ;
	float lastT ;

	// Update is called once per frame
	void Update () {
		int tRest = (int)(objectiveTime-Time.time) ;
		count.text = tRest.ToString() ;
		float t = Mathf.PingPong(Time.time, duration) / duration;
		if(t-lastT > 0f && t>=0.99f)t=1f;
		if(t-lastT < 0f && t<=0.01f)t=0f;
		
		if(MicInput.avgLoudness != -1f && !toSilent && !toNoise){
			//Debug.Log(MicInput.avgLoudness) ;
			if(MicInput.avgLoudness >= 0.02f){
				if(status == 0){
					//Debug.Log("to noise") ;
					toNoise = true ;
					status = 1 ;
				}else{
		        	//Debug.Log("noise") ;
		        	color1 = color_noise ;
		        	color2 = color_noise2 ;
		        }
	        }else{
	        	if(status == 1){
	        		//Debug.Log("to silence") ;
					toSilent = true ;
					status = 0 ;
				}else{
		        	//Debug.Log("silence") ;
		        	color1 = color_silent ;
		        	color2 = color_silent2 ;
		        }
	        }
		}
		lastT = t ;
		//Debug.Log(t) ;
        camera.backgroundColor = Color.Lerp(color1, color2, t) ;
        if(toSilent && t == 1f){
        	color1 = color_silent ;
        }else if(toSilent && t == 0f){
        	color2 = color_silent2 ;
        }else if(toNoise && t == 1f){
        	color1 = color_noise ;
        }else if(toNoise && t == 0f){
        	color2 = color_noise2 ;
        }

        if(toSilent && (camera.backgroundColor == color_silent || camera.backgroundColor == color_silent2)){
        	toSilent = false ;
        }else if(toNoise && (camera.backgroundColor == color_noise || camera.backgroundColor == color_noise2)){
			toNoise = false ;
		}

		if(Time.time >= objectiveTime){
			player.showSign(player.getGesturePoints(0)) ;
			//Invoke("Application.LoadLevel("Game")",5f);
		}
	}
}
