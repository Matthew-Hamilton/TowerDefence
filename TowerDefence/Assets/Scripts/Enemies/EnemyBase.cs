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

    public Node spawnPoint;
    private List<Node> myPath = new List<Node>();
    protected bool pathSet = false;

    [SerializeField]protected bool canMove = true;

    [SerializeField]protected int currentPathIndex = 0;
    EnemyController enemyController;
    Node targetNode;


    private void Start()
    {
        myPath = new List<Node>();
    }

    // Update is called once per frame
    public async virtual void Update()
    {
        if (currentPathIndex != -1 && canMove && pathSet)
        {
            canMove = false;
            await Move();

            currentPathIndex--;
            canMove = true;
        }
        if(currentPathIndex < 0)
            Die();
        
    }

    public virtual async Task Move()
    {
        canMove = false;

        targetNode = myPath[currentPathIndex];
        await transform.DOJump(targetNode.worldPos + new Vector3(0,-1,1), 0.5f, 8, 10 / speed).SetEase(Ease.Linear).AsyncWaitForCompletion();
    }

    public void ResetPath()
    {
        pathSet = false;
    }

    /*public void UpdatePath()
    {
        if (PathFinding.instance.path.Contains(targetNode))
        {
            currentPathIndex = PathFinding.instance.path.FindIndex(i => i == targetNode );
            return;
        }
        currentPathIndex = Mathf.Max(currentPathIndex, PathFinding.instance.path.Count-1);
    }
    */



    public void SetPath(List<Node> newPath)
    {
        myPath = newPath;
        currentPathIndex = myPath.Count-1;
        pathSet = true;
    }

    public void UpdatePath()
    {
        spawnPoint.attachedHex.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        PathFinding.instance.startPoint = spawnPoint;
        PathFinding.instance.FindPath();
        SetPath(PathFinding.instance.GetPath());
    }

    public bool CanPath()
    {

        myPath[currentPathIndex].attachedHex.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        PathFinding.instance.startPoint = myPath[currentPathIndex];
        spawnPoint = myPath[currentPathIndex];
        bool canPath = PathFinding.instance.FindPath();
        return canPath;
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
