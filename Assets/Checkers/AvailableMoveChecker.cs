using System;
using Unity.Mathematics;
using UnityEngine;

public class AvailableMoveChecker : MonoBehaviour
{
    [SerializeField] internal GameObject triggerbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //triggerbox.SetActive(false);
      
    }

    internal void ActiveTrigger(Vector3 marker)
    {
        if (triggerbox == null ) return;
        triggerbox.transform.position = marker + new Vector3(0, 0, 1);
        triggerbox.SetActive(true);
        // checksurroundings??

    }
    

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WhiteMarker"))
        {
            Debug.Log("Whitemarker detected enter");
        }
        if (other.CompareTag("DarkMarker"))
        {
            Debug.Log("Darkmarker detected enter");
           
        }
    }*/
}
