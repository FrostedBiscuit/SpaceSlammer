﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<TPooledObject> : MonoBehaviour where  TPooledObject : MonoBehaviour {

    public int NumToSpawn = 5;

    public TPooledObject PooledObject;

    protected Queue<TPooledObject> pooledObjectQueue = new Queue<TPooledObject>();

    public virtual void Start() {

        PooledObject.gameObject.SetActive(false);

        for (int i = 0; i < NumToSpawn; i++) {


            TPooledObject obj = Instantiate(PooledObject, transform);

            pooledObjectQueue.Enqueue(obj);

        }

        PooledObject.gameObject.SetActive(true);
    }

    /// <summary>
    /// Works as Instantiate.
    /// </summary>
    /// <returns>Instance of pooled object</returns>
    public virtual TPooledObject RequestObject(Vector3 position, Quaternion rotation) {

        TPooledObject obj = pooledObjectQueue.Dequeue();

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(true);

        return obj;
    }

    /// <summary>
    /// Works as Destroy.
    /// </summary>
    /// <param name="obj">Object instance to return</param>
    /// <returns>Instance of deactivated object</returns>
    public virtual TPooledObject ReturnObject(TPooledObject obj) {

        obj.gameObject.SetActive(false);

        pooledObjectQueue.Enqueue(obj);
        
        return obj;
    }
}