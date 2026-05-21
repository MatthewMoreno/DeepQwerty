using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ChessSquare : MonoBehaviour, IPointerClickHandler
{
	public Piece inhabitant;
	public int[] location;
	public bool isLight;
	public ChessBoard theBoard;
	public Material mat;
	public Material highlighted;
	public bool isHighlighted;
	public ChessSquareModel model;
    public ChessGame theGame;

	// Use this for initialization
	void Start ()
	{
		isHighlighted = false;
		model = ScriptableObject.CreateInstance<ChessSquareModel>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void Initialize()
	{
		model.inhabitant = (inhabitant == null)? null : inhabitant.model;
		model.location = location;
		model.isLight = isLight;
		model.theBoard = theBoard.model;
        model.theGame = theGame;
	}

	public void addPiece(Piece thePiece)
	{
        removePiece();
		inhabitant = thePiece;
		thePiece.location = location;
		thePiece.habitat = this;
	}

	public void removePiece()
	{
        inhabitant = null;
	}

	public bool isFree()
	{
		return inhabitant == null;
	}

	public void Highlight()
	{
		GetComponentInChildren<MeshRenderer>().material = highlighted;
        isHighlighted = true;
	}

	public void Unhighlight()
	{
		GetComponentInChildren<MeshRenderer>().material = mat;
        isHighlighted = false;
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isHighlighted)
        {
            theBoard.theGame.selectedPiece.Unhighlight();
            Move nextMove = null;
            bool isKing = theGame.selectedPiece.model is KingModel;
            bool kingSide = theGame.selectedPiece.model.location[0] == 5 && location[0] == 7;
            bool queenSide = theGame.selectedPiece.model.location[0] == 5 && location[0] == 3;
            if (isKing && kingSide)
            {
                Army a = (theGame.selectedPiece.white) ? theBoard.white : theBoard.black;
                nextMove = Move.GetMove(a.model, true);
            }
            else if (isKing && queenSide)
            {
                Army a = (theGame.selectedPiece.white) ? theBoard.white : theBoard.black;
                nextMove = Move.GetMove(a.model, false);
            }
            else if ((location[1] == 1 || location[1] == 8) && theGame.selectedPiece.model is PawnModel)
            {
                Debug.Log("Making Promote Move");
                nextMove = Move.GetMove(theGame.selectedPiece.model, this.model, 0f, "");
            }
            else
            {
                nextMove = Move.GetMove(theGame.selectedPiece.model, this.model, 0f);
            }
            theBoard.Apply(nextMove);
            theGame.selectedPiece = null;
            if (!nextMove.promotes)
            {
                theGame.whitesTurn = !theGame.whitesTurn;
            }
        }
    }
}
