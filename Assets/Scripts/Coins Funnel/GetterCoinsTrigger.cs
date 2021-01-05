using System;
using System.Collections;
using System.Collections.Generic;
using Coins_Funnel;
using Element;
using UnityEngine;
using UnityEngine.UI;

public class GetterCoinsTrigger : MonoBehaviour
{
    [SerializeField] private int countGotCoins;

    [SerializeField] private int currentScore;

    [SerializeField] private ParticleSystem effectGot;

    [SerializeField] private Coin getCoin;

    [SerializeField] private float timeSecondsShow = 2;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin"))
            return;

        var coinController = other.GetComponent<Coin>();

        if (!coinController.IsInFly)
            return;
        
        countGotCoins++;

        currentScore += coinController.Coins;

        StartCoroutine(ShowGotCoin(coinController.Index));
        
        Destroy(coinController.gameObject);

        Api.Instance().StartCoroutine(Api.Instance().ReportGame(countGotCoins.ToString()));

        if (effectGot.isPlaying)
            return;
        
        SoundControl.Instance.PlayGotSound();
        
        effectGot.Play();
    }

    private IEnumerator ShowGotCoin(int coins)
    {
        var index = SetDenominations.Instance.GetIndexOfDenomination(coins);
        
        getCoin.SetDenomination(coins);
        
        getCoin.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(timeSecondsShow);
        
        getCoin.gameObject.SetActive(false);
    }
}
