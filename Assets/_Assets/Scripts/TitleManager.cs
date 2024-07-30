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

        //ȸ������â�� �׻� ���� �ÿ� �� ���̵���
        signinPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �Ϲ� Ŭ���̾�Ʈ �������� �� ���� ���
    public void RunStartClient()
    {
        //�α��� ��ư�� ������ �� ����
        //�α��� �õ� ����, �α��� ���� �� ���� �Լ��� ����.

        // Ŭ���̾�Ʈ ���� ����
        MainManager.Instance.clientStatus = MainManager.ClientStatus.CLIENT;
        // �κ�� ���� ����
        MainManager.Instance.nextSceneNumber = 2;

        // �ε��� ����
        SceneManager.LoadSceneAsync("00 LOADING");
    }

    //ȸ������ ��ư
    public void OnClickSigninBtn()
    {
        //�α���â�� ��Ȱ��ȭ �ϰ�
        loginPanel.SetActive(false);
        //ȸ������â�� Ȱ��ȭ�Ѵ�.
        signinPanel.SetActive(true);
    }

}
