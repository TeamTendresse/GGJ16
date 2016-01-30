using UnityEngine;
using System.Collections;

public class SoundFeerie : MonoBehaviour {
    public AudioClip[] sounds ;
    public AudioClip intro ;

    public Vector3 previous ;
    public float distance = 1 ;
    bool started = false ;
    public AudioSource[] audio ;
    public bool randSound = false ;
    float zoneH ;
    float zoneV ;
    int zone = -1 ;

    void Start () {
        int height = Screen.height ;
        int width = Screen.width ;
        zoneH = width/3 ;
        zoneV = height/3 ;
        previous = new Vector3(0f,0f,5f) ; 
        for(int i = 0; i<9; i++){ 
            audio[i] = this.gameObject.AddComponent<AudioSource>() ;
            //audio[i] = new AudioSource() ;
            audio[i].clip = sounds[i] ;
            audio[i].loop = false ;
            audio[i].playOnAwake = false ;
        }

        audio[9] = this.gameObject.AddComponent<AudioSource>() ;
        audio[9].clip = intro ;
        audio[9].loop = false ;
        audio[9].playOnAwake = false ;
    }

    // Update is called once per frame
    void  Update () {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        if (Input.GetButton("Fire1") || Input.touchCount > 0){
            
            if (previous.z == 5f){
                started = true;
                if(!audio[9].isPlaying){
                    audio[9].clip = intro ;
                    audio[9].volume = 0.7f ;
                    audio[9].Play() ;
                }
            }
            
            int zoneX = (int)(Input.mousePosition.x/zoneH) ;
            int zoneY = (int)(Input.mousePosition.y/zoneV) ;

            if(zoneY == 2){
                zone = (int)zoneX ;
            }else if(zoneY == 1){
                 zone = (int)zoneX+3 ;
            }else{
                zone = (int)zoneX+6 ;
            }
            if(!audio[zone].isPlaying){
                audio[zone].loop = false ;
                if(randSound){
                    int tirage = (int)Random.Range(0, sounds.Length);
                    audio[zone].clip = sounds[tirage] ;
                }
                audio[zone].Play();
            }
            previous = pos ;
        }

        if (!Input.GetButton("Fire1")){
            previous.z = 5f ;
            started = false ;
        }
    }
}
