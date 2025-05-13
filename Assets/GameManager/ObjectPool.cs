using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; set; }
    [Serializable]
    internal class Pool
    {
        [SerializeField]internal string tag;
        [SerializeField]internal GameObject prefab;
        [SerializeField]internal int size;
    }
    
    [SerializeField]internal List<Pool> objPooList;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in objPooList)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    internal GameObject Get(string tag)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist!");
            return null;
        }

        var obj = poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        poolDictionary[tag].Enqueue(obj);
        return obj;
    }

    internal void Return(string tag, GameObject obj)
    {
        
    }
}
