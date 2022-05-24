using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class EnemyBase : MonoBehaviour
{
    
    protected float health;
    protected float shield;
    protected float speed = 5;
    protected float damage;

    protected bool pathSet = false;

    [SerializeField]protected bool canMove = true;

    [SerializeField]protected int currentPathIndex = 0;
    EnemyController enemyController;
    Node targetNode;


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
        targetNode = PathFinding.instance.path[currentPathIndex];
        await transform.DOJump(targetNode.worldPos + new Vector3(0,-2,1), 0.5f, 8, 10 / speed).SetEase(Ease.Linear).AsyncWaitForCompletion();
    }

    public void ResetPath()
    {
        pathSet = false;
    }

    public void UpdatePath()
    {
        if (PathFinding.instance.path.Contains(targetNode))
        {
            currentPathIndex = PathFinding.instance.path.FindIndex(i => i == targetNode );
            return;
        }
        currentPathIndex = Mathf.Max(currentPathIndex, PathFinding.instance.path.Count-1);
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
