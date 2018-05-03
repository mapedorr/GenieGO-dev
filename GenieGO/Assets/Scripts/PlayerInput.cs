using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	// TODO: define publics
	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	// stores the input value in the horizontal axis
	private float m_horizontal;
	public float Horizontal { get { return m_horizontal; } }

	// stores the input value in the vertical axis
	private float m_vertical;
	public float Vertical { get { return m_vertical; } }

	// stores the direction in which the world should rotate (left: -1, right: 1)
	private float m_rotate;
	public float Rotate { get { return m_rotate; } }

	// defines if the input from the player will be processed
	private bool m_inputEnabled;
	public bool InputEnabled { get { return m_inputEnabled; } set { m_inputEnabled = value; } }

	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	// TODO: define prrivates
	// ══════════════════════════════════════════════════════════════ METHODS ════
	/// <summary>
	/// Get inputs from the keyboard
	/// </summary>
	public void GetInput ()
	{
		if (!m_inputEnabled)
		{
			m_horizontal = 0f;
			m_vertical = 0f;
			m_rotate = 0f;
			return;
		}

		m_horizontal = Input.GetAxisRaw ("Horizontal");
		m_vertical = Input.GetAxisRaw ("Vertical");

		if (Input.GetKeyDown (KeyCode.Q))
		{
			m_rotate = -1f;
		}
		else if (Input.GetKeyDown (KeyCode.E))
		{
			m_rotate = 1f;
		}
		else
		{
			m_rotate = 0f;
		}
	}
}