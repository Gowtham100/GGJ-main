using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Timer : MonoBehaviour {

    private bool jailed; //If you are put in jail

    private bool sacrificed; //Boolean to trigger sacrificing sacrifices
    private bool sacrificeAdded; //Set true if someone put in shopping cart
    private int sacrifices; //The number of people sacrificed, if 0 then in mall collecting people
   
    private int hunger; //The hunger meter
    private float toHungerCount; //current count to hungerTime

    private int mealPoints; //the current points you have for each person captured
    private int mealBonus; //the next bonus you get for capture

    //public shit
    public float hungertime; //time it takes for one hunger to decrease
    public int bonus; //how much bonus you get for each added person
    public int jailTime; //home much time you get added for being put in jail
    public int personPoints; //how much time you get reduced for each person
    //This is the scale for the bar if you rescale the whole object
    public float scale;


    public Scrollbar redbar;
    public Scrollbar orangebar;




	// Use this for initialization
	void Start () {
        hunger = 0;
        toHungerCount = hungertime;
        mealBonus = 0;

        jailed = false;
        sacrificed = false;
        sacrificeAdded = false;
        sacrifices = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (toHungerCount > 0)
            toHungerCount -= Time.deltaTime;
        else updateHunger();
        if (jailed)
            jail();
        if (sacrificeAdded)
            addSacrifice();
        if (sacrificed)
            sacrifice();
	}

    private void updateHunger() {
        toHungerCount = hungertime;
        hunger++;
        Debug.Log(hunger);
        if (hunger == 100)
            gameOver();
        updateRedAndOrange(); //update the visual
    }

    //TODO: GAME OVER STFF
    private void gameOver() {
    }

    //TODO: ADD time for jail
    private void jail() {
        hunger += jailTime;
        restartPoints();
        jailed = false;
        //NEED TO RESET POINTS
    }

    //Deletes time for sacrifices
    private void sacrifice() {
        hunger -= mealPoints;
        restartPoints();
        sacrificed = false;
    }

    private void addSacrifice() {
        mealPoints += personPoints + mealBonus;
        mealBonus += bonus; //update the bonus for next capture
        updateRedAndOrange();
        sacrificeAdded = false;
    }

    private void restartPoints()
    {
        mealPoints = 0;
        sacrifices = 0;
        mealBonus = 0;
        updateRedAndOrange();
    }

    private void updateRedAndOrange() {
        redbar.size = hunger * 0.01f;
        float orangeCalc = (hunger - mealPoints) * 0.01f;
        Debug.Log(orangeCalc);
        if (orangeCalc > 0)
            orangebar.size = orangeCalc;
        else
            orangebar.size = 0;
    }




    //TESTING!!!!!!!!!!!!!!!!
    public void testJail() { jailed = true; }
    public void testSacrificeAdded() { sacrificeAdded = true; }
    public void testSacrificed() { sacrificed = true; }









}
