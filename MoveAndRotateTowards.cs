using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndRotateTowards
{
    // position reached check
    public float posThreshold = 0.01f;

    // rotation reached check
    public float rotThreshold = 0.95f;

    // rotation dot
    private float dot;

    public bool MoveTowards(Transform from, Transform target, float moveSpeed = 0.1f, float offset = 0.1f, bool moveUp = false)
    {
        // direction from the player to the target
        Vector3 direction = (from.position - target.position).normalized;

        // adding the offset in that direction
        Vector3 targetDirection = target.position + (direction * offset);

        //here we can check by bool, if we need to use "y" of target position || for futere, maybe ?
        if (!moveUp)
        {
            targetDirection = new Vector3(targetDirection.x, from.position.y, targetDirection.z);
        }

        float moveStrength = Mathf.Min(moveSpeed * Time.deltaTime, 1);

        from.position = Vector3.MoveTowards(from.position, targetDirection, moveStrength);

        float distance = Vector3.Distance(from.position, targetDirection);

        Debug.DrawRay(from.position, direction, Color.magenta);

        if (distance <= posThreshold)
        {
            Debug.Log("Target Distance Reached");

            return true;
        }
        else
        {
            Debug.Log("Moving To Target");

            return false;
        }
    }

    public bool RotateTowards(Transform from, Transform target, float rotationSpeed, bool rotateXAngle = false)
    {
        Vector3 direction = (new Vector3(target.position.x, 0, target.position.z) - new Vector3(from.position.x, 0, from.position.z)).normalized;
        
        //here we can check by bool, if we need to use "x" of rotation || for future, maybe ?
        //if (!rotateXAngle)
        //{
        //    from.rotation = Quaternion.Euler(0, from.eulerAngles.y, 0);
        //}

        //float rotationStrength = Mathf.Min(rotationSpeed * Time.deltaTime, 1);

        float rotationStrength = rotationSpeed * Time.deltaTime;

        from.rotation = Quaternion.Lerp(from.rotation, Quaternion.LookRotation(direction), rotationStrength);

        //draw a ray pointing to our target
        Debug.DrawRay(from.position, direction, Color.cyan, 0.1f);

        dot = Vector3.Dot(from.forward, direction);  // check facing towards item

        Debug.Log("Rotation Dot Is " + dot);

        if (dot >= rotThreshold)
        {
            Debug.Log("Rotation Reached");

            return true;
        }
        else
        {
            Debug.Log("Rotating");

            return false;
        }
    }

    public bool RotateToTargetRotation(Transform from, Quaternion targetRotation, float rotationSpeed)
    {
        //float rotationStrength = Mathf.Min(rotationSpeed * Time.deltaTime, 1);

        float rotationStrength = rotationSpeed * Time.deltaTime;

        from.localRotation = Quaternion.RotateTowards(from.localRotation, targetRotation, rotationStrength);

        if (Quaternion.Angle(from.localRotation, targetRotation) <= 0.01f)
        {
            Debug.Log("Rotate To Target Rotation Reached");

            return true;
        }
        else
        {
            Debug.Log("Rotate To Target Rotation");

            return false;
        }
    }

    public bool MoveToTargetPosition(Transform from, Transform targetPos, float moveSpeed)
    {
        if (Vector3.Distance(from.position, targetPos.position) <= 0.01f)
        {
            Debug.Log("Move To Target Position Reached");

            return true;
        }
        else
        {
            Debug.Log("Move To Target Position");

            float moveStrength = Mathf.Min(moveSpeed * Time.deltaTime, 1);

            from.transform.position = Vector3.MoveTowards(from.position, targetPos.position, moveStrength);

            return false;
        }
    }

    public bool MoveToTargetPosition(Vector3 from, Vector3 targetPos, float moveSpeed)
    {
        if (Vector3.Distance(from, targetPos) <= 0.01f)
        {
            Debug.Log("Move To Target Position Reached");

            return true;
        }
        else
        {
            Debug.Log("Move To Target Position");

            float moveStrength = Mathf.Min(moveSpeed * Time.deltaTime, 1);

            from = Vector3.MoveTowards(from, targetPos, moveStrength);

            return false;
        }
    }

    public bool MoveToTargetPosition(Transform from, Vector3 targetPos, float moveSpeed)
    {
        if (Vector3.Distance(from.position, targetPos) <= 0.01f)
        {
            Debug.Log("Move To Target Position Reached");

            return true;
        }
        else
        {
            Debug.Log("Move To Target Position");

            float moveStrength = Mathf.Min(moveSpeed * Time.deltaTime, 1);

            from.position = Vector3.MoveTowards(from.position, targetPos, moveStrength);

            return false;
        }
    }
}
