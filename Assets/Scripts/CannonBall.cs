using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {

	private float explosiveForce = 2.0f;
	private float explosionRadius = 4.0f;
	public Transform ragdoll;
	public ThirdPersonOrbitCam mainCamera;

	void Awake()
	{
		mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<ThirdPersonOrbitCam>();
	}

	void OnCollisionEnter (Collision other)
	{
		if(other.gameObject.tag == "AI" || isRagdoll(other.gameObject))
		{
			mainCamera.ShakeIt ();

			Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius);
			foreach (Collider c in colliders) 
			{
				Rigidbody rigid = c.GetComponent<Rigidbody> ();
				if (rigid == null)
					continue;

				if (c.gameObject.tag == "AI") {
					Transform doll = (Transform)Instantiate (ragdoll, rigid.transform.position, Quaternion.identity);
					affectByExplosion(doll.gameObject);
					Destroy (c.gameObject);
				} else if (isRagdoll(c.gameObject)) {
					affectByExplosion (c.gameObject.transform.parent.gameObject);
				}
			}
		};
	}

	void affectByExplosion(GameObject gameObject)
	{
		Rigidbody[] rigidParts = gameObject.gameObject.GetComponentsInChildren<Rigidbody> ();
		foreach (Rigidbody part in rigidParts) {
			part.AddExplosionForce (
				explosiveForce, 
				transform.position,
				explosionRadius,
				0.1f,
				ForceMode.Impulse
			);
		}
	}

	bool isRagdoll(GameObject potentiallyDoll){
		bool parentRagdoll = potentiallyDoll.gameObject.transform.parent && potentiallyDoll.gameObject.transform.parent.tag == "Ragdoll";
		return potentiallyDoll.tag == "Ragdoll" || parentRagdoll;
	}
}
