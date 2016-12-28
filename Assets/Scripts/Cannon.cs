using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	private bool fire;
	public float thrust = 30.0f;
	public float cooldown = 0.5f;
	public Transform cannonBall;
	private float lastFiredAt;
	private Transform cannonBallSpawn;

	// Use this for initialization
	void Start () {
		lastFiredAt = Time.time - cooldown;

		foreach(Transform child in transform){
			if (child.tag == "CannonBallSpawn") {
				cannonBallSpawn = child;
				break;
			}
		}
	}
	

	void Update () {
		fire = Input.GetButton("Fire1");
		if (fire &&  (lastFiredAt + cooldown) <= Time.time) {
			lastFiredAt = Time.time;
			Transform ballObject = (Transform) Instantiate(cannonBall, cannonBallSpawn.position, Quaternion.identity);
			ballObject.GetComponent<Rigidbody> ().AddForce(cannonBallSpawn.forward * thrust, ForceMode.VelocityChange);
		}
	}
}
