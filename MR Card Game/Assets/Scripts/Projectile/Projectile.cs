using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // A projectile instance needed to access non static variables in a static way
    public static Projectile instance;

    // The target of the projectile
    private Enemy target;

    // The parent tower of the projectile
    private Tower parent;

    // Method used to get the type of the tower
    public Tower getParent2
    {
        get { return parent; }
    }

    // Method used to get the type of the tower
    public static Tower getParent
    {
        get { return instance.parent; }
    }

    // // The parent tower of the projectile
    // private string projectileType;

    // // Method used to get the type of the tower
    // public string getProjectileType
    // {
    //     get { return instance.projectileType; }
    // }

    // The last position of the target is saved so that the projectile can continue travelling after the enemy is dead
    private Vector3 lastPosition;

    // The list of colliders that enter the range of the tower
    public List<Collider> colliders = new List<Collider>();

    // The method used to access the list of colliders
    public List<Collider> GetColliders()
    {
        return colliders;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the instance
        instance = this;

        // // Get the right type
        // switch(getParent.getTowerType)
        // {
        //     case "Archer Tower":
        //         projectileType = "Arrow";
        //     break;

        //     case "Fire Tower":
        //         projectileType = "Fire Ball";
        //     break;

        //     case "Earth Tower":
        //         projectileType = "Stone";
        //     break;
        // }

        // Debug.Log("Projectile type: " + projectileType + " with parent: " + parent.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        // If the game is not paused, update and make the projectile move
        if(GameAdvancement.gamePaused == false)
        {
            // If a projectile was created, it should move to the target
            MoveToTarget();
        }
    }

    // The initialize method used to set the parent and target objects
    public void Initialize(Tower parent)
    {
        // Set the parent tower of the projectile as the parent of the projectile
        this.target = parent.Target.gameObject.GetComponent<Enemy>();
        this.parent = parent;

        // // Set the game object as child under the tower
        // transform.parent = parent.transform;
    }

    // Method that makes a projectile move to a target
    private void MoveToTarget()
    {
        // Debug.Log("In move to target: target is null: " + (target == null) + " and target is alive: " + target.isAlive);
        // Check that the target is not null
        if(target != null && target.isAlive == true)
        {
            // Actualize the target last position
            lastPosition = target.transform.position;

            // If the target is not null and alive, move the projectile in the direction of the target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed * 14 * Board.greatestBoardDimension);

            // Check if the projectile is an arrow
            if(parent.TowerType == TowerType.Archer)
            {
                // Make the arrow face his target
                transform.LookAt(target.transform.position);
            }

            // Check if the projectile reached the destination
            if(ReachPosition(target.transform.position))
            {

                // Here, depending on the tower type, the enemy or enemies need to take damage depending on the type of projectile of the tower
                if(parent.TowerType == TowerType.Archer)
                {
                    // Make a single enemy take damage
                    ArrowEffect();

                } else {
                    
                    // Make all enemies inside the projectile collider take damage
                    ProjectileThatCanHitMultipleEnemiesEffect();
                }

                // Release the projectile to the right object pool
                ObjectPools.ReleaseProjectile(this, parent);
            }

        } else {

            // Check if it is not a wind tower or lightning tower that do not have a projectile
            if(parent.TowerType != TowerType.Wind && parent.TowerType != TowerType.Lightning && parent.TowerType != TowerType.Archer)
            {
                // Make the projectile move to the last position
                transform.position = Vector3.MoveTowards(transform.position, lastPosition, Time.deltaTime * parent.ProjectileSpeed);

                // Check if the projectile reached the destination
                if(transform.position == target.transform.position)
                {
                    // Make the effect of the projectile reaching his destination take effect
                    ProjectileThatCanHitMultipleEnemiesEffect();

                    // Release the projectile
                    ObjectPools.ReleaseProjectile(this, parent);
                }
            } else {
                // Case it is an arrow and needs to despawn
                ObjectPools.ReleaseProjectile(this, parent);
            }
        }
    }

    private bool ReachPosition(Vector3 position)
    {
        if (Vector3.Magnitude(transform.position - position) <= 0.002f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // // Method that makes a stone projectile move to a target location
    // private void MakeStoneFallOnEnemy()
    // {
    //     // Calculate the position the enemy will be at at the moment the stone falls on the ground
    //     Vector3 targetPosition = target.position + 
    // }

    // The method that produces the effect of an arrow arriving at destination
    private void ArrowEffect()
    {
        // Debug.Log("The enemy " + target.gameObject.name + " was in the range of " + getProjectileType + " and was hit with the arrow effect");

        // Initialize the damage variable
        int damage = 0;

        // Calculate the damage
        damage = CalculateDamage(parent.Damage, parent.WeaknessMultiplier, parent.TowerType, target.GetComponent<Enemy>());

        // Make the enemy take damage
        target.TakeDamage(damage);
    }

    // The method that produces the effect of a fire ball and rock arriving at destination
    private void ProjectileThatCanHitMultipleEnemiesEffect()
    {
        // Initialize the damage variable
        int damage = 0;

        // Initialize the list of dead enemies
        List<Collider> listOfDead = new List<Collider>();

        // For each enemy in the collider, calculate the damage they should take
        foreach(var targetEnemy in GetColliders())
        {
            Debug.Log("Currently, the target enemy is null is: " + (targetEnemy == null));
            // Debug.Log("The enemy " + targetEnemy.gameObject.name + " was in the range of " + getProjectileType + " and was hit with the projectile that can hit multiple enemies");
            if(targetEnemy != null)
            {
                // Calculate the damage
                damage = CalculateDamage(parent.Damage, parent.WeaknessMultiplier, parent.TowerType, targetEnemy.GetComponent<Enemy>());

                // Check that the damage is not null
                if(damage != 0)
                {
                    // Check if the enemy will die from this attack
                    if(targetEnemy != null && damage >= targetEnemy.GetComponent<Enemy>().CurrentHP)
                    {
                        // Add the target enemy to the list of dead enemies
                        listOfDead.Add(targetEnemy);
                    }

                    if(targetEnemy != null)
                    {
                        // Make the enemy take damage
                        targetEnemy.GetComponent<Enemy>().TakeDamage(damage);
                    }
                }
            }
        }

        // Check that the list of dead enemies is not empty
        if(listOfDead != null)
        {
            // Go over the list of dead enemies
            foreach(var enemy in listOfDead)
            {
                // Remove the enemies that are dead from the colliders list
                colliders.Remove(enemy);
            }
        }
    }

    // The method that calculates the damage a unit should take depending on the enemy, tower and tower attack type
    public static int CalculateDamage(int damage, float weaknessMultiplier, TowerType towerType, Enemy target)
    {

        Debug.Log("The enemy for which damage is calculated is: " + (target == null));
        // Initialize the additional damage multiplier
        int additionalDamageMultiplier = 0;

        if(target != null)
        {
            switch(towerType)
            {
                case TowerType.Archer:

                    if(target.Weakness == ResistenceAndWeaknessType.Archer)
                    {
                        additionalDamageMultiplier = 1;
                    } else if(target.Resistance == ResistenceAndWeaknessType.Archer)
                    {
                        additionalDamageMultiplier = -1;
                    }

                break;
                
                case TowerType.Fire:
                    if(target.Weakness == ResistenceAndWeaknessType.Fire)
                    {
                        additionalDamageMultiplier = 1;
                    } else if(target.Resistance == ResistenceAndWeaknessType.Fire)
                    {
                        additionalDamageMultiplier = -1;
                    }
                break;

                case TowerType.Lightning:
                    if(target.Weakness == ResistenceAndWeaknessType.Lighting)
                    {
                        if(GameAdvancement.raining == true || target.isWet == true)
                        {
                            additionalDamageMultiplier = 2;
                        } else {
                            additionalDamageMultiplier = 1;
                        }
                    } else if(target.Resistance == ResistenceAndWeaknessType.Lighting)
                    {
                        if(GameAdvancement.raining == true)
                        {
                            additionalDamageMultiplier = 0;
                        } else {
                            additionalDamageMultiplier = -1;
                        }
                    } else {
                        if(GameAdvancement.raining == true || target.isWet == true)
                        {
                            additionalDamageMultiplier = 1;
                        }
                    }
                break;

                case TowerType.Earth:
                    if(target.Weakness == ResistenceAndWeaknessType.Earth)
                    {
                        additionalDamageMultiplier = 1;
                    } else if(target.Resistance == ResistenceAndWeaknessType.Earth)
                    {
                        additionalDamageMultiplier = -1;
                    }
                break;

                case TowerType.Wind:
                    if(target.Weakness == ResistenceAndWeaknessType.Wind)
                    {
                        additionalDamageMultiplier = 1;
                    } else if(target.Resistance == ResistenceAndWeaknessType.Wind)
                    {
                        additionalDamageMultiplier = -1;
                    }
                break;
            }

            // Return the damage with a bonus, a malus or flat depending on if a weakness or resistance was found
            return (int) (damage + additionalDamageMultiplier * damage * weaknessMultiplier);

        } else {

            return 0;
        }
    }

    // Method used to reinitialize the colliders list
    public void ClearCollidersList()
    {
        // Reinitialize colliders list as a new list
        colliders = new List<Collider>();
    }

    // The method that adds entering enemies to the collider list
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is already in the list or not
        if(!colliders.Contains(other))
        {
            // Add colliders that enter the projectile collider
            colliders.Add(other);
        }
    }

    // The method that removes exiting enemies of the collider list
    private void OnTriggerExit (Collider other)
    {
        // Remove colliders that leave the collider of the projectile collider
        colliders.Remove(other);
    }

    // Method used to return the enemy to the right object pool upon death
    public void ReturnProjectileToObjectPool()
    {
        // Call the release enemy of the object pool class
        ObjectPools.ReleaseProjectile(this, parent);
    }
}
