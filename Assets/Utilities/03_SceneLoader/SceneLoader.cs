using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class SceneLoader : MonoBehaviour
{

    #region IP�A�h���X

    private const string IP_Adress = "127.0.0.1";

    #endregion

    #region �V�[����

    private string scenename0 = "SceneLoader";

    #endregion

    private string default_scenename = "";

    // Start is called before the first frame update
    void Start()
    {
        // �z�X�g�����擾����
        string hostname = Dns.GetHostName();
        Debug.Log("hostname : " + hostname);
        // �z�X�g������IP�A�h���X��Mac�A�h���X���擾����
        IPAddress[] adrList = Dns.GetHostAddresses(hostname);

        ReadDefaultSceneTxt();

        bool GetedIPAdressFlag = false;

        foreach (IPAddress address in adrList)
        {
            switch(address.ToString())
            {
                case IP_Adress:
                    GetedIPAdressFlag = true;
                    SceneManager.LoadScene(scenename0);
                    break;
                
            }
        }

        //���ׂĂɕs��v�������ꍇ�Adefault_scenename���J��
        if (!GetedIPAdressFlag)
        {
            SceneManager.LoadScene(default_scenename);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReadDefaultSceneTxt()
    {
        try
        {
            var TxtFilePath = Application.dataPath + "../../Files/defaultscene.txt";

            // using�X�e�[�g�����g��p���邱�ƂŁA�R���p�C�����Ŏ����I�Ƀ��\�[�X�̔j���p��IDisposable���Ă΂��
            using (var fs = new StreamReader(TxtFilePath, System.Text.Encoding.GetEncoding("UTF-8")))
            {
                // ��s���ǂݍ���
                while (fs.Peek() != -1)
                {
                    string st = fs.ReadLine();
                    Debug.Log("ReadLine : " + st);

                    if (!st.StartsWith("//") && st != "")
                    {
                        default_scenename = st;
                        Debug.Log("default_scenename : " + st);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Debug.Log("ReadDefaultSceneTxt��Error���������܂����B\n" + ex);
        }
    }
}
