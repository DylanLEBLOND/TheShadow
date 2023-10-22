using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public Player mainPlayer;

	private LevelOver endGame;

	void Start()
	{
		endGame = this.transform.GetChild(0).GetComponent<LevelOver>();
	}

	void Update()
	{
		if (! mainPlayer.isAlive)
		{
			Debug.Log ("GAME OVER");
			SceneManager.LoadScene ("ProtoLevel");
		}
		if (endGame.isOver)
		{
			Debug.Log ("LEVEL COMPLETED");
			SceneManager.LoadScene ("ProtoLevel");
		}
	}
}
