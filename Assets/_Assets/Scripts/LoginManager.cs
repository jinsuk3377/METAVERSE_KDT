using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    //로그인, 회원가입과 관련된 모든 기능

    //임시 : 닉네임을 입력받아 다음 씬으로 넘긴다.
    public TitleManager titleManager;

    [Header("Login UI")]
    public TMP_InputField idInput;
    public TMP_InputField pwInput;

    [Header("Sign in UI")]
    public TMP_InputField signIDInput;
    public TMP_InputField signPWInput;
    public TMP_InputField signNicknameInput;

    //로그인 버튼
    public void OnClickLoginBtn()
    {
        //로그인 버튼을 눌렀을 때
        //로그인 처리 이후

        //닉네임 정보 넘기기

        //ID와 PW를 인풋으로부터 받아온다.
        //ID와 PW 중 하나라도 적혀있지 않으면 디버그 로그 송출.
        //로그인을 하지 않는다.

        if (idInput.text.Length < 1 || pwInput.text.Length < 1)
        {
            Debug.Log("아이디나 패스워드를 입력하지 않았습니다.");
            return; // 함수를 여기서 종료.
        }

        //모두 입력했을 때에는
        //ID와 PW 값으로 해당 유저가 데이터베이스에 있는지 체크,
        //있을 경우 해당 유저의 닉네임을 가져온다.
        //DB 매니저의 인스턴스 함수를 만들어 진행.
        DBManager.User user = new DBManager.User();
        bool isSucesses = user.CheckNickname(idInput.text, pwInput.text);

        //데이터베이스 조회에 성공했으며 (isSuccess = true) 유저 닉네임 값을 뭐라도 받아왔을 경우
        if(isSucesses == true && user.nickname.Length > 0)
        {
            //플레이어 네임을 설정하고 클라이언트를 실행한다.
            DBManager.instance.playerName = user.nickname;
            //클라이언트를 시작
            titleManager.RunStartClient();
        }
    }

    //회원가입 기능
    //아이디, 비밀번호, 닉네임 입력을 받아와
    //해당 정보를 데이터베이스에 등록한다.
    //등록에 성공할 경우 닉네임은 playerName에 저장하고 바로 로그인 접속.

    public void SignIn()
    {
        //아이디, 비밀번호, 닉네임 중 하나라도 입력되어 있지 않으면 함수 종료.
        if(signIDInput.text.Length < 1 || signPWInput.text.Length < 1 || signNicknameInput.text.Length < 1)
        {
            return;
        }

        //회원가입 진행. => SQL 문법
        //위에 입력한 데이터를 DB에 새로운 데이터로 추가한다. (Insert 문)
        string inputID = signIDInput.text;
        string inputPW = signPWInput.text;
        string inputNickname = signNicknameInput.text;

        //데이터 등록은 DBManager에서 한다.
        DBManager.User user = new();
        bool isSuccess = user.RegistNewUser(inputID, inputPW, inputNickname);

        if (isSuccess)
        {
            DBManager.instance.playerName = inputNickname;
            titleManager.RunStartClient();
        }
    }
    
}
