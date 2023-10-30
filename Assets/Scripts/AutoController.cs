using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Scripting.APIUpdating;

public class AutoController : CombatantController
{
    CombatantController player;

    void Start(){
        CombatantController[] players = FindObjectsOfType<CombatantController>();
        foreach(CombatantController controller in players){
            if (controller.gameObject.transform!=transform){
                player = controller;
            }
        }
        swingPathfinder = GetComponentInChildren<SwingPathfinder>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        canAct = swingPathfinder.GetCurrentSwingCoroutine() == null;
        if (!canAct) return;
        int action = Random.Range(0,200);
        switch (action){
            case 0: {OnAttack(); break;}
            case 1: {OnLunge();break;}
            case 2: {OnParry();break;}
            default:{
                rawMovement = ((!player.IsDestroyed()?player.transform.position:transform.position)-transform.position).normalized;
                Move();
                break;
            }
        }
    }
}