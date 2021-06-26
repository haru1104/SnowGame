using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class Launcher : MonoBehaviourPunCallbacks //punCollBacks 붙이는 이유는 포톤에서 지원을 하는 여러 메소드를 오타를 신경쓰지 않고 자동입력이 되게 만든 클래스
{
    private string gameVer = "0.1";//포톤 버전 설정
    private Text DebugText;
    

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;//포톤에서 자동으로 로딩된 모든 씬을 동기화해주는 프로퍼티
        DebugText = GameObject.Find("FuckingUnity").GetComponent<Text>();
    }
    public void JoinServer() 
    {
        if (PhotonNetwork.IsConnected == true)//포톤네트워크에 접속이 되어있는 상태고 클라이언트에는 접속이 되어있지 않으면 랜덤으로 룸(생성되어있는) 들어간다.
        {
            // PhotonNetwork.JoinRandomRoom(); // 포톤네트워크에 생성되어있는(이미 만들어 있는 룸.) 무작위 룸중에 하나를 골라서 접속하는것.
            //ver2//PhotonNetwork.JoinOrCreateRoom("Lobby",new RoomOptions {MaxPlayers =20},TypedLobby.Default);
            PhotonNetwork.JoinLobby();
        }
        else //만약에 포톤네트워크에 접속하지 않은 상태고 클라이언트도 접속이 안되어있는 상태면 OnConnectedToMaster 메소드를 실행해서 마스터 클라이언트로 접속
        {
            PhotonNetwork.GameVersion = gameVer;//게임 버전설정을 해준다.
            PhotonNetwork.ConnectUsingSettings();//위에서 설정한 게임버전을 사용해서 포톤 네트워크에 접속을 하겠다는 뜻
                                                //네트워크에 접속이 되어있지 않은 상태라면 온라인 으로 바꿔준다,

        }
    }
    public override void OnConnectedToMaster() // 오버라이드는 상속받은 클래스 메소드를 재정의 해서 사용을 한다. (부모메소드의 활동은 정지 자식을 사용)
    {
        base.OnConnectedToMaster();// 마스터 클라이언트를(방장) 만약에 방장이 없으면 자신이 방장이 되게 만들어준다.(새 룸을 만들고 다른 플레이어들이 접속가능하게 한다.) 
        DebugText.text=("마스터 클라이언트가 서버에 연결이 되었습니다");
        PhotonNetwork.JoinOrCreateRoom("Lobby", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
       // PhotonNetwork.JoinRandomRoom();
    }
    public override void OnDisconnected(DisconnectCause cause) // DisconnectCause (연결이 끊긴 이유를 메소드 인자 값으로 가지고 온다(포톤안에 정의가 되어있다.))
    {                                                           // 포톤에서 알아서 호출을 한다 override (포톤 안에 정의가 되어있기 때문에 오버라이드 한 시점에서는 상속받은 클래스가 작동하기 떄문에
                                                                // 사용자가 따로 호출을 안해도 알아서 실행된다.)
        base.OnDisconnected(cause);
        DebugText.text = ("서버에 접속이 끊겼습니다." + cause);//cause 는 왜 연결이 끊겨있는지 이유가 들어있음.
    }
    public override void OnJoinRandomFailed(short returnCode, string message)// 룸을 만드는 과정에서 이미 생성되어있는 방을 찾지 못한다면(joinRandomRoom) 새로 방을 생성하는 과정
    {
        base.OnJoinRandomFailed(returnCode, message);//base의 의미는 부모의 메소드를 가지고와서 재정의를 했다고 컴파일러한테 알려줌
                                                     //returnCode 오류코드(왜 방에 접속을 하지 못했는지)
                                                     //Message 왜 방에 접속을 하지 못했는지에 대한 이유에 대한 값을 가지고 있음.
        DebugText.text = ("생성되어있는 방을 찾을수 없습니다.\n새로운 방을 생성합니다.\n");
        PhotonNetwork.CreateRoom("Lobby", new RoomOptions { MaxPlayers = 20}) ;//첫번째 null 쪽은 방 이름 설정을 하는것임. 그다음 new RoomOptions은 최대플레이어가 몇명인지 설정가능

    }
    public override void OnJoinedRoom()// 방에 접속을 성공했을때 실행되는 메소드
    {
        base.OnJoinedRoom();
        DebugText.text = ("룸에 접속 완료.");
        ClientChack();
    }
    public override void OnJoinedLobby()//로비(포톤에서의 로비는 단일생성가능.)
    {
        base.OnJoinedLobby();
        DebugText.text = ("룸 생성중");
        PhotonNetwork.JoinOrCreateRoom("Lobby", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
    }
    private void ClientChack()
    {
        //마스터 클라이언트가 방에 접속을하면 레벨을 로딩을 하는데 이 레벨이 로딩을 한 상태가 서버에 올라가고
       //클라이언트는 해당 서버로 방에 접속을 하면 그 상태를 받아와서 알아서 마스터클라이언트가 들어가있는 씬에 로딩을 시켜줌 

       StartCoroutine("LoadingScene");

    }
    IEnumerator LoadingScene()
    {
        DebugText.text = ("접속완료 3초뒤 로비로 이동합니다.");
        Debug.LogError(PhotonNetwork.CurrentRoom.Name);
        yield return new WaitForSeconds(3f);
        PhotonNetwork.LoadLevel("Lobby");

    }
}
