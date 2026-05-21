using UnityEngine;
using System.Collections;

public class KnightModel : PieceModel
{
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public static KnightModel GetKnightModel(bool color, int[] loc, ChessSquareModel squ, ChessBoardModel b)
	{
		KnightModel res = ScriptableObject.CreateInstance<KnightModel>();
		res.white = color;
		res.location = loc;
		res.habitat = squ;
		res.theBoard = b;
		res.moveStructure = new int[8][];
		for(int i = 0; i < 8; i++)
		{
			res.moveStructure[i] = new int[2];
		}
		res.moveStructure[0][0] = -2;
		res.moveStructure[0][1] = 1;
		res.moveStructure[1][0] = -1;
		res.moveStructure[1][1] = 2;
		res.moveStructure[2][0] = 1;
		res.moveStructure[2][1] = 2;
		res.moveStructure[3][0] = 2;
		res.moveStructure[3][1] = 1;
		res.moveStructure[4][0] = 2;
		res.moveStructure[4][1] = -1;
		res.moveStructure[5][0] = 1;
		res.moveStructure[5][1] = -2;
		res.moveStructure[6][0] = -1;
		res.moveStructure[6][1] = -2;
		res.moveStructure[7][0] = -2;
		res.moveStructure[7][1] = -1;
		res.value = 3;
		return res;
	}

	public override ArrayList GetMoveOptions()
    {
        ArrayList res = new ArrayList();
		float before = Evaluator.EvaluatePosition(theBoard);
		ChessSquareModel checkSquare;
		for(int i = 0; i < 8; i++)
		{
			checkSquare = findMoveSquare(moveStructure[i]);
			if(checkSquare != null && checkSquare.isFree())
			{
				res.Add(Move.GetMove(this, checkSquare, before));
			}
			else if(checkSquare != null && checkSquare.inhabitant.white != this.white)
			{
				res.Add(Move.GetMove(this, checkSquare, before));
			}
        }
        return RemoveIllegalMoves(res);
    }

	public override ArrayList GetSimpleMoveOptions()
	{
		ArrayList res = new ArrayList();
		ChessSquareModel checkSquare;
		for(int i = 0; i < 8; i++)
		{
			checkSquare = findMoveSquare(moveStructure[i]);
			if(checkSquare != null && checkSquare.isFree())
			{
				res.Add(Move.GetMove(this, checkSquare));
			}
			else if(checkSquare != null && checkSquare.inhabitant.white != this.white)
			{
				res.Add(Move.GetMove(this, checkSquare));
			}
		}
		return res;
	}

	public override PieceModel Clone()
	{
		return GetKnightModel(white, location, habitat, theBoard);
	}
	
	public override float GetPositionValue()
	{
		float res = 0f;
		res += .7f - Mathf.Abs(4.5f - location[0]) / 5;
		res += .7f - Mathf.Abs(4.5f - location[1]) / 5;
		return res;
	}

	public override void SetMoveStructure()
	{
		moveStructure = new int[8][];
		for(int i = 0; i < 8; i++)
		{
			moveStructure[i] = new int[2];
		}
		moveStructure[0][0] = -2;
		moveStructure[0][1] = 1;
		moveStructure[1][0] = -1;
		moveStructure[1][1] = 2;
		moveStructure[2][0] = 1;
		moveStructure[2][1] = 2;
		moveStructure[3][0] = 2;
		moveStructure[3][1] = 1;
		moveStructure[4][0] = 2;
		moveStructure[4][1] = -1;
		moveStructure[5][0] = 1;
		moveStructure[5][1] = -2;
		moveStructure[6][0] = -1;
		moveStructure[6][1] = -2;
		moveStructure[7][0] = -2;
		moveStructure[7][1] = -1;
	}
}
