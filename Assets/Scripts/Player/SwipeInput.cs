using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeInput : MonoBehaviour, IBeginDragHandler, IDragHandler {

    public float DefaultAccelerationMultiplier = 80f;
    public float MaxAccelerationMultiplier = 120f;

    private float accelerationMultiplier {
        get {
            return Mathf.Clamp(Player.instance.DamageMultiplier * DefaultAccelerationMultiplier, DefaultAccelerationMultiplier, MaxAccelerationMultiplier);
        }
    }

    Vector2 lastDragPos;

    public void OnBeginDrag(PointerEventData eventData) {

        if (Player.instance.gameObject.activeSelf == false) { return; }

        Player.instance.Stop();

        lastDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) {

        if (Player.instance.gameObject.activeSelf == false) { return; }

        Player.instance.AddForce((Camera.main.ScreenToWorldPoint(eventData.position) - Camera.main.ScreenToWorldPoint(lastDragPos)) * accelerationMultiplier);

        lastDragPos = eventData.position;
    }
}
