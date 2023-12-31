using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private List<GameObject> pooledObjects = new List<GameObject>();
    private GameObject objectPrefab;

    public ObjectPool(GameObject prefab)
    {
        objectPrefab = prefab;
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeSelf)
            {
                return obj;
            }
        }

        GameObject newObj = GameObject.Instantiate(objectPrefab);

        pooledObjects.Add(newObj);

        newObj.SetActive(false);

        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    public List<GameObject> GetActiveObjects()
    {
        List<GameObject> activeObjects = new List<GameObject>();

        foreach (GameObject obj in pooledObjects)
        {
            if (obj.activeSelf)
            {
                activeObjects.Add(obj);
            }
        }

        return activeObjects;
    }
}