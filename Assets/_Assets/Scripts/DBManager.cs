using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    //메타버스 내에서 사용할 데이터를 총괄
    //싱글톤, Dontdestroy처리로 씬이 변해도 오브젝트가 계속 남아있도록 처리.

    //싱글톤
    public static DBManager instance;
    public MySQLManager sqlManager; // MySQL 연결 클래스 가져오기

    public string playerName; // 사용자 이름

    private void Awake()
    {
        //싱글톤 생성
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

    //클래스 안에 또다른 클래스 생성
    // 유저 정보를 다룬다. = 이곳의 변수는 DB의 컬럼 종류와 같다.
    public class User
    {
        public int uuid;
        public string id, pw, nickname;

        //id와 pw 값을 받아와서 nickname을 리턴하는 함수.

        public bool CheckNickname(string _id, string _pw)
        {
            try
            {
                DataTable data = DBManager.instance.sqlManager
                    .ReceiveSQLCommand($"SELECT Nickname FROM player WHERE ID = '{_id}' AND PW = '{_pw}'");

                //받아온 데이터가 1개기 때문에, 데이터는 테이블의 0,0 에 저장되어 있다.
                nickname = data.Rows[0]["Nickname"].ToString();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //Id, PW, 닉네임을 매개변수로 받아와 데이터에 등록.
        //등록을 성공적으로 마치면 true를 반환.
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

    //방과 관련된 데이터
    public class Session
    {
        //방 정보 조회 => DataTable 형태로 비어있지 않은 방의 정보를 불러온다.
        public DataTable LoadSessionInfo()
        {
            return DBManager.instance.sqlManager
                .ReceiveSQLCommand("SELECT * FROM `session` WHERE roomName IS NOT null;");
            // uuid   roomName
            // [ 1 ] [방 이름] [인원수] [포트]
            // [ 2 ] [방 이름2][인원수2][포트]
        }

        //방 정보 갱신 - 새로운 방 만들기 or 방 정보 삭제하기

        //비어있는 방의 uuid를 가져오는 함수
        public int LoadEmptyRoomNum()
        {
            DataTable data = DBManager.instance.sqlManager
                .ReceiveSQLCommand("SELECT uuid FROM `session` WHERE roomName IS NULL ORDER BY UUID LIMIT 1;");
            // [ 1 ]
            //구한 값은 0번째 행의 0번째 열에 들어있다.
            return (int)data.Rows[0][0]; // 1
        }

        //비어있는 방에 새로운 정보를 넣어 방 갱신. => 방 생성
        public void CreateNewRoom(int _uuid, string _roomName)
        {
            DBManager.instance.sqlManager.SendSQLCommand($"UPDATE `session` SET roomName='{_roomName}' WHERE UUID = {_uuid};");
        }

        //방장이나 참여자가 입장할 때 입장객 수를 1명 늘리는 기능.
        public void IncreasePeopleNum(int _uuid) // 몇번째 방의 인원을 증가시킬지
        {
            DBManager.instance.sqlManager.SendSQLCommand($"UPDATE `session` SET playerNum = playerNum + 1 WHERE UUID = {_uuid};");
        }

        public void DecreasePeopleNum(int _uuid) // 몇번째 방의 인원을 증가시킬지
        {
            DBManager.instance.sqlManager.SendSQLCommand($"UPDATE `session` SET playerNum = playerNum - 1 WHERE UUID = {_uuid};");
        }

        //포트 번호를 순서대로 받아오는 함수
        public string[] LoadAllPortNum()
        {
            DataTable data= DBManager.instance.sqlManager
                .ReceiveSQLCommand("SELECT roomPort FROM `session` ORDER BY roomPort;");

            //리스트의 크기 초기화 = 받아온 데이터의 열의 갯수
            string[] portList = new string[data.Rows.Count];
            /* [7779]
             * [7780]
             * [7781]
             */
            //루프문을 통해 값을 삽입한다.
            for(int i=0; i< portList.Length; i++)
            {
                portList[i] = data.Rows[i][0].ToString();
            }

            return portList;
        }

        //방 정보 초기화 함수 (매개변수 : uuid)
        public void RemoveRoomInfo(int _uuid)
        {
            DBManager.instance.sqlManager
                .SendSQLCommand($"UPDATE `session` SET roomName = NULL, playerNum = 0 WHERE UUID = {_uuid};");
        }
    }
}
