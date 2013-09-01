using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
public class AstarAI : MonoBehaviour {
    //The point to move to
    public Vector3 targetPosition;
	public Transform objToFind;
    
    private Seeker seeker;
    //private CharacterController controller;
	private Rigidbody sphere_enemy;
 
    //The calculated path
    public Path path;
    
    //The AI's speed per second
    public float speed = 10;
	
	public int MULTIPLY = 2;
    
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
 
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
	
	private Vector3 lastPlayerPos;
	
	private float delta = 0.01f;
	
	public int mode = 0; //0 == following, 1 == running away
	
	public float minDistanceFromPlayer = 5f;
	
	public void Start () {		
		lastPlayerPos = objToFind.position;
		updatePathfinding();
    }
	
	public void Update() {		
		
		if(mode == 0) {
			if(Vector3.Distance(lastPlayerPos, objToFind.position) > delta) {
				updatePathfinding();
			}
			lastPlayerPos = objToFind.position;
		} else {
			RunAway();
		}
		
		Debug.Log(mode);
		
	}
	
	public void updatePathfinding(){		
		targetPosition = objToFind.position;
		
		seeker = GetComponent<Seeker>();
        //controller = GetComponent<CharacterController>();
		sphere_enemy = GetComponent<Rigidbody>();
        
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        seeker.StartPath (transform.position,targetPosition, OnPathComplete);
	}
    	
    public void OnPathComplete (Path p) {
		//Debug.Log ("Yey, we got a path back. Did it have an error? " + p.error);
        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
    }
 
    public void FixedUpdate () {
       if(mode == 0) {
			if (path == null) {
	            //We have no path to move after yet
	            return;
	        }
	        
	        if (currentWaypoint >= path.vectorPath.Count) {
	            //Debug.Log ("End Of Path Reached");
	            return;
	        }
	        
	        //Direction to the next waypoint
	        Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
	        dir *= (speed * Time.fixedDeltaTime) * MULTIPLY;
	        //controller.SimpleMove (dir);
			sphere_enemy.AddForce(dir/2);
	        
	        //Check if we are close enough to the next waypoint
	        //If we are, proceed to follow the next waypoint
	        if (Vector3.Distance (transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
	            currentWaypoint++;
	            return;
	        }
		}
    }
	
	void RunAway () {
		Vector3 diff, direction, direction2, direction3, direction4;	
		RaycastHit hit = new RaycastHit();
		RaycastHit hit2 = new RaycastHit();
		
		direction = Vector3.zero;
		
		if(Vector3.Distance(transform.position, objToFind.position) < minDistanceFromPlayer) {
		
			diff = transform.position - objToFind.position;
			
			diff.Normalize();
			
			direction = diff;
			
			direction.y = 0;
			
			direction.Normalize();
			
			Debug.DrawLine(transform.position, direction * 100, Color.red);
			
			rigidbody.AddForce(direction * speed * Time.deltaTime);
			
		}
		
		
		
		if (Physics.Raycast (transform.position, direction, out hit, 1f)) {
			direction2 = Vector3.Reflect(direction, hit.normal);
			direction2.y = 0;
			direction2.Normalize();
			
			direction3 = Vector3.Cross(new Vector3(0,1,0), hit.normal);
			direction3.y = 0;
			
			direction3.Normalize();
			
			direction4 = Vector3.Cross(new Vector3(0,1,0), -1 * hit.normal);
			direction4.y = 0;
			
			direction4.Normalize();
			
			if (Physics.Raycast (transform.position, direction3, out hit2, 1f)) {
				direction3 = direction4;
			} 
			
			if(Vector3.Distance(transform.position + direction2 * 2, objToFind.position) >
				Vector3.Distance(transform.position + direction3 * 2, objToFind.position)) {
				direction = direction2;
				Debug.DrawLine(transform.position, direction2 * 100, Color.green);
			} else {
				direction = direction3;
				Debug.DrawLine(transform.position, direction3 * 100, Color.yellow);
			}
			
			rigidbody.AddForce(direction * speed * Time.deltaTime);
		}
	}
} 