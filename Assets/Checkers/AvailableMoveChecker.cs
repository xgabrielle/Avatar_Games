using Unity.Mathematics;
using UnityEngine;

public class AvailableMoveChecker : MonoBehaviour
{
    [SerializeField] internal GameObject triggerbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        triggerbox.SetActive(false);
        if (triggerbox == null)
        {
            Debug.LogError("TriggerBox not assigned in the Inspector!");
        }
    }

    internal void CheckAvailability(Vector3 marker)
    {
        if (triggerbox == null ) return;
        triggerbox.transform.position = marker + new Vector3(0, 0, 0.5f);
        triggerbox.SetActive(true);

    }

  
}
