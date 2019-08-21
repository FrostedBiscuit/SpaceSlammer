using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeInput : MonoBehaviour, IBeginDragHandler, IDragHandler/*, IEndDragHandler*/{

    public float AccelerationMultiplier = 3.5f;

    Vector2 lastDragPos;

    public void OnBeginDrag(PointerEventData eventData) {

        if (Player.instance.gameObject.activeSelf == false) { return; }

        Player.instance.GetRigidbody().velocity = Vector2.zero;

        lastDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) {

        if (Player.instance.gameObject.activeSelf == false) { return; }

        Player.instance.GetRigidbody().AddForce((Camera.main.ScreenToWorldPoint(eventData.position) - Camera.main.ScreenToWorldPoint(lastDragPos)) * AccelerationMultiplier);
        Player.instance.GetRigidbody().velocity = Vector2.ClampMagnitude(Player.instance.GetRigidbody().velocity, Player.instance.Speed);

        lastDragPos = eventData.position;
    }
    /*
    public void OnEndDrag(PointerEventData eventData) {

        if (Player.instance == null) { return; }

        Player.instance.GetRigidbody().AddForce((Camera.main.ScreenToWorldPoint(eventData.position) - Camera.main.ScreenToWorldPoint(beginDragPosition)).normalized * Player.instance.Speed);
    }*/
}
