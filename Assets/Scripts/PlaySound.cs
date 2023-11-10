using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class PlaySound : MonoBehaviour
{
    public static AudioSource audioSource;
 
    void Start()
    {
        //DontDestroyOnLoad (this);
        audioSource = GetComponent<AudioSource>();
    }
 
    public void PlayStart()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
    public void SoundSliderOnValueChange(float newSliderValue)
	{
		audioSource.volume = newSliderValue;
	}
}
