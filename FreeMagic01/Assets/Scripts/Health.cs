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
    public GameObject BckgrLeft;
    public GameObject BckgrRight;

    // the left and right ratio texts
    public Text healthRatioLeft;
    public Text healthRatioRight;

    // the left and right player names
    public Text playerNameLeft;
    public Text playerNameRight;

    // the left portraits to display
    public GameObject archerPic;

    // game info
    public GameObject grid;
    public Grid gridScript;
    public Vector3 fullHealth;

    // Use this for initialization
    void Start()
    {
        // find all game objects
        frameLeft = GameObject.Find("PortraitFrameL");
        frameRight = GameObject.Find("PortraitFrameR");
        healthBarLeft = GameObject.Find("HealthBarL");
        healthBarRight = GameObject.Find("HealthBarR");
        BckgrLeft = GameObject.Find("BackgroundL");
        BckgrRight = GameObject.Find("BackgroundR");
        grid = GameObject.Find("Grid");
        archerPic = GameObject.Find("ArcherPic");
        // get required components
        healthRatioLeft = healthBarLeft.GetComponentInChildren<Text>();
        healthRatioRight = healthBarRight.GetComponentInChildren<Text>();
        playerNameLeft = frameLeft.GetComponentInChildren<Text>();
        playerNameRight = frameRight.GetComponentInChildren<Text>();
        gridScript = grid.GetComponent<Grid>();
        // save full health position
        fullHealth = transform.localScale;
        BckgrLeft.GetComponentInChildren<Text>().enabled = false;
        BckgrRight.GetComponentInChildren<Text>().enabled = false;
    }

    // called in Grid class
    // updates the appropriate healthbar to represent the currently selected unit,
    // their personal health, name, and image.
    public void updateHealthBar()
    {
        updateHealth(gridScript.getUnitSelected());
    }

    // called in the unit class when an enemy is attacked
    // the attacked unit is passed 
    public void updateEnemyHealthBar(Unit unit)
    {
        updateHealth(unit);
    }

    // make the necessary changes to the appropriate health bar
    public void updateHealth(Unit selected)
    {
        // hit value and unit variables
        float hit;
        bool isLeft = false;
        if (selected)
        { // get the selected unit and ensure it is not null
            float max = selected.unitMaxHealth; // get the max health
            float current = selected.unitHealth; // get the current health
            hit = ((max - current) / max) * -1; // calculate hit ratio
            if (selected.name.Equals("John") || selected.name.Equals("Steve"))
            {
                isLeft = true;
            }
            playerHealth(current, max, hit, isLeft); // to change the text on the health bar
            playerImageName(selected); // displays the unit's name on the health bar
        }
    }

    // displays the current health / max health on the appropriate health bar
    private void playerHealth(float current, float max, float hit, bool isLeft)
    {
        String ratio = current + "/" + max;
        if (isLeft)
        { // if isLeft is true then it is the left side to be updated
            healthRatioLeft.text = ratio; // the string for the healthbar
            if (current <= (max * .4))
            { // if the bar is less than or equal to 40% full
                healthRatioLeft.enabled = false; //disable bar display
                BckgrLeft.GetComponentInChildren<Text>().enabled = true; // enable the text in background
                BckgrLeft.GetComponentInChildren<Text>().text = ratio; // display also on background
            }
            else if (!healthRatioLeft.enabled && current > (max * .4))
            { // if the health has regenerated to above 40%
                healthRatioLeft.enabled = true; // enable bar text display
            }
        }
        else { // else it is the right
            healthRatioRight.text = ratio; // the string for the healthbar
            if (current <= (max * .4))
            { // if the bar is less than or equal to 40% full
                healthRatioRight.enabled = false; // disable bar text
                BckgrRight.GetComponentInChildren<Text>().enabled = true; // enable background text
                BckgrRight.GetComponentInChildren<Text>().text = ratio; // display also on background
            }
            else if (!healthRatioRight.enabled && current > (max * .4))
            { // of the health has regenerated tp above 40%
                healthRatioRight.enabled = true; // enable the bar text display
            }
        }
        healthBarRight.transform.localScale = fullHealth; // transform to full health position and...
        healthBarRight.transform.localScale += new Vector3(hit, 0); // ...move the health bar to new position
    }
    // changes the image and name base on which unit is selected
    // Image currently not changing but name is
    // took renderer off of frame in unity and now not working. 
    // Still have not gotten images to change...
    private void playerImageName(Unit unit)
    {
        if (unit.name.Equals("Steve"))
        {
            archerPic.SetActive(true); //activate steve
            playerNameLeft.text = unit.name; // display the name
        }
        else if (unit.name.Equals("John"))
        {
            archerPic.SetActive(false); // hide steve
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
