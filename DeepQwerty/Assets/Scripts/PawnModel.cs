using UnityEngine;
using System.Collections;

public class PawnModel : PieceModel
{
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public static PawnModel GetPawnModel(bool color, int[] loc, ChessSquareModel squ, ChessBoardModel b)
	{
		PawnModel res = ScriptableObject.CreateInstance<PawnModel>();
		res.white = color;
		res.location = loc;
		res.habitat = squ;
		res.theBoard = b;
		res.moveStructure = new int[8][];
		for(int i = 0; i < 4; i++)
		{
			res.moveStructure[i] = new int[2];
		}
		res.moveStructure[0][0] = -1;
		res.moveStructure[0][1] = 1;
		res.moveStructure[1][0] = 0;
		res.moveStructure[1][1] = 1;
		res.moveStructure[2][0] = 1;
		res.moveStructure[2][1] = 1;
		res.moveStructure[3][0] = 0;
		res.moveStructure[3][1] = 2;
		res.value = 1;
		if(!res.white)
		{
			res.ChangeToBlack();
		}
		return res;
	}

	private void ChangeToBlack()
	{
		moveStructure[0][1] = -1;
		moveStructure[1][1] = -1;
		moveStructure[2][1] = -1;
		moveStructure[3][1] = -2;
	}

	public override ArrayList GetMoveOptions()
    {
        ArrayList res = new ArrayList();
		float before = Evaluator.EvaluatePosition(theBoard);
		ChessSquareModel checkSquare = findMoveSquare(moveStructure[0]);
		if((checkSquare != null && !checkSquare.isFree()) && checkSquare.inhabitant.white != this.white)
		{
            if(checkSquare.location[1] == 1 || checkSquare.location[1] == 8)
            {
                res.Add(Move.GetMove(this, checkSquare, before, "Queen"));
                res.Add(Move.GetMove(this, checkSquare, before, "Rook"));
                res.Add(Move.GetMove(this, checkSquare, before, "Bishop"));
                res.Add(Move.GetMove(this, checkSquare, before, "Knight"));
            }
            else
            {
                res.Add(Move.GetMove(this, checkSquare, before));
            }
		}
		checkSquare = findMoveSquare(moveStructure[1]);
		if(checkSquare != null && checkSquare.isFree())
		{
            if (checkSquare.location[1] == 1 || checkSquare.location[1] == 8)
            {
                res.Add(Move.GetMove(this, checkSquare, before, "Queen"));
                res.Add(Move.GetMove(this, checkSquare, before, "Rook"));
                res.Add(Move.GetMove(this, checkSquare, before, "Bishop"));
                res.Add(Move.GetMove(this, checkSquare, before, "Knight"));
            }
            else
            {
                res.Add(Move.GetMove(this, checkSquare, before));
            }
            checkSquare = findMoveSquare(moveStructure[3]);
			int startRank = (white)? 2 : 7;
			if(checkSquare != null && checkSquare.isFree() && location[1] == startRank)
			{
                if (checkSquare.location[1] == 1 || checkSquare.location[1] == 8)
                {
                    res.Add(Move.GetMove(this, checkSquare, before, "Queen"));
                    res.Add(Move.GetMove(this, checkSquare, before, "Rook"));
                    res.Add(Move.GetMove(this, checkSquare, before, "Bishop"));
                    res.Add(Move.GetMove(this, checkSquare, before, "Knight"));
                }
                else
                {
                    res.Add(Move.GetMove(this, checkSquare, before));
                }
            }
		}
		checkSquare = findMoveSquare(moveStructure[2]);
		if((checkSquare != null && !checkSquare.isFree()) && checkSquare.inhabitant.white != this.white)
		{
			res.Add(Move.GetMove(this, checkSquare, before));
        }
        return RemoveIllegalMoves(res);
    }

	public override ArrayList GetSimpleMoveOptions()
	{
		ArrayList res = new ArrayList();
		ChessSquareModel checkSquare = findMoveSquare(moveStructure[0]);
		if((checkSquare != null && !checkSquare.isFree()) && checkSquare.inhabitant.white != this.white)
		{
			res.Add(Move.GetMove(this, checkSquare));
		}
		checkSquare = findMoveSquare(moveStructure[1]);
		if(checkSquare != null && checkSquare.isFree())
		{
			res.Add(Move.GetMove(this, checkSquare));
			checkSquare = findMoveSquare(moveStructure[3]);
			int startRow = (white)? 2 : 7;
			if(checkSquare != null && checkSquare.isFree() && location[1] == startRow)
			{
				res.Add(Move.GetMove(this, checkSquare));
			}
		}
		checkSquare = findMoveSquare(moveStructure[2]);
		if((checkSquare != null && !checkSquare.isFree()) && checkSquare.inhabitant.white != this.white)
		{
			res.Add(Move.GetMove(this, checkSquare));
		}
		return res;
	}
	
	public override PieceModel Clone()
	{
		return GetPawnModel(white, location, habitat, theBoard);
	}

	public override float GetPositionValue()
	{
		float res = 0f;
		res += .35f - Mathf.Abs(4.5f - location[0]) / 10f;
		res += (white)? location[1] * .05f : (9 - location[1]) * .05f;
		return res;
	}

	public override void SetMoveStructure()
	{
		moveStructure = new int[8][];
		for(int i = 0; i < 4; i++)
		{
			moveStructure[i] = new int[2];
		}
		moveStructure[0][0] = -1;
		moveStructure[0][1] = 1;
		moveStructure[1][0] = 0;
		moveStructure[1][1] = 1;
		moveStructure[2][0] = 1;
		moveStructure[2][1] = 1;
		moveStructure[3][0] = 0;
		moveStructure[3][1] = 2;
		if(!white)
		{
			ChangeToBlack();
		}
	}
}
