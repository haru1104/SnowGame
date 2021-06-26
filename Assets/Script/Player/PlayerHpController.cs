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
        float max = HpController.value;
        if (other.tag == "SnowBall")
        {
            HpController.value = max - 0.10f;
            Destroy(other.gameObject);

        }
        if (other.tag == "RedSnowBall")
        {
            HpController.value = max - 0.12f;
            Destroy(other.gameObject);
        }
        if (other.tag == "BlueSnowBall")
        {
            HpController.value = max - 0.15f;
            Destroy(other.gameObject);
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
