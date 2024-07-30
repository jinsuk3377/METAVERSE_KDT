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
    //방 리스트를 표시할 UI 오브젝트
    public GameObject[] sessionObjList;
    //각 방의 제목 텍스트
    public TextMeshProUGUI[] sessionName;
    //각 방의 인원수 텍스트
    public TextMeshProUGUI[] joinPlayerNum;
    //CreatePopup 에서 방의 이름을 받아오는 인풋필드
    public TMP_InputField createRoomNameInput;

    [Header("Button")]
    public Button closeBtn;
    public Button joinBtn;
    public Button createBtn;
    public Button[] roomSetButtons;
    public Button createConfirmBtn;

    [SerializeField]private string[] roomPortList; // 각 방의 룸 포트번호
    private int selectRoomNum; // 현재 사용자가 몇 번 방을 선택하고 있는지

    DBManager.Session session = new(); // 스크립트 내에서 사용할 세션 DB 데이터

    private void Start()
    {
        //각 버튼에 함수 연결
        closeBtn.onClick.AddListener(OnClickCloseBtn);
        joinBtn.onClick.AddListener(OnClickJoinBtn);
        createBtn.onClick.AddListener(OnClickCreateBtn);
        createConfirmBtn.onClick.AddListener(CreateRoom);

        //시작 시에는 항상 roomPanel이 꺼지도록
        roomPanel.SetActive(false);

        //Join 버튼을 비활성화. => 버튼을 하나라도 선택한 이후에 활성화되도록 한다.
        joinBtn.interactable = false;

        //포트 번호 배열을 리스트 갯수만큼으로 초기화.
        roomPortList = new string[sessionObjList.Length];

        //모든 방의 포트 번호를 DB에서 불러온다.

        //각 방의 접속 포트 번호를 저장.
        //조건 : roomPortList가 비어있을 때에만 실행.
        roomPortList = session.LoadAllPortNum();

    }
    private void OnTriggerEnter(Collider other)
    {
        //본인의 플레이어가 콜라이더에 닿았을 때 (남의 플레이어는 감지하지 않아야 한다.)
        // => Session Player를 부딪힌 물체가 갖고 있는지 체크
        //    + 로컬 플레이어인지 체크.

        SessionPlayer player = other.GetComponent<SessionPlayer>();

        //NetworkBehaviour의 islocalPlayer : 해당 오브젝트가 로컬 플레이어의 오브젝트인지 체크
        if (player != null && player.isLocalPlayer)
        {
            Debug.Log("로컬 플레이어가 닿았습니다.");

            //룸 선택 UI 팝업
            roomPanel.SetActive(true);
            //켜지는 동시에 데이터베이스로부터 룸 정보를 불러온다.

            LoadSessionData();

        }

    }

    private void LoadSessionData()
    {                                  //비어있지 않은 방 정보를 가져오는 함수.
        DataTable data = session.LoadSessionInfo();
        Debug.LogFormat("불러온 데이터의 갯수 :" + data.Rows.Count);

        //이미 만들어진 방이 최대 방 갯수일 때에는
        //방을 만들 수 없도록 Create 버튼을 비활성화한다.
        if(data.Rows.Count == sessionObjList.Length)
        {
            createBtn.interactable = false;
        }
        else
        {
            createBtn.interactable = true;
        }

        //루프문
        //데이터의 갯수 만큼만 리스트를 활성화, 나머지는 비활성화.
        for (int i = 0; i < sessionObjList.Length; i++)
        {
            if (i < data.Rows.Count)
            {
                sessionObjList[i].SetActive(true);
                //활성화된 오브젝트에 한해서 데이터를 할당한다.
                //sessionName에 SQL의 roomName 할당
                sessionName[i].text = data.Rows[i]["roomName"].ToString();

                //joinPlayerNum에 playerNum 할당
                joinPlayerNum[i].text = data.Rows[i]["playerNum"].ToString();

                //만들어진 방이 uuid 상으로 몇 번째 방인지 번호를 매겨, 버튼 함수를 넣는다.
                // => AddListener

                //AddLisener는 실행할 때마다 함수가 중첩된다.
                //똑같은 함수가 겹쳐져 여러번 실행되는 것을 방지하기 위해 
                // 버튼 초기화 이후 함수를 삽입한다.
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
        //Room 으로 이동한다. => ServerManager 클래스를 사용.
        //서버 이동에 필요한 정보 : ip 주소, 포트 번호, 어느 씬으로 이동할 건지
        //중요한 사항 : 포트
        // 같은 ip, 같은 씬에 위치해도 포트 번호가 다르면 서로 다른 방에 접속한 것으로 취급한다.
        // => 같은 씬에 접속 중이어도 포트가 다른 플레이어는 보이지 않습니다. 교류도 불가능.
        // => 같은 씬, 같은 포트여야 플레이어끼리의 상호작용이 가능하다.

        //서버를 열 때는 포트 하나당 한 개씩만 열 수 있다. 같은 포트 번호를 서버가 중복해서 열 수 없다.
        // => 씬에 무관.

        //ip, 포트 설정


        MainManager.Instance.MoveScenePort = int.Parse(_port);
        MainManager.Instance.nextSceneNumber = 3; // 룸씬

        //호스트, 서버는 이동이 불가능. => 이동하는 순간 이전에 있던 씬의 서버가 닫힌다.
        //클라이언트로만 이동 가능.

        //StopClient : 서버 - 플레이어의 연결 고리를 끊는 함수.
        // 연결을 끊은 이후에 새로운 서버에 접속이 가능해진다.
        NetworkManager.singleton.StopClient();
        SceneManager.LoadSceneAsync("00 LOADING");
    }

    private void OnClickCloseBtn()
    {
        //창닫기
        roomPanel.SetActive(false);
    }

    private void SelectRoom(int roomIndex)
    {
        //특정 방을 선택하는 버튼 (룸 리스트 버튼)
        //이 방이 몇번째 방인지 매개변수로 받아온다.
        Debug.Log("방 번호 : " + roomIndex);
        //방금 선택한 방의 번호를 저장한다.
        selectRoomNum = roomIndex;

        //join 버튼이 비활성화 되어있을 경우 활성화한다.
        if(joinBtn.interactable == false)
        {
            joinBtn.interactable = true;
        }
    }

    private void OnClickCreateBtn()
    {
        //새로운 방 만들기
        //=> 방 만들기 팝업을 활성화 한다.
        createPopup.SetActive(true);
    }

    private void OnClickJoinBtn()
    {
        //선택한 방에 접속하기
        //선택한 순서의 방을 가져와
        //해당 방의 포트 번호로 접속을 시도.
        Debug.Log("선택한 방의 포트번호 : " + roomPortList[selectRoomNum]);

        //방 인원수에 1명 추가
        session.IncreasePeopleNum(selectRoomNum);

        //플레이어가 접속할 방의 uuid를 MainManager에 저장한다.
        MainManager.Instance.connectionRoomIndex = selectRoomNum;

        MoveRoom(roomPortList[selectRoomNum]);
    }

    private void CreateRoom()
    {
        //새로운 방을 만드는데 필요한 정보 : 방 제목, 방 인원 -> 입장할 때마다 1명씩 추가

        //인풋 필드에 아무것도 안 적었을 때에는 실행 안 되도록.
        if (createRoomNameInput.text.Length < 1) return;

        //빈 방을 찾아서 내 방으로 등록한다.
        int emptyRoomNum = session.LoadEmptyRoomNum();

        //인풋 필드로부터 방 제목을 받아온다.
        string newRoomName = createRoomNameInput.text;

        //플레이어를 방장으로 설정한다.

        MainManager.Instance.isRoomMaster = true;

        //방 제목을 빈 방에 등록한다. 해당 방을 내 방으로 지정한다.

        session.CreateNewRoom(emptyRoomNum, newRoomName);

        //방의 인원수를 1명 추가
        session.IncreasePeopleNum(emptyRoomNum);

        //플레이어가 접속할 방의 uuid를 MainManager에 저장한다.
        MainManager.Instance.connectionRoomIndex = emptyRoomNum;

        Debug.Log("선택한 빈 방 : " + emptyRoomNum + ", 포트 번호 : " + roomPortList[emptyRoomNum]);

        //지정한 방의 포트로 입장한다.
        MoveRoom(roomPortList[emptyRoomNum]);
    }

}
