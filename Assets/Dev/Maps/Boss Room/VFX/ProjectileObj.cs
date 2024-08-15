using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObj : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealthHandle>().TakeHit();
        }

        gameObject.SetActive(false);
    }
}
