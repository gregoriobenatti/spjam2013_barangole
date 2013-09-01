using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public GameObject sphere;
	
	public float distanceY = 4;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(sphere.transform.position.x, sphere.transform.position.y + distanceY, sphere.transform.position.z);
	}
}
