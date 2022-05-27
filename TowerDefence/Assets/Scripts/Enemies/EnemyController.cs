using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    List<EnemyBase> enemies;
    public static EnemyController instance;
    [SerializeField] GameObject[] enemyTypes;

    bool spawned = true;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        List<EnemyBase> enemies = new List<EnemyBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnWave()
    {
        if (enemies != null)
        {
            foreach (EnemyBase enemy in enemies)
                Destroy(enemy.gameObject);

            enemies.Clear();
        }
        else
            enemies = new List<EnemyBase>();
        for(int i = 0; i < 1; i++)
        {
            spawned = false;
            StartCoroutine(SpawnEnemy(Random.Range(0, enemyTypes.Length)));
            yield return new WaitUntil(() => spawned);
            enemies[i].SetPath(PathFinding.instance.GetPath());
        }

    }

    IEnumerator SpawnEnemy(int enemyTypeIndex)
    {

        yield return new WaitForSeconds(0.5f);
        GameObject enemy = Instantiate(enemyTypes[enemyTypeIndex], PathFinding.instance.startPoint.worldPos + new Vector3(0, -1, 1), Quaternion.identity);
        EnemyBase enemyBase = new EnemyBase();
        enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.spawnPoint = PathFinding.instance.startPoint;
        enemies.Add(enemyBase);
        spawned = true;
    }

    public IEnumerator SpawnSlimes(Vector3 position, int currentPathIndex)
    {
        yield return new WaitForSeconds(0.5f);
        GameObject enemy = Instantiate(enemyTypes[2], position, Quaternion.identity);
        enemies.Add(enemy.GetComponent<EnemyBase>());
        spawned = true;
    }

    public void UpdatePaths()
    {
        if (enemies != null)
        {
            foreach (EnemyBase enemy in enemies)
            {
                enemy.UpdatePath();
            }
        }
    }

    public bool CheckAllCanPath()
    {
        if (enemies != null)
        {
            int numCanPath = 0;
            for(int i = 0; i < enemies.Count; i++)
                if (enemies[i].CanPath())
                    numCanPath++;

            return numCanPath == enemies.Count;
        }
        return true;
    }


    public void RemoveEnemy(EnemyBase enemy)
    {
        instance.enemies.Remove(enemy);
    }
}
