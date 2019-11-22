using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPickup : Consumable {

    public override void ConsumableEffect() {

        Debug.Log($"consumed invincibility pickup, lenght of effect: {Duration}");

        PlayerManager.instance.ApplyEffect(PlayerManager.Effect.INVINCIBILITY, 0, Duration);

        InvincibilityPickupPool.instance.ReturnObject(this);
    }
}
