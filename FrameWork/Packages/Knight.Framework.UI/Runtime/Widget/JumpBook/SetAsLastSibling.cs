using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAsLastSibling : MonoBehaviour
{
    void LateUpdate()
    {
        this.transform.SetAsLastSibling();
    }
}
