using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_path : MonoBehaviour {
	
	public Transform target;
	public float initial_speed;
	public float camera_rot_speed;
	public float speed_up_factor;
	public Transform goal;
	public float orbit_speed;

	private bool did_col;

	// initialize values
	void Start () {
		did_col = false;
	}

	void Update() {
		if (transform.position != target.position) {
			// after collision, rotate and move toward target
			if (did_col) {
				transform.LookAt (goal);
				// camera rotate around z axis 
				transform.RotateAround (transform.position, Vector3.forward, 
					camera_rot_speed * Time.deltaTime);
		
				// camera rotate around goal object (guide to galaxy) around x and y axis
				transform.RotateAround (goal.position, Vector3.up, orbit_speed * Time.deltaTime);
				transform.RotateAround (goal.position, Vector3.right, orbit_speed * Time.deltaTime);

				// camera move towards goal
				// 15 is the best step I found. Increase for bigger step -> less time until center
				// decrease for smaller step -> more time until center
				transform.position = Vector3.MoveTowards (transform.position, goal.position,
					15f * Time.deltaTime);

				// Increase velocity for move by given factor to break through force of
				// gravity and move faster
				orbit_speed += speed_up_factor;

				// Increase angle so spins faster.
				camera_rot_speed += (speed_up_factor * 1.2f); 

				// could use this to balance out and not have speed increase faster and faster
			//	speed_up_factor *= 0.99f;
			} 
			// before collision, move towards target (Galaxy).
			else {
				transform.LookAt (goal);
				transform.position = Vector3.MoveTowards (transform.position, target.position, initial_speed * Time.deltaTime);
			}
		}
	}

	//check for collision between object and target.
	void OnCollisionEnter(Collision col) {
		//if collision, then turn off gravity and stop object from moving.
		if (col.gameObject.name == target.name) {
			did_col = true;
			col.gameObject.GetComponent<Rigidbody> ().useGravity = false;
			col.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		}
	}
}
