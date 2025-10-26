using UnityEngine;

public class Array : MonoBehaviour
{
    [Header("排列设置")]
    [Tooltip("每排的对象数量")]
    public int itemsPerRow = 5;
    
    [Tooltip("水平间距")]
    public float horizontalSpacing = 1f;
    
    [Tooltip("垂直间距")]
    public float verticalSpacing = 1f;
    
    [Tooltip("每行的水平偏移值(奇数行/偶数行交替)")]
    public float rowOffsetX = 0f;
    
    [Tooltip("是否在Start时自动排列")]
    public bool arrangeOnStart = true;

    [Header("旋转设置")]
    [Tooltip("是否启用旋转控制")]
    public bool enableRotation = false;
    
    [Tooltip("统一旋转角度")]
    public Vector3 uniformRotation = Vector3.zero;
    
    [Tooltip("每个子对象的随机旋转范围(X,Y,Z)")]
    public Vector3 randomRotationRange = Vector3.zero;
    
    [Tooltip("每行增加的旋转偏移")]
    public Vector3 rowRotationOffset = Vector3.zero;
    
    [Tooltip("每列增加的旋转偏移")]
    public Vector3 columnRotationOffset = Vector3.zero;

    private void Start()
    {
        if (arrangeOnStart)
        {
            ArrangeChildren();
        }
    }

    /// <summary>
    /// 排列所有子对象
    /// </summary>
    public void ArrangeChildren()
    {
        int childCount = transform.childCount;
        
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            
            // 计算当前子对象在第几行、第几列
            int row = i / itemsPerRow;
            int column = i % itemsPerRow;
            
            // 计算位置
            float xPos = column * horizontalSpacing;
            float yPos = -row * verticalSpacing;
            
            // 应用行偏移(奇数行偏移)
            if (row % 2 == 1)
            {
                xPos += rowOffsetX;
            }
            
            // 设置本地位置
            child.localPosition = new Vector3(xPos, yPos, 0f);
            
            // 应用旋转
            if (enableRotation)
            {
                ApplyRotation(child, row, column);
            }
        }
    }

    /// <summary>
    /// 应用旋转到子对象
    /// </summary>
    private void ApplyRotation(Transform child, int row, int column)
    {
        // 基础旋转
        Vector3 rotation = uniformRotation;
        
        // 添加行偏移
        rotation += rowRotationOffset * row;
        
        // 添加列偏移
        rotation += columnRotationOffset * column;
        
        // 添加随机旋转
        if (randomRotationRange != Vector3.zero)
        {
            rotation.x += Random.Range(-randomRotationRange.x, randomRotationRange.x);
            rotation.y += Random.Range(-randomRotationRange.y, randomRotationRange.y);
            rotation.z += Random.Range(-randomRotationRange.z, randomRotationRange.z);
        }
        
        // 应用旋转
        child.localRotation = Quaternion.Euler(rotation);
    }

    /// <summary>
    /// 在编辑器中可视化排列
    /// </summary>
    private void OnValidate()
    {
        // 在编辑器修改参数时自动更新排列
        if (!Application.isPlaying && transform.childCount > 0)
        {
            ArrangeChildren();
        }
    }

    // 在Scene视图中绘制辅助线
    private void OnDrawGizmosSelected()
    {
        if (transform.childCount == 0) return;
        
        Gizmos.color = Color.cyan;
        
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Transform current = transform.GetChild(i);
            Transform next = transform.GetChild(i + 1);
            
            // 如果是同一行,绘制连接线
            if (i % itemsPerRow != itemsPerRow - 1)
            {
                Gizmos.DrawLine(current.position, next.position);
            }
        }
    }
}
