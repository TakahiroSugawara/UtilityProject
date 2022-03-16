/*
 * 
 * 
 * 
 * 
 */

using UnityEngine;

//add
using System;
using System.IO;

public class TextReader : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        ReadTxtFile();
    }

    // Update is called once per frame
    void Update()
    {
        //json
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ReadTxtFile();
        }
    }


    void ReadTxtFile()
    {
        try
        {
            var TxtFilePath = Application.dataPath + "../../Files/TextFile.txt";

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
                        Debug.Log("Text : " + st);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("ReadTxtFileでErrorが発生しました。\n" + ex);
        }
    }
}
