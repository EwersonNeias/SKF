using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSourceMusicaDeFundo;
    public AudioClip[] musicasDeFundo;


    // Start is called before the first frame update
    void Start()
    {
        AudioClip musicasDeFundoDessaFase = musicasDeFundo[0];
        audioSourceMusicaDeFundo.clip = musicasDeFundoDessaFase;
        audioSourceMusicaDeFundo.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
