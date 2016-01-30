using UnityEngine;
using System.Collections;

public class SoundFeerie : MonoBehaviour {
    public AudioClip intro ;
    public AudioClip haut ;
    public AudioClip bas ;
    public AudioClip gauche ;
    public AudioClip droite ;
    public AudioClip outro ;

    public Vector3 previous ;
    public float distance = 1 ;
    bool started = false ;
    float timeNoMove = 0 ;
    bool hasMoved = false ;
    AudioSource[] audio ;

    // Use this for initialization
    void Start () {
        previous = new Vector3(0f,0f,5f) ; 
        audio = GetComponents<AudioSource>() ;
        audio[0].clip = bas ;
        audio[0].volume = 0f ;
        audio[0].PlayDelayed(0.2f) ;
        audio[1].clip = haut ;
        audio[1].volume = 0f ;
        audio[1].PlayDelayed(0.4f) ;
        audio[2].clip = gauche ;
        audio[2].volume = 0f ;
        audio[2].PlayDelayed(0.6f) ;
        audio[3].clip = droite ;
        audio[3].volume = 0f ;
        audio[3].PlayDelayed(0.8f) ;
        audio[4].clip = intro ;
        audio[4].volume = 0f ;
    }

    // Update is called once per frame
    void  Update () {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        timeNoMove += Time.deltaTime;
        if(timeNoMove >= 2 && hasMoved){
            hasMoved = false;
        }

        if (Input.GetButton("Fire1") || Input.touchCount > 0){
            
            if (!hasMoved && previous.z == 5f){
                started = true;
                audio[4].volume = 0.7f ;
                audio[4].Play() ;
            }

            hasMoved = true;
            timeNoMove = 0;
            if(pos.y < 4f && pos.y > -4f){
                /* crossfade haut-bas */
                audio[0].volume = -(pos.y/4f)+1f ;
                audio[1].volume = (pos.y/4f)+1f ;
            }else if(pos.y < -4f){
                /* bas */
                audio[0].volume = 1f ;
                audio[1].volume = 0f ;
                
            }else if(pos.y >= 4f){
                /* haut */
                audio[0].volume = 0f ;
                audio[1].volume = 1f ;
                
            }

            if(pos.x < 4f && pos.x > -4f){
                /* crossfade gauche-droite */
                audio[2].volume = -(pos.x/4f)+1f ;
                audio[3].volume = (pos.x/4f)+1f ;
            }else if(pos.x < -4f){
                /* gauche */
                audio[2].volume = 1f ;
                audio[3].volume = 0f ;
                
            }else if(pos.x >= 4f){
                /* droite */
                audio[2].volume = 0f ;
                audio[3].volume = 1f ;   
            }

            previous = pos ;
        }

        if (!Input.GetButton("Fire1") && started){
            previous.z = 5f ;
            started = false ;
            audio[0].volume = 0f ;
            audio[1].volume = 0f ;
            audio[2].volume = 0f ;
            audio[3].volume = 0f ;
            audio[4].clip = outro ;
            audio[4].volume = 0.1f ;
            //audio[4].Play() ;
        }
    }
}
