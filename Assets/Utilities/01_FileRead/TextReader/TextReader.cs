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
                        Debug.Log("Text : " + st);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("ReadTxtFile��Error���������܂����B\n" + ex);
        }
    }
}
