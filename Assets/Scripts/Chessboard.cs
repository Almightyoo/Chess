using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chessboard : MonoBehaviour
{
    public PieceCoordinates pieceCoordinates;
    public GameObject squarePrefab;
    public Color lightCol;
    public Color darkCol;
    public Color highlightColor;
    public float ScaleFactor = 1.6f;
    private GameObject lastClickedSquare;
    private string[,] piecePositions = new string[8, 8];
    public bool WhiteToMove = true;
    List<string> whitedoubleMove = new List<string>();
    List<string> blackdoubleMove = new List<string>();

    void Start()
    {
        CreateGraphicalBoard();
        PlacePieces();
        initiatepiecePos();
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //LogListContents(whitedoubleMove);
            //LogListContents(blackdoubleMove);
            GameObject pieceCoordinatesObject = GameObject.Find("PieceCoordinates");
            pieceCoordinates = pieceCoordinatesObject.GetComponent<PieceCoordinates>();
            //Debug.Log("First Click: " + pieceCoordinates.firstClick);
            //Debug.Log("Second Click: " + pieceCoordinates.secondClick);
            //UnityEngine.Vector2 Levy=PositionirlCoordinate(pieceCoordinates.xcor, pieceCoordinates.ycor);
            //string pieceName = piecePositions[(int)Levy.x, (int)Levy.y];
            //Debug.Log(pieceName);
            
            //if (pieceObject != null)
            //{
            //    Debug.Log("Found GameObject with name: " + pieceName);
            //    // Now you can perform actions on the pieceObject
            //}
            //else
            //{
            //    Debug.Log("No GameObject found with name: " + pieceName);
            //}
            if (pieceCoordinates.secondClick != "")
            {
                UnityEngine.Vector2 Levy = PositionirlCoordinate(pieceCoordinates.firstClickxcor, pieceCoordinates.firstClickycor);
                string pieceName = piecePositions[(int)Levy.x, (int)Levy.y];
                string piececol = pieceName.Substring(0, 1);
                if (pieceName != "" )
                {
                    GameObject pieceObject = GameObject.Find(pieceName);
                   
                    //if (pieceObject != null)
                    //{
                    //    Debug.Log("Found " + pieceName);
                    //}
                    //else
                    //{
                    //    Debug.Log("KILL YOURSELF");
                    //}

                    if (pieceObject != null)
                    {
                        
                        if ((WhiteToMove && piececol=="w")||(WhiteToMove==false && piececol == "b"))
                        {
                            
                            UnityEngine.Vector2 Levy2 = PositionirlCoordinate(pieceCoordinates.secondClickxcor, pieceCoordinates.secondClickycor);
                            string secPiece=piecePositions[(int)Levy2.x, (int)Levy2.y];
                            //Debug.Log( "IsMOveValid: " + IsMoveValid(pieceObject, pieceCoordinates.firstClickxcor, pieceCoordinates.firstClickycor, pieceCoordinates.secondClickxcor, pieceCoordinates.secondClickycor, piecePositions)+ "secPiece: " + secPiece);
                            //Debug.Log(WhiteToMove);
                            if (BlockCheck(pieceObject, pieceCoordinates.firstClickxcor, pieceCoordinates.firstClickycor, pieceCoordinates.secondClickxcor, pieceCoordinates.secondClickycor, WhiteToMove) && IsMoveValid(pieceObject, pieceCoordinates.firstClickxcor, pieceCoordinates.firstClickycor, pieceCoordinates.secondClickxcor, pieceCoordinates.secondClickycor, piecePositions))
                            {


                                if (secPiece == "")
                                {
                                    movePiece(pieceObject, pieceCoordinates.secondClickxcor, pieceCoordinates.secondClickycor);
                                    piecePositions[(int)Levy2.x, (int)Levy2.y] = pieceName;
                                    piecePositions[(int)Levy.x, (int)Levy.y] = "";

                                    if (piececol == "b")
                                    {
                                        WhiteToMove = true;
                                    }
                                    else
                                    {
                                        WhiteToMove = false;
                                    }
                                    
                                }
                                if (secPiece != "" )
                                {
                                    string secPieceCol = secPiece.Substring(0, 1);
                                    if (secPieceCol != piececol)
                                    {
                                        PieceCapture(secPiece);
                                        movePiece(pieceObject, pieceCoordinates.secondClickxcor, pieceCoordinates.secondClickycor);
                                        piecePositions[(int)Levy2.x, (int)Levy2.y] = pieceName;
                                        UnityEngine.Vector2 Levy3 = PositionirlCoordinate(pieceCoordinates.firstClickxcor, pieceCoordinates.firstClickycor);
                                        piecePositions[(int)Levy3.x, (int)Levy3.y] = "";

                                        if (piececol == "b")
                                        {
                                            WhiteToMove = true;
                                        }
                                        else
                                        {
                                            WhiteToMove = false;
                                        }
                                        
                                    }
                                }   
                            }
                        }
                        
                    }
                    else
                    {
                        Debug.Log("No GameObject found with name: " + pieceName);
                    }

                }
            }
        }
    }
    void LogListContents(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log("Element " + i + ": " + list[i]);
        }
    }

    public bool BlockCheck(GameObject piece, int firstclickxcor, int firstclickycor, int secondclickxcor, int secondclickycor, bool blackmoved)
    {
        string pieceName = piece.name;
        char abspiecetype = pieceName[1];
        //Debug.Log(pieceName);
        int x2 = 3-firstclickycor;
        int y2 = 4+firstclickxcor;
        int x3 = 3 - secondclickycor;
        int y3 = 4 + secondclickxcor;
        //Debug.Log("firstClick pos: "+ x2 + "," + y2);
        //Debug.Log("secondCLickPosition: "+x3 + "," + y3);

        string[,] tempPiecePositions = new string[8, 8];

        string kingType = blackmoved ? "wK" : "bK";
        //Debug.Log(kingType);
        UnityEngine.Vector2 kingPosition = FindPiecePosition(kingType);
        int x1 = (int)kingPosition.x;
        int y1 = (int)kingPosition.y;
        //Debug.Log("KingPos: "+x1 + "+" + y1);
        int y1cor = 3 - x1;
        int x1cor = y1 - 4;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                tempPiecePositions[i, j] = piecePositions[i, j];
            }
        }
        //Debug.Log("I'm in 3");
        tempPiecePositions[(int)x3, (int)y3] = pieceName;
        tempPiecePositions[(int)x2, (int)y2] = "";

        if (abspiecetype == 'K')
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    int ycor = 3 - x;
                    int xcor = y - 4;

                    if (tempPiecePositions[x, y] != "" && tempPiecePositions[x, y][0] != kingType[0])
                    {
                        //Debug.Log(x + "," + y + ", " + tempPiecePositions[x, y]);
                        //Debug.Log(xcor + "+" + ycor);
                        GameObject opponentPiece = GameObject.Find(tempPiecePositions[x, y]);
                        //Debug.Log(opponentPiece.name);
                        if (IsMoveValid(opponentPiece, xcor, ycor, secondclickxcor, secondclickycor, tempPiecePositions))
                        {
                            //Debug.Log("I'm in 5");

                            return false;

                        }
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    int ycor = 3 - x;
                    int xcor = y - 4;

                    if (tempPiecePositions[x, y] != "" && tempPiecePositions[x, y][0] != kingType[0])
                    {
                        //Debug.Log(x + "," + y + ", " + tempPiecePositions[x, y]);
                        //Debug.Log(xcor + "+" + ycor);
                        GameObject opponentPiece = GameObject.Find(tempPiecePositions[x, y]);
                        //Debug.Log(opponentPiece.name);
                        if (IsMoveValid(opponentPiece, xcor, ycor, x1cor, y1cor, tempPiecePositions))
                        {
                            //Debug.Log("I'm in 5");

                            return false;
                        }
                    }
                }
            }
        }

        
        return true;
    }


    //public bool IsKingInCheck(bool blackmoved)
    //{
    //    string kingType = blackmoved ? "wK" : "bK";
    //    UnityEngine.Vector2 kingPosition = FindPiecePosition(kingType);
    //    int x1 = (int)kingPosition.x;
    //    int y1 = (int) kingPosition.y;
    //    int y1cor = 3 - x1;
    //    int x1cor = y1 - 4;
    //    for (int x = 0; x < 8; x++)
    //    {
    //        for (int y = 0; y < 8; y++)
    //        {
    //            int ycor = 3 - x;
    //            int xcor = y - 4;

    //            if (piecePositions[x, y] != "" && piecePositions[x, y][0] != kingType[0])
    //            {
    //                GameObject opponentPiece = GameObject.Find(piecePositions[x, y]);
    //                if (IsMoveValid(opponentPiece, xcor, ycor, x1cor, y1cor, piecePositions))
    //                {
    //                    //Debug.Log("King is under attack");
    //                    return true;
                        
    //                }
    //            }
    //        }
    //    }
    //    return false;
    //}

    UnityEngine.Vector2 FindPiecePosition(string pieceType)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (piecePositions[x, y] == pieceType)
                {
                    return new UnityEngine.Vector2(x, y);
                }
            }
        }
        return new UnityEngine.Vector2(-1, -1);
    }

    public void PieceCapture(string capturedPiece)
    {
        

        GameObject pieceObject = GameObject.Find(capturedPiece);
        Destroy(pieceObject);
    }

    

    public bool IsMoveValid(GameObject piece, int firstclickxcor, int firstclickycor, int secondclickxcor,int secondclickycor, string[,] piecePositions)
    {
        string pieceName = piece.name;
        string piecetype = pieceName.Substring(0, 2);
        char abspiecetype = pieceName[1];
        char pieceCol = pieceName[0];
        
        
        //bool pieceColBool;
        //if (pieceCol == 'w') {
        //    pieceColBool = true;
        //}
        //else {
        //    pieceColBool = false;
        //}
        UnityEngine.Vector2 firstirlCor = PositionirlCoordinate(firstclickxcor,firstclickycor);
        UnityEngine.Vector2 secondirlCor = PositionirlCoordinate(secondclickxcor,secondclickycor);
        string firstP = piecePositions[(int)firstirlCor.x, (int)firstirlCor.y];
        string secondP = piecePositions[(int)secondirlCor.x, (int)secondirlCor.y];
        int x1 = 3 - firstclickycor;
        int y1 = 4 + firstclickxcor;
        int x2 = 3 - secondclickycor;
        int y2 = 4 + secondclickxcor;
        //Debug.Log("ypp "+ x1 + y1 + x2 + y2);
        
        if (piecetype == "wP")
        {
            int xDiff = secondclickxcor - firstclickxcor;
            int yDiff = secondclickycor - firstclickycor;
            if(xDiff==0 && yDiff == 1 && IsNoPieceInFrontforwp((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y,piecePositions))
            {
                return true;
            }
            if(xDiff==0 && yDiff==2 && firstclickycor == -3 && IsNoPieceInFrontforwp((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                whitedoubleMove.Add(pieceName);
                Debug.Log(whitedoubleMove);
                return true;
                
            }
            if (Mathf.Abs(xDiff) == 1 && yDiff == 1 && Captureforpawn((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                return true;
            }

            //if (x1 == 3)
            //{
            //    if (y1 == 0)
            //    {
            //        if (piecePositions[x1, y1 + 1] != "")
            //        {
            //            if (blackdoubleMove.Contains(piecePositions[x1, y1 + 1]))
            //            {
            //                if (Mathf.Abs(xDiff) == 1 && yDiff == 1)
            //                {
            //                    GameObject opponentPawn = GameObject.Find(piecePositions[x1, y2]);
            //                    Debug.Log("En passant available");
            //                }
            //            }
            //        }
            //    }

            //    if (y1 > 0 && y1 < 7)
            //    {
            //        if (piecePositions[x1, y1 - 1] != "" || piecePositions[x1, y1 + 1] != "")
            //        {
            //            if (blackdoubleMove.Contains(piecePositions[x1, y1 - 1]) || blackdoubleMove.Contains(piecePositions[x1, y1 + 1]))
            //            {
            //                if (Mathf.Abs(xDiff) == 1 && yDiff == 1)
            //                {
            //                    GameObject opponentPawn = GameObject.Find(piecePositions[x1, y2]);
            //                    Debug.Log("En passant available");
            //                }
            //            }
            //        }
            //    }

            //    if (y1 == 7)
            //    {
            //        if (piecePositions[x1, y1 - 1] != "")
            //        {
            //            if (blackdoubleMove.Contains(piecePositions[x1, y1 - 1]))
            //            {
            //                if (Mathf.Abs(xDiff) == 1 && yDiff == 1)
            //                {
            //                    GameObject opponentPawn = GameObject.Find(piecePositions[x1, y2]);
            //                    Debug.Log("En passant available");
            //                }
            //            }
            //        }
            //    }

            //}





        }
        if (piecetype == "bP")
        {
            int xDiff = secondclickxcor - firstclickxcor;
            int yDiff = secondclickycor - firstclickycor;
            if (xDiff == 0 && yDiff == -1 && IsNoPieceInFrontforbp((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                return true;
            }
            if (xDiff == 0 && yDiff == -2 && firstclickycor == 2 && IsNoPieceInFrontforbp((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                blackdoubleMove.Add(pieceName);
                return true;
            }
            if (Mathf.Abs(xDiff)==1 && yDiff == -1 && Captureforpawn((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                return true;
            }

            //if (x1 == 4)
            //{
            //    if (y1 == 0)
            //    {
            //        if (piecePositions[x1, y1 + 1] != "")
            //        {
            //            if (blackdoubleMove.Contains(piecePositions[x1, y1 + 1]))
            //            {
            //                if (Mathf.Abs(xDiff) == 1 && yDiff == -1)
            //                {
            //                    GameObject opponentPawn = GameObject.Find(piecePositions[x1, y2]);
            //                    Debug.Log("En passant available");
            //                }
            //            }
            //        }
            //    }

            //    if (y1 > 0 && y1 < 7)
            //    {
            //        if (piecePositions[x1, y1 - 1] != "" || piecePositions[x1, y1 + 1] != "")
            //        {
            //            if (blackdoubleMove.Contains(piecePositions[x1, y1 - 1]) || blackdoubleMove.Contains(piecePositions[x1, y1 + 1]))
            //            {
            //                if (Mathf.Abs(xDiff) == 1 && yDiff == -1)
            //                {
            //                    GameObject opponentPawn = GameObject.Find(piecePositions[x1, y2]);
            //                    Debug.Log("En passant available");
            //                }
            //            }
            //        }
            //    }

            //    if (y1 == 7)
            //    {
            //        if (piecePositions[x1, y1 - 1] != "")
            //        {
            //            if (blackdoubleMove.Contains(piecePositions[x1, y1 - 1]))
            //            {
            //                if (Mathf.Abs(xDiff) == 1 && yDiff == -1)
            //                {
            //                    GameObject opponentPawn = GameObject.Find(piecePositions[x1, y2]);
            //                    Debug.Log("En passant available");
            //                }
            //            }
            //        }
            //    }
            //}
            

        }
        if (piecetype == "wR")
        {
            int xDiff = secondclickxcor - firstclickxcor;
            int yDiff = secondclickycor - firstclickycor;
            if(((xDiff!=0 && yDiff==0)||(xDiff==0 && yDiff != 0))&& IsNoObstacleforRook((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                return true;
            }
        }
        if (piecetype == "bR")
        {
            int xDiff = secondclickxcor - firstclickxcor;
            int yDiff = secondclickycor - firstclickycor;
            if (((xDiff != 0 && yDiff == 0) || (xDiff == 0 && yDiff != 0))&& IsNoObstacleforRook((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                return true;
            }
        }
        if(piecetype == "wN")
        {
            int xDiff = secondclickxcor - firstclickxcor;
            int yDiff = secondclickycor - firstclickycor;
            if((Mathf.Abs(xDiff)==2 && Mathf.Abs(yDiff)==1) || (Mathf.Abs(xDiff) == 1 && Mathf.Abs(yDiff) == 2))
            {
                return true;
            }
        }
        if (piecetype == "bN")
        {
            int xDiff = secondclickxcor - firstclickxcor;
            int yDiff = secondclickycor - firstclickycor;
            if ((Mathf.Abs(xDiff) == 2 && Mathf.Abs(yDiff) == 1) || (Mathf.Abs(xDiff) == 1 && Mathf.Abs(yDiff) == 2))
            {
                return true;
            }
        }
        if(piecetype == "wB")
        {
            int xDiff = secondclickxcor - firstclickxcor;
            int yDiff = secondclickycor - firstclickycor;
            if ((Mathf.Abs(xDiff) == Mathf.Abs(yDiff)) && IsNoObstacleforBishop((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                return true;
            }

        }
        if (piecetype == "bB")
        {
            int xDiff = secondclickxcor - firstclickxcor;
            int yDiff = secondclickycor - firstclickycor;
            if ((Mathf.Abs(xDiff) == Mathf.Abs(yDiff)) && IsNoObstacleforBishop((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                return true;
            }

        }
        if(abspiecetype == 'Q')
        {
            int xDiff = secondclickxcor - firstclickxcor;
            int yDiff = secondclickycor - firstclickycor;
            if (((xDiff != 0 && yDiff == 0) || (xDiff == 0 && yDiff != 0)) && IsNoObstacleforRook((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                return true;
            }
            if ((Mathf.Abs(xDiff) == Mathf.Abs(yDiff)) && IsNoObstacleforBishop((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                return true;
            }

        }
        if(abspiecetype == 'K')
        {
            int xDiff = secondclickxcor - firstclickxcor;
            int yDiff = secondclickycor - firstclickycor;
            if(((Mathf.Abs(xDiff) == 1 && yDiff ==0)||(Mathf.Abs(yDiff)==1 && xDiff == 0)) && IsNoObstacleforRook((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                return true;
            }
            if (Mathf.Abs(xDiff) == Mathf.Abs(yDiff) && (Mathf.Abs(xDiff) == 1 && Mathf.Abs(yDiff) == 1) && IsNoObstacleforBishop((int)firstirlCor.x, (int)firstirlCor.y, (int)secondirlCor.x, (int)secondirlCor.y, piecePositions))
            {
                return true;
            }

        }
        return false;
    }

    //public void blockCheck(bool isWhiteChecked)
    //{
    //    string kingType = isWhiteChecked ? "wK" : "bK";

    //}

    public bool Captureforpawn(int x1,int y1,int x2, int y2, string[,] piecePositions)
    {
        string capturedpawn = piecePositions[x2, y2];
        string selectedpawn = piecePositions[x1, y1];
        string selectedpiececol = selectedpawn.Substring(0, 1);
        string capturedpiececol = capturedpawn.Substring(0, 1);
        if (capturedpawn != "" && selectedpiececol!=capturedpiececol)
        {
            return true;
        }
        return false;
    }

    

    public bool IsNoPieceInFrontforbp(int x1,int y1,int x2,int y2, string[,] piecePositions)
    {
        for (int i = x1+1; i <= x2; i++)
        {
            if (piecePositions[i, y1] != "")
            {
                return false;
            }
            
            
        }
        return true;

    }
    public bool IsNoPieceInFrontforwp(int x1, int y1, int x2, int y2, string[,] piecePositions)
    {
        for (int i = x1-1; i >= x2; i--)
        {
            if (piecePositions[i, y1] != "")
            {
                return false;

            }
        }
        return true;

    }

    public bool IsNoObstacleforRook(int x1,int y1, int x2, int y2, string[,] piecePositions)
    {
        if (x1 == x2)
        {
            if (y2 > y1)
            {
                for(int i=y1+1; i < y2; i++)
                {
                    if (piecePositions[x1, i] != "")
                    {
                        return false;
                    }
                }
            }
            if (y1 > y2)
            {
                for (int i = y1 - 1; i > y2; i--)
                {
                    if (piecePositions[x1, i] != "")
                    {
                        return false;
                    }
                }
            }
        }
        if (y1 == y2)
        {
            if (x2 > x1)
            {
                for (int i = x1 + 1; i < x2; i++)
                {
                    if (piecePositions[i, y1] != "")
                    {
                        return false;

                    }
                }
            }
            if (x2 < x1)
            {
                for (int i = x1 - 1; i > x2; i--)
                {
                        if (piecePositions[i, y1] != "")
                    {
                        return false;

                    }
                }
            }
        }
        return true;

        
    }

    public bool IsNoObstacleforBishop(int x1, int y1, int x2, int y2, string[,] piecePositions)
    {

        if ((x1 > x2) && (y1 < y2))
        {
            for (int i = 1; i < Mathf.Abs(x1-x2); i++)
            {
                if (piecePositions[x1-i, y1+i] != "")
                {
                    return false;

                }
            }
        }
        if ((x1 < x2) && (y1 < y2))
        {
            for (int i = 1; i < Mathf.Abs(x1 - x2); i++)
            {
                if (piecePositions[x1 + i, y1 + i] != "")
                {
                    return false;

                }
            }
        }
        if ((x1 < x2) && (y1 > y2))
        {
            for (int i = 1; i < Mathf.Abs(x1 - x2); i++)
            {
                if (piecePositions[x1 + i, y1 - i] != "")
                {
                    return false;

                }
            }
        }
        if ((x1 > x2) && (y1 > y2))
        {
            for (int i = 1; i < Mathf.Abs(x1 - x2); i++)
            {
                if (piecePositions[x1 - i, y1 - i] != "")
                {
                    return false;

                }
            }
        }
        return true;
    }

    

    public void movePiece(GameObject piece, int secondclickxcor, int secondclickycor)
    {
        string pieceName = piece.name;
        GameObject pieceObject = piece;
        
        if (pieceObject != null)
        {
            pieceObject.transform.position = new UnityEngine.Vector2(secondclickxcor, secondclickycor);
        }
        else
        {
            Debug.Log("No piece found Kill yourself");
        }
    }

    UnityEngine.Vector2 PositionirlCoordinate(int xcor, int ycor)
    {
        int x = 3-ycor;
        int y = xcor+4;
        return new UnityEngine.Vector2(x, y);
    }

    public void initiatepiecePos()
    {
        piecePositions[0, 0] = "bRL";
        piecePositions[0, 1] = "bNL";
        piecePositions[0, 2] = "bBL";
        piecePositions[0, 3] = "bQ";
        piecePositions[0, 4] = "bK";
        piecePositions[0, 5] = "bBR";
        piecePositions[0, 6] = "bNR";
        piecePositions[0, 7] = "bRR";

        for (int i = 0; i < 8; i++)
        {
            piecePositions[1, i] = "bP" + (i + 1);
        }

        for (int rank = 2; rank < 6; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                piecePositions[rank, file] = "";
            }
        }

        piecePositions[7, 0] = "wRL";
        piecePositions[7, 1] = "wNL";
        piecePositions[7, 2] = "wBL";
        piecePositions[7, 3] = "wQ";
        piecePositions[7, 4] = "wK";
        piecePositions[7, 5] = "wBR";
        piecePositions[7, 6] = "wNR";
        piecePositions[7, 7] = "wRR";

        for (int i = 0; i < 8; i++)
        {
            piecePositions[6, i] = "wP" + (i + 1);
        }
    }




    void CreateGraphicalBoard()
    {
        GameObject mainBoard = new GameObject("MainBoard");

        for (int file = 0; file < 8; file++)
        {
            for (int rank = 0; rank < 8; rank++)
            {
                bool isLightSquare = (file + rank) % 2 != 0;
                Color squareColour = isLightSquare ? lightCol : darkCol;
                UnityEngine.Vector2 position = new UnityEngine.Vector2(-3.5f + file, -3.5f + rank);

                GameObject square = DrawSquare(squareColour, position, mainBoard.transform);

                
                square.GetComponent<SquareProperties>().originalColor = squareColour;

                square.AddComponent<SquareClickHandler>().chessboard = this;
            }
        }
    }

    void PlacePieces()
    {
        GameObject chessPiecesParent = new GameObject("ChessPieces");
        GameObject wRL = InstantiatePiece("wR", 'a', 1);
        wRL.transform.parent = chessPiecesParent.transform;
        GameObject wNL = InstantiatePiece("wN", 'b', 1);
        wNL.transform.parent = chessPiecesParent.transform;
        GameObject wBL = InstantiatePiece("wB", 'c', 1);
        wBL.transform.parent = chessPiecesParent.transform;
        GameObject wQ  = InstantiatePiece("wQ", 'd', 1);
        wQ.transform.parent = chessPiecesParent.transform;
        GameObject wK  = InstantiatePiece("wK", 'e', 1);
        wK.transform.parent = chessPiecesParent.transform;
        GameObject wBR = InstantiatePiece("wB", 'f', 1);
        wBR.transform.parent = chessPiecesParent.transform;
        GameObject wNR = InstantiatePiece("wN", 'g', 1);
        wNR.transform.parent = chessPiecesParent.transform;
        GameObject wRR = InstantiatePiece("wR", 'h', 1);
        wRR.transform.parent = chessPiecesParent.transform;

        GameObject wP1 = InstantiatePiece("wP", 'a', 2);
        wP1.transform.parent = chessPiecesParent.transform;
        GameObject wP2 = InstantiatePiece("wP", 'b', 2);
        wP2.transform.parent = chessPiecesParent.transform;
        GameObject wP3 = InstantiatePiece("wP", 'c', 2);
        wP3.transform.parent = chessPiecesParent.transform;
        GameObject wP4 = InstantiatePiece("wP", 'd', 2);
        wP4.transform.parent = chessPiecesParent.transform;
        GameObject wP5 = InstantiatePiece("wP", 'e', 2);
        wP5.transform.parent = chessPiecesParent.transform;
        GameObject wP6 = InstantiatePiece("wP", 'f', 2);
        wP6.transform.parent = chessPiecesParent.transform;
        GameObject wP7 = InstantiatePiece("wP", 'g', 2);
        wP7.transform.parent = chessPiecesParent.transform;
        GameObject wP8 = InstantiatePiece("wP", 'h', 2);
        wP8.transform.parent = chessPiecesParent.transform;


        GameObject bRL = InstantiatePiece("bR", 'a', 8);
        bRL.transform.parent = chessPiecesParent.transform;
        GameObject bNL = InstantiatePiece("bN", 'b', 8);
        bNL.transform.parent = chessPiecesParent.transform;
        GameObject bBL = InstantiatePiece("bB", 'c', 8);
        bBL.transform.parent = chessPiecesParent.transform;
        GameObject bQ  = InstantiatePiece("bQ", 'd', 8);
        bQ.transform.parent = chessPiecesParent.transform;
        GameObject bK  = InstantiatePiece("bK", 'e', 8);
        bK.transform.parent = chessPiecesParent.transform;
        GameObject bBR = InstantiatePiece("bB", 'f', 8);
        bBR.transform.parent = chessPiecesParent.transform;
        GameObject bNR = InstantiatePiece("bN", 'g', 8);
        bNR.transform.parent = chessPiecesParent.transform;
        GameObject bRR = InstantiatePiece("bR", 'h', 8);
        bRR.transform.parent = chessPiecesParent.transform;

        GameObject bP1 = InstantiatePiece("bP", 'a', 7);
        bP1.transform.parent = chessPiecesParent.transform;
        GameObject bP2 = InstantiatePiece("bP", 'b', 7);
        bP2.transform.parent = chessPiecesParent.transform;
        GameObject bP3 = InstantiatePiece("bP", 'c', 7);
        bP3.transform.parent = chessPiecesParent.transform;
        GameObject bP4 = InstantiatePiece("bP", 'd', 7);
        bP4.transform.parent = chessPiecesParent.transform;
        GameObject bP5 = InstantiatePiece("bP", 'e', 7);
        bP5.transform.parent = chessPiecesParent.transform;
        GameObject bP6 = InstantiatePiece("bP", 'f', 7);
        bP6.transform.parent = chessPiecesParent.transform;
        GameObject bP7 = InstantiatePiece("bP", 'g', 7);
        bP7.transform.parent = chessPiecesParent.transform;
        GameObject bP8 = InstantiatePiece("bP", 'h', 7);
        bP8.transform.parent = chessPiecesParent.transform;
      

    }

    GameObject InstantiatePiece(string pieceName, char file, int rank)
    {
        GameObject piece;
        int xPos = file - 'a';
        int yPos = rank - 1;

        UnityEngine.Vector3 position = new UnityEngine.Vector3(xPos - 4, yPos - 4, 0f);

        Texture2D texture = Resources.Load<Texture2D>(pieceName);

        if (texture == null)
        {
            Debug.LogError("Texture not found: " + pieceName);
            return null;
        }

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), UnityEngine.Vector2.zero, 100f);

        string pieceFullName = pieceName;
        if (pieceName == "wP" || pieceName == "bP")
        {
            int x = xPos + 1;
            pieceFullName = pieceName + x;
        }
        else if (pieceName == "wR" || pieceName == "bR")
        {
            if (file == 'a')
            {
                pieceFullName = pieceName + 'L';
            }
            else if (file == 'h')
            {
                pieceFullName = pieceName + 'R';
            }
        }
        else if ((pieceName == "wN" || pieceName == "bN") && (file == 'b' || file == 'g'))
        {
            pieceFullName = pieceName + (file == 'b' ? 'L' : 'R');
        }
        else if ((pieceName == "wB" || pieceName == "bB") && (file == 'c' || file == 'f'))
        {
            pieceFullName = pieceName + (file == 'c' ? 'L' : 'R');
        }

        piece = new GameObject(pieceFullName);

        piece.transform.position = position;

        SpriteRenderer renderer = piece.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        renderer.sortingLayerName = "Pieces";
        renderer.sortingOrder = 1;
        piece.transform.localScale = new UnityEngine.Vector3(ScaleFactor, ScaleFactor, 1f);
        piece.transform.parent = transform;

        return piece;
    }







    GameObject DrawSquare(Color color, UnityEngine.Vector2 position, Transform parent)
    {
        GameObject square = Instantiate(squarePrefab, position, UnityEngine.Quaternion.identity);
        square.GetComponent<SpriteRenderer>().color = color;
        square.transform.parent = parent;
        return square;
    }

    public void HighlightSquare(GameObject square)
    {
        if (lastClickedSquare != null)
        {
            lastClickedSquare.GetComponent<SpriteRenderer>().color = lastClickedSquare.GetComponent<SquareProperties>().originalColor;
        }
        square.GetComponent<SpriteRenderer>().color = highlightColor;
        lastClickedSquare = square;
    }  
}
