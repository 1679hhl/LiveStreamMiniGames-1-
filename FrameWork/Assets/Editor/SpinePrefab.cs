using System.IO;
using Spine;
using Spine.Unity;
using UnityEditor;
using UnityEngine;
public class SpinePrefab : Editor
{
    public enum SpineType
    {
        UI,
        Scene
    }
    private static string SpinePath = "Assets/Prefabs/";
    private static string GenarateTargetPath;
    [MenuItem("Assets/GeneratePrefab/Spine", false, 23)]
    public static void GenerateSpine()
    {
       // GenerateSpine(SpineType.Scene);
       SpineWindow.Init(SpineType.Scene);
    }

    public static void GenerateSpine(SpineType type,float scale = 1.28f)
    {
        Debug.Log(" Start GenerateMap ");
        UnityEngine.Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        int count = 0;
        int scaleCount = 0;
        for (int i = 0; i < objs.Length; ++i)
        {
            if (AssetDatabase.GetAssetPath(objs[i]).EndsWith("SkeletonData.asset"))
            {
                var curObj = objs[i];
                var path = AssetDatabase.GetAssetPath(curObj);
                var curAssetObj = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(path);
                
                GenarateTargetPath = SpinePath + path.Replace("Assets/", "").Replace(curObj.name+".asset","");
                
                
                // string name = curObj.name.Split("_".ToCharArray())[0];
                // int nameInt;
                // if (int.TryParse(name,out nameInt))//水果和怪物
                // {
                //     if (curObj is SkeletonDataAsset s)
                //     {
                //         s.scale = 0.01f * scale;
                //         EditorUtility.SetDirty(curObj);
                //         scaleCount++;
                //     }
                // }
                //
                // var curPath = GenarateTargetPath+name+ ".prefab";
                
                var temps = GenarateTargetPath.Split("/".ToCharArray());
                string name = temps[temps.Length-2];
                // int nameInt;
                // if (int.TryParse(name,out nameInt))//水果和怪物
                {
                    if (curObj is SkeletonDataAsset s)
                    {
                        s.scale = 0.01f * scale;
                      
                        //EditorUtility.ClearDirty(curObj);
                       
                        //EditorUtility.dir
                        AssetDatabase.SaveAssets();
                        EditorUtility.SetDirty(curObj);
                        AssetDatabase.Refresh();
                        EditorUtility.RequestScriptReload();
                        scaleCount++;
                    }
                }

                var curPath = GenarateTargetPath+name+ ".prefab";//type == SpineType.Scene?GeneratePath+name+ ".prefab": GenerateUIPath+name + ".prefab";

                var hadGameObj = AssetDatabase.LoadAssetAtPath<GameObject>(curPath);
                if (hadGameObj)
                {
                    if (AssetDatabase.DeleteAsset(curPath))
                    {
                        Generate(curAssetObj,name, curPath,type);
                        Debug.Log(curObj.name+" 已经替换!");
                    }

                }
                else
                {
                    Generate(curAssetObj,name, curPath,type);
                    Debug.Log(curObj.name+" 已经生成!");
                }

                count++;
            }
        }

        EditorUtility.DisplayDialog("生成预制体成功", " GenerateSpine End -> 总共生成预制体:" + count + "个！ 缩放个数："+scaleCount+"个！,缩放系数："+scale, "确定");
        Debug.Log(" GenerateMap End -> 总共生成预制体:"+count+"个！");
    }


    [MenuItem("Assets/GeneratePrefab/UISpine", false, 23)]
    static void GenerateUIPrefab()
    {
        SpineWindow.Init(SpineType.UI);
        //GenerateSpine(SpineType.UI);
    }


    static void Generate(SkeletonDataAsset asset,string name,string path,SpineType type = SpineType.Scene)
    {
        EditorUtility.SetDirty(asset);
        var newGo = new GameObject(name);
        var objIns = Instantiate(newGo);
        objIns.AddComponent<MeshFilter>();
        objIns.AddComponent<MeshRenderer>();
        if (type == SpineType.Scene)
        {
            var anim = objIns.AddComponent<SkeletonAnimation>();
            anim.skeletonDataAsset = asset;
        }
        else
        {
            var anim = objIns.AddComponent<SkeletonGraphic>();
            anim.skeletonDataAsset = asset;
        }
        EditorUtility.SetDirty(objIns);
        
        if (!Directory.Exists(GenarateTargetPath))
            Directory.CreateDirectory(GenarateTargetPath);

        PrefabUtility.SaveAsPrefabAsset(objIns, path);
        DestroyImmediate(newGo);
        DestroyImmediate(objIns);
    }

}

public class SpineWindow : EditorWindow
{
    public static float SpineScale = 1.0f;
    public static SpineWindow MyWindow;
    private static SpinePrefab.SpineType SpineType;
    public static void Init(SpinePrefab.SpineType type)
    {
        MyWindow = (SpineWindow)EditorWindow.GetWindow(typeof(SpineWindow), false, "SpineWindow", true);//创建窗口
        MyWindow.Show();//展示
        SpineType = type;
    }
    void OnGUI()
    {

        EditorGUI.LabelField(new Rect(10, 10, 80, 15),"请输入缩放系数:");
        SpineScale = EditorGUI.FloatField(new Rect(100, 10, 300, 15), SpineScale);
     
        if (GUI.Button(new Rect(420,10,100,20),"开始生成"))
        {
            MyWindow.Close();
            SpinePrefab.GenerateSpine(SpineType,SpineScale);
        }

        //Debug.Log("test--->"+SpineScale);
    }
    
}