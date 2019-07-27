using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe;

public class GameManager : MonoBehaviour
{
    public GameObject xMarker;
    public GameObject oMarker;

    static private int playerId1 = 1;
    static private int playerId2 = 2;
    private BoardManager boardManager = new BoardManager(playerId1, playerId2);

    private Camera cam;
    private List<Vector3> cellCenters;


    private int currentPlayerId = 1;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cellCenters = new List<Vector3>
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
                int winningPlayer = boardManager.CheckForWin();
                Debug.Log(winningPlayer);
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
