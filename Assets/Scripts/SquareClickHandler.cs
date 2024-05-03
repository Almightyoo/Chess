using UnityEngine;

public class SquareClickHandler : MonoBehaviour
{
    public Chessboard chessboard;

    void Start()
    {
        GameObject chessboardObject = GameObject.Find("Chessboard");
        chessboard = chessboardObject.GetComponent<Chessboard>();    
    }

    void OnMouseDown()
    {
        chessboard.HighlightSquare(gameObject);

    }
}
