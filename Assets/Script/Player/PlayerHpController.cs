using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class PlayerHpController : MonoBehaviourPunCallbacks
{
    public Slider HpController;

    private float maxHp = 100;

    [SerializeField]
    private GameObject NomalSnowBall;
    [SerializeField]
    private GameObject RadSnowBall;
    [SerializeField]
    private GameObject BlueSnowBall;
    Animator ani;
    PlayerController placon;
    GameManagerMap1 gm;
    string gameSceneName;
    private float InputDamage;
    // Start is called before the first frame update
    private void Awake()
    {
        ani = GetComponent<Animator>();
        placon = GetComponent<PlayerController>();
        SceneCheck();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == true)
        {
            if (other.tag == "SnowBall")
            {
               
                Destroy(other.gameObject);
                photonView.RPC("CheckHp", RpcTarget.AllBuffered, 0.1f);
    
            }
            if (other.tag == "RedSnowBall")
            {
              
                Destroy(other.gameObject);
                photonView.RPC("CheckHp", RpcTarget.AllBuffered, 0.12f);
            }
            if (other.tag == "BlueSnowBall")
            {
                
                Destroy(other.gameObject); 
                photonView.RPC("CheckHp", RpcTarget.AllBuffered,0.15f);
            }

        }
    }
    private void SceneCheck() 
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            if (photonView.IsMine==true &&PhotonNetwork.IsConnected == true)
            {
                photonView.RPC("DestoryPlayerSlider",RpcTarget.AllBuffered);
            }
            DestoryPlayerSlider();
        }
        if (SceneManager.GetActiveScene().name == "Game1")
        {
            gameSceneName = SceneManager.GetActiveScene().name;
            gm = GameObject.Find("GameManager").GetComponent<GameManagerMap1>();
        } 
    }
    [PunRPC]
    private void DestoryPlayerSlider()
    {
        HpController.gameObject.SetActive(false);
    }
    [PunRPC]
    private void CheckHp(float Damage)
    {
        float max = HpController.value;
        HpController.value = max - 0.15f;
    }
    private void HpCheck()
    {
        if (HpController.value <= 0)
        {
            if (gameSceneName == "Game1" && placon.isDead == false)
            {
                gm.playerKillCount++;
            }
            placon.isDead = true;
            ani.SetBool("Die", true);
        }
    }
    private void Update()
    {
        SceneCheck();
        HpCheck();
    }
}
