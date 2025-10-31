#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public enum PanelType
{
    UIBasePanel,
    CenterScaleOpenPanel,
    AutoClosePanel
}
public class GUICreateUIPanel : EditorWindow
{
    private static string rootDir;
    private string panelTempPath = "Assets/FrameWork/ScriptTemplate/UIPanelTemplate.cs.txt";//模版路径
    private string panelViewTemPath = "Assets/FrameWork/ScriptTemplate/UIPanelViewTemplate.cs.txt";//模版路径
    private static string EventManagerTemPath = "Assets/FrameWork/ScriptTemplate/EventManagerTemplate.cs.txt";//模版路径
    private static string HandleMsgTempPath = "Assets/FrameWork/ScriptTemplate/HandleMsgTemplate.cs.txt";//模版路径
    private string tempUIPanel = "#SCRIPTNAME#";
    private string tempUIPanelType = "#PANELTYPE#";
    private string tempUIPanelView = "#SCRIPTNAMEVIEW#";
    private static string tempRespName = "#RESPNAME#";
    private static string eventContentName = "#EVENTCONTENT#";
    private static string tempEventScript = "public static FEvent<#RESPNAME#> Re#RESPNAME#Event = new FEvent<#RESPNAME#>();";
    
    
    private string PanelName = "NewUIPanel";
    public PanelType PanelType = PanelType.CenterScaleOpenPanel;
    private string selectPath;
    
    static string  handleText = File.ReadAllText(HandleMsgTempPath);
    /// <summary>
    /// 创建一个窗口
    /// </summary>
    [MenuItem("Assets/CreateUIPanel")]
    static void CreateWin()
    {
        var win = GetWindow<GUICreateUIPanel>(false, "创建UIPanel");
        win.Show();
    }

    private void OnGUI()
    {
        PanelName = GUILayout.TextField(PanelName);
        PanelType =(PanelType)EditorGUILayout.EnumPopup(new GUIContent("UIPanel类型","panel类型"), PanelType);
        //自定义按钮
        if (GUILayout.Button("创建"))
        {
            StartCreate();
        }
    }

    void StartCreate()
    {
        //读取模版
        //创建Panel
        // ReplaceTemp(0);
        rootDir = Environment.CurrentDirectory;
        PanelName = GUILayout.TextField(PanelName);
        selectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        CreateFile(ReplaceTemp(1),$"{PanelName}View");
        CreateFile(ReplaceTemp(0),PanelName);
        //创建View

        // StartCreateMsgHandle(PanelName);
    }
    void CreateFile(string contents,string fileName)
    {
        //获取当前所选择的目录（相对于Assets的路径）
        var path = Application.dataPath.Replace("Assets", "") + "/";
        var newFileName = fileName + ".cs";
        var newFilePath = selectPath + "/" + newFileName;
        var fullPath = path + newFilePath;

        //简单的重名处理
        if (File.Exists(fullPath))
        {
            var newName = "new_UIPanel" + "-" + UnityEngine.Random.Range(0, 100) + ".cs";
            newFilePath = selectPath + "/" + newName;
            fullPath = fullPath.Replace(newFileName, newName);
        }

        //如果是空白文件，编码并没有设成UTF-8
        File.WriteAllText(fullPath, contents, Encoding.UTF8);

        AssetDatabase.Refresh();

        //选中新创建的文件
        var asset = AssetDatabase.LoadAssetAtPath(newFilePath, typeof(Object));
        Selection.activeObject = asset;
    }
    /// <summary>
    /// 替换模版
    /// </summary>
    string ReplaceTemp(int type)
    {
        string UIpanelName = PanelName;
        string UIpanelVieName = $"{PanelName}View";
        string UIpanelType = PanelType.ToString();
        string text = type == 0 ? File.ReadAllText(panelTempPath) : File.ReadAllText(panelViewTemPath);
        var newTxt = text.Replace(tempUIPanel, UIpanelName);
        newTxt = newTxt.Replace(tempUIPanelType, UIpanelType);
        newTxt = newTxt.Replace(tempUIPanelView, UIpanelVieName);
        return newTxt;
    }

    public static void StartCreateMsgHandle(string respName)
    {
        string handleSavePath = Path.Combine(Environment.CurrentDirectory,"Assets", "Scripts", "ProtoMessageExtend");
        if (!Directory.Exists(handleSavePath))
        {
            Directory.CreateDirectory(handleSavePath);
        }
       handleSavePath = Path.Combine(handleSavePath, $"{respName}Extend.cs");
       var newTxt = handleText.Replace(tempRespName, respName);
       //保存文件
       CreateNewFile(newTxt,handleSavePath);
    }

    public static void StartCreateRespEventHandle(string content)
    {
        string eventSavePath = Path.Combine(Environment.CurrentDirectory,"Assets", "Scripts","EventManagerExtend");
        if (!Directory.Exists(eventSavePath))
        {
            Directory.CreateDirectory(eventSavePath);
        }
        eventSavePath = Path.Combine(eventSavePath, "EventManagerExtend.cs");
        string eventManager = File.ReadAllText(EventManagerTemPath);
        var newEventTxt = eventManager.Replace(eventContentName, content);
        CreateNewFile(newEventTxt,eventSavePath);
    }

    public static string RespEventStrng(string respName)
    {
        string newEventTxt = tempEventScript.Replace(tempRespName, respName);
        newEventTxt += "\n" ;
        newEventTxt += "\t";
        return newEventTxt;
    }

    void CreateMsgHandle()
    {
        
    }

    static void CreateNewFile(string content, string filePath)
    {
        File.WriteAllText(filePath, content, Encoding.UTF8);
        AssetDatabase.Refresh();
    }
}
#endif

