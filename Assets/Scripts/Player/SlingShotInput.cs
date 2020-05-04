using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlingShotInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public float DefaultAccelerationMultiplier = 80f;
    public float MaxAccelerationMultiplier = 120f;

    private float accelerationMultiplier {
        get {
            return Mathf.Clamp(Player.instance.DamageMultiplier * DefaultAccelerationMultiplier, DefaultAccelerationMultiplier, MaxAccelerationMultiplier);
        }
    }

    Vector3 slingsShotAnchor;

    public void OnBeginDrag(PointerEventData eventData) {

        slingsShotAnchor = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData) {
        //throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData) {

        Vector3 dir = slingsShotAnchor - Camera.main.ScreenToWorldPoint(eventData.position);

        Player.instance.GetRigidbody().AddForce(dir * accelerationMultiplier);
        Player.instance.GetRigidbody().velocity = Vector2.ClampMagnitude(Player.instance.GetRigidbody().velocity, Player.instance.Speed);
    }
}
