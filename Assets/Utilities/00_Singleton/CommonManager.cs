using UnityEngine;
using System;
using System.IO;

using System.Threading;
using System.Threading.Tasks;

public class CommonManager : SingletonMonoBehaviour<CommonManager>
{

    #region ログファイル関連

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
            // 子クラスでAwakeを使う場合は
            // 必ず親クラスのAwakeをCallして
            // 複数のGameObjectにアタッチされないようにします.
            base.Awake();

            //Set Cursor to not be visible
            Cursor.visible = false;

            TimeoutIOUtils.TimeoutSeconds = 2; //タイムアウトを2秒に設定
            LogFileCreate();

            OutputLogText(LogLevel.Information,
                "==================================================\n" +
                "アプリケーションが起動しました。");
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



    #region ログファイルの生成・出力

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
                File.Create(FullPath).Close(); //closeしないと書き込みでエラーが発生してしまう
            }
        }
        catch(Exception ex)
        {
            Debug.Log("ログファイルの生成に失敗しました。\n" + ex );
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

            txt = DateTime.Now.ToString("hh：mm：ss") + "\t" + lv + "\n" + txt + "\n";
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


#region 非同期のタイムアウト処理

public class TimeoutIOUtils
{
    //タイムアウト秒
    public static int TimeoutSeconds { get; set; } = 3;

    //ファイル有無
    public static bool FileExists(string path)
    {
        return TimeoutCore(() => File.Exists(path));
    }

    //ディレクトリ有無
    public static bool DirectoryExists(string path)
    {
        return TimeoutCore(() => Directory.Exists(path));
    }

    //タイムアウト処理部分
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

