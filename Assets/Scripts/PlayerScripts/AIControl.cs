using UnityEngine;
using System.Collections;

public class AIControl : MonoBehaviour
{

	public PlayerControl playerControl;
	public float walkSpeed = 0.15f;
	public float runSpeed = 1.0f;
	public float sprintSpeed = 2.0f;
	public float flySpeed = 4.0f;

	public float turnSmoothing = 3.0f;
	public float speedDampTime = 0.1f;

	public float jumpHeight = 5.0f;
	public float jumpCooldown = 1.0f;

	// distance to player squared
	// if less than this AI will stop
  public float retreatThresshold = 20.0f;
	public float walkThresshold = 30.0f;
	public float runThresshold = 50.0f;

	private float timeToNextJump = 0;

	private float speed;

	private Vector3 lastDirection;
	private Transform playerTransform;

	private Animator anim;
	private int speedFloat;
	private int jumpBool;
	private int hFloat;
	private int vFloat;
	private int flyBool;
	private int groundedBool;

	private float h;
	private float v;

	private bool run;
	private bool sprint;

	private bool isMoving;

	// fly
	private bool fly = false;
	private float distToGround;
	private float sprintFactor;

	void Awake()
	{
		anim = GetComponent<Animator> ();
		playerTransform = playerControl.transform;

		speedFloat = Animator.StringToHash("Speed");
		jumpBool = Animator.StringToHash("Jump");
		hFloat = Animator.StringToHash("H");
		vFloat = Animator.StringToHash("V");
		// fly
		flyBool = Animator.StringToHash ("Fly");
		groundedBool = Animator.StringToHash("Grounded");
		distToGround = GetComponent<Collider>().bounds.extents.y;
		sprintFactor = sprintSpeed / runSpeed;
	}

	bool IsGrounded() {
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

	void Update()
	{
		float distance = distanceToPlayer ();
		if (distance >= walkThresshold) {
			v = 1.0f;
		} else if (distance <= retreatThresshold){
			v = -1.0f;
		} else {
			v = 0.0f;
		}

		fly = playerControl.fly;

		if (!fly && distance >= runThresshold) {
			run = true;
		} else {
			run = false;
		}

		isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1;
	}

	void FixedUpdate()
	{
		anim.SetFloat(hFloat, h);
		anim.SetFloat(vFloat, v);

		// Fly
		anim.SetBool (flyBool, fly);
		GetComponent<Rigidbody>().useGravity = !fly;
		anim.SetBool (groundedBool, IsGrounded ());
		if(fly)
			FlyManagement(h,v);

		else
		{
			MovementManagement (h, v, run, sprint);
			JumpManagement ();
		}
	}

	float distanceToPlayer()
	{
		Vector3 playerDirection = playerTransform.position - transform.position;
		return playerDirection.sqrMagnitude;
	}

	// fly
	void FlyManagement(float horizontal, float vertical)
	{
		Vector3 direction = Rotating(horizontal, vertical);
		GetComponent<Rigidbody>().AddForce(direction * flySpeed * 100 * (sprint?sprintFactor:1));
	}

	void JumpManagement()
	{
		if (GetComponent<Rigidbody>().velocity.y < 10) // already jumped
		{
			anim.SetBool (jumpBool, false);
			if(timeToNextJump > 0)
				timeToNextJump -= Time.deltaTime;
		}

		if (false && Input.GetButtonDown ("Jump")) // never jump for now
		{
			anim.SetBool(jumpBool, true);
			if(speed > 0 && timeToNextJump <= 0)
			{
				GetComponent<Rigidbody>().velocity = new Vector3(0, jumpHeight, 0);
				timeToNextJump = jumpCooldown;
			}
		}
	}

	void MovementManagement(float horizontal, float vertical, bool running, bool sprinting)
	{
		Rotating(horizontal, vertical);

		if(isMoving)
		{
			if(sprinting)
			{
				speed = sprintSpeed;
			}
			else if (running)
			{
				speed = runSpeed;
			}
			else
			{
				speed = walkSpeed;
			}

			anim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
		}
		else
		{
			speed = 0f;
			anim.SetFloat(speedFloat, 0f);
		}
		GetComponent<Rigidbody>().AddForce(Vector3.forward*speed);
	}

	Vector3 Rotating(float horizontal, float vertical)
	{
		Vector3 playerDirection = playerTransform.position - transform.position;

		if (!fly)
			playerDirection.y = 0.0f;
		playerDirection = playerDirection.normalized;

		Vector3 right = new Vector3(playerDirection.z, 0, -playerDirection.x);

		Vector3 targetDirection;

		float finalTurnSmoothing;


		targetDirection = playerDirection * vertical + right * horizontal;
		finalTurnSmoothing = turnSmoothing;


		if(isMoving && targetDirection != Vector3.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
			// fly
			if (fly)
				targetRotation *= Quaternion.Euler (90, 0, 0);

			Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, finalTurnSmoothing * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation (newRotation);
			lastDirection = targetDirection;
		}
		//idle - fly or grounded
		if(!(Mathf.Abs(h) > 0.9 || Mathf.Abs(v) > 0.9))
		{
			Repositioning();
		}

		return targetDirection;
	}

	private void Repositioning()
	{
		Vector3 repositioning = lastDirection;
		if(repositioning != Vector3.zero)
		{
			repositioning.y = 0;
			Quaternion targetRotation = Quaternion.LookRotation (repositioning, Vector3.up);
			Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation (newRotation);
		}
	}

	public bool IsFlying()
	{
		return fly;
	}

	public bool isSprinting()
	{
		return sprint && (isMoving);
	}
}
