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

    // �Ϲ� Ŭ���̾�Ʈ �������� �� ���� ���
    public void RunStartClient()
    {
        // Ŭ���̾�Ʈ ���� ����
        MainManager.Instance.clientStatus = MainManager.ClientStatus.CLIENT;
        // �κ�� ���� ����
        MainManager.Instance.nextSceneNumber = 2;
        NetworkManager.singleton.onlineScene = MainManager.Instance.scenes[0];
        // �ε��� ����
        SceneManager.LoadSceneAsync("00 LOADING");
    }
}
