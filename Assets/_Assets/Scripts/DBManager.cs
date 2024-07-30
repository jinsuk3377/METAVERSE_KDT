using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    //��Ÿ���� ������ ����� �����͸� �Ѱ�
    //�̱���, Dontdestroyó���� ���� ���ص� ������Ʈ�� ��� �����ֵ��� ó��.

    //�̱���
    public static DBManager instance;
    public MySQLManager sqlManager; // MySQL ���� Ŭ���� ��������

    public string playerName; // ����� �̸�

    private void Awake()
    {
        //�̱��� ����
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Ŭ���� �ȿ� �Ǵٸ� Ŭ���� ����
    // ���� ������ �ٷ��. = �̰��� ������ DB�� �÷� ������ ����.
    public class User
    {
        public int uuid;
        public string id, pw, nickname;

        //id�� pw ���� �޾ƿͼ� nickname�� �����ϴ� �Լ�.

        public bool CheckNickname(string _id, string _pw)
        {
            try
            {
                DataTable data = DBManager.instance.sqlManager
                    .ReceiveSQLCommand($"SELECT Nickname FROM player WHERE ID = '{_id}' AND PW = '{_pw}'");

                //�޾ƿ� �����Ͱ� 1���� ������, �����ʹ� ���̺��� 0,0 �� ����Ǿ� �ִ�.
                nickname = data.Rows[0]["Nickname"].ToString();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //Id, PW, �г����� �Ű������� �޾ƿ� �����Ϳ� ���.
        //����� ���������� ��ġ�� true�� ��ȯ.
        public bool RegistNewUser(string _id, string _pw, string _nickname)
        {
            try
            {
                DBManager.instance.sqlManager
                .SendSQLCommand($"INSERT INTO player VALUES(NULL, '{_id}', '{_pw}', '{_nickname}');");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    //��� ���õ� ������
    public class Session
    {
        //�� ���� ��ȸ => DataTable ���·� ������� ���� ���� ������ �ҷ��´�.
        public DataTable LoadSessionInfo()
        {
            return DBManager.instance.sqlManager
                .ReceiveSQLCommand("SELECT * FROM `session` WHERE roomName IS NOT null;");
            // uuid   roomName
            // [ 1 ] [�� �̸�] [�ο���] [��Ʈ]
            // [ 2 ] [�� �̸�2][�ο���2][��Ʈ]
        }

        //�� ���� ���� - ���ο� �� ����� or �� ���� �����ϱ�

        //����ִ� ���� uuid�� �������� �Լ�
        public int LoadEmptyRoomNum()
        {
            DataTable data = DBManager.instance.sqlManager
                .ReceiveSQLCommand("SELECT uuid FROM `session` WHERE roomName IS NULL ORDER BY UUID LIMIT 1;");
            // [ 1 ]
            //���� ���� 0��° ���� 0��° ���� ����ִ�.
            return (int)data.Rows[0][0]; // 1
        }

        //����ִ� �濡 ���ο� ������ �־� �� ����. => �� ����
        public void CreateNewRoom(int _uuid, string _roomName)
        {
            DBManager.instance.sqlManager.SendSQLCommand($"UPDATE `session` SET roomName='{_roomName}' WHERE UUID = {_uuid};");
        }

        //�����̳� �����ڰ� ������ �� ���尴 ���� 1�� �ø��� ���.
        public void IncreasePeopleNum(int _uuid) // ���° ���� �ο��� ������ų��
        {
            DBManager.instance.sqlManager.SendSQLCommand($"UPDATE `session` SET playerNum = playerNum + 1 WHERE UUID = {_uuid};");
        }

        public void DecreasePeopleNum(int _uuid) // ���° ���� �ο��� ������ų��
        {
            DBManager.instance.sqlManager.SendSQLCommand($"UPDATE `session` SET playerNum = playerNum - 1 WHERE UUID = {_uuid};");
        }

        //��Ʈ ��ȣ�� ������� �޾ƿ��� �Լ�
        public string[] LoadAllPortNum()
        {
            DataTable data= DBManager.instance.sqlManager
                .ReceiveSQLCommand("SELECT roomPort FROM `session` ORDER BY roomPort;");

            //����Ʈ�� ũ�� �ʱ�ȭ = �޾ƿ� �������� ���� ����
            string[] portList = new string[data.Rows.Count];
            /* [7779]
             * [7780]
             * [7781]
             */
            //�������� ���� ���� �����Ѵ�.
            for(int i=0; i< portList.Length; i++)
            {
                portList[i] = data.Rows[i][0].ToString();
            }

            return portList;
        }

        //�� ���� �ʱ�ȭ �Լ� (�Ű����� : uuid)
        public void RemoveRoomInfo(int _uuid)
        {
            DBManager.instance.sqlManager
                .SendSQLCommand($"UPDATE `session` SET roomName = NULL, playerNum = 0 WHERE UUID = {_uuid};");
        }
    }
}
