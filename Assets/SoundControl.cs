using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    public AudioSource source;

    public AudioClip gotItemClip;

    public static SoundControl Instance;

    public void PlayGotSound()
    {
        source.PlayOneShot(gotItemClip);
    }
    
    private void Awake()
    {
        Instance = this;
    }
}
