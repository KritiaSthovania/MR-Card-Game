using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

// The class of the castle game object
static class EnemyPools
{
    // The maximum and current health point of the castle
    public static int[] enemyPoolIds;
}

public class ObjectPools : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Create all enemy pools
        int poolId1 = ObjectPool<Enemy>.CreateNewPool(); // Pool for normal enemies
        int poolId2 = ObjectPool<Enemy>.CreateNewPool(); // Pool for fast enemies
        int poolId3 = ObjectPool<Enemy>.CreateNewPool(); // Pool for super fast enemies
        int poolId4 = ObjectPool<Enemy>.CreateNewPool(); // Pool for flying enemies
        int poolId5 = ObjectPool<Enemy>.CreateNewPool(); // Pool for tanky enemies
        int poolId6 = ObjectPool<Enemy>.CreateNewPool(); // Pool for slow enemies
        int poolId7 = ObjectPool<Enemy>.CreateNewPool(); // Pool for berzerker enemies
        int poolId8 = ObjectPool<Enemy>.CreateNewPool(); // Pool for berzerker flying enemies
        int poolId9 = ObjectPool<Enemy>.CreateNewPool(); // Pool for berzerker tanky enemies
        int poolId10 = ObjectPool<Enemy>.CreateNewPool(); // Pool for arrows
        int poolId11 = ObjectPool<Enemy>.CreateNewPool(); // Pool for fire balls
        int poolId12 = ObjectPool<Enemy>.CreateNewPool(); // Pool for stones

        int poolId13 = ObjectPool<Enemy>.CreateNewPool(); // Pool for dumping enemies with wrong enemy type

        // Store the pool ids so that they are not lost
        EnemyPools.enemyPoolIds = new int[13];
        EnemyPools.enemyPoolIds[0] = poolId1;
        EnemyPools.enemyPoolIds[1] = poolId2;
        EnemyPools.enemyPoolIds[2] = poolId3;
        EnemyPools.enemyPoolIds[3] = poolId4;
        EnemyPools.enemyPoolIds[4] = poolId5;
        EnemyPools.enemyPoolIds[5] = poolId6;
        EnemyPools.enemyPoolIds[6] = poolId7;
        EnemyPools.enemyPoolIds[7] = poolId8;
        EnemyPools.enemyPoolIds[8] = poolId9;
        EnemyPools.enemyPoolIds[9] = poolId10;
        EnemyPools.enemyPoolIds[10] = poolId11;
        EnemyPools.enemyPoolIds[11] = poolId12;
        EnemyPools.enemyPoolIds[12] = poolId13;

        // Add a normal enemy to the first enemy pool
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    private GameObject[] objectPrefabs;

    // Get an object form the pool
    public GameObject GetObject(string type)
    {
        // Go through the array
        for(int i = 0; i < objectPrefabs.Length; i++)
        {
            // Check if there is an object that has the name of the type
            if(objectPrefabs[i].name == type)
            {
                // Instantiate the object
                GameObject newObject = Instantiate(objectPrefabs[i]);
                newObject.name = type;
                return newObject;
            }
        }
        return null;
    }

    // //
    // public void ReleaseObject(GameObject object) 
    // {
    //     gameObject.SetActive(false);
    // }

    // Method that returns the correct object pool index given the enemy type
    public static int GetObjectPoolIndex(string type)
    {
        switch(type)
        {
            case "Normal Enemy":
                return EnemyPools.enemyPoolIds[0];
            break;
            case "Fast Enemy":
                return EnemyPools.enemyPoolIds[1];
            break;
            case "Super Fast Enemy":
                return EnemyPools.enemyPoolIds[2];
            break;
            case "Flying Enemy":
                return EnemyPools.enemyPoolIds[3];
            break;
            case "Tank Enemy":
                return EnemyPools.enemyPoolIds[4];
            break;
            case "Slow Enemy":
                return EnemyPools.enemyPoolIds[5];
            break;
            case "Berzerker Enemy":
                return EnemyPools.enemyPoolIds[6];
            break;
            case "Berzerker Flying Enemy":
                return EnemyPools.enemyPoolIds[7];
            break;
            case "Berzerker Tank Enemy":
                return EnemyPools.enemyPoolIds[8];
            break;
        }

        // Case the enemy does not have a correct type
        return EnemyPools.enemyPoolIds[12];
    }

    public static void ReleaseEnemy(Enemy enemy)
    {
        // Get the correclt object pool index from the object pools class
        int objectPoolIndex = GetObjectPoolIndex(enemy.GetEnemyType);

        // Release the enemy into the right object pool
        ObjectPool<Enemy>.ReleaseResource(objectPoolIndex, enemy);

        // Set the enemy as inactive
        enemy.gameObject.SetActive(false);
    }
}
