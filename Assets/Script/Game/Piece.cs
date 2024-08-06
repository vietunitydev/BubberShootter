using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board;
    public PieceType pieceType;
    public Vector3 position;
    public Vector2Int pos2;
    public SpriteRenderer sprite;

    private void Start()
    {
        position = transform.position;
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Init(int r, int c, Board b)
    {
        board = b;
        pos2.x = r;
        pos2.y = c;
        gameObject.name = $"Piece [{pos2.x},{pos2.y}]";
    }

    private void OnMouseDown()
    {
        board.FindPieceAround(pos2.x,pos2.y);
    }
}

public enum PieceType
{
    None,
    Green,
    Orange,
    Pink,
    Red,
    Yellow
}