using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TicTacToe
{
    /// <summary>
    /// Class BoardManager manages the state of the game board.  Each square on the board
    /// is called a cell.  Each cell is a member of two or more rows.  Cells and rows are
    /// arranged on the board in this pattern:
    ///          
    ///  row0 >  0 | 1 | 2
    ///           ---+---+---
    ///  row1 >  3 | 4 | 5
    ///          ---+---+---
    ///  row2 >  6 | 7 | 8
    ///          ^   ^   ^
    ///         row row row
    ///          3   4   5
    /// The two diagonals are rows 6 and 7.
    /// </summary>
    public class BoardManager
    {
        // Values for each player
        private int playerIdNone;
        private int playerId1;
        private int playerId2;

        // Constant values used to populate cellContents
        // Values of 1 and 10 for players simplify the logic to check for wins later
        private readonly int emptyCellValue = 0;
        private readonly int player1CellValue = 1;
        private readonly int player2CellValue = 10;

        // List used to track which marker is in each cell
        private List<int> cellContents;

        // Lists of which cells are members of each row
        private List<List<int>> rowMembers = new List<List<int>>
        {
            new List<int> {0, 1, 2},
            new List<int> {3, 4, 5},
            new List<int> {6, 7, 8},
            new List<int> {0, 3, 6},
            new List<int> {1, 4, 7},
            new List<int> {2, 5, 8},
            new List<int> {0, 4, 8},
            new List<int> {2, 4, 6}
        };

        /// <summary>
        /// The constructor sets up the board contents
        /// </summary>
        /// <param name="playerIdNoneIn">PlayerId set by GameManager</param>
        /// <param name="playerId1In">PlayerId set by GameManager</param>
        /// <param name="playerId2In">PlayerId set by GameManager</param>
        public BoardManager(int playerIdNoneIn, int playerId1In, int playerId2In)
        {
            // Set player IDs
            playerIdNone = playerIdNoneIn;
            playerId1 = playerId1In;
            playerId2 = playerId2In;

            // Create a new cell contents list and initialize it to empty cells
            cellContents = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                cellContents.Add(playerIdNone);
            }
        }

        /// <summary>
        /// Check to see if the selected cell is empty
        /// </summary>
        /// <param name="cellSelected">The cell index to check</param>
        /// <returns>true if the cellSelected is empty</returns>
        public bool IsLegalMove(int cellSelected)
        {
            return cellContents[cellSelected] == playerIdNone;
        }

        /// <summary>
        /// Update the selected cell with the player's cell value
        /// </summary>
        /// <param name="playerId">Id of the player placing the cell</param>
        /// <param name="cellId">Id of the selected cell</param>
        public void PlaceMarker(int playerId, int cellId)
        {
            int cellValue = emptyCellValue;
            if (playerId == playerId1)
            {
                cellValue = player1CellValue;
            }
            else if (playerId == playerId2)
            {
                cellValue = player2CellValue;
            }      

            cellContents[cellId] = cellValue;
        }

        /// <summary>
        /// Checks cell contents to if someone has won the game
        /// </summary>
        /// <returns>Returns the winning player's id and the winning rowNumber</returns>
        public (int, int) CheckForWin()
        {
            // Iterate over all rows
            int rowNum = 0;
            List<int> rowSums = GetRowSums();
            foreach(int sum in rowSums)
            {
                // Check if the row has 3 X's
                if (sum == player1CellValue * 3)
                {
                    return (playerId1, rowNum);
                }
                // Check if the row has 3 O's
                else if (sum == player2CellValue * 3)
                {
                    return (playerId2, rowNum);
                }
                rowNum++;
            }

            return (playerIdNone, -1);
        }

        /// <summary>
        /// Checks to see if all of the cells are full.
        /// </summary>
        /// <returns>true if all cells are full</returns>
        public bool IsTieGame()
        {
            int numFullCells = 0;
            foreach (int cellValue in cellContents)
            {
                if (cellValue != emptyCellValue)
                {
                    numFullCells++;
                }
            }

            bool isTieGame = numFullCells == 9;
            return isTieGame;
        }

        /// <summary>
        /// Simple logic to let the computer pick a random empty cell
        /// </summary>
        /// <returns>Index for the selected cell</returns>
        public int GetComputerMove()
        {
            // Get a list of empty cells
            List<int> emptyCells = new List<int>();
            int cellNumber = 0;
            foreach (int cell in cellContents)
            {
                if (cell == emptyCellValue)
                {
                    emptyCells.Add(cellNumber);
                }
                cellNumber++;
            }

            // Randomly select an empty cell
            int numEmptyCells = emptyCells.Count();
            int selectedIndex = Random.Range(0,numEmptyCells);
            int selectedCell = emptyCells[selectedIndex];

            return selectedCell;
        }

        /// <summary>
        /// Get sum of the contents for each row.
        /// Useful for checking for a win and for future computer logic.  
        /// A row value of 3 indicates 3 Xs.  A  row value of 30 indicates 3 Os, etc.
        /// </summary>
        /// <returns>Sum of the values in the </returns>
        private List<int> GetRowSums()
        {
            List<int> rowSums = new List<int>();

            // Iterate over all of the rows, computing the sum of the row's contents
            foreach (List<int> row in rowMembers)
            {
                int sum = 0;
                foreach (int cellId in row)
                {
                    sum += cellContents[cellId];
                }
                rowSums.Add(sum);
            }

            return rowSums;
        }
    }
}
