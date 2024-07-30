using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCliceLeaveBtn()
    {
        DBManager.Session session = new(); 

        //방장이 방을 나갈 경우 = 방 인원 전체가 나간다.
        if (MainManager.Instance.isRoomMaster == true)
        {
            //DB 갱신 => 방의 정보를 초기화.
            //입장중인 방의 이름 = null, 입장객 수는 0명으로 만든다.
            session.RemoveRoomInfo(MainManager.Instance.connectionRoomIndex);

            //방장이 서버한테 요청한다. => 이 방에 있는 모든 사람에게 [방에서 나간다] 함수를 실행시키도록.
            //서버가 모든 참가자에게서 [방에서 나간다] 함수를 실행.
            CmdAllLeaveRoom();
        }
        else // 일반 참가자
        {
            
            //현재 접속중인 방의 DB 참여 인원수를 1 감소한다.
            session.DecreasePeopleNum(MainManager.Instance.connectionRoomIndex);

            //서버 통신 필요없이 본인만 밖으로 나가면 된다.
            LeaveRoom();
            
        }

        
    }

    //방장이 서버한테 요청하는 함수 (Command)
    [Command(requiresAuthority = false)]
    private void CmdAllLeaveRoom()
    {
        //모두가 방에서 나가게 해달라고 요청.
        //방장 본인도 포함해서 퇴출
        RpcLeaveRoom();
    }

    //방에서 나가는 기능 => 여러명이 실행할 수 있는 함수.
    [ClientRpc]
    private void RpcLeaveRoom()
    {
        //서버로 LeaveRoom 실행
        LeaveRoom();
    }

    //로컬로 실행
    private void LeaveRoom()
    {
        // 본인 혼자 [방에서 나간다]
        MainManager.Instance.MoveScenePort = MainManager.LOBBYPORT;
        MainManager.Instance.nextSceneNumber = 2; // 로비씬

        //호스트, 서버는 이동이 불가능. => 이동하는 순간 이전에 있던 씬의 서버가 닫힌다.
        //클라이언트로만 이동 가능.

        //현재 접속중인 방의 uuid를 초기화시킨다.
        MainManager.Instance.connectionRoomIndex = -1;
        MainManager.Instance.isRoomMaster = false;

        //StopClient : 서버 - 플레이어의 연결 고리를 끊는 함수.
        // 연결을 끊은 이후에 새로운 서버에 접속이 가능해진다.
        NetworkManager.singleton.StopClient();
        SceneManager.LoadSceneAsync("00 LOADING");
    }
}
