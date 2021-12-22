using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private List<Agent> enemyList = null;
    private Agent target = null;
    private Ability ability = null;

    private Animator anim;

    private CombatState combatState = CombatState.Idle;

    public bool isDead = false;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    public void Setup(List<Agent> targetList, Vector3 spawnPos, Ability ability)
    {
        enemyList = targetList;
        this.ability = ability;
        this.gameObject.transform.position = spawnPos;
        this.gameObject.SetActive(true);
    }

    public void StartCombat()
    {
        StartCoroutine(CombatRoutine());
    }

    private IEnumerator CombatRoutine()
    {
        while (true)
        {
            SelectNearestTarget(enemyList);

            if (target == null)
                break;

            ChasedownTarget();
            yield return null;
        }

        PlayAnimation(CombatState.Idle);
    }

    private void ChasedownTarget()
    {
        float distance = (this.transform.position - target.transform.position).sqrMagnitude;
        this.transform.LookAt(target.transform.position);

        if (!isDead)
        {
            if (distance > ability.range)
            {
                PlayAnimation(CombatState.Run);
                this.transform.Translate(Vector3.forward * ability.moveSpeed * Time.deltaTime);
            }
            else
                PlayAnimation(CombatState.Attack);
        }
    }

    private void SelectNearestTarget(List<Agent> targetList)
    {
        float minDistance = float.MaxValue;
        float curDistance = 0;

        int count = targetList.Count;
        int index = 0;

        bool alldied = true;

        for (int i = 0; i < count; i++)
        {
            if(!targetList[i].isDead)
            {
                alldied = false;
                curDistance = (this.transform.position - targetList[i].transform.position).sqrMagnitude;

                if (minDistance > curDistance)
                {
                    minDistance = curDistance;
                    index = i;
                }
            }
        }

        if (alldied)
            target = null;
        else
            target = targetList[index];
    }

    private void PlayAnimation(CombatState state)
    {
        if(combatState != state)
        {
            anim.SetTrigger(state.ToString());
            combatState = state;
        }
    }

    private void Attack()
    {
        if (target != null)
            target.Hit(ability.power);
    }

    private void Hit(int damage)
    {
        ability.hp -= damage;

        if (ability.hp <= 0)
            Death();
    }

    private void Death()
    {
        combatState = CombatState.Death;
        PlayAnimation(combatState);
        isDead = true;
        this.gameObject.SetActive(false);
    }
}
