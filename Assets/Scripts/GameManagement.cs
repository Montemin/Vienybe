using UnityEngine;
using System.Collections;

public class GameManagement : MonoBehaviour {

	public Transform aiCharacter;
	public int numberOfFollowers = 50;
	public int startingX = -36;

	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		for (int x = 0; x < numberOfFollowers / 2; x++) {
			Instantiate(aiCharacter, new Vector3(startingX + x, 10, 30), Quaternion.identity);
			Instantiate(aiCharacter, new Vector3(startingX + x, 10, 32), Quaternion.identity);
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		if (Input.GetKeyDown("escape"))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = true;
		}
	}
}
