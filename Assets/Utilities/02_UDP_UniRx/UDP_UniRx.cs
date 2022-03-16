/*
 * ��UniRx��UnityPackage���C���X�g�[������
 * https://github.com/neuecc/UniRx
 * 
 * ��Framework��.NET �ɕύX����
 * Edit��ProjectSetting��Player��ApiCompability Level��.NET 4.x
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//add
using UniRx;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class UDP_UniRx : MonoBehaviour
{
    //UDP
    private static UdpClient UDP_RECEIVER;
    private static UdpClient UDP_SENDER;

    private static int port_local = 12345;
    private static string ip_remote = "127.0.0.1";
    private static int port_remote = 23456;

    //subject��CommonManager�Ȃ�Static Class�Ő錾���A
    //�e�X�̃V�[���}�l�[�W���[�N���X�ŏ�������������
    public static Subject<string> subject = new Subject<string>();
    bool QuitFlag = false;

    const string COMMAND_TEST = "TEST";
    const string COMMAND_HOGE = "HOGE";

    // Start is called before the first frame update
    void Start()
    {

        //UDP RECIEVER
        UDP_RECEIVER = new UdpClient(port_local);
        UDP_RECEIVER.BeginReceive(OnReceived, UDP_RECEIVER);

        Debug.Log("UDP_RECEIVER:port:" + port_local.ToString());

        //UDP SENDER
        UDP_SENDER = new UdpClient();
        UDP_SENDER.Connect(ip_remote, port_remote);

        #region UDP�̎�M�̐ݒ�

        //UDP Recieve Action
        subject
            .ObserveOnMainThread()
            .Subscribe(msg =>
            {
                if (msg.Contains(COMMAND_TEST))
                {
                    Debug.Log("Test���������s���܂�");
                }
                else if (msg.Contains(COMMAND_HOGE))
                {
                    Debug.Log("Hoge���������s���܂�");
                }
            }).AddTo(this);

        #endregion 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        QuitFlag = true;
        UDP_RECEIVER.Close();
        UDP_RECEIVER.Dispose();
        UDP_RECEIVER = null;
    }


    public void SendMessage(string message)
    {
        try
        {
            var m = Encoding.UTF8.GetBytes(message);
            UDP_SENDER.Send(m, m.Length);
        }
        catch (Exception ex)
        {
            CommonManager.Instance.OutputLogText(CommonManager.LogLevel.Error, $"[Exception SendMessage] {ex}");

        }
    }

    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        try
        {
            if (!QuitFlag)
            {
                getUdp.BeginReceive(OnReceived, getUdp);
                byte[] getByte = getUdp.EndReceive(result, ref ipEnd);

                var message = Encoding.UTF8.GetString(getByte);
                subject.OnNext(message);

                Debug.Log("UDP_RECIEVE : " + message);

            }
            else
            {
                Debug.Log("UDP_RECIEVE�F" + "UDPQuit");
                return;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("UDP_RECIEVE_ERROR" + ex);
            Debug.Log("UDP_RECIEVE_ERROR" + "�ēxUDP��M�������J�n���܂��B");
            getUdp.BeginReceive(OnReceived, getUdp);
        }
    }
}
