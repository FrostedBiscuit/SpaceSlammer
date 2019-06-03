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

    List<GameObject> activeObjects;
    List<GameObject> inactiveObjects;

    Vector3 resetPosition = new Vector3(0f, 0f, -10f);

    // Start is called before the first frame update
    void Start() {

        activeObjects = new List<GameObject>();
        inactiveObjects = new List<GameObject>();

        foreach (var obj in Objects) {

            for (int i = 0; i < obj.NumToSpawn; i++) {

                GameObject instance = Instantiate(obj.Object, resetPosition, Quaternion.identity, PoolParentTransform == null ? null : PoolParentTransform);

                instance.SetActive(false);

                inactiveObjects.Add(instance);
            }
        }

        for (int i = 0; i < PoolParentTransform.childCount; i++) {

            if (PoolParentTransform.GetChild(i).gameObject.activeSelf == true) {
                activeObjects.Add(PoolParentTransform.GetChild(i).gameObject);
            }
            else {
                inactiveObjects.Add(PoolParentTransform.GetChild(i).gameObject);
            }
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

        if (inactiveObjects == null) {
            Debug.LogError("obj is null");
        }

        // Try to find object
        // THIS HAS TO BE IMPROVED!!!!
        GameObject obj = inactiveObjects.Find(x => x.name == go.name + "(Clone)");

        if (obj != null) {

            // Reset it's properties
            obj.SetActive(true);
    
            obj.transform.position = position;
            obj.transform.rotation = rotation;

            // Move it to activeObjects
            activeObjects.Add(obj);
            inactiveObjects.Remove(obj);

            return obj;
        }

        // Logging an error if the requested object is not found and returning null
        Debug.LogError("ObjectPool::GetObject() => No object found!!! Name: " + go.name);
        return null;
    }
    // lmao
    /// <summary>
    /// Works as Destroy.
    /// </summary>
    /// <param name="go">GameObject instance to deactivate.</param>
    /// <returns>Instance of deactivated object</returns>
    public GameObject ReturnObject(GameObject go) {

        // Deactivate said object
        go.SetActive(false);

        go.transform.position = resetPosition;

        // Move it to inactiveObjects
        inactiveObjects.Add(go);
        activeObjects.Remove(go);

        return go;
    }
}

[System.Serializable]
public struct PoolObject {

    public int NumToSpawn;

    public GameObject Object;
}
