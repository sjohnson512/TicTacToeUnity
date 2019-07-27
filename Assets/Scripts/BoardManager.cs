using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TicTacToe
{
    public class BoardManager
    {

        List<int> cellContents = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public bool IsLegalMove(int cellSelected)
        {
            return cellContents[cellSelected] == 0;
        }

        public void PlaceMarker(int playerId, int cellId)
        {
            cellContents[cellId] = playerId;
        }


    }
}
