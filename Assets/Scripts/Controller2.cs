using UnityEngine;
using System.Collections;

//[System.Serializable]
//public class Boundary {
//	//create a separate class to make the code useable
//	public float xMin, xMax, yMin, yMax;
//}

public class Controller2 : MonoBehaviour {

	public float maxSpeed = 10f;

	public Boundary boundary;

	public int MYAWESOMESCORE = 0;
	
	//	public bool facingRight = true;
//	
//	Animator anim;
	
	// Use this for initialization
	void Start () {
		
		//anim = GetComponent<Animator>();
		
	}

	void Update(){
	}

	// Update is called once per frame
	void FixedUpdate () {

			move ();


	}

	void move()
	{


		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

//		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)){
//			moveVertical = 0f;
//		} else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)){
//			moveHorizontal = 0f;
//		}

		transform.position += new Vector3 (moveHorizontal, moveVertical, 0).normalized * Time.deltaTime * maxSpeed;
		
		GetComponent<Rigidbody2D>().position = new Vector3 
			(
				Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax), 
				Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.yMin, boundary.yMax),
				0.0f
				);

//		if (moveHorizontal  > 0 && ! facingRight)
//			Flip ();
//		else if (moveHorizontal < 0 && facingRight)
//			Flip ();
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "People") {
			Destroy(coll.gameObject);
			MYAWESOMESCORE += 1;
		}
	}






//	void Flip()
//	{
//		facingRight = ! facingRight;
//		Vector3 theScale = transform.localScale;
//		theScale.x *= -1;
//		transform.localScale = theScale;
//	}
}

