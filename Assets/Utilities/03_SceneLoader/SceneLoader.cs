using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class SceneLoader : MonoBehaviour
{

    #region IPアドレス

    private const string IP_Adress = "127.0.0.1";

    #endregion

    #region シーン名

    private string scenename0 = "SceneLoader";

    #endregion

    private string default_scenename = "";

    // Start is called before the first frame update
    void Start()
    {
        // ホスト名を取得する
        string hostname = Dns.GetHostName();
        Debug.Log("hostname : " + hostname);
        // ホスト名からIPアドレスとMacアドレスを取得する
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

        //すべてに不一致だった場合、default_scenenameを開く
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

            // usingステートメントを用いることで、コンパイラ側で自動的にリソースの破棄用のIDisposableが呼ばれる
            using (var fs = new StreamReader(TxtFilePath, System.Text.Encoding.GetEncoding("UTF-8")))
            {
                // 一行ずつ読み込む
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
            Debug.Log("ReadDefaultSceneTxtでErrorが発生しました。\n" + ex);
        }
    }
}
