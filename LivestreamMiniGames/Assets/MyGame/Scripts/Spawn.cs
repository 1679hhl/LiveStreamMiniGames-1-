using UnityEngine;
using Unity.Cinemachine;

public class Spawn : MonoBehaviour
{
    public Transform parentTransform;
    public GameObject unitPrefab;
    public CinemachineTargetGroup targetGroup;

    public void SpawnUnit()
    {
        GameObject newUnit = Instantiate(unitPrefab, Random.insideUnitSphere * 5, Quaternion.identity, parentTransform);
        targetGroup.AddMember(newUnit.transform, 0, 1);
    }
}
