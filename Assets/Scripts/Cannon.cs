using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	private bool fire;
	public float ballStartX = 0.0f;
	public float ballStartY = 1.4f;
	public float ballStartZ = 2.0f;
	public float thrust = 30.0f;
	public float cooldown = 0.5f;
	public Transform cannonBall;
	private float lastFiredAt;

	// Use this for initialization
	void Start () {
		lastFiredAt = Time.time - cooldown;
	}
	
	// Update is called once per frame
	void Update () {
		fire = Input.GetButton("Fire1");
		if (fire &&  (lastFiredAt + cooldown) <= Time.time) {
			lastFiredAt = Time.time;
			Vector3 startPosition = new Vector3 (transform.position.x + ballStartX, transform.position.y + ballStartY, transform.position.z + ballStartZ);
			Transform ballObject = (Transform) Instantiate(cannonBall, startPosition, Quaternion.identity);
			ballObject.GetComponent<Rigidbody> ().AddForce(ballObject.forward * thrust, ForceMode.VelocityChange);
		}
	}
}
