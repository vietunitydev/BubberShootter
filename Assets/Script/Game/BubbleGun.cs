using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BubbleGun : MonoBehaviour
{
    [Header("LINE SETUP")]
    [SerializeField] private LineGenerator lineGenerator;
    [Header("GUN SETUP")]
    [SerializeField] private Piece bubbleBullet;
    [SerializeField] private List<Piece> bubbles;
    [SerializeField] private GameObject spawnPos;
    [SerializeField] private int bullet;
    private Vector3 direction;
    private Vector3 mouse;
    
    private void Start()
    {
        bubbleBullet = Instantiate(bubbles[RandomBubble()],spawnPos.transform.position,Quaternion.identity,transform);
    }
    private void Update()
    {
        if (Camera.main != null) 
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;
        
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(mouse);
        }
        Debug.DrawRay(bubbleBullet.position, direction);
    }
    
    private int RandomBubble()
    {
        return UnityEngine.Random.Range(0, bubbles.Count);
    }

    private void Shoot(Vector3 target)
    {
        direction = (target - bubbleBullet.transform.position);
        lineGenerator.GenerateLine(bubbleBullet.transform.position, direction.normalized);

        Debug.Log($"Shoot : {direction.normalized}");
    }
}

[Serializable]
public class LineGenerator
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform up;
    [SerializeField] private Transform down;
    [SerializeField] private Transform right;
    [SerializeField] private Transform left;
    [SerializeField] private List<Vector3> points;

    [SerializeField] private float width;
    [SerializeField] private float heigh;
    
    private void GeneratePoint(Vector3 start, Vector3 direction)
    {
        points.Clear();
        points.Add(start);
        
        Vector3 lastPoint = start;
        
        float tanRad = Mathf.Atan(direction.y / direction.x);
        float degrees = tanRad * Mathf.Rad2Deg;

        lastPoint.x += width;
        lastPoint.y += Mathf.Tan(tanRad) * width;
        points.Add(lastPoint);

        lastPoint.x += - 2 * width;
        lastPoint.y += Mathf.Tan(tanRad) * 2 * width;
        points.Add(lastPoint);
        
    }
    public void GenerateLine(Vector3 start, Vector3 direction)
    {
        GeneratePoint(start, direction);
        line.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i]);
        }
    }
}