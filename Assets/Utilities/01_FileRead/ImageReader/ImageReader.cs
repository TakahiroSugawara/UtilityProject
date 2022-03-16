/*
 * 
 * Textureは生成した後に、明示的にDisposeしないと
 * メモリに溜まっていってしまうので注意
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;
using System.IO;

public class ImageReader : MonoBehaviour
{
    public RawImage UI_RawImage;
    private Texture2D _texture;
    private string _imagePath;

    //縦合わせ　VerticalAlignment 
    public RawImage UI_RawImage_VA;
    private Texture2D _texture_VA;
    private string _imagePath_VA;

    //Graphics Blit でカメラに画像を直接描画する
    public Material _material;

    // Start is called before the first frame update
    void Start()
    {
        _imagePath = Application.dataPath + "../../Files/sample-1920-1080.png";
        _texture = ReadTexture(_imagePath, 1, 1);
        UI_RawImage.texture = _texture;
        //GO_RawImage.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", _texture);


        _imagePath_VA = Application.dataPath + "../../Files/sample-1200-800.png";
        _texture_VA = ReadTexture(_imagePath_VA, 1, 1);
        UI_RawImage_VA.texture = _texture_VA;
        UI_RawImage_VA.GetComponent<AspectRatioFitter>().aspectRatio = (float)_texture_VA.width / (float)_texture_VA.height;


    }

    // Update is called once per frame
    void Update()
    {
        
        UI_RawImage_VA.GetComponent<AspectRatioFitter>().aspectRatio = (float)UI_RawImage_VA.texture.width / (float)UI_RawImage_VA.texture.height;



    }

    #region 画像読み込み

    public Sprite CreateSprite(string path)
    {

        Sprite createdSprite;

        // 1x1で確保しておけばLoadImage()で適切なサイズに変更される模様
        Texture2D texture = ReadTexture(path, 1, 1);
        createdSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 1), 1);

        return createdSprite;
    }


    public Texture2D ReadTexture(string path, int width, int height)
    {
        try
        {
            byte[] readBinary = ReadPngFile(path);
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false); //mipmapをfalseにしないと画像がぼやける

            //texture のプロパティを設定する
            texture.LoadImage(readBinary);
            texture.filterMode = FilterMode.Trilinear;

            return texture;
        }
        catch (Exception ex)
        {
            Debug.Log("pngファイルの読み込みでエラーが発生しました。" + "\n" + ex);

            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false); //mipmapをfalseにしないと画像がぼやける
            return texture;
        }
    }

    private byte[] ReadPngFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }

    #endregion
}
