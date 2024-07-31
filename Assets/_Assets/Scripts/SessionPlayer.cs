using Mirror;
using UnityEngine;
using StarterAssets;
using Cinemachine;
using TMPro;

public class SessionPlayer : NetworkBehaviour
{
   
    public Transform playerCameraRoot;
    public TextMeshProUGUI playerNameTxt; // �÷��̾� �г��� ǥ��â
    public GameObject bubbleImage; // ��ǳ�� �̹���

    public static SessionPlayer localPlayer;

    //�г����� �����Ǹ� �ڵ����� �Լ��� �����ϵ��� �Ѵ�.
    //��ũ�� ������ Command ������ ���� �ٲ� ������ hook �Լ��� �����Ѵ�.
    [SyncVar(hook = nameof(OnChangeNickname))]private string nickname;
    //��ǳ�� Ȱ��ȭ ���θ� ��ũ�ٷ� ����
    [SyncVar(hook = nameof(OnChangeActivateBubble))] private bool isActiveBubble = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    //�� ������Ʈ�� ������ �÷��̾��� �� �����ϴ� �Լ�
    // Ȯ�� : https://mirror-networking.gitbook.io/docs/manual/components/networkbehaviour
    public override void OnStartLocalPlayer()
    {
        //ĳ������ �������� �������� ���� : �� Ű����� ���� ����,
        //ī�޶� �� ĳ���͸� ����ٴϵ���
        GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
        GetComponent<ThirdPersonController>().enabled = true;
        FindObjectOfType<CinemachineVirtualCamera>().Follow = playerCameraRoot;

        //DB�Ŵ����� ����� �г��� ���� ������ �����Ѵ�.
        CmdSetNickName(DBManager.instance.playerName);
        localPlayer = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Command �Լ��� ���� �������� �� �г��� ������ �����Ѵ�.
    //Ŀ�ǵ� �Լ��� ��� Cmd�� ����. (�̷� ��Ʈ��ũ���� ����)
    [Command]
    private void CmdSetNickName(string _nickName)
    {
        // Command + SyncVar ���� Ȱ��.
        // �÷��̾��� �̸��� ����ϴ� ��� �ٸ� ��������� ����ǵ��� SyncVar Ȱ��.
        //������ ������ �÷��̾��� �г����� ����
        nickname = _nickName;
    }

    //�г��� ��ũ�� �Լ�
    private void OnChangeNickname(string _old, string _new)
    {
        //old : ���� �ٲ�� ������ ������
        //new : �ٲ� ������ ������
        //�� �÷��̾��� �г��� â�� DBManager�� playerName�� ���.
        //�濡 �������� ��� �������� (������ �÷��̾� ������Ʈ�� UI)�� �ٲ��ش�.
        playerNameTxt.text = _new;
    }

    [Command]
    //������ ��ȣ�� ������ �Լ� => ��ǳ���� Ű�ų� ���ٴ� �ൿ�� �˸���.
    public void CmdActivateBubble(bool _isActive)
    {
        //�Ű������� ��ǳ���� ���� ���� ����
        isActiveBubble = _isActive;
    }

    //������ �濡 �������� ��� Ŭ���̾�Ʈ(���� ����)���� ��ȣ�� ������. => ����� ���� �ѱ�
    // => OnChange Ȱ��
    //������ Ȱ��ȭ ����
    private void OnChangeActivateBubble(bool _old, bool _new)
    {
        bubbleImage.SetActive(_new);
    }

}
