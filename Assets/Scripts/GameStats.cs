using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public int currentMoney = 750;

    AudioSource audioSource;

    public AudioClip audioClip;

    void Start(){
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
        currentMoney = 750;
    }

    public void addFunds(int amount){
        currentMoney += amount;
        //Debug.Log("Now at "+ currentMoney.ToString());
    }

    public int getCurrentFunds()
    {
        return currentMoney;
    }

    public void spendFunds(int amount){
        currentMoney -= amount;
        //Debug.Log("Now at "+ currentMoney.ToString());
    }
}
