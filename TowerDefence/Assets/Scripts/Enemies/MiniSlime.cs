using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MiniSlime : Slime
{

    private void Start()
    {
        speed = 4f;
    }

    public override Task Move()
    {
        return base.Move();
    }
    public override void Die()
    {
        EnemyController.instance.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
