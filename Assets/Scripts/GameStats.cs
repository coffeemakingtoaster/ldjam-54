using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public int currentMoney = 0;

    public void addFunds(int amount){
        currentMoney += amount;
        Debug.Log("Now at "+ currentMoney.ToString());
    }
}
