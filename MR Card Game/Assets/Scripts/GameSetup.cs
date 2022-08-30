using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

// The class of the castle game object
public static class GameAdvancement
{
    // The maximum and current health point of the castle
    public static int castleMaxHP;
    public static int castlecurrentHP;

    // The current armor points of the castle
    public static int castleCurrentAP;

    // The amount of currency the player has right now
    public static int currencyPoints;

    // The current wave
    public static int currentWave;

    // The currency display button for the player
    public static Button currencyDisplay;

    // The health bar slider of the castle
    public static Slider castleHealthBar;

    // The armor bar slider of the castle
    public static Slider castleArmorBar;
    
    // The health counter C / M displayed on the health bar
    public static TMP_Text castleHealthCounter;

    // The wave display button;
    public static Button waveDisplay;

    // The status of the game, if it should be paused or not
    public static bool gamePaused = false;

    // The variable that states if the time was stopped for the enemies
    public static bool timeStopped = false;

    // The variable that states what the global enemy slow factor is currently
    public static float globalSlow = 1;

    // The flag that states if it is raining
    public static bool raining = false;

    // The flag that states if the game setup needs to be reset or not
    public static bool needToReset = false;

    // Define the number of buildings built
    public static int numberOfBuildingsBuilt = 0;

    // Define the maximum number of buildings
    public static int maxNumberOfBuildings = 0;

    // // The global flag that states if the player is currently building or upgrading something
    // public static bool currentlyBuildingOrUpgrading = false;
}

// // The class of the castle game object
// static class Buildings
// {
//     // The number of buildings built in this round
//     public static int numberOfBuildings;

//     // The array that binds the image target to the building
//     public static GameObject[] imageTargetToBuilding;

//     // Define the first building
//     public static GameObject firstBuilding;

//     // Define the second building
//     public static GameObject secondBuilding;
    
//     // Define the third building
//     public static GameObject thirdBuilding;
    
//     // Define the fourth building
//     public static GameObject fourthBuilding;
    
//     // Define the fifth building
//     public static GameObject fifthBuilding;
    
//     // Define the sixth building
//     public static GameObject sixthBuilding;
    
//     // Define the seventh building
//     public static GameObject seventhBuilding;
    
//     // Define the eighth building
//     public static GameObject eighthBuilding;
    
//     // Define the ninth building
//     public static GameObject ninthBuilding;
    
//     // Define the tenth building
//     public static GameObject tenthBuilding;
// }
   
public class GameSetup : MonoBehaviour
{
    // The instance of this class so that some variables can be accessed in a static way
    public static GameSetup instance;

    // The button, not interactable, on which the current wave is displayed
    [SerializeField]
    private GameObject gameOverlay;

    // The method used to access the game overlay object as a static object
    public static GameObject GetGameOverlay
    {
        get { return instance.gameOverlay; }
    }

    // The health points the castle has. Can be changed in the inspector.
    [SerializeField]
    private int castleHP;

    // The method used to access the castle health points as a static object
    public static int GetCastleHP
    {
        get { return instance.castleHP; }
    }

    // The currency the player should have at the beginning. Can be changed in the inspector.
    [SerializeField]
    private int beginingCurrency;

    // The method used to access the begining currecy as a static object
    public static int GetBeginingCurrency
    {
        get { return instance.beginingCurrency; }
    }

    // The button, not interactable, on which the current wave is displayed
    [SerializeField]
    private Button waveDisplay;

    // The button, not interactable, on which the current currency is displayed
    [SerializeField]
    private Button currencyDisplay;

    // The health bar slider of the castle
    [SerializeField]
    private Slider castleHealthBar;

    // The health bar slider of the castle
    [SerializeField]
    private TMP_Text castleHealthCounter;

    // The armor icon
    [SerializeField]
    private Slider castleArmorBar;

