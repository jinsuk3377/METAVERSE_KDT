using Mirror;
using UnityEngine;
using StarterAssets;
using Cinemachine;
using TMPro;

public class SessionPlayer : NetworkBehaviour
{
   
    public Transform playerCameraRoot;
    public TextMeshProUGUI playerNameTxt; // 플레이어 닉네임 표시창
    public static SessionPlayer localPlayer;

    //닉네임이 수정되면 자동으로 함수를 실행하도록 한다.
    //싱크바 변수는 Command 내에서 값이 바뀔 때에만 hook 함수를 실행한다.
    [SyncVar(hook = nameof(OnChangeNickname))]private string nickname;

    // Start is called before the first frame update
    void Start()
    {
    }

    //이 오브젝트가 본인이 플레이어일 때 실행하는 함수
    // 확인 : https://mirror-networking.gitbook.io/docs/manual/components/networkbehaviour
    public override void OnStartLocalPlayer()
    {
        //캐릭터의 개별적인 움직임을 설정 : 내 키보드로 제어 가능,
        //카메라가 내 캐릭터를 따라다니도록
        GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
        GetComponent<ThirdPersonController>().enabled = true;
        FindObjectOfType<CinemachineVirtualCamera>().Follow = playerCameraRoot;

        //DB매니저에 저장된 닉네임 값을 서버로 전송한다.
        CmdSetNickName(DBManager.instance.playerName);
        localPlayer = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Command 함수를 통해 서버에게 내 닉네임 정보를 전달한다.
    //커맨드 함수의 경우 Cmd로 시작. (미러 네트워크에서 권장)
    [Command]
    private void CmdSetNickName(string _nickName)
    {
        // Command + SyncVar 변수 활용.
        // 플레이어의 이름을 등록하는 즉시 다른 사람들한테 적용되도록 SyncVar 활용.
        //서버에 접속한 플레이어의 닉네임을 저장
        nickname = _nickName;
    }

    //닉네임 싱크바 함수
    private void OnChangeNickname(string _old, string _new)
    {
        //old : 값이 바뀌기 이전의 변수값
        //new : 바뀐 이후의 변수값
        //내 플레이어의 닉네임 창에 DBManager의 playerName을 등록.
        //방에 참여중인 모든 참여자의 (접속한 플레이어 오브젝트의 UI)를 바꿔준다.
        playerNameTxt.text = _new;
    }


}
