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
        SERVER,         // 서버
        CLIENT,         // 일반 사용자
        MASTERCLIENT    // 개발자용
    }

    // 빌드 초기 실행시 프로그램 사용 주체 설정
    [Header("실행 권한 상태")]
    public ClientStatus authStatus;

    // 런타임시 프로그램 사용 주체 설정
    [Header("클라이언트 상태")]
    public ClientStatus clientStatus;

    [Header("Scene 정보")]
    public int nextSceneNumber;
    [Scene]
    public string[] scenes = new string[2];

    [Header("IP 정보")]
    public const string IPADDR = "localhost";

    [Header("Port 정보")]
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
