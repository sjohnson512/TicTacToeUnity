using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe;

public class GameManager : MonoBehaviour
{
    // Marker prefabs
    public GameObject xMarker;
    public GameObject oMarker;

    // Game Objects
    private BoardManager boardManager = new BoardManager(playerIdNone, playerId1, playerId2);
    private Camera cam;
    private LineRenderer lineRenderer;

    // Player Ids
    static private int playerIdNone = 0;
    static private int playerId1 = 1;
    static private int playerId2 = 2;
    private int currentPlayerId = 1;

    // Center points for each cell.  See summary of BoardManager for how cells in the board are organized.
    private List<Vector3> cellCenters = new List<Vector3>
        {
            new Vector3(-2.0f,  2.0f, 0.0f),
            new Vector3( 0.0f,  2.0f, 0.0f),
            new Vector3( 2.0f,  2.0f, 0.0f),
            new Vector3(-2.0f,  0.0f, 0.0f),
            new Vector3( 0.0f,  0.0f, 0.0f),
            new Vector3( 2.0f,  0.0f, 0.0f),
            new Vector3(-2.0f, -2.0f, 0.0f),
            new Vector3( 0.0f, -2.0f, 0.0f),
            new Vector3( 2.0f, -2.0f, 0.0f)
        };

    private List<Vector3> rowStarts = new List<Vector3>
        {
            new Vector3(-2.8f,  2.0f, -1.0f),
            new Vector3(-2.8f,  0.0f, -1.0f),
            new Vector3(-2.8f, -2.0f, -1.0f),
            new Vector3(-2.0f,  2.8f, -1.0f),
            new Vector3( 0.0f,  2.8f, -1.0f),
            new Vector3( 2.0f,  2.8f, -1.0f),
            new Vector3(-2.8f,  2.8f, -1.0f),
            new Vector3( 2.8f,  2.8f, -1.0f)
        };

    private List<Vector3> rowEnds = new List<Vector3>
        {
            new Vector3( 2.8f,  2.0f, -1.0f),
            new Vector3( 2.8f,  0.0f, -1.0f),
            new Vector3( 2.8f, -2.0f, -1.0f),
            new Vector3(-2.0f, -2.8f, -1.0f),
            new Vector3( 0.0f, -2.8f, -1.0f),
            new Vector3( 2.0f, -2.8f, -1.0f),
            new Vector3( 2.8f, -2.8f, -1.0f),
            new Vector3(-2.8f, -2.8f, -1.0f)
        };


    // Start is called before the first frame update
    void Start()
    {
        // Grab the camera
        cam = Camera.main;

        // Setup the lineRenderer used to draw the line through the winning row
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.material.color = Color.black;
 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            int closestCell = GetClosestCell(clickPosition);

            if (boardManager.IsLegalMove(closestCell))
            {
                if (currentPlayerId == playerId1)
                {
                    Instantiate(xMarker, cellCenters[closestCell], Quaternion.identity);
                    boardManager.PlaceMarker(currentPlayerId, closestCell);
                    currentPlayerId = playerId2;
                }
                else
                {
                    Instantiate(oMarker, cellCenters[closestCell], Quaternion.identity);
                    boardManager.PlaceMarker(currentPlayerId, closestCell);
                    currentPlayerId = playerId1;
                }
                var (winningPlayerId, rowId) = boardManager.CheckForWin();
                Debug.Log(winningPlayerId);

                if (winningPlayerId == playerId1)
                {
                    lineRenderer.SetPosition(0, rowStarts[rowId]);
                    lineRenderer.SetPosition(1, rowEnds[rowId]);
                }
                else if (winningPlayerId == playerId2)
                {
                    lineRenderer.SetPosition(0, rowStarts[rowId]);
                    lineRenderer.SetPosition(1, rowEnds[rowId]);

                }
            }
        }
    }

    /// <summary>
    /// Finds the cell center closest to the clickPosition.
    /// </summary>
    /// <param name="clickPosition">The position of the mouse click in World coordinates.</param>
    /// <returns>The id of the closest cell.</returns>
    int GetClosestCell(Vector3 clickPosition)
    {
        int currentCell = 0;
        float minDistance = 10000;
        int closestCell = 0;

        // Iterate over the list of cell center locations
        foreach (Vector3 cellCenter in cellCenters)
        {

            // Calculate the distance between the click and the cell center
            float distanceToCell = (clickPosition - cellCenter).sqrMagnitude;

            // Keep track of which cell is closest
            if (distanceToCell < minDistance)
            {
                closestCell = currentCell;
                minDistance = distanceToCell;
            }
            currentCell++;
        }

        return closestCell;
    }
}
