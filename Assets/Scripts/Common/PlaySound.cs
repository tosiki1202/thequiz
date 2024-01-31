using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class PlaySound : MonoBehaviour
{
    AudioSource audioSource;
 
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
 
    public void PlayStart()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
