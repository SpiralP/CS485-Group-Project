using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinDisplay : MonoBehaviour
{
    private Vector3 angles = new Vector3(0f,0.5f,0f);

    void Update()
    {
        transform.Rotate(angles, Space.World);
    }
}
