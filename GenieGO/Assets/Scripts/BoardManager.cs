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
	// TODO: define privates

	// ══════════════════════════════════════════════════════════════ METHODS ════
	void Awake ()
	{
		// assure the spacing is always the same
		spacing = 1f;
		// get all the nodes in the current level
		FillNodeList ();
	}

	// method that get all the nodes in the scene and stores them in a list
	public void FillNodeList ()
	{
		Node[] nList = GameObject.FindObjectsOfType<Node> ();
		m_allNodes = new List<Node> (nList);
	}

	// method that looks for the node in a certain coordinate of the nodes' space
	public Node FindNodeAt (Vector3 pos)
	{
		Vector2 boardCoord = Utility.Vector2Round (new Vector2 (pos.x, pos.z));
		return m_allNodes.Find (n => n.Coordinate == boardCoord);
	}

	public void InitBoard ()
	{
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
}