using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    private ZoneFlash _zoneFlash;

    void Start()
    {
        _zoneFlash = GetComponent<ZoneFlash>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            _zoneFlash.TriggerFlash();
        }
    }
}