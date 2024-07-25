using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum ServerNum
{
    LOADING,    // 0
    TITLE,      // 1
    LOBBY,      // 2
    ROOM1       // 3
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

    [Header("Port ����")]
    public const int DEFAULTPORT = 7777;
    public const int LOBBYPORT = 7777;
    public const int ROOM1PORT = 7778;

    void Awake()
    {
        if (_instance == null) _instance = FindObjectOfType<MainManager>();
        if (_instance != this)  Destroy(this);
        else                    DontDestroyOnLoad(this);
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
