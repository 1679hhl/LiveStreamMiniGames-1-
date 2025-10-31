using System;
using System.Collections;
using System.Collections.Generic;
using Knight.Core;
using UnityEngine;

/// <summary>
/// 抖音官游直播模式
/// </summary>
public enum DouyinGuanyouLiveMode
{
    /// <summary>
    /// 直播伴侣
    /// </summary>
    BanLv = 1,

    /// <summary>
    /// obs
    /// </summary>
    OBS = 2,
}

public class DouYin_SDK : TSingleton<DouYin_SDK>
{
    private DouYin_SDK()
    {
        
    }
    //101.43.90.6:26002
    private string mTestToken = "";
    public string Token { get; protected set; } = string.Empty;
    public string Port { get; protected set; } = "26002";
    public string RemoteUrl { get; protected set; } = "101.43.90.6";
    public DouyinGuanyouLiveMode DyGuanyouLiveMode { get; set; } = DouyinGuanyouLiveMode.BanLv;
    
    public void Initialize()
    {
        InitToken();
        // InitCloud();
    }
    
    protected virtual void InitToken()
    {
        string[] arguments = Environment.GetCommandLineArgs();
        string _token = "-token=";
        string _tokenContent = mTestToken;

        foreach (var arg in arguments)
        {
            if (arg.StartsWith(_token))
            {
                _tokenContent = arg.Replace(_token, "");
                break;
            }
        }

        if (_tokenContent.Length > 0)
        {
            Token = _tokenContent;
            DyGuanyouLiveMode = DouyinGuanyouLiveMode.BanLv;
            
        }
        else
        {
            DyGuanyouLiveMode = DouyinGuanyouLiveMode.OBS;
        }
    }
}
