using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{
    [SerializeField] GameObject projectile;

    protected float damage = 10;
    protected float range = 50;
    protected DamageType damageType;


    protected float fireDelay=1;
    bool canFire = true;



    IEnumerator Fire(EnemyBase enemy)
    {
        canFire = false;
        Instantiate(projectile, transform.position, Quaternion.LookRotation((enemy.transform.position -transform.position).normalized));

        yield return new WaitForSeconds(fireDelay);
        canFire = true;

    }

    void Update()
    {
        if (canFire)
        {
            bool enemySet = false;
            EnemyBase temp = new EnemyBase();
            temp.SetCurrentIndexMax();
            foreach(EnemyBase enemy in EnemyController.instance.GetEnemies())
            {
                if ((enemy.transform.position - transform.position).magnitude < range)
                {
                    if (temp == null)
                    {
                        temp = enemy;
                        enemySet = true;
                    }
                    else if(enemy.GetCurrentPathIndex() < temp.GetCurrentPathIndex())
                    {
                        temp = enemy;
                        enemySet = true;
                    }
                }
            }
            if(enemySet)
                StartCoroutine(Fire(temp));
        } 
    }
    private void Awake()
    {
        projectile = Resources.Load<GameObject>("Projectile");
    }

    public enum DamageType
    {
        Bludgeoning = 0,
        Explosive = 1,
        Fire = 2,
        Poison = 3
    }
}
