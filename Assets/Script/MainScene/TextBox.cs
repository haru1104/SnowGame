using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class TextBox : MonoBehaviour
{
    public Text text;

    public void Start()
    {
        PlayerPrefs.SetInt("Coin", 0);
        PlayerPrefs.SetInt("Cash", 0);
    }
    public void ButtonClick()
    {
        Debug.Log(text.text);
        PlayerPrefs.SetString("PlayerName", text.text);
        PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
       

    }
}
