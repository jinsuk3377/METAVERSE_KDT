using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SearchService;

public enum ServerNum
{
    //포트 번호 고정
    LOADING,    // 0 - (오프라인이라 필요 x)
    TITLE,      // 1 - (오프라인이라 필요 x)
    LOBBY,      // 2 - 7777
    ROOM1       // 3 - 7778 ~ 룸의 갯수만큼 +a
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
    public string moveAddr = "localhost";

    [Header("Port 정보")]
    public const int DEFAULTPORT = 7777;
    public const int LOBBYPORT = 7777;
    public const int ROOM1PORT = 7778;
    //유동적으로 변경이 가능한 포트 변수
    public int MoveScenePort; // 이동할 씬의 포트

    [Header("Room 접속 정보")]
    public int connectionRoomIndex = -1; // 방에 접속중일 경우 접속중인 방의 uuid
    public bool isRoomMaster = false; // 본인이 방의 방장인지 여부

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
