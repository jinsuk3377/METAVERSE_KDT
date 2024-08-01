using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomCanvasController : MonoBehaviour
{
    public CanvasGroup urlPanel;
    public TMP_InputField urlInputField;
    public Button urlOKBtn;

    // Start is called before the first frame update
    void Start()
    {
        urlPanel.gameObject.SetActive(true);
        urlPanel.alpha = 0f;
        urlPanel.interactable = false;
    }

    public void PopupUrlPanel()
    {
        if (urlPanel.alpha < 1)
        {
            urlPanel.alpha = 1f;
            urlPanel.interactable = true;
        }

        else
        {
            urlPanel.alpha = 0f;
            urlPanel.interactable = false;
        }
    }

    public void CancelUrlPanel()
    {
        urlPanel.alpha = 0f;
        urlPanel.interactable = false;
    }
}
