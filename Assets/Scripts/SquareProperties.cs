using System;
using UnityEngine;

public class SquareProperties : MonoBehaviour
{
    public Color originalColor; 
    public char file='a'; 
    public int rank=1;
    public string Coordinate;

    void Start()
    {
        file = (char)('a' + Mathf.RoundToInt(transform.position.x + 3.5f));
        rank = Mathf.RoundToInt(transform.position.y + 3.5f) + 1;
        Coordinate = file.ToString() + rank.ToString();
    }

    

}
