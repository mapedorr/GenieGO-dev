using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	// the time it will take the node to animate to the target scale
	public float scaleTime = 0.3f;
	// the ease type to use in the animation
	public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
	// the time to wait before starting the animation
	public float delay = 1f;
	// the GameObject with the mesh renderer for the node
	public GameObject mesh;
	// the prefab that will serve for linking the node to its neighbors
	public GameObject linkPrefab;
	// the layer in which obstacles will be placed
	public LayerMask obstacleLayer;
	// the layer in which inactive ground will be placed
	public LayerMask inactiveGroundLayer;
	// TODO: remove this property
	public bool isFirstNode = false;
	// determines if the node should be taken into account as part of the nodes' space
	public bool isActive = true;

	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	// the 2D coordinate of the node in the nodes' space
	Vector2 m_coordinate;
	public Vector2 Coordinate { get { return Utility.Vector2Round (m_coordinate); } }
	// a list of all the nodes that are neighbors of this one
	private List<Node> m_neighborNodes = new List<Node> ();
	public List<Node> NeighborNodes { get { return m_neighborNodes; } }
	// a list of all the nodes that are actually linked to this one
	private List<Node> m_linkedNodes = new List<Node> ();
	public List<Node> LinkedNodes { get { return m_linkedNodes; } }

	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	// a reference to the class that manages the game board
	BoardManager m_board;
	// indicates if the node was already initialized (started its animation)
	bool m_initialized = false;

	// ══════════════════════════════════════════════════════════════ METHODS ════
	/// <summary>
	/// Called when the script instance is being loaded.
	/// </summary>
	void Awake ()
	{
		// get the reference to the BoardManager component
		m_board = Object.FindObjectOfType<BoardManager> ().GetComponent<BoardManager> ();
		// set the coordinate of the node in nodes' space
		m_coordinate = new Vector2 (transform.position.x, transform.position.z);
	}

	void Start ()
	{
		SetNodeDefaults ();
	}

	public void SetNodeDefaults ()
	{
		// set the node to its default state
		isActive = true;
		m_neighborNodes.Clear ();
		m_linkedNodes.Clear ();
		m_initialized = false;

		if (mesh != null)
		{
			mesh.transform.localScale = Vector3.zero;

			// check if beneath the node is there an inactive ground
			Vector3 checkDirection = transform.position + Vector3.down;
			RaycastHit raycastHit;

			if (!Physics.Raycast (transform.position, checkDirection, out raycastHit,
					BoardManager.spacing + 0.1f, inactiveGroundLayer))
			{
				if (m_board != null)
				{
					m_neighborNodes = FindNeighbors (m_board.AllNodes);
				}
			}
			else
			{
				isActive = false;
			}
		}
	}

	public List<Node> FindNeighbors (List<Node> nodes)
	{
		List<Node> nList = new List<Node> ();
		foreach (Vector2 dir in BoardManager.directions)
		{
			Node foundNeighbor = FindNeighborAt (nodes, dir);

			if (foundNeighbor != null && !nList.Contains (foundNeighbor))
			{
				nList.Add (foundNeighbor);
			}
		}
		return nList;
	}

	public Node FindNeighborAt (List<Node> nodes, Vector2 dir)
	{
		return nodes.Find (n => (n.Coordinate == Coordinate + dir && n.isActive));
	}

	public Node FindNeighborAt (Vector2 dir)
	{
		return FindNeighborAt (NeighborNodes, dir);
	}

	public void ShowMesh ()
	{
		if (mesh != null)
		{
			iTween.ScaleTo (mesh, iTween.Hash (
				"time", scaleTime,
				"scale", Vector3.one,
				"easetype", easeType,
				"delay", delay,
				"oncomplete", "GeometryVisible",
				"oncompletetarget", gameObject
			));
		}
	}

	public void GeometryVisible ()
	{
		m_board.VisibleNodes++;
		// if (isLevelGoal)
		// {
		// 	m_board.DrawGoal ();
		// }
	}

	public void HideMesh ()
	{
		if (mesh != null)
		{
			iTween.ScaleTo (mesh, iTween.Hash (
				"time", scaleTime,
				"scale", Vector3.zero,
				"easetype", easeType,
				"delay", 0f,
				"oncomplete", "GeometryInvisible",
				"oncompletetarget", gameObject
			));
		}
	}

	public void GeometryInvisible ()
	{
		m_board.VisibleNodes--;
	}

	public void InitNode ()
	{
		if (!m_initialized && isActive)
		{
			ShowMesh ();
			InitNeighbors ();
			m_initialized = true;
			m_board.ActiveNodes++;
		}
	}

	void InitNeighbors ()
	{
		StartCoroutine (InitNeighborsRoutine ());
	}

	IEnumerator InitNeighborsRoutine ()
	{
		yield return new WaitForSeconds (delay);

		foreach (Node n in m_neighborNodes)
		{

			if (n.isActive && !m_linkedNodes.Contains (n))
			{
				Obstacle obstacle = FindObstacle (n);
				if (obstacle == null)
				{
					LinkNode (n);
					n.InitNode ();
				}
			}
		}
	}

	Obstacle FindObstacle (Node targetNode)
	{
		Vector3 checkDirection = targetNode.transform.position - transform.position;
		RaycastHit raycastHit;

		if (Physics.Raycast (transform.position, checkDirection, out raycastHit,
				BoardManager.spacing + 0.1f, obstacleLayer))
		{
			return raycastHit.collider.GetComponent<Obstacle> ();
		}

		return null;
	}

	void LinkNode (Node targetNode)
	{
		if (linkPrefab != null)
		{
			GameObject linkInstance = Instantiate (linkPrefab, transform.position, Quaternion.identity);
			linkInstance.transform.parent = transform;

			Link link = linkInstance.GetComponent<Link> ();
			if (link != null)
			{
				link.DrawLink (transform.position, targetNode.transform.position);
			}
			if (!m_linkedNodes.Contains (targetNode))
			{
				m_linkedNodes.Add (targetNode);
			}
			if (!targetNode.LinkedNodes.Contains (this))
			{
				targetNode.LinkedNodes.Add (this);
			}
		}
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine (transform.position, transform.position + Vector3.down);
	}

}