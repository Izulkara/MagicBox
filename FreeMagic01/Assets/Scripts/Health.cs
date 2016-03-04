using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // the left and right health panels
    public GameObject frameLeft;
    public GameObject frameRight;
    // the left and right health bars
    public GameObject healthBarLeft;
    public GameObject healthBarRight;
    // the left and right ratio texts
    public Text healthRatioLeft;
    public Text healthRatioRight;
    // the left and right player names
    public Text playerNameLeft;
    public Text playerNameRight;
    // the left portraits to display
    public Image portraitLeft;
    public Sprite LumberJackJohn; // for changing imgs
    public Sprite ArcherSteve;
    // game info
    public GameObject grid;
    public Grid gridScript;
    public Vector3 fullHealth;

    // Use this for initialization
    void Start()
    {
        // load sprites
        LumberJackJohn = (Sprite)Resources.Load<Sprite>("Images/lumberjackface") as Sprite;
        ArcherSteve = (Sprite)Resources.Load<Sprite>("Images/archerface") as Sprite;
        // find all game objects
        frameLeft = GameObject.Find("PortraitFrameL");
        frameRight = GameObject.Find("PortraitFrameR");
        healthBarLeft = GameObject.Find("HealthBarL");
        healthBarRight = GameObject.Find("HealthBarR");
        grid = GameObject.Find("Grid");
        // get required components
        healthRatioLeft = healthBarLeft.GetComponentInChildren<Text>();
        healthRatioRight = healthBarRight.GetComponentInChildren<Text>();
        playerNameLeft = frameLeft.GetComponentInChildren<Text>();
        playerNameRight = frameRight.GetComponentInChildren<Text>();
        portraitLeft = frameLeft.GetComponentInChildren<Image>();
        gridScript = grid.GetComponent<Grid>();
        // save full health position
        fullHealth = transform.localScale;
    }

    // called in Grid class
    // updates the appropriate healthbar to represent the currently selected unit,
    // their personal health, name, and image.
    public void updateHealthBar()
    {
        // hit value and unit variables
        float hit;
        Unit selected;
        if (selected = gridScript.getUnitSelected())
        { // get the selected unit and ensure it is not null
            float max = selected.unitMaxHealth; // get the max health
            float current = selected.unitHealth; // get the current health
            hit = ((max - current) / max) * -1; // calculate hit ratio
            if (selected.name.Equals("John") || selected.name.Equals("Steve"))
            { // if the unit is John or Steve update the Left health bar
                healthBarLeft.transform.localScale = fullHealth;
                healthBarLeft.transform.localScale += new Vector3(hit, 0);
                displayHealthL(current, max); // to change the text on the health bar
                displayImageName(selected); // displays the unit's name on the health bar
            }
            else { // else update the right
                healthBarRight.transform.localScale = fullHealth;
                healthBarRight.transform.localScale += new Vector3(hit, 0);
                displayHealthR(current, max); // to change the text on the health bar
                displayImageName(selected); // displays the unit's name on the health bar
            }
        }
    }

    // displays the current health / max health on the left health bar
    private void displayHealthL(float current, float max)
    {
        healthRatioLeft.text = current + "/" + max;
    }

    // displays the current health / max health on the right health bar
    private void displayHealthR(float current, float max)
    {
        healthRatioRight.text = current + "/" + max;
    }

    // changes the image and name base on which unit is selected
    // Image currently not changing but name is
    private void displayImageName(Unit unit)
    {
        //get the renderer - I have one attached to the portraitLeft and to the frameLeft- neither working yet?
        SpriteRenderer sprRend = portraitLeft.GetComponent<SpriteRenderer>();
        if (unit.name.Equals("Steve"))
        {
            sprRend.sprite = ArcherSteve; // render the archer sprite
            portraitLeft.overrideSprite = ArcherSteve; // override current sprite with this one
            playerNameLeft.text = unit.name; // display the name
        }
        else if (unit.name.Equals("John"))
        {
            sprRend.sprite = LumberJackJohn; // render the lumberjack sprite
            portraitLeft.overrideSprite = LumberJackJohn;// override current sprite with this one
            playerNameLeft.text = unit.name; // display the name
        }
        else { // it is a Golem, same photo
            playerNameRight.text = unit.name; // display the name
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
