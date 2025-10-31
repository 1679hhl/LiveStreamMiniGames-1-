namespace LittleBaby.Framework.Core
{
    using Knight.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.UI;

    public class LBJoystickEditor
    {
        [MenuItem("GameObject/UI/Input/LBJoystick")]
        public static void CreateLbJoystick()
        {
            var joystick = new GameObject("Joystick").ReceiveComponent<LBJoystick>();
            joystick.transform.SetParent(Selection.activeTransform);

            joystick.gameObject.ReceiveComponent<CanvasGroup>();

            var triggerArea = UtilTool.CreateChild(joystick.gameObject, "TriggerArea");
            var limitArea = UtilTool.CreateChild(joystick.gameObject, "LimitArea");
            var background = UtilTool.CreateChild(joystick.gameObject, "Background");
            var thumb = UtilTool.CreateChild(joystick.gameObject, "Thumb");

            joystick.gameObject.layer = Selection.activeGameObject.layer;

            UtilTool.AddComponentInChilds<RectTransform>(joystick.gameObject.transform);

            var backgroundSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            var knobSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

            joystick.gameObject.ReceiveComponent<RectTransform>().anchoredPosition = Vector2.zero;
            joystick.gameObject.ReceiveComponent<RectTransform>().sizeDelta = Vector2.zero;
            joystick.ThumbMaxMoveDis = 50;

            joystick.TriggerArea = triggerArea.ReceiveComponent<RectTransform>();
            joystick.LimitArea = limitArea.ReceiveComponent<RectTransform>();
            joystick.Background = background.ReceiveComponent<RectTransform>();
            joystick.Thumb = thumb.ReceiveComponent<RectTransform>();

            triggerArea.ReceiveComponent<RectTransform>().sizeDelta = Vector2.one * 200;
            triggerArea.ReceiveComponent<Image>().sprite = backgroundSprite;
            triggerArea.ReceiveComponent<Image>().type = Image.Type.Sliced;
            triggerArea.ReceiveComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

            limitArea.ReceiveComponent<RectTransform>().sizeDelta = Vector2.one * 100;
            limitArea.ReceiveComponent<Image>().sprite = backgroundSprite;
            limitArea.ReceiveComponent<Image>().type = Image.Type.Sliced;
            limitArea.ReceiveComponent<Image>().color = new Color(1, 0, 0, 0.2f);

            background.ReceiveComponent<RectTransform>().sizeDelta = Vector2.one * 100;
            background.ReceiveComponent<Image>().sprite = knobSprite;
            background.ReceiveComponent<Image>().color = new Color(0, 1, 1, 1);

            thumb.ReceiveComponent<RectTransform>().sizeDelta = Vector2.one * 30;
            thumb.ReceiveComponent<Image>().sprite = knobSprite;
            thumb.ReceiveComponent<Image>().color = Color.white;

            joystick.transform.localScale = Vector3.one;
            joystick.transform.localPosition = Vector3.zero;
        }
        [MenuItem("GameObject/UI/Input/LBJoystick", true)]
        public static Boolean CreateLbJoystick_IsValidate()
        {
            if (Selection.activeGameObject)
            {
                if (UtilTool.GetComponentInParent<Canvas>(Selection.activeGameObject))
                {
                    return true;
                }
            }
            return false;
        }
    }
}