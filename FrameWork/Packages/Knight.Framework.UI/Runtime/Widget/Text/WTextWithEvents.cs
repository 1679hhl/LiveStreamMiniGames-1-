using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Knight.Core;
using UnityEditor.UI;
using System;

namespace UnityEngine.UI
{
    public class TextProInfo
    {
        public int StartIndex;

        public int EndIndex;

        public string Name;

        public readonly List<Rect> Boxes = new List<Rect>();
    }
    public class WTextWithEvents : WText, IPointerClickHandler
    {
        private enum mEventDataType
        {
            None,
            Click,
        }
        
        private readonly Regex mHrefRegex = new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

        private string mOutputText;

        private List<TextProInfo> mHrefInfos = new List<TextProInfo>();

        protected StringBuilder mTextBuilder = new StringBuilder();
        public UnityAction[] mClickActions;
        private string mTextMessage;
        private string mTextColor = "#000000";
        private PointerEventData mCurEventData;
        [SerializeField]
        [HideInInspector]
        public int ForcePreferredWidth = 0;

        private mEventDataType mCurEventDataType;
        private TextGenerator mTempTextCache = new TextGenerator(0);
        private LayoutElement mLayout;
        protected override void Awake()
        {
            base.Awake();
            this.mLayout = this.gameObject.ReceiveComponent<LayoutElement>();
        }
        public void SetTextColor(string rColor)
        {
            this.mTextColor = rColor;
        }
        /// <summary>
        /// 整段文本中的部分文本需要触发点击事件，使用此方法设置整段文本的文本
        /// </summary>
        /// <param name="rMessageStr">整段文本（需要format格式）</param>
        /// <param name="rEventStr">需要触发事件的文本</param>
        public void SetText(string messageStr, string[] eventStrs, params UnityAction[] rActions)
        {
            this.mOutputText = string.Empty;
            this.mTextBuilder.Clear();
            this.mHrefInfos.Clear();
            this.mTextMessage = messageStr;
            for (int i = 0; i < eventStrs.Length; i++)
            {
                string tempStr = string.Format("<a href=事件文本>{0:name}</a>", eventStrs[i]);
                this.mTextMessage = this.mTextMessage.Replace("{" + i + "}", tempStr);
            }
            this.mClickActions = rActions;
            this.SetVerticesDirty();
        }
        public override void SetVerticesDirty()
        {
            base.SetVerticesDirty();
            this.text = this.mTextMessage;
            this.mOutputText = this.GetOutputText(this.text);
        }
        protected override void OnPopulateMesh(VertexHelper rToFill)
        {
            string rOriginText = this.m_Text;
            this.m_Text = this.mOutputText;
            base.OnPopulateMesh(rToFill);
            this.m_Text = rOriginText;
            UIVertex rVerts = new UIVertex();
            if (this.rectTransform.rect.width > this.ForcePreferredWidth) 
                this.mLayout.preferredWidth = this.ForcePreferredWidth;
            // 处理文本包围框
            for (int i = 0; i < this.mHrefInfos.Count; i++)
            {
                this.mHrefInfos[i].Boxes.Clear();
                if (this.mHrefInfos[i].StartIndex >= rToFill.currentVertCount)
                {
                    continue;
                }

                // 将事件文本里面的文本顶点索引坐标加入到包围框
                rToFill.PopulateUIVertex(ref rVerts, this.mHrefInfos[i].StartIndex);
                Vector3 rPos = rVerts.position;
                Bounds rBounds = new Bounds(rPos, Vector3.zero);
                for (int TEMP = this.mHrefInfos[i].StartIndex, m = this.mHrefInfos[i].EndIndex; TEMP < m; TEMP++)
                {
                    if (TEMP >= rToFill.currentVertCount)
                    {
                        break;
                    }

                    rToFill.PopulateUIVertex(ref rVerts, TEMP);
                    rPos = rVerts.position;
                    if (rPos.x < rBounds.min.x) // 换行重新添加包围框
                    {
                        this.mHrefInfos[i].Boxes.Add(new Rect(rBounds.min, rBounds.size));
                        rBounds = new Bounds(rPos, Vector3.zero);
                    }
                    else
                    {
                        rBounds.Encapsulate(rPos); // 扩展包围框
                    }
                }
                this.mHrefInfos[i].Boxes.Add(new Rect(rBounds.min, rBounds.size));
            }
        }

