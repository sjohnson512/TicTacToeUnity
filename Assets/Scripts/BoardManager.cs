using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TicTacToe
{
    /// <summary>
    /// 
    /// </summary>
    public class BoardManager
    {
        private List<int> cellContents;

        public BoardManager()
        {
            cellContents = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        public bool IsLegalMove(int cellSelected)
        {
            return cellContents[cellSelected] == 0;
        }

        public void PlaceMarker(int playerId, int cellId)
        {
            // For player1 store a value of 1.  For player 2 store a value of 10.  
            // This simplifies the logic to inspect row contents later.
            int cellValue = playerId == 1 ? 1 : 10;
            
            cellContents[cellId] = cellValue;
        }


    }
}
