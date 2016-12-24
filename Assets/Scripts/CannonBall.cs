using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {

	private float explosiveForce = 20.0f;
	private float explosionRadius = 4.0f;
	public Transform ragdoll;

	void OnCollisionEnter (Collision other)
	{
		if(other.gameObject.tag == "AI")
		{
			Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius);
			foreach (Collider c in colliders) 
			{
				Rigidbody rigid = c.GetComponent<Rigidbody> ();
				if (rigid == null)
					continue;

				if (c.gameObject.tag == "AI") {
					Instantiate(ragdoll, rigid.transform.position, Quaternion.identity);
					Destroy (c.gameObject);

					// below doesn't work
					Rigidbody[] rigidDollparts = c.gameObject.GetComponents<Rigidbody> ();
					Debug.Log (rigidDollparts.Length);
					foreach (Rigidbody part in rigidDollparts) {
						part.AddExplosionForce (
							explosiveForce, 
							transform.position,
							explosionRadius,
							0.2f,
							ForceMode.Impulse
						);
					}
				}

			}
		};
	}
}
