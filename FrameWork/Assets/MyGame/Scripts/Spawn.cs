using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class Spawn : MonoBehaviour
{
    public Transform RedParentTransform;
    public Transform BlueParentTransform;
    public GameObject unitPrefab;

    public void SpawnUnitRed()
    {
        GameObject newUnit = Instantiate(unitPrefab, RedParentTransform);
        // 延迟一帧后排列，确保子对象已添加
        StartCoroutine(ArrangeAfterFrame(RedParentTransform));
    }
    
    public void SpawnUnitBlue()
    {
        GameObject newUnit = Instantiate(unitPrefab, BlueParentTransform);
        StartCoroutine(ArrangeAfterFrame(BlueParentTransform));
    }

    private IEnumerator ArrangeAfterFrame(Transform parent)
    {
        yield return null; // 等待一帧
        parent.GetComponent<Array>()?.ArrangeChildren();
    }
}
