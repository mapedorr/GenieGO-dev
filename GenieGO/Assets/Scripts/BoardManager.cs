using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	public static float spacing = 2f;
	public static readonly Vector2[] directions = {
		new Vector2 (spacing, 0f),
		new Vector2 (-spacing, 0f),
		new Vector2 (0f, spacing),
		new Vector2 (0f, -spacing)
	};
	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	// TODO: define properties
	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	// TODO: define privates
	// ══════════════════════════════════════════════════════════════ METHODS ════
	// TODO: define methods
}