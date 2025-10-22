using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using System.Linq;
using Pathfinding;

public class VolumeSize : MonoBehaviour
{
    public GameObject volumeObject;
    public CinemachineTargetGroup targetGroup;

    public FollowerEntity followerEntity;

    void Start()
    {
        followerEntity = volumeObject.GetComponent<FollowerEntity>();
    }

    public void AddVolume()
    {
        Vector3 targetScale = volumeObject.transform.localScale + new Vector3(0.5f, 0.5f, 0.5f); // 当前大小的 1.5 倍
        
        // 使用 LINQ 查找索引
        int i = targetGroup.Targets.ToList().FindIndex(t => t.Object == volumeObject.transform);
        
        if (i != -1)
        {
            var target = targetGroup.Targets[i];
            target.Radius += 0.5f;
            targetGroup.Targets[i] = target;
            RefreshSpeed();
        }
        
        StartCoroutine(ScaleOverTime(volumeObject, targetScale, 0.2f));
    }

    private IEnumerator ScaleOverTime(GameObject target, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = target.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            target.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.transform.localScale = targetScale; // Ensure the final scale is set
    }

    public void RefreshSpeed()
    {
        // 根据体积大小调整速度（反比关系）
        float scale = volumeObject.transform.localScale.magnitude;
        
        // 基础速度 / 缩放大小，确保速度不为0
        float baseSpeed = 5f; // 可以调整这个基础速度值
        float minSpeed = 0.02f; // 最小速度，确保永远不为0
        
        float newSpeed = Mathf.Max(baseSpeed / scale, minSpeed);
        followerEntity.maxSpeed = newSpeed;
    }
}
