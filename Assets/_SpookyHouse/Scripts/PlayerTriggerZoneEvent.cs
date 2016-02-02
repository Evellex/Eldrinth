using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerTriggerZoneEvent : MonoBehaviour {
    public UnityEvent events;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<PlayerHead>())
        {
            events.Invoke();
        }
    }
	
}
