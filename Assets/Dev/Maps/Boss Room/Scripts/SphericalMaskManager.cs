using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SphericalMaskManager : MonoBehaviour
{
    public float sphereRadius;
    public Vector3 spherePosition;

    public bool followTargetObject;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (followTargetObject)
        {
            sphereRadius = target.localScale.x / 2f;
            target.localScale = new Vector3(sphereRadius * 2f, sphereRadius * 2f, sphereRadius * 2f);

            spherePosition = target.position;
        }

        Shader.SetGlobalFloat("_SphericalRadius", sphereRadius);
        Shader.SetGlobalVector("_SphericalPosition", spherePosition);        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(spherePosition, sphereRadius);
    }


}
