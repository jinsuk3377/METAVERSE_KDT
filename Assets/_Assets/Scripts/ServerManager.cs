using UnityEngine;
using TMPro;
using Mirror;
using kcp2k;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviour
{
    [Header("Network Setting")]
    public TMP_InputField ipAddrInput;
    public TMP_InputField portInput;

    [Header("Network Connecting")]
    public TMP_Text infoTxt;

    private int serverNum;
    private string infoString;

    private KcpTransport transport;


    void Awake()
    {
        transport = FindObjectOfType<KcpTransport>();
    }

    private void OnEnable()
    {
        ipAddrInput.text = NetworkManager.singleton.networkAddress;
        portInput.text = transport.Port.ToString();
        ResetNetworkSetting();
    }

    public void ResetNetworkSetting ()
    {
        NetworkManager.singleton.networkAddress = MainManager.IPADDR;
        transport.Port = MainManager.DEFAULTPORT;

        ipAddrInput.text = MainManager.IPADDR;
        portInput.text = MainManager.DEFAULTPORT.ToString();

        SetNetworkSetting();
    }

    public void SetNetworkSetting ()
    {
        // ���� ���� �ѹ� ������ ���ٸ� 2�� Scene ����
        serverNum = 2;

        string str = null;
        str = "SELECT : 2";
        str += System.Environment.NewLine + "IP ADDR : " + ipAddrInput.text;
        NetworkManager.singleton.networkAddress = ipAddrInput.text;

        // Port InputField �Է°� ushort ��ȯ�˻�
        ushort val = 0;
        if (ushort.TryParse(portInput.text, out val))
        {
            transport.Port = val;
        }
        else
        {
            transport.Port = 0;
        }

        str += System.Environment.NewLine + "PORT : " + val.ToString();

        infoTxt.text = str;
    }

    // ���� ������ �� ��ư Ŭ����
    public void SetServerNum (int serverNum)
    {
        string str = null;

        switch (serverNum)
        {
            case 2:
                this.serverNum = serverNum;
                str = "SELECT : " + ServerNum.LOBBY.ToString();
                str += System.Environment.NewLine + "IP ADDR : " + ipAddrInput.text;
                str += System.Environment.NewLine + "PORT : " + MainManager.LOBBYPORT.ToString();

                NetworkManager.singleton.networkAddress = ipAddrInput.text;
                transport.Port = MainManager.LOBBYPORT;

                MainManager.Instance.nextSceneNumber = serverNum;
                NetworkManager.singleton.onlineScene = MainManager.Instance.scenes[0];
                
                break;
            case 3:
                this.serverNum = serverNum;
                str = "SELECT : " + ServerNum.ROOM1.ToString();
                str += System.Environment.NewLine + "IP ADDR : " + ipAddrInput.text;
                str += System.Environment.NewLine + "PORT : " + MainManager.ROOM1PORT.ToString();

                NetworkManager.singleton.networkAddress = ipAddrInput.text;
                transport.Port = MainManager.ROOM1PORT;

                MainManager.Instance.nextSceneNumber = serverNum;
                NetworkManager.singleton.onlineScene = MainManager.Instance.scenes[1];
                break;
            default:
                str = "SELECT : ";
                str += System.Environment.NewLine + "IP ADDR : " + ipAddrInput.text;
                str += System.Environment.NewLine + "PORT : " + transport.Port.ToString();
                break;
        }

        infoTxt.text = str;
    }

    // ���� ����
    public void RunStartServer ()
    {
        MainManager.Instance.clientStatus = MainManager.ClientStatus.SERVER;

        // �ε��� ����
        SceneManager.LoadSceneAsync("00 LOADING");
    }

    // Ŭ���̾�Ʈ ����
    public void RunStartClient ()
    {
        MainManager.Instance.clientStatus = MainManager.ClientStatus.CLIENT;

        // �ε��� ����
        SceneManager.LoadSceneAsync("00 LOADING");
    }

    // Ŭ���̾�Ʈ ����
    public void RunStartHost ()
    {
        MainManager.Instance.clientStatus = MainManager.ClientStatus.MASTERCLIENT;

        // �ε��� ����
        SceneManager.LoadSceneAsync("00 LOADING");
    }
}
