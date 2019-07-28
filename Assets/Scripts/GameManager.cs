using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TicTacToe;

public class GameManager : MonoBehaviour
{

    // Game Objects
    public GameObject xMarker;
    public GameObject oMarker;
    public Text endGameText;
    private Camera cam;
    private LineRenderer lineRenderer;
    private List<Object> markers = new List<Object>();
   
    // Board logic
    private BoardManager boardManager;

    // Player Ids
    static readonly private int playerIdNone = 0;
    static readonly private int playerId1 = 1;
    static readonly private int playerId2 = 2;

    private int currentPlayerId = 1;

    Dictionary<int, bool> playerIsComputer = new Dictionary<int, bool>();
    
    // Other game state info
    private bool gameIsOver = false;

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
        // Set the player types
        playerIsComputer.Add(playerId1, false);
        playerIsComputer.Add(playerId2, true);

        // Grab the camera
        cam = Camera.main;

        // Setup the lineRenderer used to draw the line through the winning row
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.material.color = Color.black;

        // Finish initializing the game
        ResetGame();
    }

    void ResetGame()
    {
        currentPlayerId = playerId1;

        endGameText.text = "";
        lineRenderer.positionCount = 0;
        lineRenderer.positionCount = 2;

        boardManager = new BoardManager(playerIdNone, playerId1, playerId2);
        if (markers.Count > 0)
        { 
            foreach (Object marker in markers)
            {
                Destroy(marker);
            }
        }

        gameIsOver = false;

        return;
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameIsOver && playerIsComputer[currentPlayerId])
        {
            int computerSelectedCell = boardManager.GetComputerMove();
            Thread.Sleep(600);
            HandleCellSelected(computerSelectedCell);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (gameIsOver)
            {
                ResetGame();
                return;
            }

            Vector3 clickPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            int closestCell = GetClosestCell(clickPosition);

            if (boardManager.IsLegalMove(closestCell))
            {
                HandleCellSelected(closestCell);
            }
        }
    }

    void HandleCellSelected(int selectedCell)
    {
        if (currentPlayerId == playerId1)
        {
            Object newMarker = Instantiate(xMarker, cellCenters[selectedCell], Quaternion.identity);
            markers.Add(newMarker);
            boardManager.PlaceMarker(currentPlayerId, selectedCell);
            currentPlayerId = playerId2;
        }
        else
        {
            Object newMarker = Instantiate(oMarker, cellCenters[selectedCell], Quaternion.identity);
            markers.Add(newMarker);
            boardManager.PlaceMarker(currentPlayerId, selectedCell);
            currentPlayerId = playerId1;
        }

        var (winningPlayerId, rowId) = boardManager.CheckForWin();

        if (winningPlayerId == playerId1)
        {
            lineRenderer.SetPosition(0, rowStarts[rowId]);
            lineRenderer.SetPosition(1, rowEnds[rowId]);
            endGameText.text = "Player1 Wins!";
            gameIsOver = true;
        }
        else if (winningPlayerId == playerId2)
        {
            lineRenderer.SetPosition(0, rowStarts[rowId]);
            lineRenderer.SetPosition(1, rowEnds[rowId]);
            endGameText.text = "Player2 Wins!";
            gameIsOver = true;
        }
        else if (boardManager.IsTieGame())
        {
            endGameText.text = "Tie Game!";
            gameIsOver = true;
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
