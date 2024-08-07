using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceFallingChecker : MonoBehaviour
{
    [SerializeField] private Board board;

     private Piece[,] boardPiece;

    private void CheckFalling()
    {
        boardPiece = board.BoardPiece;
    }
}
