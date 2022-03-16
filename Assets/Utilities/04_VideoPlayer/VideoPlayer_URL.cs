using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class VideoPlayer_URL : MonoBehaviour
{

    public RawImage Screen_Video1;
    public RawImage Screen_Video2;

    public float durationFade_Video = 0.0f;
    public float delayFade_Video = 0.2f;

    public Material _material;

    private VideoPlayer _video_1;
    private VideoPlayer _video_2;

    string _url_1 = string.Empty;
    string _url_2 = string.Empty;


    // Start is called before the first frame update
    void Start()
    {
        _url_1 = Application.dataPath + "../../Files/sample_1920x1080.mp4";
        _url_2 = Application.dataPath + "../../Files/sample_640x360.mp4";

        //本編映像用
        _video_1 = Screen_Video1.GetComponent<VideoPlayer>();
        _video_2 = Screen_Video2.GetComponent<VideoPlayer>();

        _video_1.url = _url_1;
        _video_2.url = _url_2;

        #region VideoEventHandler Add

        _video_1.sendFrameReadyEvents = true;
        _video_1.started += started_Main_1;
        _video_1.loopPointReached += loopPointReached_Main;

        _video_2.sendFrameReadyEvents = true;
        _video_2.started += started_Main_2;
        _video_2.loopPointReached += loopPointReached_Main;

        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (_video_1.isPlaying)
            {
                TogglePlay_MainVideo(_url_1);
            }
            else
            {
                TogglePlay_MainVideo(_url_2);
            }

            Debug.Log("Press Space.");
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            if (_video_1.isPlaying)
            {
                _video_1.time = (float)_video_1.frameCount / _video_1.frameRate - 2f;
            }
            else
            {
                _video_2.time = (float)_video_2.frameCount / _video_2.frameRate - 2f;
            }

            Debug.Log("Press E.");
        }

    }


    //映像再生準備ができてからクロスフェード開始
    private void started_Main_1(VideoPlayer vp)
    {
        //本編映像を表示
        Screen_Video1.material.DOFloat(1f, "_Alpha", durationFade_Video).SetDelay(delayFade_Video);
        Screen_Video1.material.DOFloat(0f, "_Black", durationFade_Video).SetDelay(delayFade_Video);

        Screen_Video2.material.DOFloat(0f, "_Alpha", durationFade_Video).SetDelay(delayFade_Video);
        Screen_Video2.material.DOFloat(1f, "_Black", durationFade_Video).SetDelay(delayFade_Video);

    }

    //映像再生準備ができてからクロスフェード開始
    private void started_Main_2(VideoPlayer vp)
    {
        //本編映像を表示
        Screen_Video2.material.DOFloat(1f, "_Alpha", durationFade_Video).SetDelay(delayFade_Video);
        Screen_Video2.material.DOFloat(0f, "_Black", durationFade_Video).SetDelay(delayFade_Video);

        //fadeout main
        Screen_Video1.material.DOFloat(0f, "_Alpha", durationFade_Video).SetDelay(delayFade_Video);
        Screen_Video1.material.DOFloat(1f, "_Black", durationFade_Video).SetDelay(delayFade_Video);

    }
    private void loopPointReached_Main(VideoPlayer vp)
    {
        if (_video_1.isPlaying)
        {
            TogglePlay_MainVideo(_url_1);
        }
        else
        {
            TogglePlay_MainVideo(_url_2);
        }

        vp.Pause();
    }


    private void TogglePlay_MainVideo(string url)
    {
        if (_video_1.isPlaying)
        {
            //映像をストップ
            _video_1.Pause();

            _video_2.url = url;

            //Mainのプレイヤーを先頭にするために停止して、再生する
            _video_2.time = 0;
            _video_2.Play();
        }

        else if (_video_2.isPlaying)
        {
            //映像をストップ
            _video_2.Pause();

            _video_1.url = url;

            //Mainのプレイヤーを先頭にするために停止して、再生する
            _video_1.time = 0;
            _video_1.Play();

        }
        //初回の動画再生時
        else
        {
            _video_1.url = url;

            //Mainのプレイヤーを先頭にするために停止して、再生する
            _video_1.time = 0;
            _video_1.Play();
        }

    }


    //ビルトインレンダリングパイプライン のみ
    //https://docs.unity3d.com/jp/current/ScriptReference/MonoBehaviour.OnPostRender.html
    // dstを指定しないことでnullが代入されるオーバーロード  
    //void OnPostRender()
    //{
    //    // カメラのレンダーターゲットを解除  
    //    Graphics.SetRenderTarget(null);

    //    // nullは画面をレンダリング対象とすることを意味する
    //    // 画面にBlitする場合
    //    Graphics.Blit(_video_1.texture, null, _material);
    //}
}
