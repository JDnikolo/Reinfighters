using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Scripting.APIUpdating;

public class AutoController : CombatantController
{
    CombatantController opponent;
    int movementDirection=1;

    void Start(){
        movementDirection=1;
        swingPathfinder = GetComponentInChildren<SwingPathfinder>();
        myRigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(RandomMoveDirectionChange());
    }
    public void SetOpponent(CombatantController targetOpponent){
        opponent = targetOpponent;
    }
    void Update()
    {
        canAct = swingPathfinder.GetCurrentSwingCoroutine() == null;
        if (canAct && !isStunned)
        {   
            int action = Random.Range(0,200);
            switch (action){
                case 0: {OnAttack(); break;}
                case 1: {OnLunge();break;}
                case 2: {OnParry();break;}
                default:{
                    rawMovement = ((!opponent.IsDestroyed()?opponent.transform.position:transform.position)-transform.position).normalized*movementDirection;
                    Debug.Log(rawMovement+" "+movementDirection);
                    Move();
                    break;
                }
            }
        }
    }
    IEnumerator RandomMoveDirectionChange(){
        while (true){
            yield return new WaitForSeconds(Random.Range(0.25f,1.5f));
            movementDirection*=-1;
        }
    }

    private void OnDestroy() {
        StopCoroutine(RandomMoveDirectionChange());    
    }

}