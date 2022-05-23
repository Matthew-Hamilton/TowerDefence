using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    List<EnemyBase> enemies;
    public static EnemyController instance;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void SpawnWave()
    {
        enemies = new List<EnemyBase>();
        for(int i = 0; i < 10; i++)
        {
            EnemyBase enemy = new EnemyBase();
            await SpawnEnemy(enemy);
        }

    }

    async Task SpawnEnemy(EnemyBase enemy)
    {
        enemies.Add(enemy);
        Task.Delay(500);
        await Task.Yield();
    }


    public void RemoveEnemy(EnemyBase enemy)
    {
        instance.enemies.Remove(enemy);
    }
}
