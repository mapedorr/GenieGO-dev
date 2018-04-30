using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	// amount of time in seconds that will pass before moving to the nest stage
	public float stageDelay = 1f;
	// used to broadcast that the level is being set up
	public UnityEvent setupEvent;
	// used to broadcast that the player has clicked on Start
	public UnityEvent startLevelEvent;
	// used to broadcast that the gameplay should start
	//   - f.e. nodes are animated with this event
	public UnityEvent playLevelEvent;
	// TODO: add other events after having the code that makes the goal node to work

	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	// indicates if the level has already started (the player clicked the Start button)
	bool m_hasLevelStarted = true;
	public bool HasLevelStarted { get { return m_hasLevelStarted; } set { m_hasLevelStarted = value; } }
	// indicates if the player is playing (can move the PC and interact with the game world)
	bool m_isGamePlaying = false;
	public bool IsGamePlaying { get { return m_isGamePlaying; } set { m_isGamePlaying = value; } }
	// TODO: define properties for other events

	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	BoardManager m_board;
	bool m_isGameOver = false;

	// ══════════════════════════════════════════════════════════════ METHODS ════
	void Awake ()
	{
		m_board = Object.FindObjectOfType<BoardManager> ().GetComponent<BoardManager> ();
	}

	// Use this for initialization
	void Start ()
	{
		if (m_board != null)
		{
			StartCoroutine (RunGameLoop ());
		}
	}

	// routine that runs the core game loop: checks, per frame, in which stage
	// should be the player
	IEnumerator RunGameLoop ()
	{
		yield return StartCoroutine ("StartLevelRoutine");
		yield return StartCoroutine ("PlayLevelRoutine");
		// yield return StartCoroutine ("EndLevelRoutine");
	}

	// routine that waits until the player clicks the start button
	IEnumerator StartLevelRoutine ()
	{
		if (setupEvent != null)
		{
			setupEvent.Invoke ();
		}

		while (!m_hasLevelStarted)
		{
			// show start screen
			// user presses button to start
			// HasLevelStarted = true;
			yield return null;
		}

		if (startLevelEvent != null)
		{
			startLevelEvent.Invoke ();
		}
	}

	// routine that waits until any lose of winning condition is triggered
	// this is the function in charge of allowing the player to move the PC
	IEnumerator PlayLevelRoutine ()
	{
		m_isGamePlaying = true;
		yield return new WaitForSeconds (stageDelay);

		if (playLevelEvent != null)
		{
			playLevelEvent.Invoke ();
		}

		while (m_board.VisibleNodes != m_board.ActiveNodes)
		{
			yield return null;
		}

		while (!m_isGameOver)
		{
			yield return null;
			// TODO: check for Level Win condition
			// win
			// player reaches the end of the level
			// m_isGameOver = IsWinner ();

			// TODO: check for Level Lose condition
			// player dies
		}
	}
}