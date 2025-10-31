using Knight.Hotfix.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewBindTool_Hotfix : TSHotfixKnightObject<ViewBindTool_Hotfix>
    {
        private ViewBindTool_Hotfix()
        {

        }
        public void Init()
        {
            if(ViewBindTool.mAllFieldSetFuncDict_Boolean.ContainsKey("Set_UIPlayAudio_IsDisable"))
                return;
            //Boolean类型
            ViewBindTool.mAllFieldSetFuncDict_Boolean.Add("Set_UIPlayAudio_IsDisable", Set_UIPlayAudio_IsDisable);
            ViewBindTool.mAllFieldSetFuncDict_Boolean.Add("Set_Animation_playAutomatically", Set_Animation_playAutomatically);
            ViewBindTool.mAllFieldSetFuncDict_Boolean.Add("Set_Animation_enabled", Set_Animation_enabled);
            ViewBindTool.mAllFieldSetFuncDict_Boolean.Add("Set_TextEmoji_LeftAlign", Set_TextEmoji_LeftAlign);
            ViewBindTool.mAllFieldSetFuncDict_Boolean.Add("Set_Animator_enabled", Set_Animator_enabled);
            ViewBindTool.mAllFieldSetFuncDict_Boolean.Add("Set_LoopRectSortItem_IsNull", Set_LoopRectSortItem_IsNull);
            //Color类型
            ViewBindTool.mAllFieldSetFuncDict_Color.Add("Set_DisplayUGUI_color", Set_DisplayUGUI_color);
            //Int类型
            ViewBindTool.mAllFieldSetFuncDict_Int32.Add("Set_ViewModelDataSourceArray_SortIndex_SortIndex", Set_ViewModelDataSourceArray_SortIndex_SortIndex);
            ViewBindTool.mAllFieldSetFuncDict_Int32.Add("Set_WatchGameBattleMobaActorInfo_ActorIndex", Set_WatchGameBattleMobaActorInfo_ActorIndex);
            ViewBindTool.mAllFieldSetFuncDict_Int32.Add("Set_LoopRectSortItem_ItemIndex", Set_LoopRectSortItem_ItemIndex);
            ViewBindTool.mAllFieldSetFuncDict_Int32.Add("Set_TouchEnterEvent_ListIndex", Set_TouchEnterEvent_ListIndex);
            /*ViewBindTool.mAllFieldSetFuncDict_Int32.Add("Set_GradualDownNum_newNum", Set_GradualDownNum_newNum);*/

            ViewBindTool.mAllFieldSetFuncDict_Int64.Add("Set_WText_MultiLangID", Set_WText_MultiLangID);
            ViewBindTool.mAllFieldSetFuncDict_Int32.Add("Set_UGUIAssistant_Custom2_nIndex", Set_UGUIAssistant_Custom2_nIndex);
            ViewBindTool.mAllFieldSetFuncDict_Int64.Add("Set_CountDown1_Num",  Set_CountDown1_Num);
            
            ViewBindTool.mAllFieldSetFuncDict_Color.Add("Set_TextMeshProUGUI_color", Set_TextMeshProUGUI_color);
            

        }

        public static void Set_UIPlayAudio_IsDisable(object rComponent, Boolean bIsDisable)
        {
            // ((UIPlayAudio)rComponent).IsDisable = bIsDisable;
        }

        public static void Set_Animation_playAutomatically(object rComponent, Boolean playAutomatically)
        {
            ((Animation)rComponent).playAutomatically = playAutomatically;
        }
        public static void Set_Animation_enabled(object rComponent, Boolean enable)
        {
            ((Animation)rComponent).enabled = enable;
        }
        public static void Set_Animator_enabled(object rComponent, Boolean enable)
        {
            ((Animator)rComponent).enabled = enable;
        }
        public static void Set_LoopRectSortItem_IsNull(object rComponent, Boolean isNull)
        {
            ((LoopRectSortItem)rComponent).IsNull = isNull;
        }
        public static void Set_DisplayUGUI_color(object rComponent, Color color)
        {
            // ((DisplayUGUI)rComponent).color = color;
        }
        public static void Set_ViewModelDataSourceArray_SortIndex_SortIndex(object rComponent, Int32 sortIndex)
        {
            ((ViewModelDataSourceArray_SortIndex)rComponent).SortIndex = sortIndex;
        }
        public static void Set_TextEmoji_LeftAlign(object rComponent, Boolean enable)
        {
            // ((TextEmoji)rComponent).LeftAlign = enable;
        }
        public static void Set_WatchGameBattleMobaActorInfo_ActorIndex(object rComponent, Int32 actorIndex)
        {
            // ((WatchGameBattleMobaActorInfo)rComponent).ActorIndex = actorIndex;
        }
        public static void Set_LoopRectSortItem_ItemIndex(object rComponent, Int32 itemIndex)
        {
            ((LoopRectSortItem)rComponent).ItemIndex = itemIndex;
        }
        public static void Set_TouchEnterEvent_ListIndex(object rComponent, Int32 listIndex)
        {
            // ((TouchEnterEvent)rComponent).ListIndex = listIndex;
        }
        public static void Set_WText_MultiLangID(object rComponent, Int64 MultiLangID)
        {
            ((WText)rComponent).MultiLangID = MultiLangID;
        }
        public static void Set_UGUIAssistant_Custom2_nIndex(object rComponent, Int32 nIndex)
        {
            ((UGUIAssistant_Custom2)rComponent).nIndex = nIndex;
        }
        
        public static void Set_CountDown1_Num(object rComponent, Int64 nIndex)
        {
            ((CountDown1)rComponent).Num = nIndex;
        }
        public static void Set_TextMeshProUGUI_color(object rComponent, Color color)
        {
            ((TMP_Text)rComponent).color = color;
        }
        public void Destroy()
        {
            //Boolean类型
            ViewBindTool.mAllFieldSetFuncDict_Boolean.Clear();
            //Color类型
            ViewBindTool.mAllFieldSetFuncDict_Color.Clear();
            //Int类型
            ViewBindTool.mAllFieldSetFuncDict_Int32.Clear();

            ViewBindTool.mAllFieldSetFuncDict_Int64.Clear();
        }

    }
}