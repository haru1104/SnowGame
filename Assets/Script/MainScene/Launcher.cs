using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class Launcher : MonoBehaviourPunCallbacks //punCollBacks ���̴� ������ ���濡�� ������ �ϴ� ���� �޼ҵ带 ��Ÿ�� �Ű澲�� �ʰ� �ڵ��Է��� �ǰ� ���� Ŭ����
{
    private string gameVer = "0.1";//���� ���� ����
    private Text DebugText;
    

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;//���濡�� �ڵ����� �ε��� ��� ���� ����ȭ���ִ� ������Ƽ
        DebugText = GameObject.Find("FuckingUnity").GetComponent<Text>();
    }
    public void JoinServer() 
    {
        if (PhotonNetwork.IsConnected == true)//�����Ʈ��ũ�� ������ �Ǿ��ִ� ���°� Ŭ���̾�Ʈ���� ������ �Ǿ����� ������ �������� ��(�����Ǿ��ִ�) ����.
        {
            // PhotonNetwork.JoinRandomRoom(); // �����Ʈ��ũ�� �����Ǿ��ִ�(�̹� ����� �ִ� ��.) ������ ���߿� �ϳ��� ��� �����ϴ°�.
            //ver2//PhotonNetwork.JoinOrCreateRoom("Lobby",new RoomOptions {MaxPlayers =20},TypedLobby.Default);
            PhotonNetwork.JoinLobby();
        }
        else //���࿡ �����Ʈ��ũ�� �������� ���� ���°� Ŭ���̾�Ʈ�� ������ �ȵǾ��ִ� ���¸� OnConnectedToMaster �޼ҵ带 �����ؼ� ������ Ŭ���̾�Ʈ�� ����
        {
            PhotonNetwork.GameVersion = gameVer;//���� ���������� ���ش�.
            PhotonNetwork.ConnectUsingSettings();//������ ������ ���ӹ����� ����ؼ� ���� ��Ʈ��ũ�� ������ �ϰڴٴ� ��
                                                //��Ʈ��ũ�� ������ �Ǿ����� ���� ���¶�� �¶��� ���� �ٲ��ش�,

        }
    }
    public override void OnConnectedToMaster() // �������̵�� ��ӹ��� Ŭ���� �޼ҵ带 ������ �ؼ� ����� �Ѵ�. (�θ�޼ҵ��� Ȱ���� ���� �ڽ��� ���)
    {
        base.OnConnectedToMaster();// ������ Ŭ���̾�Ʈ��(����) ���࿡ ������ ������ �ڽ��� ������ �ǰ� ������ش�.(�� ���� ����� �ٸ� �÷��̾���� ���Ӱ����ϰ� �Ѵ�.) 
        DebugText.text=("������ Ŭ���̾�Ʈ�� ������ ������ �Ǿ����ϴ�");
        PhotonNetwork.JoinOrCreateRoom("Lobby", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
       // PhotonNetwork.JoinRandomRoom();
    }
    public override void OnDisconnected(DisconnectCause cause) // DisconnectCause (������ ���� ������ �޼ҵ� ���� ������ ������ �´�(����ȿ� ���ǰ� �Ǿ��ִ�.))
    {                                                           // ���濡�� �˾Ƽ� ȣ���� �Ѵ� override (���� �ȿ� ���ǰ� �Ǿ��ֱ� ������ �������̵� �� ���������� ��ӹ��� Ŭ������ �۵��ϱ� ������
                                                                // ����ڰ� ���� ȣ���� ���ص� �˾Ƽ� ����ȴ�.)
        base.OnDisconnected(cause);
        DebugText.text = ("������ ������ ������ϴ�." + cause);//cause �� �� ������ �����ִ��� ������ �������.
    }
    public override void OnJoinRandomFailed(short returnCode, string message)// ���� ����� �������� �̹� �����Ǿ��ִ� ���� ã�� ���Ѵٸ�(joinRandomRoom) ���� ���� �����ϴ� ����
    {
        base.OnJoinRandomFailed(returnCode, message);//base�� �ǹ̴� �θ��� �޼ҵ带 ������ͼ� �����Ǹ� �ߴٰ� �����Ϸ����� �˷���
                                                     //returnCode �����ڵ�(�� �濡 ������ ���� ���ߴ���)
                                                     //Message �� �濡 ������ ���� ���ߴ����� ���� ������ ���� ���� ������ ����.
        DebugText.text = ("�����Ǿ��ִ� ���� ã���� �����ϴ�.\n���ο� ���� �����մϴ�.\n");
        PhotonNetwork.CreateRoom("Lobby", new RoomOptions { MaxPlayers = 20}) ;//ù��° null ���� �� �̸� ������ �ϴ°���. �״��� new RoomOptions�� �ִ��÷��̾ ������� ��������

    }
    public override void OnJoinedRoom()// �濡 ������ ���������� ����Ǵ� �޼ҵ�
    {
        base.OnJoinedRoom();
        DebugText.text = ("�뿡 ���� �Ϸ�.");
        ClientChack();
    }
    public override void OnJoinedLobby()//�κ�(���濡���� �κ�� ���ϻ�������.)
    {
        base.OnJoinedLobby();
        DebugText.text = ("�� ������");
        PhotonNetwork.JoinOrCreateRoom("Lobby", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
    }
    private void ClientChack()
    {
        //������ Ŭ���̾�Ʈ�� �濡 �������ϸ� ������ �ε��� �ϴµ� �� ������ �ε��� �� ���°� ������ �ö󰡰�
       //Ŭ���̾�Ʈ�� �ش� ������ �濡 ������ �ϸ� �� ���¸� �޾ƿͼ� �˾Ƽ� ������Ŭ���̾�Ʈ�� ���ִ� ���� �ε��� ������ 

       StartCoroutine("LoadingScene");

    }
    IEnumerator LoadingScene()
    {
        DebugText.text = ("���ӿϷ� 3�ʵ� �κ�� �̵��մϴ�.");
        Debug.LogError(PhotonNetwork.CurrentRoom.Name);
        yield return new WaitForSeconds(3f);
        PhotonNetwork.LoadLevel("Lobby");

    }
}
