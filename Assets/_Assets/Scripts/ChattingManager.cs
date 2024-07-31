using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.InputSystem;

public class ChattingManager : NetworkBehaviour
{
    //채팅 기능
    // Input Field에 텍스트를 써서
    // 전송 버튼을 누르면 (확장 : 엔터를 누르면)
    // 스크롤 뷰에 텍스트가 한 줄씩 표시된다.
    // 형식 : [닉네임] 채팅 내용
    // 스크롤 뷰는 텍스트 내역에 따라 자동 크기 조절이 된다.
    // 

    public TMP_InputField chatInput; // 채팅을 입력하는 인풋필드
    public TextMeshProUGUI chatText; // 채팅 내역
    public Scrollbar chatScroll; // 채팅의 스크롤바

    //채팅을 치는 동안에는 플레이어가 움직이지 않도록 한다.

    private void Update()
    {
        //채팅 UI를 선택 중이라면
        if (chatInput.isFocused)
        {
            //로컬 플레이어가 움직이지 않도록 한다.
            SessionPlayer.localPlayer.GetComponent<PlayerInput>().enabled = false;
            //플레이어의 말풍선을 활성화한다. => 모두한테 보여야 한다 => 서버
            SessionPlayer.localPlayer.CmdActivateBubble(true);
        }
        else
        {
            SessionPlayer.localPlayer.GetComponent<PlayerInput>().enabled = true;
            SessionPlayer.localPlayer.CmdActivateBubble(false);
        }
    }

    //네트워크용 채팅
    // 채팅 글씨가 올라온다. => 모두가 실행
    // 무엇을 기입했는지? => 본인 & 서버만 알면 된다.

    public void ChatOnEndEdit(string input)
    {
        //엔터를 누르는 지 체크 + 인풋창을 사용자가 선택중일 때
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            OnClickSendBtn();
        }
    }

    //채팅을 보내는 함수
    public void OnClickSendBtn()
    {
        //만약 chatInput에 들어있는 텍스트가 없으면
        //함수를 실행하지 않는다.
        // Trim() : 중간에 스페이스 바가 있으면 삭제해준다.
        //hello world => helloworld
        if (chatInput.text.Trim().Length < 1)
        {
            chatInput.text = "";
            return;
        }

        //텍스트를 올린다.
        CmdSendChatting(DBManager.instance.playerName, chatInput.text);

        //채팅을 보낸 후 chatInput 내의 텍스트를 삭제한다.
        chatInput.text = "";

        // 인풋 UI를 계속 선택한 상태로 유지한다.
        chatInput.ActivateInputField();

        //스크롤바를 최대한 (최신 텍스트)로 맞춘다.
        chatScroll.value = 0;
    }

    [Command(requiresAuthority = false)]
    //서버로 무엇을 채팅으로 쳤는지 정보를 보낸다.
    private void CmdSendChatting(string _playerName, string _chatMessage)
    {
        //서버한테 요청할 내용 : 내가 친 텍스트(chatMessage)를 모두의 텍스트창에 추가한다.
        RpcSendChatting(_playerName, _chatMessage);
    }

    [ClientRpc]
    private void RpcSendChatting(string _playerName, string _chatMessage)
    {
        //버튼을 누르면
        //ChatText에
        //chatInput 내용이 추가되어야 한다.

        //내가 쓴 채팅은 닉네임이 파란색,
        //남이 쓴 채팅은 닉네임이 회색

        //받아온 플레이어 이름이 내 이름과 일치한다 = 내 플레이어다.
        //이름이 일치하지 않는다 = 남의 플레이어다.
         
        if(_playerName == DBManager.instance.playerName)
        {
            chatText.text += $"<color=blue><b>[{_playerName}]</b></color> {_chatMessage}\n";
        }
        else
        {
            chatText.text += $"<color=#BDBDBD><b>[{_playerName}]</b></color> {_chatMessage}\n";
        }

    }
}
