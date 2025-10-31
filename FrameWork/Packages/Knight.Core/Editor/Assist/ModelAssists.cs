using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    public class ModelAssists
    {
        [MenuItem("GameObject/Other Tools/CalcMeshInfo", false, -1)]
        public static void CalcMeshInfo()
        {
            var rGameObject = Selection.activeGameObject;
            if (rGameObject == null) return;
            var rMeshList = new List<Mesh>();
            var rMeshFilters = rGameObject.GetComponentsInChildren<MeshFilter>();
            foreach (var rMeshFilter in rMeshFilters)
            {
                if (rMeshFilter.sharedMesh != null)
                {
                    rMeshList.Add(rMeshFilter.sharedMesh);
                }
            }
            var rSkinMeshRenderers = rGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var rSkinMeshRenderer in rSkinMeshRenderers)
            {
                if (rSkinMeshRenderer.sharedMesh != null)
                {

                    rMeshList.Add(rSkinMeshRenderer.sharedMesh);
                }
            }
            var nVertsCount = 0;
            var nTrisCount = 0;
            foreach (var rMesh in rMeshList)
            {
                nVertsCount += rMesh.vertexCount;
                nTrisCount += rMesh.triangles.Length / 3;
            }
            Debug.Log($"VertsCount:{nVertsCount} TrisCount:{nTrisCount}");
        }

        class MeshInfo
        {
            public string path;
            public Mesh mesh;
            public int refCount;
            public GameObject gameObject;
            public HashSet<string> goNames = new HashSet<string>();
        }

        [MenuItem("Tools/ResReport/Scene Mesh Info")]
        public static void ResReportSceneMeshInfo()
        {
            var rMeshList = new List<MeshInfo>();
            GameObject[] rGameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            for (int i = 0; i < rGameObjects.Length; ++i)
            {
                if (EditorUtility.IsPersistent(rGameObjects[i].transform.root.gameObject))
                    continue;
                GetMeshInfo(rMeshList, rGameObjects[i]);
            }
            Dictionary<string, MeshInfo> rReduceDupMap = new Dictionary<string, MeshInfo>();
            foreach (var rMeshInfo in rMeshList)
            {
                if (rReduceDupMap.ContainsKey(rMeshInfo.path))
                {
                    var rMeshInfoInMap = rReduceDupMap[rMeshInfo.path];
                    ++rMeshInfoInMap.refCount;
                    if (!rMeshInfoInMap.goNames.Contains(rMeshInfo.gameObject.name))
                        rMeshInfoInMap.goNames.Add(rMeshInfo.gameObject.name);
                }
                else
                {
                    rMeshInfo.refCount = 1;
                    rMeshInfo.goNames.Add(rMeshInfo.gameObject.name);
                    rReduceDupMap.Add(rMeshInfo.path, rMeshInfo);
                }
            }

            rMeshList.Clear();
            foreach (var rMeshInfo in rReduceDupMap.Values)
                rMeshList.Add(rMeshInfo);

            for (int i = 0; i < rMeshList.Count - 1; ++i)
            {
                for (int j = i + 1; j < rMeshList.Count; ++j)
                {
                    if (rMeshList[i].mesh.triangles.Length * rMeshList[i].refCount < rMeshList[j].mesh.triangles.Length * rMeshList[j].refCount)
                    {
                        MeshInfo tmp = rMeshList[i];
                        rMeshList[i] = rMeshList[j];
                        rMeshList[j] = tmp;
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("总面数,使用数,单模型面数,Mesh名,模型路径,GameObject\n");
            for (int i = 0; i < rMeshList.Count; ++i)
            {
                sb.Append(rMeshList[i].mesh.triangles.Length / 3 * rMeshList[i].refCount)
                    .Append(",")
                    .Append(rMeshList[i].refCount)
                    .Append(",")
                    .Append(rMeshList[i].mesh.triangles.Length / 3)
                    .Append(",")
                    .Append(rMeshList[i].mesh.name)
                    .Append(",")
                    .Append(rMeshList[i].path)
                    .Append(",");
                int n = 0;
                foreach (string s in rMeshList[i].goNames)
                {
                    sb.Append(s);
                    if (n != rMeshList[i].goNames.Count)
                        sb.Append("/");
                    ++n;
                    if (n > 10)
                        break;
                }
                if (rMeshList[i].goNames.Count > 10)
                    sb.Append("...");
                sb.Append("\n");
            }
            File.WriteAllText("../mesh_triangle.csv", sb.ToString());
            Debug.Log("面详细信息已输出到../mesh_triangle.csv");
        }

        static void GetMeshInfo(List<MeshInfo> rMeshList, GameObject rGameObject)
        {
            var rMeshFilters = rGameObject.GetComponents<MeshFilter>();
            foreach (var rMeshFilter in rMeshFilters)
            {
                if (rMeshFilter.sharedMesh != null)
                {
                    MeshInfo rMeshInfo = new MeshInfo();
                    rMeshInfo.mesh = rMeshFilter.sharedMesh;
                    rMeshInfo.gameObject = rMeshFilter.gameObject;
                    rMeshInfo.path = AssetDatabase.GetAssetPath(rMeshFilter.sharedMesh) + "/" + rMeshInfo.mesh.name;
                    rMeshList.Add(rMeshInfo);
                }
            }
            var rSkinMeshRenderers = rGameObject.GetComponents<SkinnedMeshRenderer>();
            foreach (var rSkinMeshRenderer in rSkinMeshRenderers)
            {
                if (rSkinMeshRenderer.sharedMesh != null)
                {
                    MeshInfo rMeshInfo = new MeshInfo();
                    rMeshInfo.mesh = rSkinMeshRenderer.sharedMesh;
                    rMeshInfo.gameObject = rSkinMeshRenderer.gameObject;
                    rMeshInfo.path = AssetDatabase.GetAssetPath(rSkinMeshRenderer.sharedMesh) + "/" + rMeshInfo.mesh.name;
                    rMeshList.Add(rMeshInfo);
                }
            }
        }

        [MenuItem("GameObject/Other Tools/CalcTextureInfo", false, -1)]
        public static void CalcTextureInfo()
        {
            var rGameObject = Selection.activeGameObject;
            if (rGameObject == null) return;
            var rMatList = new List<Material>();
            var rMeshRenderers = rGameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var rMeshRenderer in rMeshRenderers)
            {
                if (rMeshRenderer.sharedMaterials != null && rMeshRenderer.sharedMaterials.Length > 0)
                {
                    rMatList.AddRange(rMeshRenderer.sharedMaterials);
                }
            }

            StringBuilder rSB = new StringBuilder();
            rSB.Append("MatCount=").Append(rMatList.Count).Append("\n");
            HashSet<string> rTexGUIDSet = new HashSet<string>();
            SortedDictionary<int, int> rTexCoundByWidth = new SortedDictionary<int, int>();

            int nTextureCount = 0;
            foreach (var rMat in rMatList)
            {
                if (rMat == null)
                    continue;
                int[] rTextureNameIDs = rMat.GetTexturePropertyNameIDs();
                for (int i = 0; i < rTextureNameIDs.Length; ++i)
                {
                    Texture rTex = rMat.GetTexture(rTextureNameIDs[i]);
                    if (rTex != null)
                    {
                        string rPath = AssetDatabase.GetAssetPath(rTex);
                        string rGUID = AssetDatabase.AssetPathToGUID(rPath);
                        if (!rTexGUIDSet.Contains(rGUID))
                        {
                            rTexGUIDSet.Add(rGUID);
                            ++nTextureCount;
                            if (!rTexCoundByWidth.ContainsKey(rTex.width))
                                rTexCoundByWidth.Add(rTex.width, 0);
                            ++rTexCoundByWidth[rTex.width];
                        }
                    }
                }
            }
            rSB.Append("TextureCount=").Append(nTextureCount).Append("\n");
            foreach (var rPair in rTexCoundByWidth)
            {
                rSB.Append("size=").Append(rPair.Key).
                    Append(",count=").Append(rPair.Value).Append("\n");
            }
            Debug.Log(rSB.ToString());
        }

        [MenuItem("GameObject/Other Tools/Texture Size Info", false, -1)]
        public static void TextureSizeInfo()
        {
            var rGameObject = Selection.activeGameObject;
            if (rGameObject == null) return;
            var rMatList = new List<Material>();
            var rMeshRenderers = rGameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var rMeshRenderer in rMeshRenderers)
                if (rMeshRenderer.sharedMaterials != null && rMeshRenderer.sharedMaterials.Length > 0)
                    rMatList.AddRange(rMeshRenderer.sharedMaterials);

            Dictionary<string, int> rTexInfo = new Dictionary<string, int>();

            foreach (var rMat in rMatList)
            {
                int[] rTextureNameIDs = rMat.GetTexturePropertyNameIDs();
                for (int i = 0; i < rTextureNameIDs.Length; ++i)
                {
                    Texture rTex = rMat.GetTexture(rTextureNameIDs[i]);
                    if (rTex != null)
                    {
                        string rPath = AssetDatabase.GetAssetPath(rTex);
                        if (!rTexInfo.ContainsKey(rPath))
                            rTexInfo.Add(rPath, rTex.width);
                    }
                }
            }
            StringBuilder rSB = new StringBuilder();

            rSB.Clear();
            List<string> rKeys = new List<string>(rTexInfo.Keys);
            rKeys.Sort(new Comparison<string>((string a, string b) =>
            {
                return Path.GetFileName(a).CompareTo(Path.GetFileName(b));
            }));
            for (int i = 0; i < rKeys.Count; ++i)
            {
                rSB.Append(Path.GetFileName(rKeys[i]));
                rSB.Append(",").Append(rTexInfo[rKeys[i]]);
                rSB.Append("\n");
            }
            File.WriteAllText("../tex_detail.txt", rSB.ToString());
        }
    }
}
