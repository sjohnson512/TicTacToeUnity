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

    // Vars to track game state
    private int currentPlayerId = 1;
    Dictionary<int, bool> playerIsComputer = new Dictionary<int, bool>();   
    private bool gameIsOver = false;

    // Center points for each cell.  See summary of BoardManager for how cells in the board are organized.
    // TODO: Write some tests to ensure that all of the lists of cells and rows stay synchronized.
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

    // Start points for line drawn through winning row
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

    // End points for line drawn through winning row
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

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Set the player types
        // TODO: Add an options screen to let players be set to human or computer.  For now,
        // player1 is always human and player2 is computer.
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

    /// <summary>
    /// Reset does the following:
    /// 1. Set the current player to player1
    /// 2. Clear the end of game text and line
    /// 3. Reset the board manager and clear markers from the board
    /// </summary>
    void ResetGame()
    {
        currentPlayerId = playerId1;

        // Clear the end of game text and line
        endGameText.text = "";
        lineRenderer.positionCount = 0;
        lineRenderer.positionCount = 2;

        // Reset the boardManager and clear all the markers from the board
        boardManager = new BoardManager(playerIdNone, playerId1, playerId2);
        if (markers.Count > 0)
        { 
            foreach (Object marker in markers)
            {
                Destroy(marker);
            }
        }

        gameIsOver = false;
    }

    /// <summary>
    /// Do the following once per frame:
    /// 1. If it is the computer's turn, let it select a cell
    /// 2. If it is the player's turn, wait for the user to click a cell
    /// </summary>
    void Update()
    {
        // Computer player's turn
        if (!gameIsOver && playerIsComputer[currentPlayerId])
        {
            int computerSelectedCell = boardManager.GetComputerMove();
            Thread.Sleep(600); // TODO: do this with a coroutine instead
            HandleCellSelected(computerSelectedCell);
            return;
        }

        // Human player's turn
        if (Input.GetMouseButtonDown(0))
        {
            // If the game is over, let the player click once to reset the game
            if (gameIsOver)
            {
                ResetGame();
                return;
            }

            // Get the cell the player clicked on
            Vector3 clickPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            int closestCell = GetClosestCell(clickPosition);

            // If the player clicked on an empty cell, update the board
            if (boardManager.IsLegalMove(closestCell))
            {
                HandleCellSelected(closestCell);
            }
        }
    }

    /// <summary>
    /// Given a selected cell, updates the board and checks if the game is over.
    /// </summary>
    /// <param name="selectedCell">Index for the selected cell.</param>
    void HandleCellSelected(int selectedCell)
    {
        // Draw the player's marker on the board and update the boardManager
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

        // Check to see if the game is over
        var (winningPlayerId, rowId) = boardManager.CheckForWin();
        if (winningPlayerId == playerId1)
        {
            // Draw a line through the winning row
            lineRenderer.SetPosition(0, rowStarts[rowId]);
            lineRenderer.SetPosition(1, rowEnds[rowId]);

            endGameText.text = "Player1 Wins!";
            gameIsOver = true;
        }
        else if (winningPlayerId == playerId2)
        {
            // Draw a line through the winning row
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
