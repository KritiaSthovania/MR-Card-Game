using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

public class SpawnTrap : MonoBehaviour
{
    // The instance, needed to access the static versions of the tower prefabs
    public static SpawnTrap instance;

    // The prefab for the hole
    [SerializeField]
    private Trap hole;

    // The hole size
    [SerializeField]
    private float holeSize;

    // The prefab for the swamp
    [SerializeField]
    private Trap swamp;

    // The swamp size
    [SerializeField]
    private float swampSize;

    // The method used to access to the hole prefab as a static object
    public static Trap Hole
    {
        get { return instance.hole; }
    }
    // The method used to access to the hole size in a static way
    public static float HoleSize
    {
        get { return instance.holeSize; }
    }
    // The method used to access to the swamp prefab as a static object
    public static Trap Swamp
    {
        get { return instance.swamp; }
    }
    // The method used to access to the swamp size in a static way
    public static float SwampSize
    {
        get { return instance.swampSize; }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // The method used to spawn a hole
    public static Trap SpawnHole(GameObject parent)
    {
        // Get the right object pool index for the trap type
        int poolIndex = ObjectPools.GetTrapPoolIndex(TrapType.Hole);

        // Get a new trap from the object pool of the hole
        Trap trap = ObjectPool<Trap>.RequestResource(poolIndex, () => {return Instantiate(Hole);});

        // Set the trap as active
        trap.gameObject.SetActive(true);

        // Set them as children of the parent that was passed
        trap.gameObject.transform.parent = parent.transform;

        // Scale the trap down
        trap.transform.localScale = new Vector3(Board.greatestBoardDimension * HoleSize * 3, Board.greatestBoardDimension * 3, Board.greatestBoardDimension * HoleSize * 3);

        // Vector3 newPosition =  new Vector3(0, towerOverhead * Board.greatestBoardDimension, 0);

        // Reset the position of the trap and add the necessary overhead so that the trap is over the ground
        trap.gameObject.transform.localPosition = new Vector3(0, 0, 0);

        // // Add the reference to this building to the Buildings class so that it can be accessed
        // AddBuildingReference(tower, parent);

        return trap;
    }

    // The method used to spawn a swamp
    public static Trap SpawnSwamp(GameObject parent)
    {
        // Get the right object pool index for the trap type
        int poolIndex = ObjectPools.GetTrapPoolIndex(TrapType.Swamp);

        // Get a new trap from the object pool of the swamp
        Trap trap = ObjectPool<Trap>.RequestResource(poolIndex, () => {return Instantiate(Swamp);});

        // Set the trap as active
        trap.gameObject.SetActive(true);

        // // Scale the trap down
        // trap.transform.localScale = new Vector3(1, 1, 1);

        // Set them as children of the parent that was passed
        trap.gameObject.transform.parent = parent.transform;

        // Scale the trap down
        trap.transform.localScale = new Vector3(Board.greatestBoardDimension * SwampSize * 3, Board.greatestBoardDimension * 3, Board.greatestBoardDimension * SwampSize * 3);

        // Vector3 newPosition =  new Vector3(0, towerOverhead * Board.greatestBoardDimension, 0);

        // Reset the position of the trap and add the necessary overhead so that the trap is over the ground
        trap.gameObject.transform.localPosition = new Vector3(0, 0, 0);

        // // Add the reference to this building to the Buildings class so that it can be accessed
        // AddBuildingReference(tower, parent);

        return trap;
    }
}