    private void Awake()
    {
        if(instance == null)
        {
            // Set the instance to this script
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // armorPoints.GetComponent<Renderer>().sortingLayerID = armorPoints.transform.parent.GetComponent<Renderer>().sortingLayerID;

        // // Set the number of buildings built in this round to zero
        // Buildings.numberOfBuildings = 0;

        // // Initialize the array of image targets
        // Buildings.imageTargetToBuilding = new GameObject[10];

        // // Set the castle max and current hp to the hp given in the inspector
        // GameAdvancement.castleMaxHP = castleHP;
        // GameAdvancement.castlecurrentHP = castleHP;

        // // Set the currency points to the amount the player should have in the begining
        // GameAdvancement.currencyPoints = beginingCurrency;

        // // Set the wave counter to wave 0
        // GameAdvancement.currentWave = 0;

        GameAdvancement.waveDisplay = waveDisplay;

        // Set the currency display button
        GameAdvancement.currencyDisplay = currencyDisplay;

        // Set the castle health bar slider
        GameAdvancement.castleHealthBar = castleHealthBar;

        // Set the castle armor bar slider
        GameAdvancement.castleArmorBar = castleArmorBar;

        // Set the castle health counter
        GameAdvancement.castleHealthCounter = castleHealthCounter;

        // Reset the game setup
        ResetGameSetup();
    }

    // Update is called once per frame
    void Update()
    {
        // // Check if the game setup needs to be reset
        // if(GameAdvancement.needToReset == true)
        // {
        //     // Reset the game setup
        //     ResetGameSetup();

        //     // Set the flag to false
        //     GameAdvancement.needToReset = false;
        // }
    }

    // Method used to actualize the current currency display of the player
    public static void UpdateCurrencyDisplay()
    {
        // Actualize the currency currently owned
        GameAdvancement.currencyDisplay.GetComponentInChildren<TMP_Text>().text = "Currency: " + GameAdvancement.currencyPoints;
    }

    // Method used to actualize the current health points of the castle
    public static void ActualizeCastleHealthPoints()
    {
        // Actualize the value of the castle health bar
        GameAdvancement.castleHealthBar.value = (float)((float)GameAdvancement.castlecurrentHP / (float)GameAdvancement.castleMaxHP);

        // Change the text field that displayed current HP / max HP
        GameAdvancement.castleHealthCounter.text = GameAdvancement.castlecurrentHP + " / " + GameAdvancement.castleMaxHP;

        // Actualize the value of the castle armor bar
        GameAdvancement.castleArmorBar.value = (float)((float)GameAdvancement.castleCurrentAP / (float)GameAdvancement.castleMaxHP);
    }

    // Method used to actualize the current wave
    public static void UpdateWaveDisplay()
    {
        GameAdvancement.waveDisplay.GetComponentInChildren<TMP_Text>().text = "Wave: " + GameAdvancement.currentWave;

    }

    // Method used to reset the game setup after a level was finished
    public static void ResetGameSetup()
    {
        // Check the number of waves, and set the castle health points accordingly
        if(LevelInfo.numberOfWaves == 1)
        {
            // Set the max health points to 10
            GameAdvancement.castleMaxHP = 10;

        } else if(LevelInfo.numberOfWaves == 2)
        {
            // Set the max health points to 20
            GameAdvancement.castleMaxHP = 20;

        } else {

            // Reset the max health points
            GameAdvancement.castleMaxHP = GetCastleHP;
        }

        // // Reset the max health points
        // GameAdvancement.castleMaxHP = getCastleHP;

        // Set the castle current health points to their maximum
        GameAdvancement.castlecurrentHP = GameAdvancement.castleMaxHP;

        // Set the armor points to 0
        GameAdvancement.castleCurrentAP = 0;

        // Set the current currency points to the begining currency
        GameAdvancement.currencyPoints = GetBeginingCurrency;

        // Set the wave counter to wave 0
        GameAdvancement.currentWave = 0;

        // Set the number of buildings built to 0
        GameAdvancement.numberOfBuildingsBuilt = 0;

        // Set the maximum number of buildings correctly
        SetMaxNumberOfBuildings();

        // Make sure the game is not paused
        GameAdvancement.gamePaused = false;

        // Make sure the time is not stopped
        GameAdvancement.timeStopped = false;

        // Make sure there is no global slow
        GameAdvancement.globalSlow = 1;

        // Make sure it is not raining
        GameAdvancement.raining = false;

        // Actualize the wave display
        UpdateWaveDisplay();

        // Actualize the currency display
        UpdateCurrencyDisplay();

        // Actualize the castle health points
        ActualizeCastleHealthPoints();
    }

    // The method used to set the max number of buildings correctly
    public static void SetMaxNumberOfBuildings()
    {
        // Check the number of waves
        if(LevelInfo.numberOfWaves == 1)
        {
            // Set the maximum number of buildings to 3
            GameAdvancement.maxNumberOfBuildings = 3;

        } else if(LevelInfo.numberOfWaves == 2)
        {
            // Set the maximum number of buildings to 6
            GameAdvancement.maxNumberOfBuildings = 6;

        } else {
            // Set the maximum number of buildings to 10
            GameAdvancement.maxNumberOfBuildings = 10;
        }
    }
}
