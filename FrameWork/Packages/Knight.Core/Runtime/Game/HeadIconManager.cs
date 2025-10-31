using System;
using System.Collections;
using System.Collections.Generic;
using Knight.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebP;

public class HeadIconManager : TSingleton<HeadIconManager>
{
    public Dictionary<string, Sprite> HeadIncoDic = new Dictionary<string, Sprite>(500);

    private HeadIconManager()
    {
        // UIRoot.Instance.UICamera
    }

    public void LoadHeadInfo(string rAvatartUrl, Image rImg)
    {
        CoroutineManager.Instance.Start(this.LoadHeadInfoC(rAvatartUrl, rImg));
    }

    public void LoadHeadInfo(string rAvatarUrl, SpriteRenderer rSpriteRenderer)
    {
        CoroutineManager.Instance.Start(this.LoadHeadInfoC(rAvatarUrl, rSpriteRenderer));
    }

    public Sprite LoadSprte(string rLoadPath)
    {
        Sprite rTargetSrite = null;
        if (HeadIncoDic.TryGetValue(rLoadPath, out var rSprite))
        {
            return rSprite;
        }
        else
        {
            rTargetSrite = Resources.Load<Sprite>(rLoadPath);
            this.HeadIncoDic.TryAdd(rLoadPath, rTargetSrite);
        }
        return rTargetSrite;
    }

    public IEnumerator LoadHeadInfoC(string rAvatarUrl, Image rImg)
    {
        if(string.IsNullOrEmpty(rAvatarUrl) || rImg == null)
            yield break;
        if (this.HeadIncoDic.TryGetValue(rAvatarUrl, out var rSprite))
        {
            rImg.sprite = rSprite;
            yield break;
        }
        UnityWebRequest m_webrequest = UnityWebRequest.Get(rAvatarUrl);
        yield return m_webrequest.SendWebRequest();
        try
        {
            if (m_webrequest.result != UnityWebRequest.Result.Success)
                Debug.LogWarning("Failed to download image");
            else
            {
                var rBytes = m_webrequest.downloadHandler.data;
                Texture2D texture = Texture2DExt.CreateTexture2DFromWebP(rBytes, lMipmaps: true, lLinear: false,
                    lError: out Error lError);
                Sprite createSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                if (rImg != null) rImg.sprite = createSprite;
                this.HeadIncoDic.TryAdd(rAvatarUrl, createSprite);
            }
        }
        catch (Exception e)
        {
            LogManager.LogRelease(e);
        }

        m_webrequest = UnityWebRequestTexture.GetTexture(rAvatarUrl);
        yield return m_webrequest.SendWebRequest();
        if (m_webrequest.result != UnityWebRequest.Result.Success)
            Debug.LogWarning("Failed to download image");
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)m_webrequest.downloadHandler).texture;
            Sprite createSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            if (rImg != null) rImg.sprite = createSprite;
            this.HeadIncoDic.TryAdd(rAvatarUrl, createSprite);
        }
    }

    public IEnumerator LoadHeadInfoC(string rAvatarUrl, SpriteRenderer rImg)
    {
        if(string.IsNullOrEmpty(rAvatarUrl) || rImg == null)
            yield break;
        if (this.HeadIncoDic.TryGetValue(rAvatarUrl, out var rSprite))
        {
            rImg.sprite = rSprite;
            yield break;
        }
        UnityWebRequest m_webrequest = UnityWebRequest.Get(rAvatarUrl);
        yield return m_webrequest.SendWebRequest();
        try
        {
            if (m_webrequest.result != UnityWebRequest.Result.Success)
                Debug.LogWarning("Failed to download image");
            else
            {
                var rBytes = m_webrequest.downloadHandler.data;
                Texture2D texture = Texture2DExt.CreateTexture2DFromWebP(rBytes, lMipmaps: true, lLinear: false,
                    lError: out Error lError);
                Sprite createSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f),texture.width);
                if (rImg != null) rImg.sprite = createSprite;
                this.HeadIncoDic.TryAdd(rAvatarUrl, createSprite);
            }
        }
        catch (Exception e)
        {
            LogManager.LogRelease(e);
        }

        m_webrequest = UnityWebRequestTexture.GetTexture(rAvatarUrl);
        yield return m_webrequest.SendWebRequest();
        if (m_webrequest.result != UnityWebRequest.Result.Success)
            Debug.LogWarning("Failed to download image");
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)m_webrequest.downloadHandler).texture;
            Sprite createSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f),tex.width);
            if (rImg != null) rImg.sprite = createSprite;
            this.HeadIncoDic.TryAdd(rAvatarUrl, createSprite);
        }
    }
    
    public IEnumerator LoadHeadInfoC(string rAvatarUrl, Image rImg, GameObject Root, float KillTime)
    {
        if(string.IsNullOrEmpty(rAvatarUrl) || rImg == null)
            yield break;
        if (this.HeadIncoDic.TryGetValue(rAvatarUrl, out var rSprite))
            rImg.sprite = rSprite;
        UnityWebRequest m_webrequest = UnityWebRequest.Get(rAvatarUrl);
        yield return m_webrequest.SendWebRequest();
        try
        {
            if (m_webrequest.result != UnityWebRequest.Result.Success)
                Debug.LogWarning("Failed to download image");
            else
            {
                var rBytes = m_webrequest.downloadHandler.data;
                Texture2D texture = Texture2DExt.CreateTexture2DFromWebP(rBytes, lMipmaps: true, lLinear: false,
                    lError: out Error lError);
                Sprite createSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f),100);
                if (rImg != null) rImg.sprite = createSprite;
                this.HeadIncoDic.TryAdd(rAvatarUrl, createSprite);
            }
        }
        catch (Exception e)
        {
            LogManager.LogWarning(e);
        }

        m_webrequest = UnityWebRequestTexture.GetTexture(rAvatarUrl);
        yield return m_webrequest.SendWebRequest();
        if (m_webrequest.result != UnityWebRequest.Result.Success)
            Debug.LogError("Failed to download image");
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)m_webrequest.downloadHandler).texture;
            Sprite createSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f),100);
            if (rImg != null) rImg.sprite = createSprite;
            this.HeadIncoDic.TryAdd(rAvatarUrl, createSprite);
        }

        yield return new WaitForSeconds(KillTime);
        GameObject.Destroy(Root);
    }

    public void LoadHeadInfo(string rAvatartUrl, Image rImg, GameObject Root, float DestroyTime)
    {
        CoroutineManager.Instance.Start(this.LoadHeadInfoC(rAvatartUrl, rImg,Root,DestroyTime));
    }
    public void Destroy()
    {
        this.HeadIncoDic.Clear();
    }
}