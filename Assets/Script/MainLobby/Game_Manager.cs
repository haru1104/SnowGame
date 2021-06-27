using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class Game_Manager : MonoBehaviourPunCallbacks
{   
    [SerializeField]
    private GameObject camSet;
    private GameObject player;
    private Slider playerSlider;
    
    private bool sliderDes = false;
    public int usedPotalNum = 0;//포탈에 따라서 어디로 이동했는데 체크 해주는 int값

    public void LeaveRoom()//방을 나가게하는 메소드
    {
        if (PhotonNetwork.CurrentRoom.Name == "Lobby") // 방이름이 로비라면 룸을 나간다,
        {
            PhotonNetwork.LeaveRoom(); //룸을 나간다.
        }
        else
        {
            PhotonNetwork.LeaveLobby();//만약에 로비라는 방이 아니라면 로비를 나간다.
        }
    }


    // Update is called once per frame
    void Update()
    {
        PlayerSet();
        CamSet();
        //if (photonView.IsMine)
        //{
        //    photonView.RPC("PlayerSliderSet", RpcTarget.All,photonView.ViewID);
        //}

    }
    void PlayerSet()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

    }
    void CamSet()
    {
        if (player !=null)
        {
            camSet.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            camSet.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
        }
        
    }
    public override void OnConnectedToMaster()//
    {
        base.OnConnectedToMaster();
        Debug.LogError("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()//
    {
        base.OnJoinedLobby();
        Debug.LogError("OnJoinedLobby");
        if (usedPotalNum == 1)
        {
            PhotonNetwork.JoinOrCreateRoom("Game1", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
        }
        else if (usedPotalNum ==2)
        {
            PhotonNetwork.JoinOrCreateRoom("Game2", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
        }
        else
        {
            PhotonNetwork.JoinOrCreateRoom("Lobby", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
            //만약에 다른 게임 씬에서 로비씬으로 넘어오는 플레이어들을 로비룸으로 접속을 시킨다.

        }
    }
    public override void OnJoinedRoom()//
    {
        base.OnJoinedRoom();
        Debug.LogError("OnJoinedRoom");


        if (usedPotalNum == 1)
        {
           
            PhotonNetwork.LoadLevel("Game1");
        }
        else if (usedPotalNum == 2)
        {
           
            PhotonNetwork.LoadLevel("Game2");
        }
        else
        {
            
        }
    }
    //[PunRPC]
    //void PlayerSliderSet(int id)
    //{

    //    if (player != null && sliderDes == false)
    //    {
    //        Slider _playerSlider = PhotonView.Find(id).gameObject.GetComponentInChildren<Slider>();
    //        // playerSlider = player.GetComponentInChildren<Slider>();
    //        _playerSlider.gameObject.SetActive(false);
    //        sliderDes = true;
    //    }
    //}
}
