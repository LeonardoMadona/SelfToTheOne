using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBossFight : MonoBehaviour
{
    [SerializeField] UnityEvent turnOnBossFight;
    bool active = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!active)
        {
            return;
        }
        active = false;

        turnOnBossFight.Invoke();

        enabled = false;
    }
}
