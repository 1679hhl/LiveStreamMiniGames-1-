//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

namespace Knight.Core.Editor
{
    public static class FindMissingScriptGameObjectTool
    {
        [MenuItem("GameObject/查询丢失脚本的节点", false, -200)]
        public static void FindMissingScriptGameObject()
        {
            bool bIsPrintError = false;
            var rGameObjects = Selection.gameObjects;
            var rTransformList = new List<Transform>();
            for (int i = 0; i < rGameObjects.Length; i++)
            {
                rTransformList.AddRange(rGameObjects[i].GetComponentsInChildren<Transform>(true));
            }
            for (int i = 0; i < rTransformList.Count; i++)
            {
                var rTransform = rTransformList[i];
                var rMonoBehaviours = rTransform.GetComponents<MonoBehaviour>();
                foreach (var rMonoBehaviour in rMonoBehaviours)
                {
                    if (rMonoBehaviour == null)
                    {
                        if (!bIsPrintError)
                        {
                            bIsPrintError = true;
                            Debug.LogError("以下节点存在丢失脚本");
                        }
                        Debug.LogError(UtilTool.GetTransformPath(rTransform), rTransform);
                        continue;
                    }
                }
            }
        }
    }
}