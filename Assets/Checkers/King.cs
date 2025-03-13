using UnityEngine;

public class King : MonoBehaviour
{
    [SerializeField]internal bool isKing;
    
    internal void SpawnCrown(GameObject marker, GameObject crownPrefab)
    {
        Vector3 parentPos = marker.transform.position + Vector3.up *0.2f;
        Debug.Log("marker: "+marker.transform.position);
        Debug.Log("parentPos: "+parentPos);
        GameObject crown = Instantiate(crownPrefab, parentPos, Quaternion.identity);
        crown.transform.SetParent(marker.transform);

        isKing = true;
    }
    
}
