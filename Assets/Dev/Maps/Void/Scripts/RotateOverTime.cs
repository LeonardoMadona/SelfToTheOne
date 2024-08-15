using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    [SerializeField] Vector3 eulersPerSecond;
    
    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(eulersPerSecond * Time.deltaTime);
    }
}
