using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]

public class EnemyAI : MonoBehaviour
{
	
	public GameObject target;
	
	// How many times per second to update path
	public float updateRate = 2f;
	
	private Seeker seeker;
	private Rigidbody2D rb;
	
	//Calculated Path
	public Path path;
	
	//AI speed per second
	public float speed = 300f;
	public ForceMode2D fMode;
	
	[HideInInspector]
	public bool pathIsEnded = false;
	
	//Detection range
	public float nextWayPointDistance = 3;
	
	// the way point currently moving towards
	private int currentWaypoint = 0;
	
	void Start()
	{
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		
		target = GameObject.FindGameObjectWithTag("Player");
		
		if (target == null)
		{
			Debug.Log("No aztic found");
			return;
		}
		
		//Start a new path to aztec position, and return the result to the OnPathComplete method
		seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
		
		StartCoroutine(UpdatePath());
		
	}
	
	IEnumerator UpdatePath()
	{
		if (target == null)
		{
			yield return false;
		}
		
		//Start a new path to aztec position, and return the result to the OnPathComplete method
		seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
		
		yield return new WaitForSeconds(1f / updateRate);
		StartCoroutine(UpdatePath());
	}
	
	public void OnPathComplete(Path p)
	{
		Debug.Log("We got a pth, did it have an error?" + p.error);
		if (!p.error)
		{
			path = p;
			currentWaypoint = 0;
			
		}
	}
	
	void FixedUpdate()
	{
		if (target == null)
		{
			return;
		}
		
		//TODO: Always look at player
		
		if (path == null)
		{
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Count)
		{
			if (pathIsEnded)
				return;
			
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;
		
		//direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;
		
		//Move the cop
		rb.AddForce(dir, fMode);
		
		float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
		
		if (dist < nextWayPointDistance)
		{
			currentWaypoint++;
			return;
			
		}
		
	}
	
	
}