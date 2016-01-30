using UnityEngine;
using System.Collections;

public class SoundFeerie : MonoBehaviour {
    public AudioClip[] sounds ;
    public AudioSource[] audio ;
    public bool randSound = false ;
    float zoneH ;
    float zoneV ;
    int zone ;

    void Start () {
        int height = Screen.height ;
        int width = Screen.width ;
        zoneH = width/3 ;
        zoneV = height/3 ;

        for(int i = 0; i<=8; i++){ 
            audio[i] = this.gameObject.AddComponent<AudioSource>() ;
            audio[i].clip = sounds[i] ;
            audio[i].loop = false ;
            audio[i].playOnAwake = false ;
        }
    }

    // Update is called once per frame
    void  Update () {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        if (Input.GetButton("Fire1") || Input.touchCount > 0){
            int zoneX = (int)(Input.mousePosition.x/zoneH) ;
            int zoneY = (int)(Input.mousePosition.y/zoneV) ;
            
            if(zoneX>2)zoneX=2;
            if(zoneY>2)zoneY=2;
            zone = (int)zoneX+6-(3*zoneY) ;
            Debug.Log(zone) ;
            if(!audio[zone].isPlaying){
                audio[zone].loop = false ;
                if(randSound){
                    int tirage = (int)Random.Range(0, sounds.Length);
                    audio[zone].clip = sounds[tirage] ;
                }
                audio[zone].Play();
            }
        }
    }
}
