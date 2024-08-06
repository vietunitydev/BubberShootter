using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class BubbleGun : MonoBehaviour
{
    [Header("LINE SETUP")]
    [SerializeField] private LineGenerator lineGenerator;
    [Header("BOARD SETUP")]
    [SerializeField] private Board board;
    [Header("GUN SETUP")]
    [SerializeField] private Piece bubbleBullet;
    [SerializeField] private List<Piece> bubbles;
    [SerializeField] private GameObject spawnPos;
    [SerializeField] private int bullet;

    private List<Vector3> points;
    
    [Header("Debug ListCheck")] [SerializeField]
    private List<Piece> piecesCheckLine;
    [SerializeField] private Piece bubbleCanShot;

    
    private void Start()
    {
        bubbleBullet = Instantiate(bubbles[RandomBubble()],spawnPos.transform.position,Quaternion.identity,transform);
    }
    private void Update()
    {
        if (Camera.main != null)
        {
            var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0;
        
            if (Input.GetMouseButton(0))
            {
                DrawLine(mouse);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            CheckLastPoint();
            if (CanShoot())
            {
                Debug.Log("Shoot");
                points[^1] = bubbleCanShot.position;
                Shoot();
            }
        }
    }
    
    private int RandomBubble()
    {
        return UnityEngine.Random.Range(0, bubbles.Count);
    }

    private void DrawLine(Vector3 target)
    {
        var direction = (target - bubbleBullet.transform.position);
        points = lineGenerator.GenerateLine(bubbleBullet.transform.position, direction.normalized);
    }
    private void CheckLastPoint()
    {
        piecesCheckLine.Clear();
        for (int i = 0; i < points.Count - 1; i++)
        {
            var start = points[i];
            var direct = points[i + 1] - start;
            var hits = Physics2D.RaycastAll(start,  direct,5f);
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    var piece = hit.collider.GetComponent<Piece>();
                    if (piece == null)
                    {
                        continue;
                    }

                    if (piece != bubbleBullet)
                    {
                        piecesCheckLine.Add(piece);
                    }
                }
            }
        }
    }

    private bool CanShoot()
    {
        foreach (var piece in piecesCheckLine)
        {
            if (board.HasPieceAround(piece))
            {
                bubbleCanShot = piece;
                return true;
            }
        }

        bubbleCanShot = null;
        return false;
    }

    private void Shoot()
    {
        if (points==null)
        {
            return;
        }

        if (points.Count == 0)
        {
            return;
        }

        StartCoroutine(FLyAsync());
    }

    IEnumerator FLyAsync()
    {
        bubbleBullet.transform.DOMove(points[1], 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        for (int i = 2; i < points.Count ; i++)
        {
            bubbleBullet.transform.DOMove(points[i], 1f);
            yield return new WaitForSeconds(1f);
        }

        bubbleBullet = Instantiate(bubbles[RandomBubble()],spawnPos.transform.position,Quaternion.identity,transform);
        lineGenerator.ClearLine();
    }
}

[Serializable]
public class LineGenerator
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private List<Vector3> points;

    [SerializeField] private float width;

    public void ClearLine()
    {
        line.positionCount = 0;
    }
    
    public List<Vector3> GenerateLine(Vector3 start, Vector3 direction)
    {
        GeneratePoint(start, direction);
        line.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i]);
        }

        return points;
    }
    
    private void GeneratePoint(Vector3 start, Vector3 direction)
    {
        points.Clear();
        points.Add(start);
        
        Vector3 lastPoint = start;
        
        float tanRad = Mathf.Atan(direction.y / direction.x);
        float degrees = tanRad * Mathf.Rad2Deg;

        if (degrees > 0)
        {
            lastPoint.x += width;
            lastPoint.y += Mathf.Tan(tanRad) * width;
            points.Add(lastPoint);

            lastPoint.x += - 2 * width;
            lastPoint.y += Mathf.Tan(tanRad) * 2 * width;
            points.Add(lastPoint);
            
            lastPoint.x += 2 * width;
            lastPoint.y += Mathf.Tan(tanRad) * 2 * width;
            points.Add(lastPoint);
        }

        else
        {
            lastPoint.x += - width;
            lastPoint.y += - Mathf.Tan(tanRad) * width;
            points.Add(lastPoint);

            lastPoint.x += 2 * width;
            lastPoint.y += - Mathf.Tan(tanRad) * 2 * width;
            points.Add(lastPoint);
            
            lastPoint.x += -2 * width;
            lastPoint.y += - Mathf.Tan(tanRad) * 2 * width;
            points.Add(lastPoint);
        }
    }
}