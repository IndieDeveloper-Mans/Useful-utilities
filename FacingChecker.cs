using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceDirection
{
    forward,
    backward,
    left,
    right
}

public class FacingChecker
{
    public FaceDirection facingDirection;

    public float angleTreshHold = 0.99f; // set smaller value for lower accuracy

    public FaceDirection CheckIfFacingObject(Transform from, Transform target)
    { 
        Vector3 targetRelative = from.InverseTransformPoint(target.position).normalized;

        //forward and backward
        if (targetRelative.z >= angleTreshHold)
        {
            Debug.Log("Facing forward");

            facingDirection = FaceDirection.forward;       
        }
            
        else if (targetRelative.z <= -angleTreshHold)
        {
            Debug.Log("Facing backward");

            facingDirection = FaceDirection.backward;
        }
           
        // right and left
        if (targetRelative.x >= angleTreshHold)
        {
            Debug.Log("Facing right");

            facingDirection = FaceDirection.right;
        }
            
        else if(targetRelative.x <= -angleTreshHold)
        {
            Debug.Log("Facing left");

            facingDirection = FaceDirection.left;
        }

        return facingDirection;
    }
}