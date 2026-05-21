using UnityEngine;
using System.Collections;

public class QueenModel : PieceModel
{
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public static QueenModel GetQueenModel(bool color, int[] loc, ChessSquareModel squ, ChessBoardModel b)
	{
		QueenModel res = ScriptableObject.CreateInstance<QueenModel>();
		res.white = color;
		res.location = loc;
		res.habitat = squ;
		res.theBoard = b;
		int[] spot = new int[2];
		res.moveStructure = new int[56][];
		for(int i = 0; i < 7; i++)
		{
			spot[0] = -i - 1;
			spot[1] = i + 1;
			res.moveStructure[i] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = i + 1;
			spot[1] = i + 1;
			res.moveStructure[i + 7] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = i + 1;
			spot[1] = -i - 1;
			res.moveStructure[i + 14] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = -i - 1;
			spot[1] = -i - 1;
			res.moveStructure[i + 21] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = 0;
			spot[1] = i + 1;
			res.moveStructure[i + 28] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = i + 1;
			spot[1] = 0;
			res.moveStructure[i + 35] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = 0;
			spot[1] = -i - 1;
			res.moveStructure[i + 42] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = -i - 1;
			spot[1] = 0;
			res.moveStructure[i + 49] = spot;
			spot = new int[2];
		}
		res.value = 9;
		return res;
	}

	public override ArrayList GetMoveOptions()
    {
        ArrayList res = new ArrayList();
		float before = Evaluator.EvaluatePosition(theBoard);
		int spot = 0;
		ChessSquareModel checkSquare;
		for(int i = 1; i < 9; i++)
		{
			while(spot < i * 7)
			{
				checkSquare = findMoveSquare(moveStructure[spot]);
				if(checkSquare == null)
				{
					spot = i * 7;
				}
				else if(checkSquare.isFree())
				{
					res.Add(Move.GetMove(this, checkSquare, before));
					spot++;
				}
				else if(checkSquare.inhabitant.white != this.white)
				{
					res.Add(Move.GetMove(this, checkSquare, before));
					spot = i * 7;
				}
				else
				{
					spot = i * 7;
				}
			}
        }
        return RemoveIllegalMoves(res);
    }

	public override ArrayList GetSimpleMoveOptions()
	{
		ArrayList res = new ArrayList();
		int spot = 0;
		ChessSquareModel checkSquare;
		for(int i = 1; i < 9; i++)
		{
			while(spot < i * 7)
			{
				checkSquare = findMoveSquare(moveStructure[spot]);
				if(checkSquare == null)
				{
					spot = i * 7;
				}
				else if(checkSquare.isFree())
				{
					res.Add(Move.GetMove(this, checkSquare));
					spot++;
				}
				else if(checkSquare.inhabitant.white != this.white)
				{
					res.Add(Move.GetMove(this, checkSquare));
					spot = i * 7;
				}
				else
				{
					spot = i * 7;
				}
			}
		}
		return res;
	}

	public override PieceModel Clone()
	{
		return GetQueenModel(white, location, habitat, theBoard);
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
		int[] spot = new int[2];
		moveStructure = new int[56][];
		for(int i = 0; i < 7; i++)
		{
			spot[0] = -i - 1;
			spot[1] = i + 1;
			moveStructure[i] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = i + 1;
			spot[1] = i + 1;
			moveStructure[i + 7] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = i + 1;
			spot[1] = -i - 1;
			moveStructure[i + 14] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = -i - 1;
			spot[1] = -i - 1;
			moveStructure[i + 21] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = 0;
			spot[1] = i + 1;
			moveStructure[i + 28] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = i + 1;
			spot[1] = 0;
			moveStructure[i + 35] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = 0;
			spot[1] = -i - 1;
			moveStructure[i + 42] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)
		{
			spot[0] = -i - 1;
			spot[1] = 0;
			moveStructure[i + 49] = spot;
			spot = new int[2];
		}
	}
}
