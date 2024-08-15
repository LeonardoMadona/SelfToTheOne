using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveVoidPlayer : MonoBehaviour
{
    public float moveSpeed;
    public UnityEvent OnGrabCube;

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Vertical"), 0f, -Input.GetAxis("Horizontal"));

        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BodyCube"))
        {
            OnGrabCube?.Invoke();
        }
    }
}
