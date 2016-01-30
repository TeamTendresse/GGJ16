using UnityEngine;
using System.Collections;

public class SoundFeerie : MonoBehaviour {
    public AudioClip intro ;
    public AudioClip haut ;
    public AudioClip bas ;
    public AudioClip gauche ;
    public AudioClip droite ;
    //public AudioClip validation ;
    //public Transform prefabDotDisapear;
    public Vector3 previous ;
    public float distance = 1;
    bool started = false;
    //Transform previous = null;
    //Color CurrentColor;
    float timeNoMove = 0;
    bool hasMoved = false;
    AudioSource[] audio ;
    // Use this for initialization
    void Start () {
        previous = new Vector3(0f,0f,5f); 
        audio = GetComponents<AudioSource>() ;
        audio[0].clip = bas ;
        audio[0].volume = 0f;
        audio[0].Play() ;
        audio[1].clip = haut ;
        audio[1].volume = 0f;
        audio[1].Play() ;
        audio[2].clip = gauche ;
        audio[2].volume = 0f;
        audio[2].Play() ;
        audio[3].clip = droite ;
        audio[3].volume = 0f;
        audio[3].Play() ;
        audio[4].clip = intro ;
        audio[4].volume = 0f;
        /*
        CurrentColor = new Color(Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), 0);
        previous = GameObject.Instantiate(prefabDotDisapear, new Vector3(0,0,0), Quaternion.identity) as Transform;
        previous.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color",
           CurrentColor);
        //previous.GetChild(0).GetComponent<Animator>().enabled = false;
        previous.GetChild(0).transform.localScale = new Vector3(0, 0, 1);
        previous.GetChild(0).GetComponent<Dot>().SendWorldScaleToShader();
        previous.GetChild(0).GetComponent<Animator>().SetTrigger("First");*/
        //
        //
    }

    // Update is called once per frame
    void  Update () {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        timeNoMove += Time.deltaTime;
        if(timeNoMove >= 2 && hasMoved){
            hasMoved = false;
            //CurrentColor = new Color(Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), 0);
            /*previous = GameObject.Instantiate(prefabDotDisapear, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
            previous.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color",
               CurrentColor);
            previous.GetChild(0).transform.localScale = new Vector3(0, 0, 1);
            previous.GetChild(0).GetComponent<Dot>().SendWorldScaleToShader();
            previous.GetChild(0).GetComponent<Animator>().SetTrigger("First");
            */
            
        }

        if (Input.GetButton("Fire1") || Input.touchCount > 0)
        {
            
            if (!hasMoved && previous.z == 5f)
            {
                /*previous.GetChild(0).GetComponent<Animator>().SetTrigger("Dis");
                previous.GetChild(0).GetComponent<Animator>().speed = 10;
                previous.GetChild(0).GetComponent<Dot>().kill = true;*/
                started = true;
                //audio[0].clip = intro ;
                audio[4].volume = 0.7f ;
                audio[4].Play() ;
                //Debug.Log("startMove") ;
            }
            //Debug.Log("toc") ;
            //Debug.Log(pos) ;
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
           /* }else if(pos.x < 0f && pos.y < 0f){
                Debug.Log("bas gauche") ;
                audio.clip = bas_gauche ;
                audio.Play() ;
            }else if(pos.x > 0f && pos.y < 0f){
                audio.clip = bas_droite ;
                audio.Play() ;
                Debug.Log("bas droite") ;
            }else if(pos.x < 0f && pos.y > 0f){
                audio.clip = haut_gauche ;
                audio.Play() ;
                Debug.Log("haut gauche") ;
            }else if(pos.x > 0f && pos.y > 0f){
                audio.clip = haut_droite ;
                audio.Play() ;
                Debug.Log("haut droite") ;
            }*/
            previous = pos ;
        }

        if (!Input.GetButton("Fire1") && started)
        {
            previous.z = 5f ;
            //Debug.Log("endMove") ;
            started = false ;
        }

    }
}
