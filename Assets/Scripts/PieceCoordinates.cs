using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PieceCoordinates : MonoBehaviour
{
    public char file = 'a';
    public int rank = 1;
    public string Coordinate="";
    public string firstClick="";
    public string secondClick="";
    public int fileindex = -1;
    public int rankindex = -1;
    public int firstClickxcor=-11;
    public int firstClickycor=-11;
    public int secondClickxcor=-11;
    public int secondClickycor=-11;
    public bool LookingforsecClick = false;
    public int xcor;
    public int ycor;

    public void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        { 
            UnityEngine.Vector2 mousePosition = Input.mousePosition;
            UnityEngine.Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new UnityEngine.Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
            worldPosition.z = 0f;
            UnityEngine.Vector2 worldPos = new UnityEngine.Vector2(worldPosition.x, worldPosition.y);
            //Debug.Log(worldPos);
            
            xcor = (Mathf.RoundToInt(worldPos.x + 3.5f)) - 4;
            //Debug.Log(xcor);
            //Debug.Log(ycor);
            file = (char)('a' + Mathf.RoundToInt(worldPos.x + 3.5f));
            rank = Mathf.RoundToInt(worldPos.y + 3.5f) + 1;
            ycor = rank - 5;
            

            if (file >= 'a' && file <= 'h' && rank >= 1 && rank <= 8)
            {
                Coordinate = file.ToString() + rank.ToString();
            }
            else
            {
                Coordinate = "";
            }
            if (Coordinate != "")
            {
                if (LookingforsecClick)
                {
                    secondClick = Coordinate;
                    secondClickxcor = xcor;
                    secondClickycor = ycor;
                    LookingforsecClick = false;
                }
                else
                {
                    firstClick = Coordinate;
                    firstClickxcor = xcor;
                    firstClickycor = ycor;
                    secondClickxcor = -11;
                    secondClickycor = -11;
                    secondClick = "";

                    LookingforsecClick = true;
                }
            }
            //Debug.Log(firstClickxcor);
            //Debug.Log(firstClickycor);
            //Debug.Log(secondClickxcor);
            //Debug.Log(secondClickycor);
            Debug.Log("firstClick: " + firstClick);
            Debug.Log("secondClick: " + secondClick);
        }
    }
}
