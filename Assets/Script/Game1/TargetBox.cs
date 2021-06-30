using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class TargetBox : MonoBehaviourPunCallbacks
{
    private Slider boxSlider;
    private float damage;
    private bool gameOver;
    private GameManagerMap1 gm;
    private GameObject[] bot;


    private void Start()
    {
        boxSlider = GetComponentInChildren<Slider>();
        gm = GameObject.Find("GameManager").GetComponent<GameManagerMap1>();
        damage = 0.05f;
    }
    private void Update()
    {
        ChackSlider();
       
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Axe")
        {
            photonView.RPC("HpRpc",RpcTarget.AllBuffered);
        }
    }
    private void ChackSlider()
    {
        if (boxSlider.value <= 0)
        {
            bot = GameObject.FindGameObjectsWithTag("bot");
            for (int i = 0; i < bot.Length; i++)
            {
               PhotonNetwork.Destroy(bot[i]);
            }
            gameObject.SetActive(false);
            gameOver = true;
            gm.gameOverScene.SetActive(true);
        }
    }

   [PunRPC]
   private void HpRpc()
    {
        boxSlider.value -= damage;
    }
}
