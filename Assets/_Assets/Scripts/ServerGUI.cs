using kcp2k;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServerGUI : MonoBehaviour
{
    [Header("Network Setting")]
    public TMP_InputField ipAddrInput;
    public TMP_InputField portInput;

    [Header("Network Connecting")]
    public TMP_Text infoTxt;

    private KcpTransport transport;

    void Awake()
    {
        transport = FindObjectOfType<KcpTransport>();
    }

    private void OnEnable()
    {
        ipAddrInput.text = MainManager.IPADDR;
        portInput.text = transport.Port.ToString();
        ResetNetworkSetting();
    }


    public void ResetNetworkSetting()
    {
        NetworkManager.singleton.networkAddress = MainManager.IPADDR;
        transport.Port = MainManager.DEFAULTPORT;

        ipAddrInput.text = MainManager.IPADDR;
        portInput.text = MainManager.DEFAULTPORT.ToString();

        infoTxt.text = ServerManager.instance.SetNetworkSetting(ipAddrInput.text, portInput.text);

    }


    // 사전 설정된 룸 버튼 클릭시
    public void SetServerNum(int serverNum)
    {
        string str = null;

        switch (serverNum)
        {
            case 2:
                ServerManager.instance.serverNum = serverNum;
                str = "SELECT : " + ServerNum.LOBBY.ToString();
                str += System.Environment.NewLine + "IP ADDR : " + ipAddrInput.text;
                str += System.Environment.NewLine + "PORT : " + MainManager.LOBBYPORT.ToString();

                MainManager.Instance.moveAddr = ipAddrInput.text;
                MainManager.Instance.MoveScenePort = MainManager.LOBBYPORT;

                MainManager.Instance.nextSceneNumber = serverNum;

                break;
            case 3:
                ServerManager.instance.serverNum = serverNum;
                str = "SELECT : " + ServerNum.ROOM1.ToString();
                str += System.Environment.NewLine + "IP ADDR : " + ipAddrInput.text;
                str += System.Environment.NewLine + "PORT : " + portInput.text;

               MainManager.Instance.moveAddr = ipAddrInput.text;

                ushort val = 0;
                if (ushort.TryParse(portInput.text, out val))
                {
                    MainManager.Instance.MoveScenePort = val;
                }
                

                MainManager.Instance.nextSceneNumber = serverNum;
                break;
            default:
                str = "SELECT : ";
                str += System.Environment.NewLine + "IP ADDR : " + ipAddrInput.text;
                str += System.Environment.NewLine + "PORT : " + transport.Port.ToString();
                break;
        }

        infoTxt.text = str;
    }

    public void OnClickServerBtn()
    {
        ServerManager.instance.RunStartServer();
    }

    public void OnClickClientBtn()
    {
        ServerManager.instance.RunStartClient();
    }

    public void OnClickHostBtn()
    {
        ServerManager.instance.RunStartHost();
    }

}