        /// <summary>
        /// 获取超链接解析后的最后输出文本
        /// </summary>
        /// <returns></returns>
        /*
        protected virtual string GetOutputText(string outputText)
        {
            Vector2 extents = rectTransform.rect.size;
            var settings = GetGenerationSettings(extents);
            this.mTextBuilder.Length = 0;
            this.mHrefInfos.Clear();
            int nIndexText = 0;
            foreach (Match rMatch in this.mHrefRegex.Matches(outputText))
            {
                this.mTextBuilder.Append(outputText.Substring(nIndexText, rMatch.Index - nIndexText));

                var rGroup = rMatch.Groups[1];
                TextProInfo rHrefInfo = new TextProInfo
                {
                    StartIndex = this.mTextBuilder.Length * 4, // 超链接里的文本起始顶点索引
                    EndIndex = (this.mTextBuilder.Length + rMatch.Groups[2].Length - 1) * 4 + 3,
                    Name = rGroup.Value
                };
                this.mHrefInfos.Add(rHrefInfo);

                this.mTextBuilder.Append(rMatch.Groups[2].Value);
                nIndexText = rMatch.Index + rMatch.Length;
            }
            this.mTextBuilder.Append(outputText.Substring(nIndexText, outputText.Length - nIndexText));
            return this.mTextBuilder.ToString();
        }
        */
        protected virtual string GetOutputText(string outputText)
        {
            Vector2 extents = this.rectTransform.rect.size;
            var settings = this.GetGenerationSettings(extents);
            this.mTextBuilder.Length = 0;
            this.mTextBuilder.Clear();
            this.mHrefInfos.Clear();
            var indexText = 0;
            var textLength = 0;
            foreach (Match match in this.mHrefRegex.Matches(outputText))
            {
                this.mTextBuilder.Append(outputText.Substring(indexText, match.Index - indexText));

                this.mTempTextCache.PopulateWithErrors(this.mTextBuilder.ToString(), settings, this.gameObject);
                textLength = this.mTempTextCache.verts.Count / 4;

                var rTempStr1 = "<color=" + this.mTextColor + ">";
                this.mTextBuilder.Append(rTempStr1);  // 事件文本颜色

                var group = match.Groups[1];
                TextProInfo hrefInfo = new TextProInfo
                {
                    StartIndex = textLength * 4, // 超链接里的文本起始顶点索引
                    EndIndex = (textLength + match.Groups[2].Length - 1) * 4 + 3,
                    Name = group.Value
                };
                this.mHrefInfos.Add(hrefInfo);

                this.mTextBuilder.Append(match.Groups[2].Value);
                this.mTextBuilder.Append("</color>");
                indexText = match.Index + match.Length;
            }
            this.mTextBuilder.Append(outputText.Substring(indexText, outputText.Length - indexText));
            return this.mTextBuilder.ToString();
        }
        /// <summary>
        /// 点击事件检测是否点击到超链接文本
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            this.mCurEventData = eventData;
            this.mCurEventDataType = mEventDataType.Click;
        }
        private void OnGUI()
        {
            this.checkEventData();
        }
        private void checkEventData()
        {
            if (this.mCurEventData == null) return;


            Vector2 rLP = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, this.mCurEventData.position, this.mCurEventData.pressEventCamera, out rLP);

            //foreach (var rHrefInfo in this.mHrefInfos)
            for(int i = 0; i < this.mHrefInfos.Count; i++)
            {
                var rBoxes = this.mHrefInfos[i].Boxes;
                for (var j = 0; j < rBoxes.Count; j++)
                {
                    if (rBoxes[j].Contains(rLP))
                    {
                        if (this.mCurEventDataType == mEventDataType.Click)
                        {
                            this.mClickActions[i]?.Invoke();
                            this.mCurEventDataType = mEventDataType.None;
                        }
                        return;
                    }
                }
            }
            this.mCurEventDataType = mEventDataType.None;
        }
        /*
        private void ChangeColor(string rColor)
        {
            //print(this.m_Text);
            string rTempText = this.m_Text;
            rTempText.Replace("<a href=事件文本>", "");
        }
        */
    }
}

