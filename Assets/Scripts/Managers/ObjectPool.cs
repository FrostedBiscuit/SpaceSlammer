using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    #region Singelton  
    public static ObjectPool instance;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("ObjectPool::Awake() => More than 1 instance of ObjectPool in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField]
    Transform PoolParentTransform = null;

    [SerializeField]
    List<PoolObject> Objects = null;

    List<GameObject> activeObjects = new List<GameObject>();

    Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    // Start is called before the first frame update
    void Start() {

        foreach (var obj in Objects) {

            createPool(obj.Object, obj.NumToSpawn);
        }
    }

    /// <summary>
    /// Works as Instantiate.
    /// </summary>
    /// <param name="go">Requested GameObject</param>
    /// <param name="position">Object's position</param>
    /// <param name="rotation">Object's rotation</param>
    /// <returns>Instance of requested object</returns>
    public GameObject RequestObject(GameObject go, Vector3 position, Quaternion rotation) {

        string key = go.name;

        if (poolDictionary.ContainsKey(key)) {

            if (poolDictionary[key].Count == 0) {

                GameObject newObj = Instantiate(go, PoolParentTransform);
                newObj.SetActive(false);

                poolDictionary[key].Enqueue(newObj);
            }

            GameObject result = poolDictionary[key].Dequeue();
            result.transform.position = position;
            result.transform.rotation = rotation;
            result.SetActive(true);

            activeObjects.Add(result);

            return result;
        }

        Debug.LogError("ObjectPool::RequestObject() => Requested an invalid object!!!");

        return null;
    }
    // lmao
    /// <summary>
    /// Works as Destroy.
    /// </summary>
    /// <param name="go">GameObject instance to deactivate.</param>
    /// <returns>Instance of deactivated object</returns>
    public GameObject ReturnObject(GameObject go) {

        string key = go.name.Remove(go.name.Length - 7);

        if (poolDictionary.ContainsKey(key)) {

            activeObjects.Remove(go);

            go.SetActive(false);

            poolDictionary[key].Enqueue(go);

            // TODO: MAKE THIS DYNAMIC!!!
            if (poolDictionary[key].Count > 50) {

                for (int i = 0; i < (poolDictionary[key].Count - 50); i++) {

                    Destroy(poolDictionary[key].Dequeue());
                }
            }

            return go;
        }

        Debug.Log("ObjectPool::ReturnObject() => Returned an invalid object!!!");

        return null;
    }

    public void ClearObjects() {

        for (int i = activeObjects.Count - 1; i > -1; i--) {

            ReturnObject(activeObjects[i]);
        }

        activeObjects.Clear();
    }

    /// <summary>
    /// Creates new pool for a desired prefab. It will
    /// prewarm it to initialSize.
    /// </summary>
    /// <param name="prefab">Object you want to pool</param>
    /// <param name="initialSize">Number of initial instances of prefab</param>
    private void createPool(GameObject prefab, int initialSize) {

        string key = prefab.name;


        if (poolDictionary.ContainsKey(key) == false) {

            poolDictionary.Add(key, new Queue<GameObject>());

            for (int i = 0; i < initialSize; i++) {

                GameObject go = Instantiate(prefab, PoolParentTransform);

                go.SetActive(false);

                poolDictionary[key].Enqueue(go);
            }
        }
    }
}

[System.Serializable]
public struct PoolObject {

    public int NumToSpawn;

    public GameObject Object;
}
