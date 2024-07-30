using kcp2k;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SessionRoom : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject roomPanel;
    public GameObject createPopup;

    [Header("Session UI")]
    //�� ����Ʈ�� ǥ���� UI ������Ʈ
    public GameObject[] sessionObjList;
    //�� ���� ���� �ؽ�Ʈ
    public TextMeshProUGUI[] sessionName;
    //�� ���� �ο��� �ؽ�Ʈ
    public TextMeshProUGUI[] joinPlayerNum;
    //CreatePopup ���� ���� �̸��� �޾ƿ��� ��ǲ�ʵ�
    public TMP_InputField createRoomNameInput;

    [Header("Button")]
    public Button closeBtn;
    public Button joinBtn;
    public Button createBtn;
    public Button[] roomSetButtons;
    public Button createConfirmBtn;

    [SerializeField]private string[] roomPortList; // �� ���� �� ��Ʈ��ȣ
    private int selectRoomNum; // ���� ����ڰ� �� �� ���� �����ϰ� �ִ���

    DBManager.Session session = new(); // ��ũ��Ʈ ������ ����� ���� DB ������

    private void Start()
    {
        //�� ��ư�� �Լ� ����
        closeBtn.onClick.AddListener(OnClickCloseBtn);
        joinBtn.onClick.AddListener(OnClickJoinBtn);
        createBtn.onClick.AddListener(OnClickCreateBtn);
        createConfirmBtn.onClick.AddListener(CreateRoom);

        //���� �ÿ��� �׻� roomPanel�� ��������
        roomPanel.SetActive(false);

        //Join ��ư�� ��Ȱ��ȭ. => ��ư�� �ϳ��� ������ ���Ŀ� Ȱ��ȭ�ǵ��� �Ѵ�.
        joinBtn.interactable = false;

        //��Ʈ ��ȣ �迭�� ����Ʈ ������ŭ���� �ʱ�ȭ.
        roomPortList = new string[sessionObjList.Length];

        //��� ���� ��Ʈ ��ȣ�� DB���� �ҷ��´�.

        //�� ���� ���� ��Ʈ ��ȣ�� ����.
        //���� : roomPortList�� ������� ������ ����.
        roomPortList = session.LoadAllPortNum();

    }
    private void OnTriggerEnter(Collider other)
    {
        //������ �÷��̾ �ݶ��̴��� ����� �� (���� �÷��̾�� �������� �ʾƾ� �Ѵ�.)
        // => Session Player�� �ε��� ��ü�� ���� �ִ��� üũ
        //    + ���� �÷��̾����� üũ.

        SessionPlayer player = other.GetComponent<SessionPlayer>();

        //NetworkBehaviour�� islocalPlayer : �ش� ������Ʈ�� ���� �÷��̾��� ������Ʈ���� üũ
        if (player != null && player.isLocalPlayer)
        {
            Debug.Log("���� �÷��̾ ��ҽ��ϴ�.");

            //�� ���� UI �˾�
            roomPanel.SetActive(true);
            //������ ���ÿ� �����ͺ��̽��κ��� �� ������ �ҷ��´�.

            LoadSessionData();

        }

    }

    private void LoadSessionData()
    {                                  //������� ���� �� ������ �������� �Լ�.
        DataTable data = session.LoadSessionInfo();
        Debug.LogFormat("�ҷ��� �������� ���� :" + data.Rows.Count);

        //�̹� ������� ���� �ִ� �� ������ ������
        //���� ���� �� ������ Create ��ư�� ��Ȱ��ȭ�Ѵ�.
        if(data.Rows.Count == sessionObjList.Length)
        {
            createBtn.interactable = false;
        }
        else
        {
            createBtn.interactable = true;
        }

        //������
        //�������� ���� ��ŭ�� ����Ʈ�� Ȱ��ȭ, �������� ��Ȱ��ȭ.
        for (int i = 0; i < sessionObjList.Length; i++)
        {
            if (i < data.Rows.Count)
            {
                sessionObjList[i].SetActive(true);
                //Ȱ��ȭ�� ������Ʈ�� ���ؼ� �����͸� �Ҵ��Ѵ�.
                //sessionName�� SQL�� roomName �Ҵ�
                sessionName[i].text = data.Rows[i]["roomName"].ToString();

                //joinPlayerNum�� playerNum �Ҵ�
                joinPlayerNum[i].text = data.Rows[i]["playerNum"].ToString();

                //������� ���� uuid ������ �� ��° ������ ��ȣ�� �Ű�, ��ư �Լ��� �ִ´�.
                // => AddListener

                //AddLisener�� ������ ������ �Լ��� ��ø�ȴ�.
                //�Ȱ��� �Լ��� ������ ������ ����Ǵ� ���� �����ϱ� ���� 
                // ��ư �ʱ�ȭ ���� �Լ��� �����Ѵ�.
                roomSetButtons[i].onClick.RemoveAllListeners();
                int index = i;
                roomSetButtons[i].onClick.AddListener(() => SelectRoom((int)data.Rows[index]["uuid"]));

            }
            else
            {
                sessionObjList[i].SetActive(false);
            }
        }
    }

    private void MoveRoom(string _port)
    {
        //Room ���� �̵��Ѵ�. => ServerManager Ŭ������ ���.
        //���� �̵��� �ʿ��� ���� : ip �ּ�, ��Ʈ ��ȣ, ��� ������ �̵��� ����
        //�߿��� ���� : ��Ʈ
        // ���� ip, ���� ���� ��ġ�ص� ��Ʈ ��ȣ�� �ٸ��� ���� �ٸ� �濡 ������ ������ ����Ѵ�.
        // => ���� ���� ���� ���̾ ��Ʈ�� �ٸ� �÷��̾�� ������ �ʽ��ϴ�. ������ �Ұ���.
        // => ���� ��, ���� ��Ʈ���� �÷��̾���� ��ȣ�ۿ��� �����ϴ�.

        //������ �� ���� ��Ʈ �ϳ��� �� ������ �� �� �ִ�. ���� ��Ʈ ��ȣ�� ������ �ߺ��ؼ� �� �� ����.
        // => ���� ����.

        //ip, ��Ʈ ����


        MainManager.Instance.MoveScenePort = int.Parse(_port);
        MainManager.Instance.nextSceneNumber = 3; // ���

        //ȣ��Ʈ, ������ �̵��� �Ұ���. => �̵��ϴ� ���� ������ �ִ� ���� ������ ������.
        //Ŭ���̾�Ʈ�θ� �̵� ����.

        //StopClient : ���� - �÷��̾��� ���� ���� ���� �Լ�.
        // ������ ���� ���Ŀ� ���ο� ������ ������ ����������.
        NetworkManager.singleton.StopClient();
        SceneManager.LoadSceneAsync("00 LOADING");
    }

    private void OnClickCloseBtn()
    {
        //â�ݱ�
        roomPanel.SetActive(false);
    }

    private void SelectRoom(int roomIndex)
    {
        //Ư�� ���� �����ϴ� ��ư (�� ����Ʈ ��ư)
        //�� ���� ���° ������ �Ű������� �޾ƿ´�.
        Debug.Log("�� ��ȣ : " + roomIndex);
        //��� ������ ���� ��ȣ�� �����Ѵ�.
        selectRoomNum = roomIndex;

        //join ��ư�� ��Ȱ��ȭ �Ǿ����� ��� Ȱ��ȭ�Ѵ�.
        if(joinBtn.interactable == false)
        {
            joinBtn.interactable = true;
        }
    }

    private void OnClickCreateBtn()
    {
        //���ο� �� �����
        //=> �� ����� �˾��� Ȱ��ȭ �Ѵ�.
        createPopup.SetActive(true);
    }

    private void OnClickJoinBtn()
    {
        //������ �濡 �����ϱ�
        //������ ������ ���� ������
        //�ش� ���� ��Ʈ ��ȣ�� ������ �õ�.
        Debug.Log("������ ���� ��Ʈ��ȣ : " + roomPortList[selectRoomNum]);

        //�� �ο����� 1�� �߰�
        session.IncreasePeopleNum(selectRoomNum);

        //�÷��̾ ������ ���� uuid�� MainManager�� �����Ѵ�.
        MainManager.Instance.connectionRoomIndex = selectRoomNum;

        MoveRoom(roomPortList[selectRoomNum]);
    }

    private void CreateRoom()
    {
        //���ο� ���� ����µ� �ʿ��� ���� : �� ����, �� �ο� -> ������ ������ 1�� �߰�

        //��ǲ �ʵ忡 �ƹ��͵� �� ������ ������ ���� �� �ǵ���.
        if (createRoomNameInput.text.Length < 1) return;

        //�� ���� ã�Ƽ� �� ������ ����Ѵ�.
        int emptyRoomNum = session.LoadEmptyRoomNum();

        //��ǲ �ʵ�κ��� �� ������ �޾ƿ´�.
        string newRoomName = createRoomNameInput.text;

        //�÷��̾ �������� �����Ѵ�.

        MainManager.Instance.isRoomMaster = true;

        //�� ������ �� �濡 ����Ѵ�. �ش� ���� �� ������ �����Ѵ�.

        session.CreateNewRoom(emptyRoomNum, newRoomName);

        //���� �ο����� 1�� �߰�
        session.IncreasePeopleNum(emptyRoomNum);

        //�÷��̾ ������ ���� uuid�� MainManager�� �����Ѵ�.
        MainManager.Instance.connectionRoomIndex = emptyRoomNum;

        Debug.Log("������ �� �� : " + emptyRoomNum + ", ��Ʈ ��ȣ : " + roomPortList[emptyRoomNum]);

        //������ ���� ��Ʈ�� �����Ѵ�.
        MoveRoom(roomPortList[emptyRoomNum]);
    }

}
