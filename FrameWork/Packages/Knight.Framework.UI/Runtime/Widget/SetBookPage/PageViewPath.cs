
using Knight.Core;
using System.Collections.Generic;
using System.IO;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(100)]
    [DisallowMultipleComponent]
    public class PageViewPath : MonoBehaviour
    {
        [HideInInspector][SerializeField]
        protected List<PageObject> mObjects;

        public List<PageObject> Objects { get { return this.mObjects; } }

        private List<PageObjectInfo> mPageObjectsData = new List<PageObjectInfo>();
        public void ExportConfig()
        {
            this.mPageObjectsData.Clear();
            for (int i = 0; i < this.mObjects.Count; i++)
            {
                var rPosInfo = new PageObjectInfo();
                rPosInfo.Name = this.mObjects[i].Name;
                rPosInfo.ObjectPath = this.mObjects[i].ObjectPath;
                rPosInfo.Component = this.mObjects[i].Component;
                this.mPageObjectsData.Add(rPosInfo);
            }

            var rStrJson = Newtonsoft.Json.JsonConvert.SerializeObject(this.mPageObjectsData, Newtonsoft.Json.Formatting.Indented);
            var StrPath = PageViewPathTool.GetPathConfigName(this.gameObject.name);
            if (File.Exists(StrPath))
            {
                File.Delete(StrPath);
            }
            File.WriteAllText(StrPath, rStrJson);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
            Debug.LogWarning($"{this.gameObject.name}导出成功！！！");
        }


        public void ImportConfig()
        {
            this.mObjects.Clear();

            var StrPath = PageViewPathTool.GetPathConfigName(this.gameObject.name);
            var rStrJson = File.ReadAllText(StrPath);

            var rData = Newtonsoft.Json.JsonConvert.DeserializeObject(rStrJson, typeof(List<PageObjectInfo>));
            this.mPageObjectsData = rData as List<PageObjectInfo>;
            for (int i = 0; i < this.mPageObjectsData.Count; i++)
            {
                var rPosInfo = new PageObject();
                rPosInfo.Name = this.mPageObjectsData[i].Name;
                rPosInfo.ObjectPath = this.mPageObjectsData[i].ObjectPath;
                rPosInfo.Component = this.mPageObjectsData[i].Component;
                 rPosInfo.Object = this.transform.Find(rPosInfo.ObjectPath);
                this.mObjects.Add(rPosInfo);
            }

#if UNITY_EDITOR

            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
            Debug.LogWarning($"{this.gameObject.name}导入成功！！！");
        }

            public T Get<T>(string rName) where T : class
        {
            if (this.Objects == null) return null;
            var rUObj = this.Objects.Find((rItem) => { return rItem.Name.Equals(rName); });
            if (rUObj == null) return null;
            return rUObj.Object as T;
        }
    }
}