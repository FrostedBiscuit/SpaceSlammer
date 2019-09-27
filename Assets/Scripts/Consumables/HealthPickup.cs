using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Consumable {

    public override void ConsumableEffect() {

        PlayerManager.instance.ApplyEffect(PlayerManager.Effect.HEAL, Amount);

        HealthPickupPool.instance.ReturnObject(this);
    }
}
