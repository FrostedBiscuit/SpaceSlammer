using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    float Speed = 5f;

    public Transform FollowTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FollowTransform != null) {

            transform.position = Vector3.Lerp(transform.position, FollowTransform.position + new Vector3(0f, 0f, -10f), Speed * Time.deltaTime);
        }
    }
}
