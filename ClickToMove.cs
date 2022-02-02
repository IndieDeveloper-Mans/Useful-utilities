using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ClickToMove : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    [Space]
    [SerializeField] RaycastHit _raycastHit;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out _raycastHit))
            {
                _navMeshAgent.destination = _raycastHit.point;
            }
        }
    }
}
