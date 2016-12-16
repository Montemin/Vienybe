using UnityEngine;
using System.Collections;

public class GameManagement : MonoBehaviour {

	public Transform aiCharacter;
	public int numberOfFollowers = 50;
	public int startingX = -36;
	// Use this for initialization
	void Start () {
		for (int x = 0; x < numberOfFollowers; x++) {
			Instantiate(aiCharacter, new Vector3(startingX + x, 10, 30), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
