using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject serverPanel;
    public GameObject clientPanel;
    public GameObject loginPanel;
    public GameObject signinPanel;

    // Start is called before the first frame update
    void Start()
    {
        switch (MainManager.Instance.authStatus)
        {
            case MainManager.ClientStatus.SERVER:
                serverPanel.SetActive(true);
                clientPanel.SetActive(false);
                loginPanel.SetActive(false);
                break;
            case MainManager.ClientStatus.CLIENT:
                serverPanel.SetActive(false);
                clientPanel.SetActive(true);
                loginPanel.SetActive(true);
                break;
            case MainManager.ClientStatus.MASTERCLIENT:
                serverPanel.SetActive(true);
                clientPanel.SetActive(false);
                loginPanel.SetActive(true);
                break;
        }

        //회원가입창은 항상 시작 시에 안 보이도록
        signinPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 일반 클라이언트 권한자의 룸 접속 기능
    public void RunStartClient()
    {
        //로그인 버튼을 눌렀을 때 실행
        //로그인 시도 이후, 로그인 성공 시 접속 함수를 실행.

        // 클라이언트 상태 변경
        MainManager.Instance.clientStatus = MainManager.ClientStatus.CLIENT;
        // 로비씬 접속 정보
        MainManager.Instance.nextSceneNumber = 2;

        // 로딩씬 실행
        SceneManager.LoadSceneAsync("00 LOADING");
    }

    //회원가입 버튼
    public void OnClickSigninBtn()
    {
        //로그인창을 비활성화 하고
        loginPanel.SetActive(false);
        //회원가입창을 활성화한다.
        signinPanel.SetActive(true);
    }

}
