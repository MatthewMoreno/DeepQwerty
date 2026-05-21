using UnityEngine;
using System.Collections;

public class Promotion : MonoBehaviour
{
    public ChessGame theGame;
    public Piece whitePiece;
    public Piece blackPiece;
    public Army white;
    public Army black;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Promote()
    {
        ChessSquare squ = theGame.selectedPiece.habitat;
        Army theArmy = theGame.selectedPiece.team;
        Piece thePiece = (theArmy.white) ? whitePiece : blackPiece;
        thePiece.theGame = theGame;
        theArmy.pieces.Remove(theGame.selectedPiece);
        GameObject.Destroy(theGame.selectedPiece);
        theArmy.pieces.Add(thePiece);
        thePiece.transform.position = new Vector3(squ.transform.position.x, squ.transform.position.y + thePiece.height, squ.transform.position.z);
        thePiece.habitat = squ;
        thePiece.model.habitat = squ.model;
        squ.inhabitant = thePiece;
        squ.model.inhabitant = thePiece.model;
        thePiece.Initialize();
        theGame.whitesTurn = !theGame.whitesTurn;
        theGame.queenButton.enabled = false;
        theGame.rookButton.enabled = false;
        theGame.bishopButton.enabled = false;
        theGame.knightButton.enabled = false;
    }
}
