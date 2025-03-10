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

    internal void ActiveTrigger(Vector3 marker)
    {
        if (triggerbox == null ) return;
        triggerbox.transform.position = marker + new Vector3(0, 0, 1);
        triggerbox.SetActive(true);

    }
    
    void CheckSurroundings()
    {
        Vector3 position = transform.position;
        Debug.Log("Pos: " + position);

        Vector3[] directions = new []
        {
            new Vector3(position.x + 1, position.y, position.z), //R-U
            new Vector3(position.x - 1, position.y, position.z), // L-U
            new Vector3(position.x + 1, position.y, position.z - 2), // R-D
            new Vector3(position.x - 1, position.y, position.z - 2)  // L-D
        };

        foreach (Vector3 dir in directions)
        {
            Collider[] hitColliders = Physics.OverlapSphere(dir, 0.1f);
            Debug.Log("Dir: "+dir);
            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("DarkMarker"))
                {
                    Debug.Log("Marker found at: " + dir);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WhiteMarker"))
        {
            Debug.Log("Whitemarker detected enter");
        }
        if (other.CompareTag("DarkMarker"))
        {
            Debug.Log("Darkmarker detected enter");
            CheckSurroundings();
        }
    }
}
