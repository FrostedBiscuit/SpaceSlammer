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

        target = null;

        enemySpriteRenderer = null;
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

            Vector3 camPos = new Vector3(cam.transform.position.x, cam.transform.position.y);
            Vector3 dir = (target.position - Player.instance.transform.position).normalized;
            //Vector3 finalpos = new Vector3(Mathf.Clamp(dir.x, -(Screen.width / 2f), Screen.width / 2f), Mathf.Clamp(dir.y, -(Screen.height / 2f), Screen.height / 2f));

            //transform.position = cam.ScreenToWorldPoint(finalpos);
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

            Debug.Log("Enemy indicator should be visible");
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
