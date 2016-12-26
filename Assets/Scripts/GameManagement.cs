using UnityEngine;
using System.Collections;

public class GameManagement : MonoBehaviour {

	public Transform aiCharacter;
	public int numberOfFollowers = 50;
	public int startingX = -36;

	void Start () {
		for (int x = 0; x < numberOfFollowers / 2; x++) {
			Instantiate(aiCharacter, new Vector3(startingX + x, 10, 30), Quaternion.identity);
			Instantiate(aiCharacter, new Vector3(startingX + x, 10, 32), Quaternion.identity);
		}
	}
}
