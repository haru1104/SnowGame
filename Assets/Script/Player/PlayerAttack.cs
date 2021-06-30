using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class PlayerAttack : MonoBehaviourPunCallbacks 
{
    [SerializeField]
    private Transform SnowSpawnPoint;
    [SerializeField]
    private GameObject SnowBallPrefab;
    [SerializeField]
    private GameObject RadSnowBallPrefab;
    [SerializeField]
    private GameObject BlueSnowBallPrefab;
    [SerializeField]
    private Transform StickTransform;
    [SerializeField]
    private GameObject BlueStickPreFab;
    [SerializeField]
    private GameObject RadStickPreFab;
    private GameObject go;
    private Button axeButton;
    [SerializeField]
    private Transform axePos;
    [SerializeField]
    private GameObject axePrefab;
    private Animator playerAni;
    private PlayerController plaCon;
    private Rigidbody BallRigid;
    private Button attackButton;
    private float SnowBallSpeed = 1000;
    private float SnowBallUpForce = 300;
    private float DelayTime = 0.5f;
    private float DeltaTime;
    private bool axeOn=false;
    public int SetNumber = 20;


    public bool NomalSnow = false;
    public bool RedSnow = false;
    public bool BlueSnow = false;
    public bool stick = false;

  

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            return;
        }
        else
        {
            attackButton = GameObject.FindGameObjectWithTag("AttackButton").GetComponent<Button>();
        }
        GameSceneChack();
        ButtonSet();
    }

    // Update is called once per frame
    void Update()
    {
        SetWeapon(SetNumber);
       
    }
    public void AttackSet()
    {
        if (axeOn != true && photonView.IsMine == true)
        {
            if (NomalSnow == true)
            {
                GameObject ballSpawn =PhotonNetwork.Instantiate("SnowBall", SnowSpawnPoint.position, Quaternion.identity);
                ballSpawn.GetComponent<Rigidbody>().AddForce(SnowSpawnPoint.forward * SnowBallSpeed);
                ballSpawn.GetComponent<Rigidbody>().AddForce(Vector3.up * SnowBallUpForce);
                Destroy(ballSpawn.gameObject, 5);
            }
            else if (RedSnow == true)
            {
                GameObject ballSpawn = PhotonNetwork.Instantiate("RedSnowBall", SnowSpawnPoint.position, Quaternion.identity);
                ballSpawn.GetComponent<Rigidbody>().AddForce(SnowSpawnPoint.forward * SnowBallSpeed);
                ballSpawn.GetComponent<Rigidbody>().AddForce(Vector3.up * SnowBallUpForce);
                Destroy(ballSpawn.gameObject, 10);
            }
            else if (BlueSnow == true)
            {
                GameObject ballSpawn = PhotonNetwork.Instantiate("BlueSnowBall", SnowSpawnPoint.position, Quaternion.identity);
                ballSpawn.GetComponent<Rigidbody>().AddForce(SnowSpawnPoint.forward * SnowBallSpeed);
                ballSpawn.GetComponent<Rigidbody>().AddForce(Vector3.up * SnowBallUpForce);
                Destroy(ballSpawn.gameObject, 10);
            }
            else if (stick == true)
            {

            }
            else
            {
                return;
            }
        }
    }
    public void OnClikAxeButton()
    {
        if (photonView.IsMine == true)
        {
            //AxeRpc();
            photonView.RPC("AxeRpc", RpcTarget.AllBuffered);
        }
    }
    public void GameSceneChack()
    {
        if (SceneManager.GetActiveScene().name == "Game1")
        {
            plaCon = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerAni = GetComponent<Animator>();
            axeButton = GameObject.Find("AxeButton").GetComponent<Button>();
            axeButton.onClick.AddListener(OnClikAxeButton);
        }
         else if (SceneManager.GetActiveScene().name == "Game2")
        {
            plaCon = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerAni = GetComponent<Animator>();
        }
    }
    private void SetWeapon(int number)
    {
        switch (number)
        {
            case 0:
                NomalSnow = true;
                break;
            case 1:
                RedSnow = true;
                break;

            case 2:
                BlueSnow = true;
                break;

            case 3:
                stick = true;
                break;
        }
    }
    private void AttackChack()
    {
        if (/*Input.GetMouseButtonDown(0) &&*/ plaCon.isDead == false)
        {
            if (SetNumber != 20)
            {
                playerAni.SetTrigger("Attack");
            }
        }
    }
    private void ButtonSet()
    {
        attackButton.onClick.AddListener(AttackChack);
    }
    [PunRPC]
   void AxeRpc()
    {
        if (axeOn == false)
        {
            go = PhotonNetwork.Instantiate("Axe", axePos.position, axePos.rotation);
            //부모오브젝트 하위로 들어가게 만드는 설정.
            //부모의 좌표를 받아와서 같이 동작하게 만들게 하려고 사용함.
            go.transform.parent = axePos.transform;
            axeOn = true;
            go.transform.Rotate(90, 0, 0);
        }
        else if (axeOn == true)
        {
            PhotonNetwork.Destroy(go.gameObject);
            axeOn = false;
        }
    }
}
