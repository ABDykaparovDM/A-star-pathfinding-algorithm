using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour {

    public float speed = 5f;
	public Vector3[] path;
	
    public Vector3 target;
    public GameObject TargetObj;
    int targetIndex;

    public bool stopMoving; 
    private Grid grid; 

    public enum MoveFSM
    {
        recalculatePath,    //stage of recalculatePath (almost always in this stage)
        move                //stage of move (just for understanding how it works)
    }

    public MoveFSM moveFSM; 

	void Start()
    {
        grid = GameObject.FindGameObjectWithTag("A*").GetComponent<Grid>();
    }

    void FixedUpdate()//runs Update script for a Target Position, Clearing outdated path from UnitManager, Sending updated path to the PathRequestManager, and switching MoveStates if needed 60 times per second
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            target = TargetObj.transform.position;
            RemoveUnitFromUnitManagerMovingUnitsList();
            PathRequestManager.RequestPath(transform.position, target, OnPathFound);
        }
        MoveStates();
    }

    public void MoveStates()            //Switching Movement States
    {
        switch (moveFSM)
        {
            case MoveFSM.recalculatePath:               
                {
                    Node targetNode = grid.NodeFromWorldPoint(target);
                    if (targetNode.walkable == false)               //sops movement and checks different node if the node is unwalkable 
                    {
                        stopMoving = false;
                        FindClosestWalkableNode(targetNode);
                        moveFSM = MoveFSM.move;
                    }
                    else if (targetNode.walkable == true)           //moves if hits if the node is walkable
                    {
                        Debug.Log("target node is walkable");
                        stopMoving = false;
                        PathRequestManager.RequestPath(transform.position, target, OnPathFound);
                        moveFSM = MoveFSM.move;
                    }
                }
                break;
            case MoveFSM.move:                          
                break;
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) // Switches MoveState to Move after finding a path
    {
		if (pathSuccessful)
        {
			path = newPath;
			targetIndex = 0;
            RemoveUnitFromUnitManagerMovingUnitsList();             // Clears out unit from the list to refresh the path and avoid repetitions 
            UnitManager.instance.movingUnits.Add(this.gameObject);  // Adds it back
            StopCoroutine("FollowPath");                            // Refreshes the path
			StartCoroutine("FollowPath");
            moveFSM = MoveFSM.move; 
		}
	}

    private void FindClosestWalkableNode(Node originalNode)         //Finds Closest Walkable Node
    {
        Node comparisonNode = grid.grid[0, 0];
        Node incrementedNode = originalNode;
        for (int x = 0; x < incrementedNode.gridX; x++)
        {
            incrementedNode = grid.grid[incrementedNode.gridX - 1, incrementedNode.gridY];

            if (incrementedNode.walkable == true)
            {
                comparisonNode = incrementedNode;
                target = comparisonNode.nodeWorldPosition;
                PathRequestManager.RequestPath(transform.position, target, OnPathFound);
                moveFSM = MoveFSM.move;
                break;
            }
        }

    }

	IEnumerator FollowPath()                //FollowPath routin for OnPathFound functions routines
    {
		Vector3 currentWaypoint = path[0];
		while (true)
        {
			if (transform.position == currentWaypoint)
            {
				targetIndex ++;
				if (targetIndex >= path.Length || stopMoving == true)   //if reached the goal or stoped movement for recalculation
                {
                    yield break;            //Tells the iterator that it's reached the end.
                }
                currentWaypoint = path[targetIndex];                    //changes curent goal position (waypoint)
			}

			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime); //moves toward the curent goal(waypoint)
			yield return null;              //wait for the next frame and continue execution from this line

        }
	}

    private void RemoveUnitFromUnitManagerMovingUnitsList()         //removes entities from the list of UnitManager.cs script
    {
        if (UnitManager.instance.movingUnits.Count > 0)
        {
            for (int i = 0; i < UnitManager.instance.movingUnits.Count; i++)
            {
                if (this.gameObject == UnitManager.instance.movingUnits[i])
                {
                    UnitManager.instance.movingUnits.Remove(UnitManager.instance.movingUnits[i]);
                }
            }
        }
    }

    public void OnDrawGizmos()      //Draws a path on editor window
    {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
