using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIConsumableIcon : MonoBehaviour {

    public Sprite ActiveSprite = null;
    public Sprite InactiveSprite = null;

    Image image;

    private void OnEnable() {

        image = GetComponentInChildren<Image>();

        Deactivate();
    }

    public virtual void Activate() {

        image.sprite = ActiveSprite;
    }

    public virtual void Deactivate() {

        image.sprite = InactiveSprite;
    }
}
