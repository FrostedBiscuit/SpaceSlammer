using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyIndicator : MonoBehaviour {

    Camera cam;

    Image img;

    Transform target;

    SpriteRenderer enemySpriteRenderer;

    void OnEnable() {

        cam = cam == null ? Camera.main : cam;
        img = img == null ? GetComponentInChildren<Image>() : img;

        if (target != null && target.gameObject.activeSelf == false) {

            target = null;

            enemySpriteRenderer = null;
        }
    }

    // Update is called once per frame
    void Update() {
        
        if (target == null) {
            Debug.Log("target is null");
            return;
        }

        if (enemySpriteRenderer == null) {
            Debug.Log("sr is null");
            return;
        }


        if (enemySpriteRenderer.isVisible == false) {

            img.enabled = true;

            Vector3 dir = (target.position - Player.instance.transform.position).normalized;
            
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        }
        else {

            img.enabled = false;
        }

    }

    public void SetTarget(Transform enemy) {

        target = enemy;
        enemySpriteRenderer = enemy.transform.GetComponentInChildren<SpriteRenderer>();
    }
}
