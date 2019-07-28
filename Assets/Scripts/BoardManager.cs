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

        private int emptyCellValue = 0;
        private int player1CellValue = 1;
        private int player2CellValue = 10;

        private List<int> cellContents;
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

        public BoardManager(int playerIdNoneIn, int playerId1In, int playerId2In)
        {
            playerIdNone = playerIdNoneIn;
            playerId1 = playerId1In;
            playerId2 = playerId2In;
            cellContents = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                cellContents.Add(playerIdNone);
            }
        }

        public bool IsLegalMove(int cellSelected)
        {
            return cellContents[cellSelected] == playerIdNone;
        }

        public void PlaceMarker(int playerId, int cellId)
        {
            // For player1 store a value of 1.  For player 2 store a value of 10.  
            // This simplifies the logic to inspect row contents later.

            int cellValue = emptyCellValue;
            if (playerId == playerId1)
            {
                cellValue = player1CellValue;
            }
            else
            {
                cellValue = player2CellValue;
            }

            cellContents[cellId] = cellValue;
        }

        public (int, int) CheckForWin()
        {
            int rowNum = 0;
            List<int> rowSums = GetRowSums();
            foreach(int sum in rowSums)
            {
                if (sum == player1CellValue * 3)
                {
                    return (playerId1, rowNum);
                }
                else if (sum == player2CellValue * 3)
                {
                    return (playerId2, rowNum);
                }
                rowNum++;
            }

            return (playerIdNone, -1);
        }

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

            int numEmptyCells = emptyCells.Count();
            int selectedIndex = Random.Range(0,numEmptyCells);
            int selectedCell = emptyCells[selectedIndex];

            Debug.Log("SelectedCell: " + selectedCell.ToString());
            return selectedCell;
        }

        private List<int> GetRowSums()
        {
            List<int> rowSums = new List<int>();
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
