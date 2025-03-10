using System;
using Unity.Mathematics;
using UnityEngine;

public class AvailableMoveChecker : MonoBehaviour
{
    [SerializeField] internal GameObject triggerbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Instantiate(triggerbox, new Vector3(0, 0.6f, 0), Quaternion.identity);
        triggerbox.SetActive(false);
        if (triggerbox == null)
        {
            Debug.LogError("TriggerBox not assigned in the Inspector!");
        }
    }

    internal void CheckAvailability(Vector3 marker)
    {
        if (triggerbox == null ) return;
        triggerbox.transform.position = marker + new Vector3(0, 0, 1);
        triggerbox.SetActive(true);

    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DarkMarker"))
        {
            Debug.Log("Darkmarker detected stay");
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WhiteMarker"))
        {
            Debug.Log("Whitemarker detected enter");
        }
        if (other.CompareTag("DarkMarker"))
        {
            Debug.Log("Darkmarker detected enter");
        }
    }
    

    /*private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("WhiteMarker"))
        {
            Debug.Log("whitemarker detected collision");
        }
    }*/
}
