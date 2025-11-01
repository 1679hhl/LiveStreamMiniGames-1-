using UnityEngine;
using TMPro;
using System.Collections;

public class test : MonoBehaviour
{
    public TMP_Text textMeshPro; // 绑定 TextMeshPro 组件

    [Header("跳动效果设置")]
    [Tooltip("跳动的垂直偏移量（Unity单位）")]
    public float bounceOffset = 10f;

    [Tooltip("跳动时的缩放比例")]
    public float bounceScale = 1.5f;

    [Tooltip("跳动时的旋转角度")]
    public float bounceRotation = 15f;

    [Tooltip("每个字符跳动的间隔时间（秒）")]
    public float bounceInterval = 0.1f;

    [Tooltip("字符跳动持续时间（秒）")]
    public float bounceDuration = 0.05f;

    private bool isAnimating = false; // 标志位，防止重复执行
    private Coroutine currentCoroutine; // 保存当前协程引用

    public void test1()
    {
        if (textMeshPro == null) return;

        // 如果正在执行,先停止当前协程
        if (isAnimating && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(AnimateTextBounceWithRichText());
    }

    private IEnumerator AnimateTextBounceWithRichText()
    {
        isAnimating = true;
        
        // 保存原始文本，防止外部修改
        string originalText = textMeshPro.text;
        
        for (int i = 0; i < textMeshPro.textInfo.characterCount; i++)
        {
            // 每次循环前强制更新网格和文本信息
            textMeshPro.ForceMeshUpdate();
            TMP_TextInfo textInfo = textMeshPro.textInfo;
            
            // 检查文本是否被外部修改
            if (textMeshPro.text != originalText)
            {
                Debug.LogWarning("文本在动画过程中被外部修改，动画终止");
                break;
            }
            
            if (i >= textInfo.characterCount || !textInfo.characterInfo[i].isVisible) 
                continue;

            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            Vector3[] sourceVertices = textInfo.meshInfo[materialIndex].vertices;

            // 保存原始顶点位置
            Vector3[] originalVertices = new Vector3[4];
            for (int j = 0; j < 4; j++)
            {
                originalVertices[j] = sourceVertices[vertexIndex + j];
            }

            // 计算字符中心点
            Vector3 center = (originalVertices[0] + originalVertices[2]) / 2f;

            // 应用变换：缩放 + 旋转 + 偏移
            Vector3 offset = new Vector3(0, bounceOffset, 0);
            float rotationRad = bounceRotation * Mathf.Deg2Rad;
            
            for (int j = 0; j < 4; j++)
            {
                // 相对于中心点的位置
                Vector3 localPos = originalVertices[j] - center;
                
                // 应用缩放
                localPos *= bounceScale;
                
                // 应用旋转（Z轴）
                float newX = localPos.x * Mathf.Cos(rotationRad) - localPos.y * Mathf.Sin(rotationRad);
                float newY = localPos.x * Mathf.Sin(rotationRad) + localPos.y * Mathf.Cos(rotationRad);
                localPos = new Vector3(newX, newY, localPos.z);
                
                // 应用偏移
                sourceVertices[vertexIndex + j] = center + localPos + offset;
            }
            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            yield return new WaitForSeconds(bounceDuration);

            // 恢复原始位置
            textMeshPro.ForceMeshUpdate();
            textInfo = textMeshPro.textInfo;
            if (i < textInfo.characterCount && textInfo.characterInfo[i].isVisible)
            {
                sourceVertices = textInfo.meshInfo[materialIndex].vertices;
                for (int j = 0; j < 4; j++)
                {
                    sourceVertices[vertexIndex + j] = originalVertices[j];
                }
                textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            }

            yield return new WaitForSeconds(bounceInterval - bounceDuration);
        }

        isAnimating = false;
        currentCoroutine = null;
    }
}
