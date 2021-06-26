using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LobbyGameSpawn : MonoBehaviourPunCallbacks
{
    //2�� ��Ż

    private int PlayerNum;
    Game_Manager gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game_Manager>();
    }
    private void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)//���� �浹
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PhotonView>().Owner.IsMasterClient)
            {
                Dictionary<int, Photon.Realtime.Player> playerList = PhotonNetwork.CurrentRoom.Players;// Dictionary Ű�� �������� �������ִ� �ڷᱸ��
                if (playerList.Count != 1)
                {
                    PhotonNetwork.SetMasterClient(playerList[playerList.Count - 1]); //���� �������ִ� �÷��̾� �߿� ���� �����ִ� �÷��̾����� ������Ŭ���̾�Ʈ�� �Ѱ��ش�
                }
            }
            ChangeRoom();
          
        }


    }
    private void ChangeRoom()
    {
        if (PhotonNetwork.CurrentRoom != null)// �濡 ������ �Ǿ��ִٸ� ���� ������.
        {
            PhotonNetwork.LeaveRoom();
            //��Ż�� ������� ������ Ŭ���̾�Ʈ�� �ٸ� �÷��̾����� �Ѱ���� �ϴµ� 
            //�濡 ������������ ���� ������
            //���� ���� ���� ��Ż�� usedpotalnum =2)���� GameManager ���� �Ѱ��ش�.
        }
        gm.usedPotalNum = 2;
    }
}
//�κ񿡼��� ��Ŀ����
//���� ������ �Ǹ� 
//������ �ٽ� ������ ������ ������ �õ��ϰ�
//GameManager OnconnectToMaster�� ������ �ɰŰ� 
//�� �޼ҵ� �ȿ� �ִ� joinLobby ������ OnjoinLobby �� ������ �ǰ�
//���� �б⹮(usedPotalNum)�� ���� �� ������ �� ������ �̸����� ���� �����ϰ�
//�� �濡 ���� �Ǹ� LoadLevel �� ���� ���� �ε����ش�. 
