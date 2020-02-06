using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeInput : MonoBehaviour, IBeginDragHandler, IDragHandler/*, IEndDragHandler*/{

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

        Player.instance.GetRigidbody().velocity = Vector2.zero;

        lastDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) {

        if (Player.instance.gameObject.activeSelf == false) { return; }

        Player.instance.GetRigidbody().AddForce((Camera.main.ScreenToWorldPoint(eventData.position) - Camera.main.ScreenToWorldPoint(lastDragPos)) * accelerationMultiplier);
        Player.instance.GetRigidbody().velocity = Vector2.ClampMagnitude(Player.instance.GetRigidbody().velocity, Player.instance.Speed);

        lastDragPos = eventData.position;
    }

    /*
    public void OnEndDrag(PointerEventData eventData) {

        if (Player.instance == null) { return; }

        Player.instance.GetRigidbody().AddForce((Camera.main.ScreenToWorldPoint(eventData.position) - Camera.main.ScreenToWorldPoint(beginDragPosition)).normalized * Player.instance.Speed);
    }*/
}
