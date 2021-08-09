using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class BoxScript : MonoBehaviourPunCallbacks
{

    private GameObject player;
    private GameObject boxSpawn;
 
    private GameManeger GmMap2;
    private GameManagerMap1 GmMap1;
    List<Enemy> enemy = new List<Enemy>();

    private int random;
    public int gameSceneNum;
    // Start is called before the first frame update
    void Start()
    {
        SceneChack();
        SceneReset();
        ResetWeapon();
    }

    

    private void OnTriggerEnter(Collider other)
    {
        ResetWeapon();
        if (other.tag == "Player" )
        {
            Debug.Log("Player");

            //boxSpawn.GetComponent<Spawn>().count--;

            if (random >= 0 && random <= 1)
            {
                player.GetComponent<PlayerAttack>().SetNumber = 2;
                PhotonNetwork.Destroy(gameObject);              
            }
            else if (random >= 2 && random <= 3)
            {
                player.GetComponent<PlayerAttack>().SetNumber = 1;
                PhotonNetwork.Destroy(gameObject);                
            }
            else if (random >= 4 && random <= 8)
            {
                player.GetComponent<PlayerAttack>().SetNumber = 0;
                PhotonNetwork.Destroy(gameObject);               
            }
            else
            {
                player.GetComponent<PlayerAttack>().SetNumber = 3;
                PhotonNetwork.Destroy(gameObject);               
            }
            
        }
        if (other.tag == "bot")
        {
            for (int i = 0; i < enemy.Count; i++)
            {
                if (enemy[i] == other.GetComponent<Enemy>())
                {
                    if (gameSceneNum == 2)
                    {
                        boxSpawn.GetComponent<Spawn>().count--;
                    }
                    else
                    {
                        boxSpawn.GetComponent<GameManagerMap1>().giftCount--;
                    }


                    if (random >= 0 && random <= 1)
                    {
                      enemy[i].weaponSet = 2;
                        PhotonNetwork.Destroy(gameObject);
                    }
                    else if (random >= 2 && random <= 3)
                    {
                        enemy[i].weaponSet = 1;
                        PhotonNetwork.Destroy(gameObject);
                    }
                    else if (random >= 4 && random <= 8)
                    {
                        enemy[i].weaponSet = 0;
                        PhotonNetwork.Destroy(gameObject);
                    }
                    else
                    {
                        enemy[i].weaponSet = 3;
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
        }
    }
  private void ResetWeapon()
  {
        if (player == null)
        {
            Debug.Log("Player Is Null");
        }
      
        player.GetComponent<PlayerAttack>().NomalSnow = false;
        player.GetComponent<PlayerAttack>().BlueSnow = false;
        player.GetComponent<PlayerAttack>().RedSnow = false;
        player.GetComponent<PlayerAttack>().stick = false;
  }
    private void SceneChack()
    {
        if (SceneManager.GetActiveScene().name == "Game1")
        {
            gameSceneNum = 1;
        }
        else if (SceneManager.GetActiveScene().name == "Game2")
        {
            gameSceneNum = 2;
        }
    }

    private void SceneReset()
    {
        if (gameSceneNum == 1)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            boxSpawn = GameObject.Find("GameManager");
            random = Random.Range(0, 10);
            GmMap1 = GameObject.Find("GameManager").GetComponent<GameManagerMap1>();
            // Debug.Log("罚待积己 :" + random);
            enemy = GmMap1.bot;
        }
        else if (gameSceneNum == 2)
        {
            boxSpawn = GameObject.Find("BoxSpawn");

            player = GameObject.FindGameObjectWithTag("Player");
            random = Random.Range(0, 10);
            
            GmMap2 = GameObject.Find("GameManeger").GetComponent<GameManeger>();
            // Debug.Log("罚待积己 :" + random);
            enemy = GmMap2.enemySet;
        }
        
    }
}

