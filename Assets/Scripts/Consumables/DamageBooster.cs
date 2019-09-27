using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBooster : Consumable {

    public override void ConsumableEffect() {

        PlayerManager.instance.ApplyEffect(PlayerManager.Effect.DAMAGEBOOST, Amount, Duration);

        DamageBoosterPool.instance.ReturnObject(this);
    }
}
