using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


[Serializable]
public class PieceFallingChecker : MonoBehaviour
{
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDuration;
    
    // ====================== Use for BFS
    private Board board;
    private Piece[,] boardPiece;
    private List<Piece> listPieceFound = new List<Piece>();
    private bool[,] visited;
    private int[,] directionsOdd = { { 0, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { -1, -1 }, { -1, 0 } };
    private int[,] directionsEven = { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 }, { -1, 1 } };
    // ======================
    public void Init(Board b)
    {
        board = b;
    }
     
    public void CheckFalling()
    {
        boardPiece = board.BoardPiece;
        int row = board.row;
        int column = board.column;
        
        visited = new bool[row, column+1];

        Piece pieceTemp;
        for (int i = 0; i < column + 1; i++)
        {
            pieceTemp = boardPiece[row -1 ,i];
            if (pieceTemp == null)
            {
                continue;
            }

            if (pieceTemp.pieceType == PieceType.None)
            {
                continue;
            }

            BFS(pieceTemp.pos2, row, column);
        }
        
        List<Vector2Int> listFalling = new List<Vector2Int>();
        
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column + 1; j++)
            {
                pieceTemp = boardPiece[i ,j];
                
                if (pieceTemp == null)
                {
                    continue;
                }
                if (pieceTemp.pieceType == PieceType.None)
                {
                    continue;
                }

                if (!visited[i, j])
                {
                    listFalling.Add(pieceTemp.pos2);
                }
            }
        }
        SetFallingAnimation(listFalling);
    }

    private void SetFallingAnimation(List<Vector2Int> listFall)
    {
        foreach (var fall in listFall)
        {
            Piece piece = boardPiece[fall.x, fall.y];
            Piece newPiece = Instantiate(piece,piece.position,Quaternion.identity);

            newPiece.transform.DOJump(endPoint,jumpForce,1,jumpDuration).OnComplete(() =>
            {
                Destroy(newPiece.gameObject);
                Debug.Log("Destroy Piece Fall");
            });
            piece.SetNone();
        }
    }

    private void BFS(Vector2Int start, int row, int column)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        
        visited[start.x, start.y] = true;
        
        // check piece found
        listPieceFound.Clear();
        listPieceFound.Add(boardPiece[start.x, start.y]);
        
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            // Debug.Log("Visiting: " + current);

            int[,] directions = (current.x % 2 == 0) ? directionsEven : directionsOdd;

            for (int i = 0; i < 6; i++)
            {
                int nx = current.x + directions[i, 0];
                int ny = current.y + directions[i, 1];
                
                // limit 
                if (nx >= 0 && nx < row && ny >= 0 && ny < column + 1 && !visited[nx, ny])
                {
                    // check null
                    if (boardPiece[nx, ny] == null)
                    {
                        continue;
                    }
                    if (boardPiece[nx, ny].pieceType != PieceType.None)
                    {
                        queue.Enqueue(new Vector2Int(nx, ny));
                        listPieceFound.Add(boardPiece[nx,ny]);
                        visited[nx, ny] = true;
                    }
                }
            }
        }
    }
}
