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

        //������ ���� ���� ��� = �� �ο� ��ü�� ������.
        if (MainManager.Instance.isRoomMaster == true)
        {
            //DB ���� => ���� ������ �ʱ�ȭ.
            //�������� ���� �̸� = null, ���尴 ���� 0������ �����.
            session.RemoveRoomInfo(MainManager.Instance.connectionRoomIndex);

            //������ �������� ��û�Ѵ�. => �� �濡 �ִ� ��� ������� [�濡�� ������] �Լ��� �����Ű����.
            //������ ��� �����ڿ��Լ� [�濡�� ������] �Լ��� ����.
            CmdAllLeaveRoom();
        }
        else // �Ϲ� ������
        {
            
            //���� �������� ���� DB ���� �ο����� 1 �����Ѵ�.
            session.DecreasePeopleNum(MainManager.Instance.connectionRoomIndex);

            //���� ��� �ʿ���� ���θ� ������ ������ �ȴ�.
            LeaveRoom();
            
        }

        
    }

    //������ �������� ��û�ϴ� �Լ� (Command)
    [Command(requiresAuthority = false)]
    private void CmdAllLeaveRoom()
    {
        //��ΰ� �濡�� ������ �ش޶�� ��û.
        //���� ���ε� �����ؼ� ����
        RpcLeaveRoom();
    }

    //�濡�� ������ ��� => �������� ������ �� �ִ� �Լ�.
    [ClientRpc]
    private void RpcLeaveRoom()
    {
        //������ LeaveRoom ����
        LeaveRoom();
    }

    //���÷� ����
    private void LeaveRoom()
    {
        // ���� ȥ�� [�濡�� ������]
        MainManager.Instance.MoveScenePort = MainManager.LOBBYPORT;
        MainManager.Instance.nextSceneNumber = 2; // �κ��

        //ȣ��Ʈ, ������ �̵��� �Ұ���. => �̵��ϴ� ���� ������ �ִ� ���� ������ ������.
        //Ŭ���̾�Ʈ�θ� �̵� ����.

        //���� �������� ���� uuid�� �ʱ�ȭ��Ų��.
        MainManager.Instance.connectionRoomIndex = -1;
        MainManager.Instance.isRoomMaster = false;

        //StopClient : ���� - �÷��̾��� ���� ���� ���� �Լ�.
        // ������ ���� ���Ŀ� ���ο� ������ ������ ����������.
        NetworkManager.singleton.StopClient();
        SceneManager.LoadSceneAsync("00 LOADING");
    }
}
