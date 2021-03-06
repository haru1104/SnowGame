using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Potal : MonoBehaviourPunCallbacks
{
    //2번 포탈

    private int PlayerNum;
    Game_Manager gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game_Manager>();
    }
    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)//텔포 충돌
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PhotonView>().Owner.IsMasterClient)
            {
                Dictionary<int, Photon.Realtime.Player> playerList = PhotonNetwork.CurrentRoom.Players;// Dictionary 키를 벨류값을 저장해주는 자료구조
                if (playerList.Count != 1)
                {
                    PhotonNetwork.SetMasterClient(playerList[playerList.Count - 1]); //방을 접속해있는 플레이어 중에 가장 남아있는 플레이어한테 마스터클라이언트를 넘겨준다
                }
            }
            ChangeRoom();

        }


    }
    private void ChangeRoom()
    {
        if (PhotonNetwork.CurrentRoom != null)// 방에 접속이 되어있다면 방을 나가라.
        {
            PhotonNetwork.LeaveRoom();
            //포탈에 닿았을때 마스터 클라이언트면 다른 플레이어한테 넘겨줘야 하는데 
            //방에 접속해있으면 방을 나가고
            //지금 내가 닿은 포탈의 usedpotalnum =2)값을 GameManager 한테 넘겨준다.
        }
        gm.usedPotalNum = 1;
    }
}