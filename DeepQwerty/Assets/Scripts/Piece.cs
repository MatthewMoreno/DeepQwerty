using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerClickHandler
{
	public int[] location;
	public ChessSquare habitat;
	public ChessBoard theBoard;
	public float height;
	public PieceModel model;
	public Material mat;
	public Material highlighted;
	public bool isHighlighted;
	public bool white;
	public enum type {PAWN, ROOK, KNIGHT, BISHOP, QUEEN, KING};
	public type t;
    public Army team;
    public ChessGame theGame;

	// Use this for initialization
	void Start ()
	{
		switch(t)
		{
			case type.PAWN:
				model = ScriptableObject.CreateInstance<PawnModel>();
				model.value = 1f;
				break;
			case type.ROOK:
				model = ScriptableObject.CreateInstance<RookModel>();
				model.value = 5f;
				break;
			case type.KNIGHT:
				model = ScriptableObject.CreateInstance<KnightModel>();
				model.value = 3f;
				break;
			case type.BISHOP:
				model = ScriptableObject.CreateInstance<BishopModel>();
				model.value = 3f;
				break;
			case type.QUEEN:
				model = ScriptableObject.CreateInstance<QueenModel>();
				model.value = 9f;
				break;
			case type.KING:
				model = ScriptableObject.CreateInstance<KingModel>();
				model.value = 3f;
				break;
		}
		isHighlighted = false;
	}

	public void Initialize()
	{
		model.location = location;
		model.habitat = habitat.model;
		model.white = white;
		model.theBoard = theBoard.model;
		model.SetMoveStructure();
        model.team = team.model;
        model.theGame = theGame;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Move(ChessSquare place)
	{
		habitat.inhabitant = null;
		place.addPiece(this);
        model.MoveSquare(place.model);
        transform.position = new Vector3(place.transform.position.x, place.transform.position.y + height, place.transform.position.z);
    }

	public void die()
	{
		habitat.removePiece();
	}

	public void Highlight()
	{
		GetComponentInChildren<MeshRenderer>().material = highlighted;
        foreach (Move move in model.GetMoveOptions())
        {
            ChessSquareModel sq = move.targetSquare;
            theBoard.squares[sq.location[0]][sq.location[1]].Highlight();
            theGame.FreeMove(move);
        }
        if (model is KingModel)
        {
            ArmyModel a = (white) ? theBoard.white.model : theBoard.black.model;
            if(a.castleKing && theBoard.squares[6][location[1]].inhabitant == null && theBoard.squares[7][location[1]].inhabitant == null)
            {
                theBoard.squares[7][location[1]].Highlight();
            }
            if (a.castleQueen && theBoard.squares[2][location[1]].inhabitant == null && theBoard.squares[3][location[1]].inhabitant == null && theBoard.squares[4][location[1]].inhabitant == null)
            {
                theBoard.squares[3][location[1]].Highlight();
            }
        }
        isHighlighted = true;
    }

	public void Unhighlight()
	{
		GetComponentInChildren<MeshRenderer>().material = mat;
        foreach (Move move in model.GetMoveOptions())
        {
            ChessSquareModel sq = move.targetSquare;
            theBoard.squares[sq.location[0]][sq.location[1]].Unhighlight();
            theGame.FreeMove(move);
        }
        if (model is KingModel)
        {
            ArmyModel a = (white) ? theBoard.white.model : theBoard.black.model;
            if (a.castleKing)
            {
                theBoard.squares[7][location[1]].Unhighlight();
            }
            if (a.castleQueen)
            {
                theBoard.squares[3][location[1]].Unhighlight();
            }
        }
        isHighlighted = false;
    }

	public void OnPointerClick (PointerEventData eventData)
	{
        bool yourturn = white && theGame.whiteIsPlayer && theGame.whitesTurn && !theGame.gameOver;
        yourturn = (yourturn) ? true : !white && theGame.blackIsPlayer && !theGame.whitesTurn && !theGame.gameOver;
        if (yourturn)
		{
            if(theGame.selectedPiece != null)
            {
                theGame.selectedPiece.Unhighlight();
            }
            theGame.selectedPiece = this;
			Highlight();
		}
	}
}
