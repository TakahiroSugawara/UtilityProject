/*
 * 
 * Texture�͐���������ɁA�����I��Dispose���Ȃ���
 * �������ɗ��܂��Ă����Ă��܂��̂Œ���
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

    //�c���킹�@VerticalAlignment 
    public RawImage UI_RawImage_VA;
    private Texture2D _texture_VA;
    private string _imagePath_VA;

    //Graphics Blit �ŃJ�����ɉ摜�𒼐ڕ`�悷��
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

    #region �摜�ǂݍ���

    public Sprite CreateSprite(string path)
    {

        Sprite createdSprite;

        // 1x1�Ŋm�ۂ��Ă�����LoadImage()�œK�؂ȃT�C�Y�ɕύX�����͗l
        Texture2D texture = ReadTexture(path, 1, 1);
        createdSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 1), 1);

        return createdSprite;
    }


    public Texture2D ReadTexture(string path, int width, int height)
    {
        try
        {
            byte[] readBinary = ReadPngFile(path);
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false); //mipmap��false�ɂ��Ȃ��Ɖ摜���ڂ₯��

            //texture �̃v���p�e�B��ݒ肷��
            texture.LoadImage(readBinary);
            texture.filterMode = FilterMode.Trilinear;

            return texture;
        }
        catch (Exception ex)
        {
            Debug.Log("png�t�@�C���̓ǂݍ��݂ŃG���[���������܂����B" + "\n" + ex);

            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false); //mipmap��false�ɂ��Ȃ��Ɖ摜���ڂ₯��
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
