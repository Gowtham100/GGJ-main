using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	//create a separate class to make the code useable
	public float xMin, xMax, yMin, yMax;
}

public class AIMechanics : MonoBehaviour {

	private Vector3[] directions = {new Vector3(0.3f,0,0), new Vector3(-0.3f,0,0), new Vector3(0,0.3f,0), new Vector3(0,-0.3f,0)};
	private string[] directionNames = {"East", "West", "North", "South"};
	public Vector3 curDirection;
	public string curDirectionName;

	public float speed;
	public float dirSpeed;
	public float pauseLength; //how long
	public float timeBetweenPause; //how often

	private float nextDir;
	private float nextMovement;
	private float nextPause;
	private bool isPause;

	public Boundary boundary;

	// Use this for initialization
	void Start () {
		curDirection = new Vector3(0.3f,0,0);
		curDirectionName = "East";
		isPause = false;
		disableChildRenderer ();
		this.gameObject.transform.GetChild (convert ("East")).gameObject.GetComponent<SpriteRenderer>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextMovement && !isPause) {
			Move();
			nextMovement = Time.time + speed;
		}

		if (!canMove (convert (curDirectionName))) {
			changeDir ();
		}

		if (isAtBranch()) {	
			if(Time.time > nextDir){
				changeDir();
				nextDir = Time.time + dirSpeed;
			}
		}

		if (Time.time > nextPause) {
			if(canPause())
				pause();
			nextPause = Time.time + timeBetweenPause;
		}

		checkPlayer ();

	}

	public void Move(){
		transform.position += curDirection;

		transform.position = new Vector3 
			(
				Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax), 
				Mathf.Clamp(transform.position.y, boundary.yMin, boundary.yMax),
				-0.368f
			);
	}

	public bool canMove(int index){
		return this.gameObject.transform.GetChild (index).gameObject.GetComponent<detection> ().canMove;
	}

	public bool playerDetected(int index){
		return this.gameObject.transform.GetChild (index).gameObject.GetComponent<detection> ().playerDetected;
	}
	
	public int convert (string dir){
		if (dir.Equals ("East")) {
			return 0;
		} else if (dir.Equals ("West")) {
			return 1;
		} else if (dir.Equals ("North")) {
			return 2;
		} else if (dir.Equals ("South")) {
			return 3;
		} else {
			return 0;
		}
		
	}

	public void changeDir(){

		this.gameObject.transform.GetChild (convert (curDirectionName)).gameObject.GetComponent<SpriteRenderer>().enabled = false;
		int index = generateIndex ();

		if (canMove(index)) {
			curDirection = directions[index];
			curDirectionName = directionNames [index];
			this.gameObject.transform.GetChild (index).gameObject.GetComponent<SpriteRenderer>().enabled = true;
		}

	}

	public int generateIndex(){
		int index = Random.Range (0,4);
		if (curDirectionName.Equals ("East")) {
			while (index == 1) // don't go west
				index = Random.Range (0,4);
		} else if (curDirectionName.Equals ("West")) {
			index = Random.Range (1,4);
		} else if (curDirectionName.Equals ("North")) {
			index = Random.Range (1,3);
		} else if (curDirectionName.Equals ("South")) {
			while (index == 2) // don't go north
				index = Random.Range (0,4);
		}
		return index;
		
	}

	public bool isAtBranch ()
	{
		if (curDirectionName.Equals ("East")) {
			return (canMove (2) || canMove (3));
		} else if (curDirectionName.Equals ("West")) {
			return (canMove (2) || canMove (3));
		} else if (curDirectionName.Equals ("North")) {
			return (canMove (0) || canMove (1));
		} else if (curDirectionName.Equals ("South")) {
			return (canMove (0) || canMove (1));
		}

		return false;
	}	



	bool canPause ()
	{
		int index = Random.Range (0,3);
		if (index == 0) {
			return true; //25% of the time
		} else {
			return false;
		}
	}

	public void pause(){
		isPause = true;
		Invoke ("print", pauseLength);
	}

	private void print(){
		isPause = false;
	}

	public void checkPlayer(){
		if (playerDetected(convert (curDirectionName))){
			//Debug.Log ("FOUND YOUU!!");
		}
	}


	void disableChildRenderer ()
	{
		for (int i = 0; i < 4; i ++){
			this.gameObject.transform.GetChild (i).gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}	