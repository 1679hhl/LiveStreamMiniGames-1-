using UnityEngine;

public class positionDrive : MonoBehaviour
{
    [SerializeField] private Transform targetObject; // 需要位移的对象
    [SerializeField] private Transform startPoint; // 起始位置
    [SerializeField] private Transform endPoint;   // 结束位置
    [SerializeField] private Transform rotationObject; // 需要旋转的对象
    [SerializeField] private Transform rotationStartPoint; // 起始旋转位置
    [SerializeField] private Transform rotationEndPoint;   // 结束旋转位置
    [SerializeField, Range(0f, 1f)] public float sliderValue = 0f; // 滑条值 (0-1)

    // Update is called once per frame
    void Update()
    {
        if (startPoint != null && endPoint != null && targetObject != null)
        {
            // 使用滑条值进行插值位置
            targetObject.position = Vector3.Lerp(startPoint.position, endPoint.position, sliderValue);
        }

        if (rotationStartPoint != null && rotationEndPoint != null && rotationObject != null)
        {
            // 使用滑条值进行插值旋转
            rotationObject.rotation = Quaternion.Lerp(rotationStartPoint.rotation, rotationEndPoint.rotation, sliderValue);
        }
    }
}
