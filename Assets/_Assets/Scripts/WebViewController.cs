using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Vuplex.WebView;

// URL 패널을 통해 링크를 동기화하여 모든 접속자가 같은 화면을 볼 수 있게 합니다.
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
        // 해당 버튼에 onClick 시 SendURL 함수 이벤트 추가
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
            // url 이 비어있다면
            if (url == null || url == "")
            {
                canvasWebViewPrefab.InitialUrl = "streaming-assets://방송정보_원본.png";
                canvasWebViewPrefab.WebView.LoadUrl(canvasWebViewPrefab.InitialUrl);
            }
            else
            {
                canvasWebViewPrefab.InitialUrl = url;
                // WebView.LoadUrl 시도
                try
                {
                    canvasWebViewPrefab.WebView.LoadUrl(canvasWebViewPrefab.InitialUrl);
                }
                // 만약 WebView.LoadUrl 을 실패했다면
                catch
                {
                    canvasWebViewPrefab.InitialUrl = "streaming-assets://방송정보_원본.png";
                    canvasWebViewPrefab.WebView.LoadUrl(canvasWebViewPrefab.InitialUrl);
                }
            }
        }
        else
        {
            canvasWebViewPrefab.InitialUrl = "streaming-assets://방송정보_원본.png";
            canvasWebViewPrefab.WebView.LoadUrl(canvasWebViewPrefab.InitialUrl);

        }
    }
}
