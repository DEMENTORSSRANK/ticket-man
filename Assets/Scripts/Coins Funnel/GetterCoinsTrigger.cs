using System;
using System.Collections;
using System.Collections.Generic;
using Element;
using UnityEngine;

public class GetterCoinsTrigger : MonoBehaviour
{
    public ParticleSystem effectGot;
    
    private int currentScore;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin"))
            return;

        var coinController = other.GetComponent<Coin>();

        if (!coinController.IsInFly)
            return;
        
        currentScore++;
        
        Destroy(coinController.gameObject);

        Api.Instance().StartCoroutine(Api.Instance().ReportGame(currentScore.ToString()));

        if (effectGot.isPlaying)
            return;
        
        SoundControl.Instance.PlayGotSound();
        
        effectGot.Play();
    }
}
