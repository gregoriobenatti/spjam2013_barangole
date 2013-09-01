using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour {

	private Vector3 zeroAc;
	
	private Vector3 curAc;
	
	private Vector3 curAcWithoutZ;
	
	private float GetAxisH = 0;
	
	private float GetAxisV = 0;
	
	public float sensH = 10;
	
	public float sensV = 10;
	
	public float filter = 5f;
	
	public float force = 500f;
	
	public float delta = 0.2f;
	
	public GameObject bebado;
	
	void ResetAxes(){
	    zeroAc = Input.acceleration;
	    curAc = Vector3.zero;
	}
	
	// Use this for initialization
	void Start () {
		ResetAxes();
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update () {
		
		curAcWithoutZ = Input.acceleration;
		
		curAcWithoutZ.z = 0;
		
		if((Vector3.Distance(curAcWithoutZ, zeroAc) > delta)) {
			
			curAc = Vector3.Lerp(curAc, Input.acceleration-zeroAc, Time.deltaTime * filter);
		    GetAxisV = Mathf.Clamp(curAc.y * sensV, -1, 1);
		    GetAxisH = Mathf.Clamp(curAc.x * sensH, -1, 1);
			
			Vector3 dir = new Vector3(GetAxisH, 0, GetAxisV);
		    // limit dir vector to magnitude 1:
		    dir.Normalize();
		    // move the object at the velocity defined in speed:
		 	rigidbody.AddForce(dir * force * Time.deltaTime);
		}
	}
	
	void OnCollisionEnter(Collision hit) {
		if(hit.collider.tag == "hidrante") {
			bebado.GetComponent<AstarAI>().mode = 1;
		}
	}
}
