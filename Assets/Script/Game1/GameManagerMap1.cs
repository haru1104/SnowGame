using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class GameManagerMap1 : MonoBehaviourPunCallbacks , IPunObservable
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
    private GameObject playerLoding;
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
    private int botSpawnCount=2;
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
    private bool _botSpawn = false;
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
        BotSpawn();
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
       // Screen.SetResolution(1920, 1080, true);
        playerLoding = GameObject.FindGameObjectWithTag("PlayerLoding");
        playerLoding.SetActive(false);

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
        playerLoding.SetActive(true);
        //PlayerSetting
        player =PhotonNetwork.Instantiate("Player", playerSpawnPoint.position, Quaternion.identity);
        playerSlider = player.GetComponentInChildren<Slider>();
        playerAni = player.GetComponent<Animator>();
        camSet.Follow=player.transform;
        camSet.LookAt = player.transform;
        //BotSpawnSetting

    
    }
    void BotSpawn()
    {
        if (PhotonNetwork.CurrentRoom.Players.Count != 1 && PhotonNetwork.IsMasterClient == true &&_botSpawn == false)
        {
            playerLoding.SetActive(false);
            for (int i = 0; i < botSpawnCount; i++)
            {
                GameObject go = PhotonNetwork.Instantiate("Bot", botSpawnPoint[i].position, botSpawnPoint[i].rotation);
                bot.Add(go.GetComponent<Enemy>());
            }
            _botSpawn = true;
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

            if (random >= 2)
            {
                PhotonNetwork.Instantiate("RandomBox", giftpos, Quaternion.identity);
            }
            else if(random  >= 4)
            {
                PhotonNetwork.Instantiate("RandomBox2", giftpos, Quaternion.identity);
            }
            else if (random >= 6)
            {
                PhotonNetwork.Instantiate("RandomBOx", giftpos, Quaternion.identity);
            }
           
        }
    }
    void CreateBox()
    {
        if (blueBoxSpawn == false)
        {
            GameObject targetBlueBox = PhotonNetwork.Instantiate("TargetBox", blueCreateBoxSpawnPosition.position, blueCreateBoxSpawnPosition.rotation);
            blueBoxSpawn = true;
        }
        if (redBoxSpawn == false)
        {
            GameObject targetRedBox =PhotonNetwork.Instantiate("TargetBox", redCreateBoxSpawnPosition.position, redCreateBoxSpawnPosition.rotation);
            redBoxSpawn = true;
        }
    }
    void ReSpawn()
    {
        for (int i = 0; i < bot.Count; i++)
        {
            if (bot[i].isDead == true)
            {
                photonView.RPC("ScoreRpc", RpcTarget.AllBuffered, 1);
                // StartCoroutine("BotReSpawn", i);
                BotReSpawn(i);
            }
        }
        if (player.GetComponent<PlayerController>().isDead == true)
        {
            photonView.RPC("ScoreRpc", RpcTarget.AllBuffered, 2);
            StartCoroutine("PlayerReSpawn");
        }
    }
    [PunRPC]
    private void ScoreRpc(int i)
    {
        if (i == 1 )
        {
            blueScore += 1000;
        }
        else
        {
            redScore += 1000;
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
            PhotonNetwork.Destroy(collision.gameObject);
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
    private void BotReSpawn(int i)
    {
        
        PhotonNetwork.Destroy(bot[i].gameObject);
        GameObject temp = PhotonNetwork.Instantiate("Bot", botSpawnPoint[i].position,botSpawnPoint[i].rotation);
        bot[i] = temp.GetComponent<Enemy>();
    }
    IEnumerator PlayerReSpawn()
    {
        yield return new WaitForSeconds(5f);
        player.transform.position = playerSpawnPoint.position;
        photonView.RPC("HpSet", RpcTarget.AllBuffered);
        player.GetComponent<PlayerController>().isDead = false;
        playerAni.SetBool("Die", false);
    }
    [PunRPC]
    private void HpSet()
    {
        playerSlider.value = 1;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerKillCount);
            stream.SendNext(botKillCount);
        }
        else
        {
            playerKillCount = (int)stream.ReceiveNext();
            botKillCount = (int)stream.ReceiveNext();
        }
    }
}
