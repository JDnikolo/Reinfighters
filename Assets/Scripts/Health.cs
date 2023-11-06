using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 3;
    LifeBarSpriteChanger lifeBarSpriteChanger;
    UIHandler uiHandler;
    enum Side {left,right}
    Side mySide;

    void Start(){
        lifeBarSpriteChanger = GetComponentInChildren<LifeBarSpriteChanger>();
        uiHandler = FindObjectOfType<UIHandler>();
        mySide = Math.Sign(transform.localScale.x) > 0 ? Side.left : Side.right;
        if (uiHandler!=null)
        {
            uiHandler.SetMaxHealth(health,(int)mySide);
        }
    }

    public int GetHealth(){
        return health;
    }

    public void TakeDamage(int damage=1){
        health-=damage;
        uiHandler.SetHealth(health,(int)mySide);
        if (health<=0){
            Die();
        }
        else{
            lifeBarSpriteChanger.ChangeSprite(health-1);
        }
    }
    void Die(){
        FindObjectOfType<MatchManager>().RoundEnd(gameObject);
    }
}
