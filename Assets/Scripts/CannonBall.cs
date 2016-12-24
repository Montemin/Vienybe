using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {

	private float explosiveForce = 20.0f;
	private float explosionRadius = 30.0f;

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

				rigid.AddExplosionForce (
					explosiveForce, 
					transform.position,
					explosionRadius,
					0.2f,
					ForceMode.Impulse
				);
			}
			Destroy (gameObject);
		};
	}
}
