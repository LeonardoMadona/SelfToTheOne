using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbitalBehaviour : MonoBehaviour
{

    [SerializeField]
    Transform lookAtTarget;

    public float minYAngle, maxYAngle, rotationSpeed;

    [Range(0f, 1f)]
    public float positionSmooth;

    [SerializeField] bool invertYAxis;

    private float distToTarget;
    private float xzAngle, yAngle;
    public LayerMask cameraCollisionMask;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 startAngles = GrabAngles(transform.position, lookAtTarget.position);
        xzAngle = startAngles.x;
        yAngle = startAngles.y;

        distToTarget = (transform.position - lookAtTarget.position).magnitude;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateByInput();

        SetPositionByAngles();

        transform.LookAt(lookAtTarget, Vector3.up);
    }

    void RotateByInput()
    {
        xzAngle = Mathf.Repeat(xzAngle - Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed, 360f);

        float yRotationDirection = invertYAxis ? -1 : 1;

        yAngle = Mathf.Clamp(yAngle - Input.GetAxis("Mouse Y") * yRotationDirection * Time.deltaTime * rotationSpeed, minYAngle, maxYAngle);
    }

    void SetPositionByAngles()
    {
        float xzRad = xzAngle * Mathf.Deg2Rad; //angles, but in radians
        float yRad = yAngle * Mathf.Deg2Rad;

        Vector3 xzPos = new Vector3(Mathf.Cos(xzRad), 0f, Mathf.Sin(xzRad));

        Vector3 targetPosition = Vector3.Lerp(transform.position, lookAtTarget.position + (xzPos * Mathf.Cos(yRad) + Vector3.up * Mathf.Sin(yRad)).normalized * distToTarget, positionSmooth);

        RaycastHit cameraHit;

        float cameraDistance = (targetPosition - lookAtTarget.position).magnitude;

        Physics.Raycast(lookAtTarget.position, targetPosition - lookAtTarget.position, out cameraHit, cameraDistance, cameraCollisionMask);

        if(cameraHit.collider != null)
        {
            targetPosition = lookAtTarget.position + (targetPosition - lookAtTarget.position) * (cameraHit.distance / cameraDistance);
        }

        transform.position = targetPosition;
    }

    Vector2 GrabAngles(Vector3 cameraPos, Vector3 targetPos)
    {
        Vector3 offset = cameraPos - targetPos;

        Vector3 projectOnFloor = new Vector3(offset.x, 0f, offset.z).normalized;

        float xz =  Vector3.Angle(Vector3.right, projectOnFloor);

        if(projectOnFloor.z < 0f)
            xz = 360f - xz;

        float y = Vector3.Angle(projectOnFloor, offset);


        Vector2 result = new Vector2(xz, y);

        return result;
    }
}
