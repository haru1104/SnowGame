using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class GoBack : MonoBehaviourPunCallbacks
{
    private int playerNum;

    public void Onclick()
    {
        playerNum = PhotonNetwork.PlayerList.Length;

        if (photonView.IsMine == true)
        {
            if (photonView.Owner.IsMasterClient == true && playerNum != 1)
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.CurrentRoom.Players[playerNum-1]);
               
            }
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("Lobby",new RoomOptions { MaxPlayers = 20 },TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.CurrentRoom.Name == "Lobby")
        {
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
}
