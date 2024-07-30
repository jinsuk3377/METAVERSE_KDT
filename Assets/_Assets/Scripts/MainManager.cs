using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SearchService;

public enum ServerNum
{
    //��Ʈ ��ȣ ����
    LOADING,    // 0 - (���������̶� �ʿ� x)
    TITLE,      // 1 - (���������̶� �ʿ� x)
    LOBBY,      // 2 - 7777
    ROOM1       // 3 - 7778 ~ ���� ������ŭ +a
}

public class MainManager : MonoBehaviour
{
    private static MainManager _instance;
    public static MainManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<MainManager>();
            return _instance;
        }
    }

    public enum ClientStatus
    {
        SERVER,         // ����
        CLIENT,         // �Ϲ� �����
        MASTERCLIENT    // �����ڿ�
    }

    // ���� �ʱ� ����� ���α׷� ��� ��ü ����
    [Header("���� ���� ����")]
    public ClientStatus authStatus;

    // ��Ÿ�ӽ� ���α׷� ��� ��ü ����
    [Header("Ŭ���̾�Ʈ ����")]
    public ClientStatus clientStatus;

    [Header("Scene ����")]
    public int nextSceneNumber;
    [Scene]
    public string[] scenes = new string[2];

    [Header("IP ����")]
    public const string IPADDR = "localhost";
    public string moveAddr = "localhost";

    [Header("Port ����")]
    public const int DEFAULTPORT = 7777;
    public const int LOBBYPORT = 7777;
    public const int ROOM1PORT = 7778;
    //���������� ������ ������ ��Ʈ ����
    public int MoveScenePort; // �̵��� ���� ��Ʈ

    [Header("Room ���� ����")]
    public int connectionRoomIndex = -1; // �濡 �������� ��� �������� ���� uuid
    public bool isRoomMaster = false; // ������ ���� �������� ����

    void Awake()
    {
        if (_instance == null) _instance = FindObjectOfType<MainManager>();
        if (_instance != this)  Destroy(this);
        else                    DontDestroyOnLoad(this);
        MoveScenePort = 7777;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
