using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class EnemyBase : MonoBehaviour
{

    float health;
    float shield;
    float speed = 5;
    float damage;

    bool pathSet = false;

    [SerializeField]bool canMove = true;

    [SerializeField]int currentPathIndex = 0;
    EnemyController enemyController;


    private void Start()
    {
    }

    // Update is called once per frame
    public async virtual void Update()
    {
        if (PathFinding.instance.path.Count >= 1)
        {
            if (!pathSet)
            {
                pathSet = true;
                currentPathIndex = PathFinding.instance.path.Count - 1;
            }
            while (currentPathIndex != -1 && canMove)
            {
                canMove = false;
                await Move();

                currentPathIndex--;
                canMove = true;
            }
        }
    }

    public virtual async Task Move()
    {
        canMove = false;
        await transform.DOJump(PathFinding.instance.path[currentPathIndex].worldPos + new Vector3(0,-2,1), 0.5f, 8, 10 / speed).SetEase(Ease.Linear).AsyncWaitForCompletion();
    }

    public virtual void Die()
    {
        EnemyController.instance.RemoveEnemy(this);
        Destroy(gameObject);
    }

    public virtual void AttackTower()
    {

    }


}
