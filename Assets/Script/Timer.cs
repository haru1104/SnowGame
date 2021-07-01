using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Timer : MonoBehaviourPunCallbacks ,IPunObservable
{
    private Text timer;
    private int count = 120;
    public bool zeroTime = false;
  
    private GameManeger gm;
    // Start is called before the first frame update

    private void Start()
    {
        timer = GetComponentInChildren<Text>();
        if (PhotonNetwork.IsMasterClient == true)
        {
            StartCoroutine("SetTimer");
        }
        gm = GameObject.Find("GameManeger").GetComponent<GameManeger>() ;
      
    }
    private void Update()
    {
        PrintTime();
        Gameover();
    }
    private void Gameover()
    {
        if (zeroTime == true)
        {
            gm.GameOverScreen.SetActive(true);
        }
    }
    void PrintTime()
    {
        timer.text = "Time : " + count;
    }
    IEnumerator SetTimer()
    {
        Debug.Log("count -- ");
        yield return new WaitForSeconds(1f);
        count--;
        
        StartCoroutine("SetTimer");
        if (count <= 0)
        {
            zeroTime = true;
            GameoverDis();
           
            StopCoroutine("SetCount");
        }
    }

    private void GameoverDis()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("bot");

        for (int i = 0; i < go.Length ; i++)
        {
            Destroy(go[i]);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(count);
        }
        else
        {
            count = (int)stream.ReceiveNext();
        }
    }
}
