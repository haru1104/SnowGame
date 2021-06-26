using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class GameManagerMap1 : MonoBehaviourPunCallbacks
{
    
    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private GameObject botPrefab;
    [SerializeField]
    private GameObject createBox;
    [SerializeField]
    private GameObject botBox;
    private GameObject player;
    [SerializeField]
    private Transform redCreateBoxSpawnPosition;
    [SerializeField]
    private Transform blueCreateBoxSpawnPosition;
    public GameObject gameOverScene;
    private Transform playerSpawnPoint;
    private CinemachineVirtualCamera camSet;
    private Slider playerSlider;
    private Animator playerAni;
    private List<Transform> botSpawnPoint = new List<Transform>();
    public List<GameObject> giftBoxPrefab = new List<GameObject>();
    public List<Enemy> bot = new List<Enemy>();
    private Text winnerText;
    private Text winnerkill;
    
    [SerializeField]
    private int botSpawnCount;
    private int giftBoxMaxCount=5;
    private int randomX, randomZ;
    private int SetSceneNum = 1;
    public int boxSpawnTime = 0;
    public int giftCount = 0;
    public int redScore = 0, blueScore = 0;
    private float gbSpawnX, gbSpawnZ;
    public bool blueBoxSpawn = false;
    public bool redBoxSpawn = false;
    public bool gameOver = false;
    private int countDown = 3; // 3초후 게임을 시작하는 ui 띄우는 작업 해야함.

    private int startCount = 0;
    public int playerKillCount=0;
    public int botKillCount=0;

    // Start is called before the first frame update
  
    void Start()
    {
        StartSet();
        SpawnSet();
    }
    // Update is called once per frame
    void Update()
    {
        Score();
        CreateBox();
        GiftSpawn();
        ReSpawn();
        ChackGameOver();
        Botdle();
        Score();
    }
    void StartSet()
    {
        Debug.Log("spawn");
        playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint").GetComponent<Transform>();
        botSpawnPoint.Add(GameObject.FindWithTag("BotSpawnPoint").gameObject.GetComponent<Transform>());
        botSpawnPoint.Add(GameObject.FindWithTag("BotSpawnPoint2").gameObject.GetComponent<Transform>());
        camSet = GameObject.FindGameObjectWithTag("CMvcam").GetComponent<CinemachineVirtualCamera>();
        winnerText = GameObject.Find("WinnerText").GetComponent<Text>();
        winnerkill = GameObject.Find("WinnerKill").GetComponent<Text>();
        gameOverScene = GameObject.Find("GameOver");
        gameOverScene.SetActive (false);
    }

    void SpawnSet()
    {
        //PlayerSetting
        player =PhotonNetwork.Instantiate("Player", playerSpawnPoint.position, Quaternion.identity);
        playerSlider = player.GetComponentInChildren<Slider>();
        playerAni = player.GetComponent<Animator>();
        camSet.Follow=player.transform;
        camSet.LookAt = player.transform;
        //BotSpawnSetting
        for (int i = 0; i < botSpawnCount; i++)
        {
            GameObject go = (GameObject)Instantiate(botPrefab, botSpawnPoint[i].position, botSpawnPoint[i].rotation);
            bot.Add(go.GetComponent<Enemy>());
        }

    }
    void GiftSpawn()
    {
        //GiftBoxSetting
        for (; giftCount <= giftBoxMaxCount; giftCount++)
        {
            int random = Random.Range(0, 6);
            randomX = Random.Range(-25, 25);
            randomZ = Random.Range(-11, 11);
            Vector3 giftpos = new Vector3(randomX, 2.6f, randomZ);
            Instantiate(giftBoxPrefab[random], giftpos, Quaternion.identity);
        }
    }
    void CreateBox()
    {
        if (blueBoxSpawn == false)
        {
            GameObject targetBlueBox = Instantiate(createBox, blueCreateBoxSpawnPosition.position, blueCreateBoxSpawnPosition.rotation);
            blueBoxSpawn = true;
        }
        if (redBoxSpawn == false)
        {
            GameObject targetRedBox = Instantiate(botBox, redCreateBoxSpawnPosition.position, redCreateBoxSpawnPosition.rotation);
            redBoxSpawn = true;
        }
    }
    void ReSpawn()
    {
        for (int i = 0; i < bot.Count; i++)
        {
            if (bot[i].isDead == true)
            {
                blueScore += 1000;
                StartCoroutine("BotReSpawn", i);
            }
        }
        if (player.GetComponent<PlayerController>().isDead == true)
        {
            redScore += 1000;
            StartCoroutine("PlayerReSpawn");
        }
    }
    void ChackGameOver()
    {
        if (gameOver == true)
        {
            Debug.Log("레드점수 :" + redScore+ "블루점수 :" + blueScore);
        }
    }
    private void OnCollisionEnter(Collision collision) // 맵 밖으로 뛰어내리면 
    {
        if (collision.gameObject.tag == "Player")
        {
            playerAni.SetBool("Die", true);
            player.GetComponent<PlayerController>().isDead = true;
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
    private void Botdle()//임시로 제작 나중에 게임 제작후 제거 요망(shift Key 입력시 Bot제거)
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Destroy(GameObject.FindGameObjectWithTag("bot"));
        }
    }
    public void Score()
    {
        Debug.Log(playerKillCount);
        Debug.Log(botKillCount);
        if (playerKillCount >botKillCount)
        {
            winnerText.text="Red Team\n";
            
            winnerkill.text = "Kill : " + playerKillCount;
        }
        else if(botKillCount > playerKillCount)
        {
            winnerText.text = "Blue Team\n";
            
            winnerkill.text = "Kill : " + botKillCount;
        }
        else
        {
            winnerText.text = "";
        }

    }
    private void Time()
    {

    }
    IEnumerator BotReSpawn(int i)
    {
        yield return new WaitForSeconds(5f);
        Destroy(bot[i].gameObject);
        GameObject temp = Instantiate(botPrefab, botSpawnPoint[i].position,botSpawnPoint[i].rotation);
        bot[i] = temp.GetComponent<Enemy>();
    }
    IEnumerator PlayerReSpawn()
    {
        yield return new WaitForSeconds(5f);
        player.transform.position = playerSpawnPoint.position;
        playerSlider.value = 1;
        player.GetComponent<PlayerController>().isDead = false;
        playerAni.SetBool("Die", false);
    }
}
