using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarSpriteChanger : MonoBehaviour
{
    [SerializeField] Sprite[] sprites = new Sprite[3];
    SpriteRenderer spriteRenderer;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[^1];
    }

    public void ChangeSprite(int index){
        spriteRenderer.sprite=sprites[index];
    }
}
