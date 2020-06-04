using System.Collections;
using System.Collections.Generic;
using System.IO;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class ImageDownloadUtils : MonoBehaviour,ISingleton
{ 
    
    public static ImageDownloadUtils Instance
    {
        get { return MonoSingletonProperty<ImageDownloadUtils>.Instance; }
    }

    
    public void OnSingletonInit()
    {
    }
    
    private static List<string> files = null;
    public void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/ImageCache/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/ImageCache/");
        }
    }

    public void SetAsyncImage(string url, Image image,bool autoImageSize = false)
    {
        if (url == null ||"".Equals(url))
        {
            //Log.I("the url is empty");
            return;
        }
        StartCoroutine(AsyncImage(url,image,autoImageSize));
    }

    IEnumerator AsyncImage(string url, Image image, bool autoImageSize = false)
    {
        //开始下载图片前，将UITexture的主图片设置为占位图
        //image.sprite = placeholder;


        if (url.StartsWith("file://"))
        {
           yield  return  StartCoroutine(LoadLocalImage(url, image,autoImageSize));;
        }
        //判断是否是第一次加载这张图片
        if (!File.Exists(path + url.GetHashCode()))
        {
            if (files != null)
            {
                files.Add(url.GetHashCode().ToString());
            }
            else {
                files = new List<string>();
                files.Add(url.GetHashCode().ToString());
            }
            
            //如果之前不存在缓存文件
           yield return StartCoroutine(DownloadImage(url, image,autoImageSize));
        }
        else
        {
            yield return  StartCoroutine(LoadLocalImage(url, image,autoImageSize));
        }
    }

    IEnumerator DownloadImage(string url, Image image, bool autoImageSize)
    {
        WWW www = new WWW(url);
        yield return www;
        
        Texture2D tex2d = www.texture;
        //将图片保存至缓存路径
        byte[] pngData = tex2d.EncodeToPNG();
        File.WriteAllBytes(path + url.GetHashCode(), pngData);
        Sprite m_sprite = null;
        m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width,tex2d.height), new Vector2(0, 0));
        if (image != null && !image.IsDestroyed())
        {
            if (autoImageSize)
            {
                float asp =tex2d.width*1f / tex2d.height; 
                float height = image.preferredWidth/(asp);
                Log.E("height=="+height);
                RectTransform rect = image.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2( rect.sizeDelta.x, height);
            }
            image.sprite = m_sprite;
        } 
        www.Dispose();
        www = null;
        Resources.UnloadUnusedAssets();
    }

    IEnumerator LoadLocalImage(string url, Image image, bool autoImageSize)
    {
        string filePath;
        if (!url.StartsWith("file://"))
        {
            filePath = "file://" + path + url.GetHashCode();
        }
        else
        {
            filePath = url;
        }


        // Debug.Log("getting local image:" + filePath);
        WWW www = new WWW(filePath);
        yield return www;

        Texture2D tex2d = www.texture;
        Sprite m_sprite = null;
        m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width,tex2d.height), new Vector2(0, 0));

        if (image != null && !image.IsDestroyed())
        {
            if (autoImageSize)
            {
                float asp =tex2d.width*1f / tex2d.height; 
                float height = image.preferredWidth/(asp);
                Log.E("height=="+height);
                RectTransform rect = image.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2( rect.sizeDelta.x, height);
            }
            image.sprite = m_sprite;
        } 
        www.Dispose();
        www = null;
        Resources.UnloadUnusedAssets();
    }

    public string path
    {
        get
        {
            //pc,ios //android :jar:file//
            return Application.persistentDataPath + "/ImageCache/";

        }
    }

    public void OnDisable()
    {
        Resources.UnloadUnusedAssets();
    }

    public void OnDestroy()
    {
        //CleanFiles();
    }

    public void CleanFiles() {
        if (files != null && files.Count > 0) {
            for (int i = files.Count - 1; i >= 0; i--) {
                if (!Directory.Exists(path + files[i])) {
                    File.Delete(path + files[i]);
                    files.RemoveAt(i);
                }
            }
        }
    }

}

