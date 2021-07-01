using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    private Text coin;
    private Text cash;


    // Start is called before the first frame update
    void Start()
    {
        Find();
        Printing();
    }
   private void Find()
    {
        coin = GameObject.Find("Coin").GetComponentInChildren<Text>();
        cash = GameObject.Find("Cash").GetComponentInChildren<Text>();

    }
    private void Printing()
    {
        coin.text = PlayerPrefs.GetInt("Coin")+"";
        cash.text = PlayerPrefs.GetInt("Cash").ToString();
    }
}
