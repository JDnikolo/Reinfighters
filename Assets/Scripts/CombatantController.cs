using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;


public class CombatantController : MonoBehaviour
{
    protected Vector2 rawMovement;
    [SerializeField] float moveSpeed=10f;
    protected SwingPathfinder swingPathfinder;
    protected Rigidbody2D myRigidBody;
    [SerializeField] Vector2 attackKnockbackAmount = new(5,4);
    [SerializeField] Vector2 lungeKnockbackAmount = new(8,4);
    protected bool canAct=true;
    protected bool isStunned=false;
    enum Actions {attack, lunge, parry}
    readonly int parry = (int)Actions.parry;
    readonly int attack = (int)Actions.attack;
    readonly int lunge = (int)Actions.lunge;
    Health myHealth;    
    [SerializeField] float[] hitStunDurations = {1,2,2};
    // Start is called before the first frame update
    void Awake()
    {
        myHealth = GetComponent<Health>();
        swingPathfinder = GetComponentInChildren<SwingPathfinder>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        canAct = swingPathfinder.GetCurrentSwingCoroutine() == null;
        if(canAct && !isStunned){
            Move();
        }
    }

    protected void Move(){
        if (canAct && !isStunned){
            transform.position += moveSpeed * Time.deltaTime * (Vector3)rawMovement;
        }
    }

    void OnMove(InputValue value){
        rawMovement = new Vector2(value.Get<Vector2>().x,0);  
    }

    protected void OnAttack(){
        if (canAct && !isStunned){
        swingPathfinder.Swing(attack);
        canAct=false;
        }
    }
    protected void OnLunge(){
        if (canAct && !isStunned){
        swingPathfinder.Swing(lunge);
        canAct=false;
        }
    }
    protected void OnParry(){
        if (canAct && !isStunned){
        swingPathfinder.Swing(parry);
        canAct=false;
        }
    }
    void OnEscape(){
        FindObjectOfType<LevelManager>().Escape();
    }

    public void HitKnockback(int intensity=0)
    {
        Vector2 knockback=new();
        if (intensity==attack){
            knockback=attackKnockbackAmount;
            StartCoroutine(HitStun(attack));
        }
        else if (intensity==lunge){
            knockback=lungeKnockbackAmount;
            StartCoroutine(HitStun(lunge));
        }
        else if (intensity==parry){
            knockback=attackKnockbackAmount;
            StartCoroutine(HitStun(parry));
        }
        myRigidBody.velocity=new Vector2(-knockback.x*transform.localScale.x,knockback.y);
    }
    protected IEnumerator HitStun(int action){
        isStunned=true;
        yield return new WaitForSeconds(hitStunDurations[action]);
        if (myHealth.GetHealth()>0) isStunned=false;
        myRigidBody.velocity=new();
    }
    public void SetStun(bool value){
        isStunned=value;
    }

    public void StartRoundStartSwing(){
        swingPathfinder.RoundStartSwing();
    }
    public void StartVictorySwing(){
        swingPathfinder.VictorySwing();
    }
    public void StartDeathSwing(){
        
        swingPathfinder.DeathSwing();
    }
}
