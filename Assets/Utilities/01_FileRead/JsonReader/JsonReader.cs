/*
 * ��Utf8Json��UnityPackage���C���X�g�[������
 * https://github.com/neuecc/Utf8Json
 * 
 * ���T���v����Json�t�@�C���͉��L�ɔz�u
 * C:---\UtilityProject\Json
 * 
 * ��Framework��.NET �ɕύX����
 * Edit��ProjectSetting��Player��ApiCompability Level��.NET 4.x
 * 
 * ��Unsafe Code��L���ɂ���
 * Edit��ProjectSetting��Player��Allow unsafe Code
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//add
using System;
using System.IO;
using System.Text;
using Utf8Json;


public class JsonReader : MonoBehaviour
{
    public static ConfigJsonFormat JSON_DATA = new ConfigJsonFormat();

    // Start is called before the first frame update
    void Start()
    {
        ReadJsonFile();
    }

    // Update is called once per frame
    void Update()
    {
        //json
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ReadJsonFile();
        }
    }

    public void ReadJsonFile()
    {
        try
        {
            //exe�̏�̊K�w�ɂȂ�
            //var JsonFilePath = Application.dataPath + "../../../Config/";
            var JsonFileName = "config.json";
            var JsonFilePath = Application.dataPath + "../../Files/" + JsonFileName;

            if(!File.Exists(JsonFilePath))
            { 
                Debug.Log("Json�t�@�C����������܂���ł����B\n" + JsonFilePath);
                return;
            }

            string stringJson = File.ReadAllText(JsonFilePath);
            dynamic json_string = Utf8JsonExtension.ParseJsonText<dynamic>(stringJson);

            JSON_DATA.remote_ip = json_string["remote_ip"];

            double d_remote_port = json_string["remote_port"];
            JSON_DATA.remote_port = (int)d_remote_port;

            double d_local_port = json_string["local_port"];
            JSON_DATA.local_port = (int)d_local_port;

            JSON_DATA.mode = json_string["mode"];

            JSON_DATA.data_path = json_string["data_path"];
            JSON_DATA.server_setting_path = json_string["server_setting_path"];

        }
        catch (Exception ex)
        {
            Debug.Log("Config�t�@�C���̓ǂݍ��݂ŃG���[���������܂����B" + ex);
        }
    }

}



public class ConfigJsonFormat
{
    public string remote_ip = "127.0.0.1";
    public int remote_port = 12345;
    public int local_port = 23456;
    //�����́A���ꂩ�T�[�o�[�T�C�h����f�[�^���~�����̂�
    public string mode = "ghana"; //"ghana" or "pie" or ""

    public string data_path = "data_path";
    public string server_setting_path = "";
    public string server_customfile_path = "";

}


#region �@json�֘A
public static class Utf8JsonExtension
{
    /// <summary>
    /// JSON�t�@�C����ǂ�� dynamic �I�u�W�F�N�g��Ԃ��B���A�z��͑S�� List<dynamic> �ŕԂ�
    /// </summary>
    public static T ReadJsonFile<T>(string fileName)
    {
        string jsonText = ReadFile(fileName);
        return ParseJsonText<T>(jsonText);
    }

    public static T ParseJsonText<T>(string jsonText)
    {
        T tmpJson = JsonSerializer.Deserialize<T>(jsonText);

        if (tmpJson == null /*|| tmpJson.Count == 0*/)
        {
            Debug.LogErrorFormat("Utf8JsonExtension.ParseJsonText�Fjson�`���Ƃ��Ė�肪����܂�");
        }
        else
        {
            Debug.LogFormat("Utf8JsonExtension.ParseJsonText�Fjson�Ƃ��ăp�[�X���܂���");
        }
        return tmpJson;
    }

    /// <summary>
    /// �w�肵���t�@�C�������݂�����A�S�ēǂݍ���ŕ�����ŕԂ�
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="Enc">�w�肵�Ȃ��� Encoding.UTF8 �Ƃ��ēǂݍ���</param>
    /// <returns></returns>
    public static string ReadFile(string fileName, Encoding Enc = null)
    {
        string resultText = "";
        FileInfo fi = new FileInfo(fileName);
        if (fi.Exists)
        {
            if (Enc == null) Enc = Encoding.UTF8;
            resultText = File.ReadAllText(fileName, Enc);
        }
        else
        {
            Debug.LogWarningFormat("{0}( {1} ) file not found !", "Utf8JsonExtension", fileName);
        }
        return resultText;
    }
}

#endregion

