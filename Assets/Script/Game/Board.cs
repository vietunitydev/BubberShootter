using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private List<Piece> bubbles;
    [SerializeField] private Piece[,] board;
    public Vector2 sizeDefault;
    public int column;
    public int row;

    public List<Piece> listPieceFound = new List<Piece>();

    private void Start()
    {
        GenerateMap();
    }
    private void GenerateMap()
    {
        board = new Piece[row, column+1];
        
        for (int r = 0; r < row; r++)
        {
            if (r % 2 == 0)
            {
                for (int c = 0; c < column; c++)
                {
                    board[r,c] = Instantiate(bubbles[RandomBubble()],CalculatePosition(r,c),Quaternion.identity,transform);
                    board[r,c].Init(r,c,this);
                }
            }
            else
            {
                for (int c = 0; c < column+1; c++)
                {
                    board[r,c] = Instantiate(bubbles[RandomBubble()],CalculatePosition(r,c),Quaternion.identity,transform);
                    board[r,c].Init(r,c,this);
                }
            }
        }
    }

    public void FindPieceAround(int r, int c)
    {
        BFS(new Vector2Int(r,c));
    }

    private int RandomBubble()
    {
        return UnityEngine.Random.Range(0, bubbles.Count);
    }

    private Vector3 CalculatePosition(int r, int c)
    {
        if (r % 2 == 0)
        {
            return new Vector3(-1.425f + c*sizeDefault.x,r*sizeDefault.y,0);
        }
        
        return new Vector3(-1.6f + c*sizeDefault.x,r*sizeDefault.y,0);
    }
    
    private int[,] directionsOdd = { { 0, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { -1, -1 }, { -1, 0 } };

    private int[,] directionsEven = { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 }, { -1, 1 } };
    

    public void BFS(Vector2Int start)
    {
        bool[,] visited = new bool[row, column+1];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        listPieceFound.Clear();
        queue.Enqueue(start);
        visited[start.x, start.y] = true;
        listPieceFound.Add(board[start.x, start.y]);
        PieceType type = board[start.x, start.y].pieceType;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            // Debug.Log("Visiting: " + current);

            int[,] directions = (current.x % 2 == 0) ? directionsEven : directionsOdd;

            for (int i = 0; i < 6; i++)
            {
                int nx = current.x + directions[i, 0];
                int ny = current.y + directions[i, 1];

                if (nx >= 0 && nx < row && ny >= 0 && ny < column && !visited[nx, ny] && type == board[nx,ny].pieceType)
                {
                    queue.Enqueue(new Vector2Int(nx, ny));
                    listPieceFound.Add(board[nx,ny]);
                    visited[nx, ny] = true;
                }
            }
        }

        if (listPieceFound.Count >= 3)
        {
            foreach (var p in listPieceFound)
            {
                p.sprite.sprite = null;
            }
        }
    }
}
