using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Vuplex.WebView;

// URL �г��� ���� ��ũ�� ����ȭ�Ͽ� ��� �����ڰ� ���� ȭ���� �� �� �ְ� �մϴ�.
public class WebViewController : NetworkBehaviour
{
    CanvasWebViewPrefab canvasWebViewPrefab;
    RoomCanvasController roomCanvasController;

    [SyncVar]
    public string url;

    // Start is called before the first frame update
    public void Start()
    {
        canvasWebViewPrefab = FindObjectOfType<CanvasWebViewPrefab>();
        roomCanvasController = FindObjectOfType<RoomCanvasController>();
        // �ش� ��ư�� onClick �� SendURL �Լ� �̺�Ʈ �߰�
        roomCanvasController.urlOKBtn.onClick.AddListener(SendURL);
    }

    public void SendURL(string _old, string _new)
    {
        LoadURL();
    }

    public void SendURL()
    {
        CmdSendURL(roomCanvasController.urlInputField.text);

        //LoadURL();
    }

    [Command]
    public void CmdSendURL(string url)
    {
        this.url = url;
    }

    void LoadURL()
    {
        if (canvasWebViewPrefab != null)
        {
            // url �� ����ִٸ�
            if (url == null || url == "")
            {
                canvasWebViewPrefab.InitialUrl = "streaming-assets://�������_����.png";
                canvasWebViewPrefab.WebView.LoadUrl(canvasWebViewPrefab.InitialUrl);
            }
            else
            {
                canvasWebViewPrefab.InitialUrl = url;
                // WebView.LoadUrl �õ�
                try
                {
                    canvasWebViewPrefab.WebView.LoadUrl(canvasWebViewPrefab.InitialUrl);
                }
                // ���� WebView.LoadUrl �� �����ߴٸ�
                catch
                {
                    canvasWebViewPrefab.InitialUrl = "streaming-assets://�������_����.png";
                    canvasWebViewPrefab.WebView.LoadUrl(canvasWebViewPrefab.InitialUrl);
                }
            }
        }
        else
        {
            canvasWebViewPrefab.InitialUrl = "streaming-assets://�������_����.png";
            canvasWebViewPrefab.WebView.LoadUrl(canvasWebViewPrefab.InitialUrl);

        }
    }
}
