using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rord : MonoBehaviour
{
    private void Start()
    {
        Init();
    }
    void Init()
    {
        Vector3 pos = transform.position;
        pos.y = 1.875f;
        transform.position = pos;
    }
}
