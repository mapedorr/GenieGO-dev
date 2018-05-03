using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	// the amount of units between nodes. this will also define the amount of
	// units PCs and NPCs can move in any direction
	public static float spacing = 2f;
	// the available directions from each node. this will be used to determine
	// the connection between nodes, movement behaviour, NPCs views
	public static readonly Vector2[] directions = {
		new Vector2 (spacing, 0f),
		new Vector2 (-spacing, 0f),
		new Vector2 (0f, spacing),
		new Vector2 (0f, -spacing)
	};
	// the GameObject that holds the game world
	public GameObject world;
	// TODO: move this properties to a WorldManager or World class ──────────────┐
	// what easetype to use for iTweening
	public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
	// time to rotate to face destination
	public float rotateTime = 0.5f;
	// delay to use before any call to iTween
	public float iTweenDelay = 0f;
	// ──────────────────────────────────────────────────────────────────────────┘

	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	// list of all the nodes available in the game board
	List<Node> m_allNodes;
	public List<Node> AllNodes { get { return m_allNodes; } }
	// list of all the nodes that are visible after their animations has finished
	int m_visibleNodes = 0;
	public int VisibleNodes { get { return m_visibleNodes; } set { m_visibleNodes = value; } }
	// list of all the nodes that can be used by PC and NPCs
	int m_activeNodes = 0;
	public int ActiveNodes { get { return m_activeNodes; } set { m_activeNodes = value; } }

	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	Vector3 m_worldDestination;

	// ══════════════════════════════════════════════════════════════ METHODS ════
	/// <summary>
	/// Called when the script instance is being loaded.
	/// </summary>
	void Awake ()
	{
		// assure the spacing is always the same
		spacing = 1f;
		// get all the nodes in the current level
		FillNodeList ();
	}

	/// <summary>
	/// Get all the nodes in the scene and stores them in a list
	/// </summary>
	public void FillNodeList ()
	{
		Node[] nList = GameObject.FindObjectsOfType<Node> ();
		m_allNodes = new List<Node> (nList);
	}

	/// <summary>
	/// Looks for the node in a certain coordinate of the nodes' space
	/// </summary>
	/// <param name="pos">The position to look at</param>
	/// <returns></returns>
	public Node FindNodeAt (Vector3 pos)
	{
		Vector2 boardCoord = Utility.Vector2Round (new Vector2 (pos.x, pos.z));
		return m_allNodes.Find (n => n.Coordinate == boardCoord);
	}

	public void InitBoard ()
	{
		m_visibleNodes = 0;
		m_activeNodes = 0;

		// TODO: start the nodes drawing from the one occuped by the PC
		// if (m_playerNode != null)
		// {
		// 	m_playerNode.InitNode ();
		// }

		// TODO: remove this temporal behavior ────────────────────────────────┐
		// look for the node marked as autorun
		if (m_allNodes.Count > 0)
		{
			foreach (Node node in m_allNodes)
			{
				if (node.isFirstNode)
				{
					node.InitNode ();
				}
			}
		}
		// └───────────────────────────────────────────────────────────────────┘
	}

	public void RemoveBoard ()
	{
		foreach (Node node in m_allNodes)
		{
			if (node.isActive)
			{
				node.HideMesh ();
			}
		}

		// TODO: trigger animations on each node and its links
		Link[] allLinks = Object.FindObjectsOfType<Link> ();
		foreach (Link link in allLinks)
		{
			link.GetComponent<Link> ().DeleteLink ();
		}
	}

	/// <summary>
	/// Rotates the world by 90º based on a given direction
	/// </summary>
	/// <param name="direction">-1: left, 0: nothing, 1: right</param>
	public void RotateWorld (float direction)
	{
		// prevent execution if there's no rotation direction
		if (direction == 0f || world == null)
		{
			return;
		}

		// hide all the nodes and the current links between them
		RemoveBoard ();

		// rotate the world by 90 degrees
		StartCoroutine (RotateWorldRoutine (90f * direction));
	}

	IEnumerator RotateWorldRoutine (float destination)
	{
		// TODO: improve this so the wait time is related with the dissapearance of all
		// the nodes and links in the board
		yield return new WaitForSeconds (1f);

		iTween.RotateAdd (world, iTween.Hash (
			"y", destination,
			"delay", 0f,
			"easetype", easeType,
			"time", rotateTime
		));

		yield return new WaitForSeconds (rotateTime + 0.3f);

		foreach (Node node in m_allNodes)
		{
			node.SetNodeDefaults ();
		}

		InitBoard ();

		yield return null;
	}
}