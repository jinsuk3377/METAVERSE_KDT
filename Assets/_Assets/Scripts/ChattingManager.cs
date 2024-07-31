using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.InputSystem;

public class ChattingManager : NetworkBehaviour
{
    //ä�� ���
    // Input Field�� �ؽ�Ʈ�� �Ἥ
    // ���� ��ư�� ������ (Ȯ�� : ���͸� ������)
    // ��ũ�� �信 �ؽ�Ʈ�� �� �پ� ǥ�õȴ�.
    // ���� : [�г���] ä�� ����
    // ��ũ�� ��� �ؽ�Ʈ ������ ���� �ڵ� ũ�� ������ �ȴ�.
    // 

    public TMP_InputField chatInput; // ä���� �Է��ϴ� ��ǲ�ʵ�
    public TextMeshProUGUI chatText; // ä�� ����
    public Scrollbar chatScroll; // ä���� ��ũ�ѹ�

    //ä���� ġ�� ���ȿ��� �÷��̾ �������� �ʵ��� �Ѵ�.

    private void Update()
    {
        //ä�� UI�� ���� ���̶��
        if (chatInput.isFocused)
        {
            //���� �÷��̾ �������� �ʵ��� �Ѵ�.
            SessionPlayer.localPlayer.GetComponent<PlayerInput>().enabled = false;
            //�÷��̾��� ��ǳ���� Ȱ��ȭ�Ѵ�. => ������� ������ �Ѵ� => ����
            SessionPlayer.localPlayer.CmdActivateBubble(true);
        }
        else
        {
            SessionPlayer.localPlayer.GetComponent<PlayerInput>().enabled = true;
            SessionPlayer.localPlayer.CmdActivateBubble(false);
        }
    }

    //��Ʈ��ũ�� ä��
    // ä�� �۾��� �ö�´�. => ��ΰ� ����
    // ������ �����ߴ���? => ���� & ������ �˸� �ȴ�.

    public void ChatOnEndEdit(string input)
    {
        //���͸� ������ �� üũ + ��ǲâ�� ����ڰ� �������� ��
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            OnClickSendBtn();
        }
    }

    //ä���� ������ �Լ�
    public void OnClickSendBtn()
    {
        //���� chatInput�� ����ִ� �ؽ�Ʈ�� ������
        //�Լ��� �������� �ʴ´�.
        // Trim() : �߰��� �����̽� �ٰ� ������ �������ش�.
        //hello world => helloworld
        if (chatInput.text.Trim().Length < 1)
        {
            chatInput.text = "";
            return;
        }

        //�ؽ�Ʈ�� �ø���.
        CmdSendChatting(DBManager.instance.playerName, chatInput.text);

        //ä���� ���� �� chatInput ���� �ؽ�Ʈ�� �����Ѵ�.
        chatInput.text = "";

        // ��ǲ UI�� ��� ������ ���·� �����Ѵ�.
        chatInput.ActivateInputField();

        //��ũ�ѹٸ� �ִ��� (�ֽ� �ؽ�Ʈ)�� �����.
        chatScroll.value = 0;
    }

    [Command(requiresAuthority = false)]
    //������ ������ ä������ �ƴ��� ������ ������.
    private void CmdSendChatting(string _playerName, string _chatMessage)
    {
        //�������� ��û�� ���� : ���� ģ �ؽ�Ʈ(chatMessage)�� ����� �ؽ�Ʈâ�� �߰��Ѵ�.
        RpcSendChatting(_playerName, _chatMessage);
    }

    [ClientRpc]
    private void RpcSendChatting(string _playerName, string _chatMessage)
    {
        //��ư�� ������
        //ChatText��
        //chatInput ������ �߰��Ǿ�� �Ѵ�.

        //���� �� ä���� �г����� �Ķ���,
        //���� �� ä���� �г����� ȸ��

        //�޾ƿ� �÷��̾� �̸��� �� �̸��� ��ġ�Ѵ� = �� �÷��̾��.
        //�̸��� ��ġ���� �ʴ´� = ���� �÷��̾��.
         
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
