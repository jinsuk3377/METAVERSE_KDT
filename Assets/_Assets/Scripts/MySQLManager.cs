using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public class MySQLManager : MonoBehaviour
{
    //MySQL (MariaDB)�� ������ ������ ����
    MySqlConnection sqlconn = null; // Mysql ���� �۾��� ������ �� Ŭ����

    // �����ͺ��̽��� ������ ip
    // �ٸ� pc���� ����Ϸ��� localhost�� �������� �ʴ�.
    // > �� ���α׷��� ������ ��ǻ�� ��ü�� ������ ������ �õ��ϱ� ����.
    readonly string sqlDB_ip = "localhost";

    // ������ �����ͺ��̽��� �̸�
    readonly string sqlDB_name = "metaversedb";

    // ������ �����ͺ��̽��� ���� id
    // ó�� ������ ������� �� ����Ʈ ���̵�� root
    // ����Ƽ���� ����� ���� ���� id�� �����ϴ� ���� ��õ�Ѵ�.
    readonly string sqlDB_id = "root";

    // ������ �����ͺ��̽��� ��й�ȣ
    readonly string sqlDB_pw = "iibi1131";

    //Mysql �����ͺ��̽��� ������ �õ��Ѵ�.
    void SqlConnectOpen()
    {
        //�����ͺ��̽��� ������ �õ��� Ŀ�ǵ� ����
        string command = $"Server={sqlDB_ip}; Database={sqlDB_name}; " +
            $"Userid={sqlDB_id}; Password={sqlDB_pw};";

        //���� �õ� (������ �ڵ尡 ���ߴ� ���� �����ϱ� ���� try-catch�� ���)
        try
        {
            //���ο� SQL Ŀ�ؼ� �ν���Ʈ Ŭ���� ����
            sqlconn = new MySqlConnection(command);
            sqlconn.Open();
            Debug.Log("SQL�� ���� ���� : " + sqlconn.State);
        }
        catch(Exception msg)
        { // ���ӿ� ������ �� �Ʒ� �ڵ� ����
            Debug.Log(msg);
        }
    }

    //������ ���� �Լ�
    void SqlConnectClose()
    {
        sqlconn.Close();
        Debug.Log("SQL�� ���� ���� : " + sqlconn.State);
    }

    //SQL ������ ���ؼ� �����͸� ����Ƽ�� �޾ƿ� �� �ִ�.
    //��ɾ�� String���� �޾ƿ´�.
    //��ɾ �Ϲ������� SQL�� ������ �Լ� (ex : update, insert ���� ��ɿ� ���)
    //�ǵ����޾ƾ� �� �����Ͱ� ���� ��쿡 ���.

    public void SendSQLCommand(string _command)
    {
        //SQL ������ ����.
        SqlConnectOpen();

        //��ɾ SQL ������ ������.
        //MySqlCommand(���� ��ɾ�, ��� ������ ��������)
        MySqlCommand dbCommand = new MySqlCommand(_command, sqlconn);
        //������ ������� ��ɾ �����Ѵ�.
        dbCommand.ExecuteNonQuery();
        
        //���� ������ ������ ���� ���� ������ ���´�.
        SqlConnectClose();
    }

    //�������� �����͸� �޾ƿ� �� ����ϴ� �Լ� (ex : select)

    public DataTable ReceiveSQLCommand(string _command)
    {
        //���� �����ϱ�.
        SqlConnectOpen();
        //�������� � ���� �޾ƿ��� ��, ������ ������ �� �ִ� �ν��Ͻ� Ŭ����
        MySqlDataAdapter adapter = new MySqlDataAdapter(_command, sqlconn);

        //DataTable : ����Ƽ �󿡼� �����͸� ���̺� �������� �����ϴ� ����.
        DataTable dt = new DataTable();
        //dt���ٰ� adapter���� �޾ƿ� ������ ������ �����Ѵ�. (���� -> ����Ƽ)
        adapter.Fill(dt);

        //���� �ݱ�
        SqlConnectClose();

        //dt�� ���� �Լ� �ۿ����� ����� �� �ֵ��� �����Ѵ�.
        return dt;
    }
}
