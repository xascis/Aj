using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Vector3 currentDestination;
    public Slider heathBar;
    public Slider energyBar;

    void Start()
    {
        heathBar.value = CalculateHealth();
        energyBar.value = CalculateEnergy();

        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

// Update is called once per frame
    void FixedUpdate()
    {

        energyBar.value = CalculateEnergy();

        _animator.SetFloat("speed", _navMeshAgent.desiredVelocity.magnitude);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 200.0f, LayerMask.GetMask("Ground")))
            {
                if (hit.transform != null)
                {
                    _navMeshAgent.speed = 2f;

                    GameManager.currentEnergy += 1f;

                    currentDestination = hit.point;
                    SetDestination();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 200.0f, LayerMask.GetMask("Ground")))
            {
                if (hit.transform != null)
                {
                    if (GameManager.currentEnergy > 0) {
                        _navMeshAgent.speed = 4f;
                    } else {
                        _navMeshAgent.speed = 2f;
                    }

                    currentDestination = hit.point;
                    SetDestination();
                }
            }
        }

    }

    private void SetDestination()
    {
        if (currentDestination != null)
        {
            _navMeshAgent.SetDestination(currentDestination);
        }
    }

    float CalculateHealth(){
        return GameManager.currentHealth / GameManager.maxHealth;
    }

    private void CharacterDamaged(){
        GameManager.currentHealth -= 1f;
        heathBar.value = CalculateHealth();
    }

    float CalculateEnergy(){
        return GameManager.currentEnergy / GameManager.maxEnergy;
    }
}