using UnityEngine;
using System.Collections;

public class detection : MonoBehaviour {

	public bool canMove;
	public bool playerDetected;
	// Use this for initialization
	void Start () {
		canMove = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other){

		canMove = false;
		if (other.CompareTag("People")){
			canMove = false;
		} else if (other.CompareTag("Player")){
			playerDetected = true;
		}

	}

	void OnTriggerExit2D (Collider2D other){
		canMove = true;
		if (other.CompareTag("Player")){
			playerDetected = false;
		}
	}


}
