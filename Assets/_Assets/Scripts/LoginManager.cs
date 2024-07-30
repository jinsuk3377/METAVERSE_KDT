using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    //�α���, ȸ�����԰� ���õ� ��� ���

    //�ӽ� : �г����� �Է¹޾� ���� ������ �ѱ��.
    public TitleManager titleManager;

    [Header("Login UI")]
    public TMP_InputField idInput;
    public TMP_InputField pwInput;

    [Header("Sign in UI")]
    public TMP_InputField signIDInput;
    public TMP_InputField signPWInput;
    public TMP_InputField signNicknameInput;

    //�α��� ��ư
    public void OnClickLoginBtn()
    {
        //�α��� ��ư�� ������ ��
        //�α��� ó�� ����

        //�г��� ���� �ѱ��

        //ID�� PW�� ��ǲ���κ��� �޾ƿ´�.
        //ID�� PW �� �ϳ��� �������� ������ ����� �α� ����.
        //�α����� ���� �ʴ´�.

        if (idInput.text.Length < 1 || pwInput.text.Length < 1)
        {
            Debug.Log("���̵� �н����带 �Է����� �ʾҽ��ϴ�.");
            return; // �Լ��� ���⼭ ����.
        }

        //��� �Է����� ������
        //ID�� PW ������ �ش� ������ �����ͺ��̽��� �ִ��� üũ,
        //���� ��� �ش� ������ �г����� �����´�.
        //DB �Ŵ����� �ν��Ͻ� �Լ��� ����� ����.
        DBManager.User user = new DBManager.User();
        bool isSucesses = user.CheckNickname(idInput.text, pwInput.text);

        //�����ͺ��̽� ��ȸ�� ���������� (isSuccess = true) ���� �г��� ���� ���� �޾ƿ��� ���
        if(isSucesses == true && user.nickname.Length > 0)
        {
            //�÷��̾� ������ �����ϰ� Ŭ���̾�Ʈ�� �����Ѵ�.
            DBManager.instance.playerName = user.nickname;
            //Ŭ���̾�Ʈ�� ����
            titleManager.RunStartClient();
        }
    }

    //ȸ������ ���
    //���̵�, ��й�ȣ, �г��� �Է��� �޾ƿ�
    //�ش� ������ �����ͺ��̽��� ����Ѵ�.
    //��Ͽ� ������ ��� �г����� playerName�� �����ϰ� �ٷ� �α��� ����.

    public void SignIn()
    {
        //���̵�, ��й�ȣ, �г��� �� �ϳ��� �ԷµǾ� ���� ������ �Լ� ����.
        if(signIDInput.text.Length < 1 || signPWInput.text.Length < 1 || signNicknameInput.text.Length < 1)
        {
            return;
        }

        //ȸ������ ����. => SQL ����
        //���� �Է��� �����͸� DB�� ���ο� �����ͷ� �߰��Ѵ�. (Insert ��)
        string inputID = signIDInput.text;
        string inputPW = signPWInput.text;
        string inputNickname = signNicknameInput.text;

        //������ ����� DBManager���� �Ѵ�.
        DBManager.User user = new();
        bool isSuccess = user.RegistNewUser(inputID, inputPW, inputNickname);

        if (isSuccess)
        {
            DBManager.instance.playerName = inputNickname;
            titleManager.RunStartClient();
        }
    }
    
}
