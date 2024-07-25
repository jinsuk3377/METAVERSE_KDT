using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using kcp2k;

public class ServerNetManager : MonoBehaviour
{
    public GameObject serverPanel;
    public GameObject serverStatusPanel;
    public GameObject clientStatusPanel;
    public GameObject hostStatusPanel;

    public TMP_Text infoTxt;

    private KcpTransport transport;

    // Start is called before the first frame update
    void Start()
    {
        transport = FindObjectOfType<KcpTransport>();

        switch (MainManager.Instance.authStatus)
        {
            case MainManager.ClientStatus.SERVER:
                serverPanel.SetActive(true);
                break;
            case MainManager.ClientStatus.CLIENT:
                serverPanel.SetActive(false);
                break;
            case MainManager.ClientStatus.MASTERCLIENT:
                serverPanel.SetActive(true);
                break;
        }

        switch (MainManager.Instance.clientStatus)
        {
            case MainManager.ClientStatus.SERVER:
                serverStatusPanel.SetActive(true);
                clientStatusPanel.SetActive(false);
                hostStatusPanel.SetActive(false);
                break;
            case MainManager.ClientStatus.CLIENT:
                serverStatusPanel.SetActive(false);
                clientStatusPanel.SetActive(true);
                hostStatusPanel.SetActive(false);
                break;
            case MainManager.ClientStatus.MASTERCLIENT:
                serverStatusPanel.SetActive(false);
                clientStatusPanel.SetActive(false);
                hostStatusPanel.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PrintStatus();
    }

    void PrintStatus()
    {
        // 1. Host 라면
        if (NetworkServer.active && NetworkClient.active)
        {
            infoTxt.text = $"<b>HOST<b> : running {Transport.active} port {transport.Port}";
        }

        // 2. Server only 라면
        else if (NetworkServer.active)
        {
            infoTxt.text = $"<b>SERVER<b> : running {Transport.active} port {transport.Port}";
        }

        // 3. 클라이언트 라면
        else if (NetworkClient.active)
        {
            infoTxt.text = $"<b>CLIENT<b> : connected to {NetworkManager.singleton.networkAddress} " +
                           $"running {Transport.active} port {transport.Port}";
        }

        else
        {
            infoTxt.text = $"running {Transport.active} port {transport.Port}";
        }
    }

    public void StopServer()
    {
        MainManager.Instance.nextSceneNumber = 1;
        NetworkManager.singleton.StopServer();

        SceneManager.LoadSceneAsync("00 LOADING");
    }

    public void StopClient()
    {
        MainManager.Instance.nextSceneNumber = 1;
        NetworkManager.singleton.StopClient();

        SceneManager.LoadSceneAsync("00 LOADING");
    }

    public void StopHost()
    {
        MainManager.Instance.nextSceneNumber = 1;
        NetworkManager.singleton.StopHost();

        SceneManager.LoadSceneAsync("00 LOADING");
    }

    public void StopHostClient()
    {
        // HOST -> SERVER 접속상태 변경
        MainManager.Instance.clientStatus = MainManager.ClientStatus.SERVER;

        NetworkManager.singleton.StopClient();

        serverStatusPanel.SetActive(true);
        clientStatusPanel.SetActive(false);
        hostStatusPanel.SetActive(false);
    }
}
