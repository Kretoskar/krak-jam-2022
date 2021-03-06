using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Game.Combat;
using Game.Control;
using UnityEngine;

[RequireComponent(typeof(BossStateIdle))]
[RequireComponent(typeof(BossStateAttack))]
[RequireComponent(typeof(BossStateDeath))]
[RequireComponent(typeof(EnemyHealth))]
public class BossStateMachine : StateMachine
{
    [SerializeField] private float appearTime = 3;
    [SerializeField] private float idleTime = 1;
    [SerializeField] private float attackTime = 1;
    [SerializeField] private Bullet bullet;
    
    
    private BossStateIdle idleState;
    private BossStateAttack attackState;
    private BossStateDeath deathState;
    private EnemyHealth health;

    private Animator animator;

    private float timer;

    private void Awake()
    {
        idleState = GetComponent<BossStateIdle>();
        attackState = GetComponent<BossStateAttack>();
        deathState = GetComponent<BossStateDeath>();
        health = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health.Death += Die;
    }

    private void OnDisable()
    {
        health.Death -= Die;
        
    }

    private void Start()
    {
        ChangeState(idleState);

        timer = 0;
    }
    
    private void Update()
    {
        if (timer < appearTime)
        {
            timer += Time.deltaTime;
            return;
        }
        
        if (animator.IsInTransition(0))
        {
            if (animator.GetAnimatorTransitionInfo(0).duration < .15f)
            {
                ChangeState(idleState);
            }
            else
            {
                ChangeState(attackState);
            }
        }
        
        currentState?.Execute();
    }

    private void Die()
    {
        ChangeState(deathState);
    }
}
