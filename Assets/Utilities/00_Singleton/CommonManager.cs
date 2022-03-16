using UnityEngine;
using System;
using System.IO;

using System.Threading;
using System.Threading.Tasks;

public class CommonManager : SingletonMonoBehaviour<CommonManager>
{

    #region ���O�t�@�C���֘A

    private static string LogPath = string.Empty;
    private static string LogName = string.Empty;
    private static string FullPath = string.Empty;

    public enum LogLevel
    {
        Information,
        Error
    }

    #endregion

    // Start is called before the first frame update
    override protected void Awake()
    {
        try
        {
            // �q�N���X��Awake���g���ꍇ��
            // �K���e�N���X��Awake��Call����
            // ������GameObject�ɃA�^�b�`����Ȃ��悤�ɂ��܂�.
            base.Awake();

            //Set Cursor to not be visible
            Cursor.visible = false;

            TimeoutIOUtils.TimeoutSeconds = 2; //�^�C���A�E�g��2�b�ɐݒ�
            LogFileCreate();

            OutputLogText(LogLevel.Information,
                "==================================================\n" +
                "�A�v���P�[�V�������N�����܂����B");
            OutputLogText(LogLevel.Information,  $"ver.{Resources.Load<TextAsset>("BuildDate").text}");


        }
        catch(Exception ex)
        {
            OutputLogText(LogLevel.Error, "Awake" + ex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    #region ���O�t�@�C���̐����E�o��

    public void LogFileCreate()
    {
        try
        {
            LogPath = Application.dataPath + "../../../Log/";
            LogName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            FullPath = LogPath + LogName;

            if (!TimeoutIOUtils.DirectoryExists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            if (!TimeoutIOUtils.FileExists(FullPath))
            {
                File.Create(FullPath).Close(); //close���Ȃ��Ə������݂ŃG���[���������Ă��܂�
            }
        }
        catch(Exception ex)
        {
            Debug.Log("���O�t�@�C���̐����Ɏ��s���܂����B\n" + ex );
        }
    }

    public  void OutputLogText(LogLevel Level, string txt)
    {
        try
        {
            Debug.Log( Level + ":" + txt);

            string lv = string.Empty;

            switch (Level)
            {
                case LogLevel.Information:
                    lv = "Information";
                    break;
                case LogLevel.Error:
                    lv = "Error";
                    break;
            }

            txt = DateTime.Now.ToString("hh�Fmm�Fss") + "\t" + lv + "\n" + txt + "\n";
            var sw = new StreamWriter(FullPath, true);
            sw.WriteLine(txt);
            sw.Flush();
            sw.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    #endregion


}


#region �񓯊��̃^�C���A�E�g����

public class TimeoutIOUtils
{
    //�^�C���A�E�g�b
    public static int TimeoutSeconds { get; set; } = 3;

    //�t�@�C���L��
    public static bool FileExists(string path)
    {
        return TimeoutCore(() => File.Exists(path));
    }

    //�f�B���N�g���L��
    public static bool DirectoryExists(string path)
    {
        return TimeoutCore(() => Directory.Exists(path));
    }

    //�^�C���A�E�g��������
    private static bool TimeoutCore(Func<bool> existFunction)
    {
        try
        {
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeoutSeconds * 1000);
            var task = Task.Factory.StartNew(() => existFunction(), source.Token);
            task.Wait(source.Token);
            return task.Result;
        }
        catch (OperationCanceledException ex)
        {
            return false;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

#endregion

