using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 3;
    LifeBarSpriteChanger lifeBarSpriteChanger;

    void Start(){
        lifeBarSpriteChanger = GetComponentInChildren<LifeBarSpriteChanger>();
    }

    public int GetHealth(){
        return health;
    }

    public void TakeDamage(int damage=1){
        health-=damage;
        
        if (health<=0){
            Die();
        }
        else{
            lifeBarSpriteChanger.ChangeSprite(health-1);
        }
    }
    void Die(){
        Destroy(gameObject);
    }
}
