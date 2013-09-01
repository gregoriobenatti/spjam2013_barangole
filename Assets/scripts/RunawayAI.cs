using UnityEngine;
using System.Collections;

public class RunawayAI : MonoBehaviour {

	public GameObject player;
	
	public float minDistanceToPlayer = 5f;
	
	public float force = 500f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 diff, direction, direction2, direction3, direction4;	
		RaycastHit hit = new RaycastHit();
		RaycastHit hit2 = new RaycastHit();
		
		direction = Vector3.zero;
		
		if(Vector3.Distance(transform.position, player.transform.position) < minDistanceToPlayer) {
		
			diff = transform.position - player.transform.position;
			
			diff.Normalize();
			
			direction = diff;
			
			direction.y = 0;
			
			direction.Normalize();
			
			Debug.DrawLine(transform.position, direction * 100, Color.red);
			
			rigidbody.AddForce(direction * force * Time.deltaTime);
			
		}
		
		
		
		if (Physics.Raycast (transform.position, direction, out hit, 0.1f)) {
			direction2 = Vector3.Reflect(direction, hit.normal);
			direction2.y = 0;
			direction2.Normalize();
			
			direction3 = Vector3.Cross(new Vector3(0,1,0), hit.normal);
			direction3.y = 0;
			
			direction3.Normalize();
			
			direction4 = Vector3.Cross(new Vector3(0,1,0), -1 * hit.normal);
			direction4.y = 0;
			
			direction4.Normalize();
			
			if (Physics.Raycast (transform.position, direction3, out hit2, 0.1f)) {
				direction3 = direction4;
			} 
			
			if(Vector3.Distance(transform.position + direction2 * 2, player.transform.position) >
				Vector3.Distance(transform.position + direction3 * 2, player.transform.position)) {
				direction = direction2;
				Debug.DrawLine(transform.position, direction2 * 100, Color.green);
			} else {
				direction = direction3;
				Debug.DrawLine(transform.position, direction3 * 100, Color.yellow);
			}
			
			rigidbody.AddForce(direction * force * Time.deltaTime);
		}
	}
	
	/*void OnCollisionEnter(Collision hit) {
		
		
		if(hit.collider.tag == "Wall" && Vector3.Distance(transform.position, player.transform.position) < minDistanceToPlayer) {
		
			Vector3 direction2;
			
			direction2 = Vector3.Reflect(direction,  hit.contacts[0].normal);
			
			direction2.y = 0;
			
			direction2.Normalize();
			
			rigidbody.AddForce(direction2 * force * Time.deltaTime);
			
			bounceTime = 2;
		}
	}*/
	
	
	
	
}
