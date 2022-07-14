using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private PoolableObject prefab;
    private int size;
    private List<PoolableObject> availableObjectsPool;

    private ObjectPool(PoolableObject prefab, int size)
    {
        this.prefab = prefab;
        this.size = size;
        availableObjectsPool = new List<PoolableObject>(size);
    }

    public static ObjectPool CreateInstance(PoolableObject prefab, int size)
    {
        ObjectPool pool = new ObjectPool(prefab, size);
        GameObject poolGameObject = new GameObject(prefab + " Pool");
        pool.CreateObjects(poolGameObject);
        return pool;
    }

    private void CreateObjects(GameObject parent)
    {
        for (int i = 0; i < size; i++)
        {
            PoolableObject poolableObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObject.Parent = this;
            poolableObject.gameObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
        }
    }

    // Get & Return Methods
    public PoolableObject GetObject()
    {
        // gets a gameobject from pool
        PoolableObject instance = availableObjectsPool[0];
        availableObjectsPool.RemoveAt(0);
        instance.gameObject.SetActive(true);
        return instance;
    }
    public void ReturnObjectToPool(PoolableObject poolableObject)
    {
        availableObjectsPool.Add(poolableObject);
    }
}