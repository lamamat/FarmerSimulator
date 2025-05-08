using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisPlayStatus : MonoBehaviour
{
    PlayerData player = new PlayerData();
    [SerializeField] TMP_Text Day;
    [SerializeField] TMP_Text MoneyCollect;
    [SerializeField] TMP_Text MoneyAmount;
    [SerializeField] TMP_Text MoneyDisplay;
    int PreviousMoneyAmount; // for display only day amout money 
    int TheDayMoney;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //when money add DEBUG
        {
            player.AddMoney(100);
        }

        if (Input.GetKeyDown(KeyCode.X)) //DEBUG when sleep
        {
            PreviousMoneyAmount = player.Money;
            TheDayMoney = 0;
            Time_Manager.instance.ToNextDay();
        }

        Day.text = "Day : " + Time_Manager.instance.currentDayCount.ToString();
        MoneyAmount.text = player.Money.ToString();
        MoneyDisplay.text = player.Money.ToString();

        TheDayMoney = player.Money - PreviousMoneyAmount;
        MoneyCollect.text = TheDayMoney.ToString();
    }
}
