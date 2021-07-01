using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManeger : MonoBehaviourPunCallbacks ,IPunObservable
{
    public GameObject playerSet;
    GameObject Player;
    public GameObject enemyPrefab;
    public GameObject CamSet;
    //추후 포톤 연결시 주석처리 수정.
    public GameObject spawnPosition;
    // public GameObject BotSet; 
    public GameObject enemySpawn;
    public GameObject enemyspawn2;
    public GameObject GameOverScreen;
    public Slider playerSlider;
    public Text bluePlayerKill;
    public Text RedPlayerKill;
    public Text blueGetCoin;
    public Text redGetCoin;
    private Animator playerAni;
    public List<Enemy> enemySet = new List<Enemy>();
    public int EnemyCount = 2;
    private float TimeSet;
    private int spawnCount;
    public bool chack = false;
    private bool gameoverChack = false;
    public Text teamName;
    public Text killCount;
    public int playerKill = 0;//플레이어가 킬한거
    public int botkill = 0;//봇이 킬한거
    public int PlayerGetPoint = 0;

    //private int PlayerSpawnCount=0;
    //public List<GameObject> span = new List<GameObject>();//불루 레드 
    // Start is called before the first frame update
    void Start()
    {
        GameOverScreen = GameObject.Find("GameOverScene");
        GameOverScreen.SetActive(false);
        spawnCount = EnemyCount;
        spawnPosition = GameObject.Find("PlayerSpawn");
        Player = PhotonNetwork.Instantiate("Player", spawnPosition.transform.position, Quaternion.identity);
        playerSlider = Player.GetComponentInChildren<Slider>();
        playerAni = Player.GetComponent<Animator>();
        if (PhotonNetwork.IsMasterClient == true)
        {
            for (int i = 0; i < EnemyCount; i++)
            {
                GameObject obj =PhotonNetwork.Instantiate("Bot", enemySpawn.transform.position, Quaternion.identity);
                enemySet.Add(obj.GetComponent<Enemy>());
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogError(PhotonNetwork.CurrentRoom.Players.Count);

        if (Player.GetComponent<PlayerController>().isDead == false)
        {
            CamSet.GetComponent<CinemachineVirtualCamera>().Follow = Player.transform;
            CamSet.GetComponent<CinemachineVirtualCamera>().LookAt = Player.transform;
        }
        else if (Player.GetComponent<PlayerController>().isDead == true)
        {
            CamSet.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            CamSet.GetComponent<CinemachineVirtualCamera>().LookAt = transform;
            StartCoroutine("ReSpawnSet");
        }
        for (int i = 0; i < enemySet.Count; i++)
        {
            if (enemySet[i].isDead == true)
            {
                StartCoroutine("BotReSpawn", i);
            }
        }
        DeadCount();
        Gameover();
    }
    private void Respawn()
    {
        Player =PhotonNetwork.Instantiate("Player", spawnPosition.transform.position, Quaternion.identity);
       
        playerAni.SetBool("Die", false);
    }
    private void Gameover()
    {
        bluePlayerKill.text = "PlayerKills : " + playerKill;
        RedPlayerKill.text = "PlayerKills : " + botkill;
        blueGetCoin.text = "Get Coin :" +playerKill * 5;
        redGetCoin.text = "Get Coun : " + botkill * 5;
        if (gameoverChack == true)
        {
            GameOverScreen.SetActive(true);
        }
        PlayerGetPoint = PlayerPrefs.GetInt("Coin");
        PlayerGetPoint =+ int.Parse(blueGetCoin.text);
        PlayerPrefs.SetInt("Coin",PlayerGetPoint);
    }
    private void DeadCount()
    {
        if (Player.GetComponent<PlayerController>().isDead == true && chack == false)
        {
            botkill++;
            chack = true;
        }
        else if (Player.GetComponent<PlayerController>().isDead == false)
        {
            chack = false;
        }
        Debug.Log("BotkillCount" + botkill);
        Debug.Log("Playerkill" + playerKill);
        if (botkill > playerKill)
        {
          teamName.text = "Red Team";
          killCount.text= "kill Count : "+botkill;
        }
        else if (botkill < playerKill)
        {
            teamName.text = "Blue Team";
            killCount.text = "Kill Count : " + playerKill;
        }
        else
        {

        }
    }
    IEnumerator ReSpawnSet()
    {
        yield return new WaitForSeconds(4);
        Player.transform.position = spawnPosition.transform.position;
        playerSlider.value = 1;
        Player.GetComponent<PlayerController>().isDead = false;
        playerAni.SetBool("Die", false);
    }
    IEnumerator BotReSpawn(int i)
    {
        yield return new WaitForSeconds(4);
        PhotonNetwork.Destroy(enemySet[i].gameObject);
        GameObject obj =PhotonNetwork.Instantiate("Bot", enemySpawn.transform.position, Quaternion.identity);
        enemySet[i] = obj.GetComponent<Enemy>();
        Debug.Log(spawnCount);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(botkill);
            stream.SendNext(playerKill);
            stream.SendNext(gameoverChack);
        }
        else
        {
            botkill = (int)stream.ReceiveNext();
            playerKill = (int)stream.ReceiveNext();
            gameoverChack = (bool)stream.ReceiveNext();
        }
    }
}
