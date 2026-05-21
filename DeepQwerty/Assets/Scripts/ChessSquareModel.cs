using UnityEngine;
using System.Collections;

public class ChessSquareModel : ScriptableObject
{
	public PieceModel inhabitant;
	public int[] location;
	public bool isLight;
	public ChessBoardModel theBoard;
    public ChessGame theGame;
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public ChessSquareModel GetChessSquareModel(PieceModel piece, int[] loc, bool color, ChessBoardModel b)
	{
		ChessSquareModel res = ScriptableObject.CreateInstance<ChessSquareModel>();
		res.inhabitant = piece;
		res.location = loc;
		res.isLight = color;
		res.theBoard = b;
		return res;
	}

	public void addPiece(PieceModel thePiece)
	{
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

	public ChessSquareModel Clone()
	{
		ChessSquareModel res = ScriptableObject.CreateInstance<ChessSquareModel>();
		res.location = location;
		if(inhabitant == null)
		{
			res.inhabitant = null;
		}
		else
		{
			res.inhabitant = inhabitant.Clone();
			res.inhabitant.habitat = res;
            res.inhabitant.theGame = theGame;
		}
		res.isLight = isLight;
        res.theGame = theGame;
		return res;
	}
	
	public static ChessSquareModel operator +(ChessSquareModel start, int[] path)
	{
		ChessSquareModel res = null;
		int[] newSpot = new int[2];
		newSpot[0] = start.location[0] + path[0];
		newSpot[1] = start.location[1] + path[1];
		res = (newSpot[0] >= 0 && newSpot[0] < 8 && newSpot[1] >= 0 && newSpot[1] < 8)? start.theBoard.squares[newSpot[0]][newSpot[1]] : null;
		return res;
	}
}
