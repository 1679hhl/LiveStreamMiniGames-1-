using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingText2D : MonoBehaviour
{
    public bool isTmp;
    public RectTransform ValueText;

    private float 第几秒后开始_上升;
    private float 第几秒后开始_渐隐;
    private float 激活Time;

    private float r,g,b,a;

    float 原始大小;
    private Camera MainCamera;
    private RectTransform mRectTransform;
    private RectTransform rRectTransform;
   
    /// <summary>
    /// 1代表刚进入  2代表放弃进入缩小阶段 3代表结束放大缩小
    /// </summary>
    int ls_放大缩小type;

    bool 是否上升;

    Transform 跟随目标;
    Vector3 偏移;
    public float moveSpeed = 1f;  // 控制上升速度，可以根据需要调整

    private Color mColor;
    private void Awake()
    {
        this.mRectTransform = UIRoot.Instance.HeadinfoTransform;
        this.MainCamera=GameObject.Find("Main Camera").GetComponent<Camera>();
        this.transform.position = Vector3.zero;
        this.rRectTransform = this.transform.GetComponent<RectTransform>();
        
        Vector3 position = rRectTransform.localPosition;
        position.z = 0;
        rRectTransform.localPosition = position;
        rRectTransform.anchoredPosition = Vector2.one * 10000f;
        rRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rRectTransform.localScale=Vector3.one;
        
    }
    
    private void Start()
    {
        this.mColor = ValueText.GetComponent<TextMeshProUGUI>().color;
    }

    void Update()
    {
        
        // 初创 先变大在变小
        if (ls_放大缩小type == 1)
        {
            rRectTransform.localScale *= 1.1f;
            if (rRectTransform.localScale.x >= 原始大小 * 2)
                ls_放大缩小type = 2;
        }
        else if (ls_放大缩小type==2)
        {
            rRectTransform.localScale /= 1.1f;
            if (rRectTransform.localScale.x <= 原始大小 )
                ls_放大缩小type = 3;
        }

        if (是否上升 == false)
            return;

        if (Time.time - 激活Time > 第几秒后开始_上升)
        {
            // 使用 Time.deltaTime 控制平滑上升
            rRectTransform.Translate(Vector3.up*0.1f*Time.deltaTime);
        }

        if (Time.time - 激活Time > 第几秒后开始_渐隐)
        {
            a -=  Time.deltaTime;
            if (a < 0)
            {
                 a = 0;
                 Join内存池();
            }

           
            Color color = new Color(a,g,b,a);
            // 如果是 TextMeshProUGUI，更新它的颜色
            if (isTmp)
            {
                ValueText.GetComponent<TextMeshProUGUI>().color = color;
            }
            else
            {
                // 如果是普通的 Text，更新它的颜色
                ValueText.GetComponent<Text>().color = color;
            }
        }
    }

    public void Set(string cValue, UnityEngine.Color color,Vector3 发生地,Vector3 c偏移, Transform 跟随目标 = null, string 发生地Type = "2D", float cScale = 1,bool c是否上升 = true,float c第几秒后开始_上升=0.3f, float c第几秒后开始_渐隐 = 1.0f,bool c是否需要放大=true)
    {
        if (cValue ==null)
        {
            Debug.LogError("cValue ==null");
            return;
        }

        偏移 = c偏移;

        ls_放大缩小type = 1;
        if (c是否需要放大==false)
            ls_放大缩小type = 3;

        this.跟随目标 = 跟随目标;

        原始大小 = cScale;
        if (发生地Type == "2D")
            transform.position = 发生地+ c偏移;
        else
            SetPosition(发生地,c偏移);
        
        rRectTransform.localScale = new Vector3(cScale, cScale, cScale);
        ValueText.localPosition = Vector2.zero;
        是否上升 = c是否上升;

      
        if (color!=Color.grey)
        {
            r = color.r; g = color.g; b = color.b; a = 1;
            if (isTmp)
            {
                ValueText.GetComponent<TextMeshProUGUI>().color = color;
                ValueText.GetComponent<TextMeshProUGUI>().text = cValue;
                /*var data = GameM.Ins.exlDM.GetFont();
                ValueText.GetComponent<TextMeshProUGUI>().font = ResManager.instance.LoadFont($"{data.FontPath}{data.FontRes}");*/
            }
            else
            {
                ValueText.GetComponent<Text>().color = color;
                ValueText.GetComponent<Text>().text = cValue;
            }
        }
        else
        {
            var rColor = ValueText.GetComponent<TextMeshProUGUI>().color;
            ValueText.GetComponent<TextMeshProUGUI>().color = new Color(rColor.a, rColor.g, rColor.b, 1);
            if (isTmp)
            {
                ValueText.GetComponent<TextMeshProUGUI>().text = cValue;
            }
            else
            {
                ValueText.GetComponent<Text>().text = cValue;
            }
        }
        
        第几秒后开始_上升 = c第几秒后开始_上升;
        第几秒后开始_渐隐 = c第几秒后开始_渐隐;
        激活Time = Time.time;
    }

    public void UpdataData(string cValue)
    {
        if (ValueText == null)
        {
            Debug.LogError("ValueText.text == null");
            return;
        }
        else if (cValue == null)
        {
            Debug.LogError("cValue == null");
            return;
        }
        else if (ValueText.gameObject.activeInHierarchy == false)
        {
            Debug.LogWarning("ValueText.gameObject.activeInHierarchy == false");
            // PoolM.Ins.JoinPool(gameObject);
            ValueText.gameObject.SetActive(true);
            return;
        }

        if (isTmp)
        {
            ValueText.GetComponent<TextMeshProUGUI>().text = cValue;
        }
        else
        {
            ValueText.GetComponent<Text>().text = cValue;
        }
    }
    private void Join内存池()
    {
        BattleManager.Instance.PoolManager.DelayRecycleEffect(this.gameObject.name,this.gameObject,0,false);
    }

    private void SetPosition(Vector3 发生地, Vector3 r偏移)
    {
        Vector2 screenPoint =
            RectTransformUtility.WorldToScreenPoint(MainCamera, 发生地+r偏移); // 先将3D坐标转换成屏幕坐标
        Vector2 localPoint; // 再将屏幕坐标转换成UGUI坐标
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mRectTransform, screenPoint, UIRoot.Instance.UICamera, out localPoint))
        {
            Vector3 position = rRectTransform.localPosition;
            position.z = 0;
            rRectTransform.localPosition = position;
            this.rRectTransform.anchoredPosition = localPoint;
        }
    }
}
