using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwingPathfinder : MonoBehaviour
{
    
    [SerializeField] SwordSwingSO[] swordSwings = new SwordSwingSO[3];
    BoxCollider2D myBoxCollider;
    SwordSwingSO swordSwing = null;
    [SerializeField] Transform returnPoint;
    List<Transform> transforms;
    int transformIndex=0;
    Coroutine currentSwingCoroutine=null;
    enum Actions {parry, attack, lunge}
    Actions currentAction;

    void Start(){
        myBoxCollider=GetComponent<BoxCollider2D>();
        myBoxCollider.enabled = false;
    }
    public int GetCurrentAction(){
        return (int)currentAction;
    }

    public Coroutine GetCurrentSwingCoroutine(){
        return currentSwingCoroutine;
    }

    public void Swing(int swingIndex){
        if (currentSwingCoroutine!=null) return;
        currentAction = (Actions)swingIndex;
        swordSwing = swordSwings[swingIndex];
        transforms=swordSwing.GetTransforms();
        currentSwingCoroutine=StartCoroutine(MoveAndRotateSword());
    }

    IEnumerator MoveAndRotateSword(){
        while (transformIndex<transforms.Count){
            Vector3 targetPosition = transforms[transformIndex].localPosition;
            Quaternion targetRotation = transforms[transformIndex].localRotation;
            float positionDelta = swordSwing.GetSwingMoveSpeed() * Time.deltaTime;
            float rotationDelta = swordSwing.GetSwingRotationSpeed() * Time.deltaTime;
            transform.SetLocalPositionAndRotation(Vector2.MoveTowards(
                transform.localPosition,
                targetPosition,
                positionDelta), Quaternion.Lerp(
                transform.localRotation,
                targetRotation,
                rotationDelta
            ));
            if (transform.localPosition==targetPosition){
                if (transformIndex==0) {
                    yield return new WaitForSeconds(swordSwing.GetWindUpDelay());
                    myBoxCollider.enabled=true;}
                transformIndex++;
            }
            yield return new WaitForNextFrameUnit();   
        }
        transformIndex=0;
        yield return new WaitForSeconds(swordSwing.GetRemainActiveDelay());
        myBoxCollider.enabled=false;
        yield return new WaitForSeconds(swordSwing.GetFollowThroughDelay());
        currentSwingCoroutine=StartCoroutine(ReturnToStarting());
    }
    IEnumerator ReturnToStarting(){
        Vector3 targetPosition = returnPoint.localPosition;
        Quaternion targetRotation = returnPoint.localRotation;
        while (transform.localPosition!=returnPoint.localPosition){
            float positionDelta = swordSwing.GetSwingMoveSpeed() * Time.deltaTime;
            transform.SetLocalPositionAndRotation(Vector2.MoveTowards(
                transform.localPosition,
                targetPosition,
                positionDelta), Quaternion.Lerp(
                transform.localRotation,
                targetRotation,
                1
            ));
            yield return new WaitForNextFrameUnit();   
        }
        currentSwingCoroutine=null;
    }
    public void CancelCurrentSwing(){
        if (currentSwingCoroutine==null) return;
        StopCoroutine(currentSwingCoroutine);
        transformIndex=0;
        myBoxCollider.enabled=false;
        currentSwingCoroutine=StartCoroutine(ReturnToStarting());
    }
}
