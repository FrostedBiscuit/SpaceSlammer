using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    Vector2 beginDragPosition;

    public void OnBeginDrag(PointerEventData eventData) {

        if (Player.instance == null) { return; }

        Player.instance.GetRigidbody().velocity = Vector2.zero;

        beginDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) {

        if (Player.instance == null) { return; }

        Player.instance.GetRigidbody().velocity = (Camera.main.ScreenToWorldPoint(eventData.position) - Camera.main.ScreenToWorldPoint(beginDragPosition)).normalized * Player.instance.FollowSpeed;
    }

    public void OnEndDrag(PointerEventData eventData) {

        if (Player.instance == null) { return; }

        Player.instance.GetRigidbody().AddForce((Camera.main.ScreenToWorldPoint(eventData.position) - Camera.main.ScreenToWorldPoint(beginDragPosition)).normalized * Player.instance.Speed);
    }
}
