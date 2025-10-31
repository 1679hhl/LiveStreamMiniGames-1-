#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Google.Protobuf.Compiler;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AllPro2Cs : Editor
{
    private static int handlePb2CsIndex;
    private static string rootDir, protoDir, hotfixMessageCodePath, idDirPath;
    private static StringBuilder idDicStringBulider = new StringBuilder();
    private static StringBuilder idDicStringBulider2 = new StringBuilder();
    private static StringBuilder MsgID = new StringBuilder();
    private static StringBuilder respEventStringBuilder = new StringBuilder();
    private static string outputPath = "Assets\\Scripts\\Game\\NetWorkGenerate";

    [MenuItem("Tools/Proto2CS")]
    public static void AllProto2CS()
    {
        handlePb2CsIndex = 0;

        rootDir = Environment.CurrentDirectory;
        protoDir = Path.Combine(rootDir, "Proto/");

        string protoc;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            protoc = Path.Combine(protoDir, "protoc.exe");
        }
        else
        {
            protoc = Path.Combine(protoDir, "protoc");
        }

        idDicStringBulider.Clear();
        idDicStringBulider2.Clear();
        MsgID.Clear();
        respEventStringBuilder.Clear();
        InitReflectionMsgIDDic();

        hotfixMessageCodePath = Path.Combine(rootDir, outputPath);
        if (Directory.Exists(hotfixMessageCodePath))
        {
            Directory.Delete(hotfixMessageCodePath, true);
        }

        Directory.CreateDirectory(hotfixMessageCodePath);

        HandleForeachFile(protoc, protoDir);
        Debug.Log($"pb2cs count:{handlePb2CsIndex}");


        EndReflectionMsgIDDic();
        WriteEvent();
        //Run(protoc, argument2, waitExit: true);
        AssetDatabase.Refresh();
    }


    private static void HandleForeachFile(string protoc, string filePathByForeach)
    {
        hotfixMessageCodePath = Path.Combine(rootDir, $"{outputPath}/");

        DirectoryInfo theFolder = new DirectoryInfo(filePathByForeach);
        DirectoryInfo[] dirInfo = theFolder.GetDirectories(); //获取所在目录的文件夹
        FileInfo[] file = theFolder.GetFiles(); //获取所在目录的文件

        foreach (FileInfo fileItem in file) //遍历文件
        {
            //result += fileItem.DirectoryName + @"\" + fileItem.Name + "\n";

            var splitRes = fileItem.Name.Split("_");
            if (splitRes.Length > 1)
            {
                if (fileItem.FullName.Contains(".proto"))
                {
                    //Debug.Log($"proto--->{fileItem.Name}");
                    // string argument2 = $"--csharp_out=\"{hotfixMessageCodePath}\" --proto_path=\"{protoDir}{fileItem.Name}\"";

                    string argument2 =
                        $"--csharp_out=\"{hotfixMessageCodePath}\" --proto_path=\"{fileItem.DirectoryName}\"/ {fileItem.Name}";

                    handlePb2CsIndex++;
                    AddMsgID2Dic(fileItem.Name);
                    Run(protoc, argument2, waitExit: true);
                    //CreateRespHandle(fileItem.Name);
                }
            }
        }

        //遍历文件夹
        foreach (DirectoryInfo NextFolder in dirInfo)
        {
            if (!NextFolder.FullName.Contains("enum") && !NextFolder.FullName.Contains("NetSvnProto"))
            {
                HandleForeachFile(protoc, NextFolder.FullName);
            }
        }
    }

    static void InitReflectionMsgIDDic()
    {
        idDirPath = Path.Combine(rootDir, $"{outputPath}/ProtoBufDic.cs");
        if (File.Exists(idDirPath))
        {
            File.Delete(idDirPath);
        }

        idDicStringBulider2.Clear();
        idDicStringBulider.Clear();
        MsgID.Clear();

        idDicStringBulider.Append("using System;\r\n");
        idDicStringBulider.Append("using System.Collections.Generic;\r\n");
        idDicStringBulider.Append("using Pb;\r\n");
        idDicStringBulider.Append("public class ProtoBufDic \r\n{\r\n");

        idDicStringBulider.Append("    public static Dictionary<int, Type> PbDic = new Dictionary<int, Type>()\r\n");
        idDicStringBulider.Append("    { \r\n");

        idDicStringBulider2.Append(
            "    public static Dictionary<Type, int> PbTypeDic = new Dictionary<Type, int>()\r\n");
        idDicStringBulider2.Append("    { \r\n");
    }

    static void EndReflectionMsgIDDic()
    {
        var endChar = "    }; \r\n";
        idDicStringBulider.Append(endChar);

        idDicStringBulider2.Append(endChar);
        idDicStringBulider.Append(idDicStringBulider2);
        idDicStringBulider.Append(MsgID);

        idDicStringBulider.Append("} \r\n");
        File.WriteAllText(idDirPath, idDicStringBulider.ToString());
    }

    static void AddMsgID2Dic(string fileName)
    {
        bool isZ;

        // int2type
        string numId = System.Text.RegularExpressions.Regex.Replace(fileName, @"[^0-9]+", "");
        isZ = fileName.StartsWith("Z" + numId);

        idDicStringBulider.Append("        {");
        idDicStringBulider.Append((isZ ? "" : "-") + numId);
        idDicStringBulider.Append(",");

        idDicStringBulider.Append("typeof(");
        idDicStringBulider.Append((fileName.Split("_")[1].Replace(".proto", "")));
        idDicStringBulider.Append(")");

        idDicStringBulider.Append("},\r\n");


        // type2int
        idDicStringBulider2.Append("        {");
        idDicStringBulider2.Append("typeof(");
        idDicStringBulider2.Append((fileName.Split("_")[1].Replace(".proto", "")));
        idDicStringBulider2.Append(")");

        idDicStringBulider2.Append(",");
        idDicStringBulider2.Append((isZ ? "" : "-") + numId);

        idDicStringBulider2.Append("},\r\n");
        
        //MsgID
        MsgID.Append("        public const int ");
        MsgID.Append(fileName.Split("_")[1].Replace(".proto", "")+"MsgID");
        MsgID.Append("="+ (isZ ? "" : "-") + numId);
        MsgID.Append(";\r\n");
    }
    
    //生成RespHandle
    static void CreateRespHandle(string fileName)
    {
        string respName = (fileName.Split("_")[1].Replace(".proto", ""));
        if (respName.EndsWith("Resp") || respName.EndsWith("Notify"))
        {
            GUICreateUIPanel.StartCreateMsgHandle(respName);
            AddRespEvent(respName);
        }
    }

    /// <summary>
    /// 添加到事件
    /// </summary>
    static void AddRespEvent(string respName)
    {
        respEventStringBuilder.Append(GUICreateUIPanel.RespEventStrng(respName));
    }
    
    //
    static void WriteEvent()
    {
        // GUICreateUIPanel.StartCreateRespEventHandle(respEventStringBuilder.ToString());
    }

    public static Process Run(string exe, string arguments, string workingDirectory = ".", bool waitExit = false)
    {
        try
        {
            bool redirectStandardOutput = true;
            bool redirectStandardError = true;
            bool useShellExecute = false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                redirectStandardOutput = false;
                redirectStandardError = false;
                useShellExecute = true;
            }

            if (waitExit)
            {
                redirectStandardOutput = true;
                redirectStandardError = true;
                useShellExecute = false;
            }

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = exe,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = useShellExecute,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = redirectStandardOutput,
                RedirectStandardError = redirectStandardError,
            };

            Process process = Process.Start(info);

            if (waitExit)
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"{process.StandardOutput.ReadToEnd()} {process.StandardError.ReadToEnd()}");
                }
            }

            return process;
        }
        catch (Exception e)
        {
            throw new Exception($"dir: {Path.GetFullPath(workingDirectory)}, command: {exe} {arguments}", e);
        }
    }
}
#endif
