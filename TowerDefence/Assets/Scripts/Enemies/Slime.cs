using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Slime : EnemyBase
{
    
    private void Start()
    {
        speed = 2.5f;
    }

    public override void Die()
    {
        for(int i = 0; i < 4; i++)
        {
            Vector3 randomOffset = new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0);
            EnemyController.instance.SpawnSlimes(transform.position + randomOffset, currentPathIndex);
        }
        base.Die();
    }
}
