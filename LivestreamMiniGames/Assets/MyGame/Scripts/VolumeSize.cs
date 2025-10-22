using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using System.Linq;

public class VolumeSize : MonoBehaviour
{
    public GameObject volumeObject;
    public CinemachineTargetGroup targetGroup;

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
}
