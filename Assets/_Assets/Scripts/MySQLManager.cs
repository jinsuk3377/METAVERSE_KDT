using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public class MySQLManager : MonoBehaviour
{
    //MySQL (MariaDB)에 접속할 유저의 정보
    MySqlConnection sqlconn = null; // Mysql 연결 작업을 진행할 빈 클래스

    // 데이터베이스에 접속할 ip
    // 다른 pc에서 사용하려면 localhost는 적합하지 않다.
    // > 이 프로그램을 실행한 컴퓨터 자체의 서버에 접속을 시도하기 때문.
    readonly string sqlDB_ip = "localhost";

    // 접속할 데이터베이스의 이름
    readonly string sqlDB_name = "metaversedb";

    // 접속할 데이터베이스의 계정 id
    // 처음 계정을 만들었을 때 디폴트 아이디는 root
    // 유니티에서 사용할 때는 별도 id를 생성하는 편을 추천한다.
    readonly string sqlDB_id = "root";

    // 접속할 데이터베이스의 비밀번호
    readonly string sqlDB_pw = "iibi1131";

    //Mysql 데이터베이스에 접속을 시도한다.
    void SqlConnectOpen()
    {
        //데이터베이스에 접속을 시도할 커맨드 제작
        string command = $"Server={sqlDB_ip}; Database={sqlDB_name}; " +
            $"Userid={sqlDB_id}; Password={sqlDB_pw};";

        //접속 시도 (오류로 코드가 멈추는 것을 방지하기 위해 try-catch문 사용)
        try
        {
            //새로운 SQL 커넥션 인스턴트 클래스 생성
            sqlconn = new MySqlConnection(command);
            sqlconn.Open();
            Debug.Log("SQL의 접속 상태 : " + sqlconn.State);
        }
        catch(Exception msg)
        { // 접속에 실패할 시 아래 코드 실행
            Debug.Log(msg);
        }
    }

    //연결을 끊는 함수
    void SqlConnectClose()
    {
        sqlconn.Close();
        Debug.Log("SQL의 접속 상태 : " + sqlconn.State);
    }

    //SQL 문법을 통해서 데이터를 유니티로 받아올 수 있다.
    //명령어는 String으로 받아온다.
    //명령어를 일방적으로 SQL에 보내는 함수 (ex : update, insert 등의 명령에 사용)
    //되돌려받아야 할 데이터가 없을 경우에 사용.

    public void SendSQLCommand(string _command)
    {
        //SQL 서버에 접속.
        SqlConnectOpen();

        //명령어를 SQL 서버로 보낸다.
        //MySqlCommand(보낼 명령어, 어느 서버로 보낼건지)
        MySqlCommand dbCommand = new MySqlCommand(_command, sqlconn);
        //설정한 정보대로 명령어를 전송한다.
        dbCommand.ExecuteNonQuery();
        
        //서버 전송이 끝나면 도로 서버 연결을 끊는다.
        SqlConnectClose();
    }

    //서버에서 데이터를 받아올 때 사용하는 함수 (ex : select)

    public DataTable ReceiveSQLCommand(string _command)
    {
        //서버 접속하기.
        SqlConnectOpen();
        //서버에서 어떤 값을 받아왔을 때, 정보를 저장할 수 있는 인스턴스 클래스
        MySqlDataAdapter adapter = new MySqlDataAdapter(_command, sqlconn);

        //DataTable : 유니티 상에서 데이터를 테이블 형식으로 저장하는 포맷.
        DataTable dt = new DataTable();
        //dt에다가 adapter에서 받아온 데이터 정보를 저장한다. (서버 -> 유니티)
        adapter.Fill(dt);

        //서버 닫기
        SqlConnectClose();

        //dt의 값을 함수 밖에서도 사용할 수 있도록 리턴한다.
        return dt;
    }
}
