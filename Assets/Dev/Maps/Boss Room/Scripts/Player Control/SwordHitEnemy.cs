using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitEnemy : MonoBehaviour
{
    [SerializeField] EnemyBehaviour enemyBehaviour;

    bool canHitEnemy = false;

    private void OnTriggerStay(Collider other)
    {
        if(canHitEnemy)
        {
            enemyBehaviour.TakeHit();
            canHitEnemy = false;
        }
    }

    public void ToggleCanHitEnemyOn()
    {
        canHitEnemy = true;
    }
    public void ToggleCanHitEnemyOff()
    {
        canHitEnemy = false;
    }
}
