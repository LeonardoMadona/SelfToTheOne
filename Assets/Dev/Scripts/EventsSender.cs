using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsSender : MonoBehaviour
{
    public UnityEvent eventsToFire;
    public UnityEvent eventsToFire2;

    public void FireEvents()
    {
        eventsToFire.Invoke();
    }
    public void FireEvents2()
    {
        eventsToFire2.Invoke();
    }
}
