using System;
using System.Collections;
using System.Collections.Generic;
using Knight.Core;
using UnityEngine;

public class TikTok_SDK : TSingleton<TikTok_SDK>
{
    private TikTok_SDK()
    {
        
    }

    public string TiktokID = string.Empty;
    public void Initialize()
    {
        InitToken();
    }
    
    protected virtual void InitToken()
    {
        string[] arguments = Environment.GetCommandLineArgs();
        foreach (var rLine in arguments)
        {
            LogManager.LogRelease($"[Tiktok]:{rLine}");
        }
        
        for (int i = 0; i < arguments.Length; i++)
        {
            if (arguments[i] == "-tkId" && i + 1 < arguments.Length)
            {
                this.TiktokID = arguments[i + 1];
                Debug.Log("Received name parameter: " + this.TiktokID);
            }
        }
    }
}