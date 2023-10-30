using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : MonoBehaviour
{
    enum Actions {attack, lunge, parry}
    SwingPathfinder swingPathfinder;
    void Start(){
        swingPathfinder = GetComponent<SwingPathfinder>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other==transform.parent.parent.GetComponent<Collider2D>()) return;
        if (other.CompareTag("Player")){
            other.GetComponent<Health>().TakeDamage();
            other.GetComponent<CombatantController>()
                .HitKnockback(swingPathfinder.GetCurrentAction());
            other.GetComponentInChildren<SwingPathfinder>().CancelCurrentSwing();
        }
        else if(other.CompareTag("Weapon")){
            HandleWeaponCollision(other);
        }
    }

    private void HandleWeaponCollision(Collider2D other)
    {
        Debug.Log(swingPathfinder.GetCurrentAction()+" "+other.GetComponent<SwingPathfinder>().GetCurrentAction());
        Actions actionSelf,actionOther;
        actionSelf=(Actions)swingPathfinder.GetCurrentAction();
        actionOther=(Actions)other.GetComponent<SwingPathfinder>().GetCurrentAction();
        if (actionSelf==Actions.parry && actionOther==Actions.lunge){
            swingPathfinder.CancelCurrentSwing();
        }
        else if(actionSelf==Actions.lunge && actionOther==Actions.parry){
            swingPathfinder.CancelCurrentSwing();
            transform.parent.parent.GetComponent<CombatantController>().HitKnockback(0);
        }
    }


}
