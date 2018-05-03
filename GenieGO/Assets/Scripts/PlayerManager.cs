using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerInput))]
public class PlayerManager : MonoBehaviour
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	public PlayerInput playerInput;

	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	//TODO: define properties

	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	BoardManager m_board;

	// ══════════════════════════════════════════════════════════════ METHODS ════
	/// <summary>
	/// Called when the script instance is being loaded.
	/// </summary>
	void Awake ()
	{
		// get the reference to the required components
		playerInput = GetComponent<PlayerInput> ();

		// get the reference to the board
		m_board = Object.FindObjectOfType<BoardManager> ().GetComponent<BoardManager> ();
	}

	/// <summary>
	/// Called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start ()
	{
		// TODO: remove this, the GameManager will handle it
		if (playerInput != null)
		{
			playerInput.InputEnabled = true;
		}
	}

	/// <summary>
	/// Called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update ()
	{
		// TODO: check for conditions to prevent checking for player input and other things
		// - prevent when it's not the player's turn
		// - prevent when the player is moving or the board is rotating

		// check if the player made an input
		playerInput.GetInput ();

		if (playerInput.Rotate != 0f)
		{
			if (m_board != null)
			{
				m_board.RotateWorld (playerInput.Rotate);
			}
		}
	}
}