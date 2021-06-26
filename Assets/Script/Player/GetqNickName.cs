using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GetqNickName : MonoBehaviourPun , IPunObservable
{
    private Text playerNickNAme;
    private string name;
    // Start is called before the first frame update
    void Start()
    {
        playerNickNAme = GetComponentInChildren<Text>();
        SetNickName();
    }

    private void SetNickName()
    { 
        playerNickNAme.text = photonView.Owner.NickName;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.IsWriting)
        //{
        //    stream.SendNext(PlayerPrefs.GetString("PlayerName"));
        //}
        //else
        //{
        //    playerNickNAme.text = (string)stream.ReceiveNext();
        //}
    }
}
