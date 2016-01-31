using UnityEngine;
using System.Collections;

public class SoundDotFeerie : MonoBehaviour {
    public AudioClip[] sounds ;
    public AudioSource audio ;

    void Start () {
        audio = this.gameObject.AddComponent<AudioSource>() ;
        //audio[i] = new AudioSource() ;
       int tirage = (int)Random.Range(0, sounds.Length);
        //Debug.Log(tirage) ;
        audio.clip = sounds[tirage] ;
        audio.loop = false ;
        audio.Play() ;
    }
}
