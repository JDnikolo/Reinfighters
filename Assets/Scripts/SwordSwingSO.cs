using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Sword Swing", menuName = "SwordSwingSO")]
public class SwordSwingSO : ScriptableObject 
{
    [SerializeField] Transform swingPrefab;
    [SerializeField] float moveSpeed=10f;
    [SerializeField] float rotationSpeed=90f;
    [SerializeField] float windUpDelay=0.5f;
    [SerializeField] float remainActiveDelay=0f;
    [SerializeField] float followThroughDelay=0.5f;

    public Transform GetStartingTransform(){
        return swingPrefab.GetChild(0);
    }

    public float GetSwingMoveSpeed(){
        return moveSpeed;
    }

    public float GetSwingRotationSpeed(){
        return rotationSpeed;
    }

    public List<Transform> GetTransforms(){
        List<Transform> transforms = new();
        foreach (Transform child in swingPrefab){
            transforms.Add(child);
        }
        return transforms;
    }
    public float GetWindUpDelay(){
        return windUpDelay;
    }
    public float GetFollowThroughDelay(){
        return followThroughDelay;
    }
    public float GetRemainActiveDelay(){
        return remainActiveDelay;
    }

}
