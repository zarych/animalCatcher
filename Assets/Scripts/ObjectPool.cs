using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject animalPrefab;
    [SerializeField] private int animalsMaxCount = 15;

    public static ObjectPool Instance;

    private List<GameObject> _pooledObjects = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        for(int i = 0; i < animalsMaxCount; i++)
        {
            GameObject obj = Instantiate(animalPrefab);
            _pooledObjects.Add(obj);
            obj.SetActive(false);
        }
    }

    public GameObject GetObjectFromPool() 
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            { 
                return _pooledObjects[i];
            }
        }
        return null;
    }
}
