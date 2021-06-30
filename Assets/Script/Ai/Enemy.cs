using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Enemy : MonoBehaviourPunCallbacks
{
    public enum State
    {
        Find,
        Move,
        Attack,
        Dead,
    }
    private float dis;
    private float SnowBallSpeed = 1000;
    private float SnowBallUpForce = 300;
    public float delayTime =0;
    private float turnAngle = 360f; //회전반경
    public int weaponSet = 20;  //무기 번호 확인
    private int bulletCount = 10;
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
    [SerializeField]
    private GameObject Axe;

    Slider bothp;
    EnemyAni myAniSet;
    Transform player;
    Transform spawnBox;
    NavMeshAgent nav;
    PlayerController playerCon;
    GameManagerMap1 gm;
    GameManeger gm1;
    GameObject boxTarget;
    GameObject axeInHand;
    [SerializeField]
    Transform handPos;
    Quaternion turnRotation;

    public bool isDead = false;
    public bool boxPickUp = false;  //박스 주웠는지 확인
    public bool nomalSnow = false;
    public bool redSnow = false;
    public bool blueSnow = false;
    public bool stick = false;
    public State CurrentState = State.Find;
    private string sceneName;
    private bool targetBoxFind = false;
    private bool axeIsHand = false;
    // Start is called before the first frame update
    void Start()
    {
        myAniSet = GetComponent<EnemyAni>();
        nav = GetComponent<NavMeshAgent>();
        bothp = GetComponent<BotHp>().Botsli;
        sceneName = SceneManager.GetActiveScene().name;
        ChackNav();


    }


    void UpdateState()
    {
        switch (CurrentState)
        {
            case State.Find:
                //Idle();
                ObjectFind();
                break;
            case State.Move:
                moveTurnDes();
                break;
            case State.Attack:
                SetWeapon();
                
                break;
            case State.Dead:
                Dead();

                break;
            default:
                break;
        }
    }
    void Update()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }
        else
        {
            Debug.LogError(photonView.ViewID);
            ChackState();
            UpdateState();
        }
    }
    private void Idle()
    {     
        if (player== null && spawnBox == null)
        {
            myAniSet.ChangeAni(EnemyAni.ANI_IDLE);
        }  
    }

    private void moveTurnDes()
    {
        if (sceneName == "Game2")
        {
            myAniSet.ChangeAni(EnemyAni.ANI_RUN);
            if (boxPickUp == true)
            {
                nav.SetDestination(player.position);
                turnRotation = Quaternion.LookRotation(player.position - transform.position);

            }
            else if (boxPickUp == false)
            {
                if (spawnBox != null)
                {
                    nav.SetDestination(spawnBox.position);
                    turnRotation = Quaternion.LookRotation(spawnBox.position - transform.position);
                }
                else
                {
                    CurrentState = State.Find;
                }
            }
        }
        if (sceneName == "Game1")
        {
            myAniSet.ChangeAni(EnemyAni.ANI_RUN);
            if (boxPickUp == true && targetBoxFind == false)
            {
                nav.SetDestination(player.position);
                turnRotation = Quaternion.LookRotation(player.position - transform.position);

            }
            else if (boxPickUp == false && targetBoxFind == false)
            {
                if (spawnBox != null)
                {
                    nav.SetDestination(spawnBox.position);
                    turnRotation = Quaternion.LookRotation(spawnBox.position - transform.position);
                }
                else
                {
                    CurrentState = State.Find;
                }
            }
            else if (targetBoxFind == true)
            {
                Debug.LogError("MoveSet");
                nav.SetDestination(boxTarget.transform.position);
                turnRotation = Quaternion.LookRotation(boxTarget.transform.position - transform.position);
                
            }
        }
        
    }

    void ObjectFind()
    {
        delayTime += Time.deltaTime;

        if (delayTime >= 7)
        {
            if (sceneName == "Game2")
            {
                if (boxPickUp == false)
                {
                    spawnBox = GameObject.FindGameObjectWithTag("GiftBox").gameObject.transform;
                    CurrentState = State.Move;
                }
                else if (boxPickUp == true)
                {
                    player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
                    playerCon = player.GetComponent<PlayerController>();
                    CurrentState = State.Move;
                }
            }
            if (sceneName == "Game1")
            {
                if (boxPickUp == false)
                {
                    spawnBox = GameObject.FindGameObjectWithTag("GiftBox").gameObject.transform;
                    CurrentState = State.Move;
                }
                else if (boxPickUp == true&& targetBoxFind == false)
                {
                    player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
                    playerCon = player.GetComponent<PlayerController>();
                    CurrentState = State.Move;
                }
                else if (boxPickUp == true && targetBoxFind == true)
                {
                    Debug.LogError("Find");
                    boxTarget = GameObject.FindGameObjectWithTag("Target");
                    if (boxTarget == null)
                    {
                        Debug.LogError("Target Null");
                    }
                    CurrentState = State.Move;
                }
            }
           
        }   

    }

    void ChackState()
    {

        if (bothp.value <= 0.1&&CurrentState != State.Dead && photonView.IsMine==true)
        {
            if (SceneManager.GetActiveScene().name == "Game1")
            {
                gm = GameObject.Find("GameManager").GetComponent<GameManagerMap1>();
                gm.botKillCount++;
            }
            else if (SceneManager.GetActiveScene().name == "Game2")
            {
                gm1 = GameObject.Find("GameManeger").GetComponent<GameManeger>();
                gm1.playerKill++;
            }
            CurrentState = State.Dead;
        }
        else
        {
            if (sceneName == "Game2")
            {
                ChackSceneGame2();
            }
            else if (sceneName == "Game1")
            {
                ChackSceneGame1();
            }
        }
    }
   
    void Dead()
    {
        Debug.Log("DEAD");
        myAniSet.ChangeAni(EnemyAni.ANI_DEAD);
        player = null;
        boxPickUp = false;
        weaponSet = 20;
        isDead = true;

    }
    void attack() //애니메이터에 걸려있음.
    {
       if (nomalSnow == true && axeIsHand == false)
       {
           GameObject botballSpawn =PhotonNetwork.Instantiate("SnowBall", SnowSpawnPoint.position, Quaternion.identity);
           botballSpawn.GetComponent<Rigidbody>().AddForce(SnowSpawnPoint.forward * SnowBallSpeed);
           botballSpawn.GetComponent<Rigidbody>().AddForce(Vector3.up * SnowBallUpForce);
           Destroy(botballSpawn.gameObject, 5);

       }
       else if (redSnow == true && axeIsHand == false)
       {
           GameObject botballSpawn = PhotonNetwork.Instantiate("RedSnowBall", SnowSpawnPoint.position, Quaternion.identity);
           botballSpawn.GetComponent<Rigidbody>().AddForce(SnowSpawnPoint.forward * SnowBallSpeed);
           botballSpawn.GetComponent<Rigidbody>().AddForce(Vector3.up * SnowBallUpForce);
           Destroy(botballSpawn.gameObject, 10);
       }
       else if (blueSnow == true && axeIsHand == false)
       {
           GameObject botballSpawn = PhotonNetwork.Instantiate("BlueSnowBall", SnowSpawnPoint.position, Quaternion.identity);
           botballSpawn.GetComponent<Rigidbody>().AddForce(SnowSpawnPoint.forward * SnowBallSpeed);
           botballSpawn.GetComponent<Rigidbody>().AddForce(Vector3.up * SnowBallUpForce);
           Destroy(botballSpawn.gameObject, 10);
       }
       else if (axeIsHand == true)
       {
            photonView.RPC("AxeRpc", RpcTarget.AllBuffered);
       }
    }
    [PunRPC]
    private void AxeRpc()
    {
        if (axeInHand == null)
        {
            axeInHand = PhotonNetwork.Instantiate("Axe", handPos.transform.position, handPos.rotation);
            axeInHand.transform.Rotate(90, 0, 0);
        }
        axeInHand.transform.parent = handPos.transform;

        return;
    }
    private void SetWeapon()
    {
        switch (weaponSet)
        {
            case 0:
                nomalSnow = true;
                break;
            case 1:
                redSnow = true;
                break;

            case 2:
                blueSnow = true;
                break;

            case 3:
                stick = true;
                break;
        }
    }

   private void ChackSceneGame1() //타겟 박스 false 해야함.
    {
        if (boxPickUp == true && playerCon.isDead == true && targetBoxFind == false)
        {
            Debug.LogError(" state 전환부분.");
            nav.isStopped = false;
            targetBoxFind = true;
            CurrentState = State.Find;

        }
        else if (targetBoxFind == true && playerCon.isDead == true)
        {
            if (boxTarget == null)
            {
                Debug.LogError("box error");
            }
            dis = Vector3.Distance(boxTarget.transform.position, transform.position);
            Debug.LogError(dis);
            if (dis <= 3)
            {
                nav.isStopped = true;
                Debug.LogError(weaponSet);
                CurrentState = State.Attack;
                axeIsHand = true;
                myAniSet.ChangeAni(EnemyAni.ANI_ATTACK);

            }
     
        }
        else if (targetBoxFind == true && playerCon.isDead == false)
        {
            targetBoxFind = false;
            axeIsHand = false;
            PhotonNetwork.Destroy(axeInHand.gameObject);
            CurrentState = State.Find;
        }
        if (weaponSet != 20 && boxPickUp == false)
        {
            boxPickUp = true;
            CurrentState = State.Find;
        }
        if (player != null && playerCon.isDead == false && targetBoxFind == false)
        {
            turnRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, Time.deltaTime * turnAngle);

            dis = Vector3.Distance(player.position, transform.position);
            Debug.Log(dis);
            if (dis <= 15)
            {
                nav.isStopped = true;
                CurrentState = State.Attack;
                myAniSet.ChangeAni(EnemyAni.ANI_ATTACK);
            }
            else if (dis >= 15 && nav.isStopped == true)
            {
                nav.isStopped = false;
                CurrentState = State.Find;
            }
        }
        
    }
    private void ChackSceneGame2()
    {
        if (boxPickUp == true && playerCon.isDead == true )
        {
            boxPickUp = false;
            weaponSet = 20;
            spawnBox = null;
            nav.isStopped = false;
            CurrentState = State.Find;
        }
        if (weaponSet != 20 && boxPickUp == false)
        {
            boxPickUp = true;
            CurrentState = State.Find;
        }
        if (player != null && playerCon.isDead == false)
        {
            turnRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, Time.deltaTime * turnAngle);

            dis = Vector3.Distance(player.position, transform.position);
            Debug.Log(dis);
            if (dis <= 15)
            {
                nav.isStopped = true;
                CurrentState = State.Attack;
                myAniSet.ChangeAni(EnemyAni.ANI_ATTACK);
            }
            else if (dis >= 15 && nav.isStopped == true)
            {
                nav.isStopped = false;
                CurrentState = State.Find;
            }
        }
    }
    void ChackNav()
    {
        if (PhotonNetwork.IsConnected == true && PhotonNetwork.IsMasterClient == false)
        {
            nav.enabled=false;
        }
    }

}
