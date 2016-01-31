using UnityEngine;
using System.Collections;

public class manageScene : MonoBehaviour {

	private Vector3 westDoor = new Vector3(0 + 1,12/2 );
	private Vector3 northDoor = new Vector3(31/2, 12 -1 );
	private Vector3 eastDoor = new Vector3(31 -1, 12/2 );
	private Vector3 southDoor = new Vector3(31/2, 0 + 1 );
	
	public GameObject copMan;
	public GameObject people;
	public float waitingTime;
	public bool alert;

	private float nextGenerate;

	// Use this for initialization
	void Start () {
		alert = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextGenerate) {
			StartCoroutine ("Generate");
			nextGenerate = Time.time + waitingTime;
		}
	
	}

	void FixedUpdate(){
		if (alert) {
			initCops ();
			Debug.Log ("OMG!!!");
		}
	}

	public void setAlert(){
		alert = true;
	}

	public void initCops(){
		Instantiate (copMan, westDoor, Quaternion.identity);
		Instantiate (copMan, eastDoor, Quaternion.identity);
		Instantiate (copMan, southDoor, Quaternion.identity);
		Instantiate (copMan, northDoor, Quaternion.identity);
		alert = false;
	}

	IEnumerator Generate() {
		int index = Random.Range (0,4);
		if (index == 0) {
			Instantiate (people, westDoor, Quaternion.identity);
		} else if (index == 1) {
			Instantiate (people, eastDoor, Quaternion.identity);
		} else if (index == 2) {
			Instantiate (people, southDoor, Quaternion.identity);
		} else if (index == 3) {
			Instantiate (people, northDoor, Quaternion.identity);
		}
		yield return null;
	}
}
