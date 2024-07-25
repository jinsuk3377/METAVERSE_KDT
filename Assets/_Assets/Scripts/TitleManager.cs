using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject serverPanel;
    public GameObject clientPanel;

    // Start is called before the first frame update
    void Start()
    {
        switch (MainManager.Instance.authStatus)
        {
            case MainManager.ClientStatus.SERVER:
                serverPanel.SetActive(true);
                clientPanel.SetActive(false);
                break;
            case MainManager.ClientStatus.CLIENT:
                serverPanel.SetActive(false);
                clientPanel.SetActive(true);
                break;
            case MainManager.ClientStatus.MASTERCLIENT:
                serverPanel.SetActive(true);
                clientPanel.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 일반 클라이언트 권한자의 룸 접속 기능
    public void RunStartClient()
    {
        // 클라이언트 상태 변경
        MainManager.Instance.clientStatus = MainManager.ClientStatus.CLIENT;
        // 로비씬 접속 정보
        MainManager.Instance.nextSceneNumber = 2;
        NetworkManager.singleton.onlineScene = MainManager.Instance.scenes[0];
        // 로딩씬 실행
        SceneManager.LoadSceneAsync("00 LOADING");
    }
}
