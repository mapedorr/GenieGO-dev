using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	// the amount of units to offset the link so it starts from the border of
	// the node
	public float borderWidth = 0.5f;
	// the scale in X for the link
	public float lineThickness = 0.5f;
	// the time it will take the link to animate to its target size
	public float scaleTime = 0.25f;
	// the time to wait before starting the animation
	public float iTweenDelay = 0.1f;
	// the ease type to use in the animation
	public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	// TODO: define properties
	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	// TODO: define privates

	// ══════════════════════════════════════════════════════════════ METHODS ════
	// makes the link grow from its starting position to its target position
	public void DrawLink (Vector3 startPos, Vector3 endPos)
	{
		// set the original size of the link
		transform.localScale = new Vector3 (lineThickness, 1f, 0f);
		// get the direction in which the link will point
		Vector3 dirVector = endPos - startPos;
		// get how long the link will be
		float zScale = dirVector.magnitude - borderWidth * 2f;
		// define the target size of the link
		Vector3 newScale = new Vector3 (lineThickness, 1f, zScale);
		// rotate the link in the direction of the target vector
		transform.rotation = Quaternion.LookRotation (dirVector);
		// set the starting position of the link
		transform.position = startPos + (transform.forward * borderWidth);
		// start the iTween that will make the line grow to toward its target
		iTween.ScaleTo (gameObject, iTween.Hash (
			"time", scaleTime,
			"scale", newScale,
			"easetype", easeType,
			"delay", iTweenDelay
		));
	}
}