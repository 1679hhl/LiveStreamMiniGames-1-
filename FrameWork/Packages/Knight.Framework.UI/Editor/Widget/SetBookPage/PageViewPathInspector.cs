using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using System.Reflection;
using Knight.Core;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(PageViewPath), true)]
    public class PageViewPathInspector : UnityEditor.Editor
    {
        public class ObjectType
        {
            public SerializedProperty Object;
            public SerializedProperty Component;
            public SerializedProperty Name;
            //public SerializedProperty Property;
            public SerializedProperty ObjectPath;
            public int SelectedComp;

            //public int SelectedProp;
        }

        private SerializedProperty mObjects;

        private List<ObjectType> mObjectTypes;

        public PageViewPath mTarget;

        protected void OnEnable()
        {
            this.mObjects = this.serializedObject.FindProperty("mObjects");

            this.mTarget = this.target as PageViewPath;

            this.mObjectTypes = this.ToObjectTypes(this.mObjects);

        }

        protected void DrawBaseInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            using (var space = new EditorGUILayout.VerticalScope())
            {
                this.DrawUnityEngineObjects();

                EditorGUILayout.Space(20);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Export"))
                {
                    this.mTarget.ExportConfig();
                }
                if (GUILayout.Button("Import"))
                {
                    this.mTarget.ImportConfig();
                }
                EditorGUILayout.EndHorizontal();
            }
            this.serializedObject.ApplyModifiedProperties();
        }
        private void DrawUnityEngineObjects()
        {
            this.mObjectTypes = this.ToObjectTypes(this.mObjects);
            EditorGUILayout.LabelField("Objects: ");
            for (int i = 0; i < this.mObjectTypes.Count; i++)
            {
                using (var space1 = new EditorGUILayout.HorizontalScope("TextField"))
                {
                    GUILayout.Label(i.ToString() + ": ", GUILayout.Width(15));

                    var rElementNameProperty = this.mObjectTypes[i].Name;
                    var rElementObjProperty = this.mObjectTypes[i].Object;
                    var rElementComponentProperty = this.mObjectTypes[i].Component;
                    //var rElementTypeProperty = this.mObjectTypes[i].Property;
                    var rrElementGoPathProperty = this.mObjectTypes[i].ObjectPath;

                    EditorGUILayout.PropertyField(rElementNameProperty, new GUIContent(""));

                    EditorGUILayout.PropertyField(rElementObjProperty, new GUIContent(""));


                    List<string> rElemTypes = this.GetObjectComponentTypes(rElementObjProperty);
                    this.mObjectTypes[i].SelectedComp = EditorGUILayout.Popup(this.mObjectTypes[i].SelectedComp, rElemTypes.ToArray());
                    this.ChangeElementObjectBySelectedComp(rElementObjProperty, rElementComponentProperty, this.mObjectTypes[i].SelectedComp);


                    //List<string> rProperties = this.GetComponentProperties(rElementComponentProperty);
                    //this.mObjectTypes[i].SelectedProp = EditorGUILayout.Popup(this.mObjectTypes[i].SelectedProp, rProperties.ToArray());
                    //this.ChangeElementObjectBySelectedComp(rElementObjProperty, rElementComponentProperty, this.mObjectTypes[i].SelectedProp);
                    if (GUILayout.Button("Del", GUILayout.Width(40)))
                    {
                        this.mObjects.DeleteArrayElementAtIndex(i);
                        break;
                    }

                    
                }
            }

            if (GUILayout.Button("Add Page Objects"))
            {
                this.mObjects.InsertArrayElementAtIndex(this.mObjects.arraySize);
            }
        }

        private void ChangeElementObjectBySelectedComp(SerializedProperty rObjectProp, SerializedProperty rComponentProp, int nSelected)
        {
            List<string> rElemTypes = this.GetObjectComponentTypes(rObjectProp);
            List<UnityEngine.Object> rElemObjs = this.GetObjectComponents(rObjectProp);

            if (nSelected >= 0 && nSelected < rElemTypes.Count)
            {
                string rRealType = rElemTypes[nSelected];
                rComponentProp.stringValue = rRealType;
                rObjectProp.objectReferenceValue = rElemObjs[nSelected];
                return;
            }
        }

        private List<ObjectType> ToObjectTypes(SerializedProperty rObjects)
        {
            var rObjectTypes = new List<ObjectType>();

            if (rObjects == null) return rObjectTypes;

            for (int i = 0; i < rObjects.arraySize; i++)
            {
                SerializedProperty rElementProp = rObjects.GetArrayElementAtIndex(i);
                var rObjectProperty = rElementProp.FindPropertyRelative("Object");
                var rGoPathProperty = rElementProp.FindPropertyRelative("ObjectPath");

                //生成路径
                GameObject rElementGo = this.GetElementGameObject(rObjectProperty);
                if (rElementGo)
                {
                    rGoPathProperty.stringValue = UtilTool.GetTransformPathByTrans(this.mTarget.transform, rElementGo.transform);
                }
                var rObjectType = new ObjectType()
                {
                    Object = rObjectProperty,
                    Component = rElementProp.FindPropertyRelative("Component"),
                    Name = rElementProp.FindPropertyRelative("Name"),
                    ObjectPath = rGoPathProperty,
                    //Property = rElementProp.FindPropertyRelative("Property"),
                    SelectedComp = this.GetSelectedCompIndex(rObjectProperty, rElementProp.FindPropertyRelative("Component")),
                    //SelectedProp = this.GetSelectedPropIndex(rElementProp.FindPropertyRelative("Component"), rElementProp.FindPropertyRelative("Property")),
                };
                if (rObjectType.Object != null && rObjectType.Object.objectReferenceValue != null && string.IsNullOrEmpty(rObjectType.Name.stringValue))
                {
                    rObjectType.Name.stringValue = rObjectType.Object.objectReferenceValue.name;
                }

                rObjectTypes.Add(rObjectType);
            }
            return rObjectTypes;
        }

        public void GerneratePath(int nIndex)
        {
            if (this.mObjects == null) return;
            var rElementProperty = this.mObjects.GetArrayElementAtIndex(nIndex);
            var rObjectProperty = rElementProperty.FindPropertyRelative("Object");
            GameObject rElementGo = this.GetElementGameObject(rObjectProperty);
            var rGoPath = rElementProperty.FindPropertyRelative("ObjectPath");
            rGoPath.stringValue = UtilTool.GetTransformPathByTrans(this.mTarget.transform, rElementGo.transform);
            Debug.LogError($"{rGoPath}");
        }

        #region 组件相关

        /// <summary>
        /// 获取选中的组件Index
        /// </summary>
        /// <param name="rObjectProp"></param>
        /// <param name="rComponentProp"></param>
        /// <returns></returns>
        private int GetSelectedCompIndex(SerializedProperty rObjectProp, SerializedProperty rComponentProp)
        {
            string rTypeStr = rComponentProp.stringValue;
            List<string> rElemTypes = this.GetObjectComponentTypes(rObjectProp);
            int nFindIndex = rElemTypes.FindIndex((rItem) => { return rItem.Equals(rTypeStr); });
            //if (nFindIndex == -1) return 0;

            return nFindIndex;
        }

       
        /// <summary>
        /// 获取节点的所有组件
        /// </summary>
        /// <param name="rObjectProp"></param>
        /// <returns></returns>
        private List<UnityEngine.Object> GetObjectComponents(SerializedProperty rObjectProp)
        {
            if (rObjectProp == null) return new List<UnityEngine.Object>();

            GameObject rElementGo = this.GetElementGameObject(rObjectProp);
            List<UnityEngine.Object> rElemObjs = new List<UnityEngine.Object>();
            if (rElementGo != null)
            {
                rElemObjs.Add(rElementGo);
                var rElemCmps = rElementGo.GetComponents<Component>();
                for (int k = 0; k < rElemCmps.Length; k++)
                {
                    rElemObjs.Add(rElemCmps[k]);
                }
            }
            return rElemObjs;
        }

        private GameObject GetElementGameObject(SerializedProperty rObjectProp)
        {
            GameObject rElementGo = rObjectProp.objectReferenceValue as GameObject;
            if (rElementGo == null)
            {
                var rTempCmp = rObjectProp.objectReferenceValue as Component;
                if (rTempCmp != null)
                    rElementGo = rTempCmp.gameObject;
            }

            return rElementGo;
        }

        /// <summary>
        /// 获取节点的所有组件名字
        /// </summary>
        /// <param name="rObjectProp"></param>
        /// <returns></returns>
        private List<string> GetObjectComponentTypes(SerializedProperty rObjectProp)
        {
            if (rObjectProp == null) return new List<string>();

            GameObject rElementGo = rObjectProp.objectReferenceValue as GameObject;
            if (rElementGo == null)
            {
                var rTempCmp = rObjectProp.objectReferenceValue as Component;
                if (rTempCmp != null)
                    rElementGo = rTempCmp.gameObject;
            }
            List<string> rElemTypes = new List<string>();
            if (rElementGo != null)
            {
                rElemTypes.Add("UnityEngine.GameObject");
                var rElemCmps = rElementGo.GetComponents<Component>();
                for (int k = 0; k < rElemCmps.Length; k++)
                {
                    rElemTypes.Add(rElemCmps[k].GetType().ToString());
                }
            }
            return rElemTypes;
        }
        #endregion

        //#region 组件属性


        ///// <summary>
        ///// 获取组件所有的属性名
        ///// </summary>
        ///// <param name="rComponentProp"></param>
        ///// <returns></returns>
        //private List<string> GetComponentProperties(SerializedProperty rComponentProp)
        //{
        //    if (rComponentProp == null) return new List<string>();

        //    Component rComponent = typeof() rComponentProp. as Component;
        //    var rProperties = rComponent.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

        //    var rPropertiesName = new List<string>();
        //    for (int i = 0; i < rProperties.Length; i++)
        //    {
        //        rPropertiesName.Add(rProperties[i].Name);
        //    }
        //    return rPropertiesName;
        //}

        ///// <summary>
        ///// 获取组件选中的Index
        ///// </summary>
        ///// <param name="rComponentProp"></param>
        ///// <param name="rPropertyProp"></param>
        ///// <returns></returns>
        //private int GetSelectedPropIndex(SerializedProperty rComponentProp, SerializedProperty rPropertyProp)
        //{
        //    string rPropStr = rPropertyProp.stringValue;
        //    List<string> rProperties = this.GetComponentProperties(rComponentProp);
        //    int nFindIndex = rProperties.FindIndex((rItem) => { return rItem.Equals(rPropStr); });
        //    if (nFindIndex == -1) return 0;

        //    return nFindIndex;
        //}
        //#endregion

    }
}
