using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public int currentMoney = 750;

    void Start(){
        currentMoney = 750;
    }

    public void addFunds(int amount){
        currentMoney += amount;
        Debug.Log("Now at "+ currentMoney.ToString());
    }

    public int getCurrentFunds()
    {
        return currentMoney;
    }
}
