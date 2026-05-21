using UnityEngine;
using System.Collections;

public class KingModel : PieceModel
{

    // Use this for initialization
    void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public static KingModel GetKingModel(bool color, int[] loc, ChessSquareModel squ, ChessBoardModel b)
	{
		KingModel res = ScriptableObject.CreateInstance<KingModel>();
		res.white = color;
		res.location = loc;
		res.habitat = squ;
		res.theBoard = b;
		res.moveStructure = new int[8][];
		for(int i = 0; i < 8; i++)
		{
			res.moveStructure[i] = new int[2];
		}
		res.moveStructure[0][0] = -1;
		res.moveStructure[0][1] = 1;
		res.moveStructure[1][0] = 0;
		res.moveStructure[1][1] = 1;
		res.moveStructure[2][0] = 1;
		res.moveStructure[2][1] = 1;
		res.moveStructure[3][0] = 1;
		res.moveStructure[3][1] = 0;
		res.moveStructure[4][0] = 1;
		res.moveStructure[4][1] = -1;
		res.moveStructure[5][0] = 0;
		res.moveStructure[5][1] = -1;
		res.moveStructure[6][0] = -1;
		res.moveStructure[6][1] = -1;
        res.moveStructure[7][0] = -1;
        res.moveStructure[7][1] = 0;
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
			if(checkSquare == null)
			{
				
			}
			else if(checkSquare.isFree())
			{
				res.Add(Move.GetMove(this, checkSquare, before));
			}
			else if(checkSquare.inhabitant.white != this.white)
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
			if(checkSquare == null)
			{
				
			}
			else if(checkSquare.isFree())
			{
				res.Add(Move.GetMove(this, checkSquare));
			}
			else if(checkSquare.inhabitant.white != this.white)
			{
				res.Add(Move.GetMove(this, checkSquare));
			}
		}
		return res;
	}

	public override PieceModel Clone()
	{
		return GetKingModel(white, location, habitat, theBoard);
	}

	public override float GetPositionValue()
	{
		return 0f;
	}

	public override void SetMoveStructure()
	{
		moveStructure = new int[8][];
		for(int i = 0; i < 8; i++)
		{
			moveStructure[i] = new int[2];
		}
		moveStructure[0][0] = -1;
		moveStructure[0][1] = 1;
		moveStructure[1][0] = 0;
		moveStructure[1][1] = 1;
		moveStructure[2][0] = 1;
		moveStructure[2][1] = 1;
		moveStructure[3][0] = 1;
		moveStructure[3][1] = 0;
		moveStructure[4][0] = 1;
		moveStructure[4][1] = -1;
		moveStructure[5][0] = 0;
		moveStructure[5][1] = -1;
		moveStructure[6][0] = -1;
		moveStructure[6][1] = -1;
		moveStructure[7][0] = -1;
		moveStructure[7][1] = 0;
	}
}
