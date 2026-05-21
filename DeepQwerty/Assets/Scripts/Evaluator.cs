using UnityEngine;
using System.Collections;

public class Evaluator : ScriptableObject
{

    public static ChessGame theGame;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public static float EvaluatePosition(ChessBoardModel board)
	{
		float res = 0f;
		foreach(PieceModel p in board.white.pieces)
		{
			p.FindValue();
			res += p.realValue;
		}
		foreach(PieceModel p in board.black.pieces)
		{
			p.FindValue();
			res -= p.realValue;
		}
		return res;
	}

	public static float EvaluateMove(ChessBoardModel board, Move move)
	{
		return move.moveGain;
	}

	public static bool IsCheck(ChessBoardModel board, bool white)
	{
		ChessSquareModel target = null;
		ArmyModel offense = (white)? board.black : board.white;
		ArmyModel defense = (white)? board.white : board.black;
		foreach(PieceModel p in defense.pieces)
		{
			if(p is KingModel)
			{
				target = p.habitat;
			}
		}
        if(target == null)
        {
            return true;
        }
		foreach(Move m in offense.getAllSimpleMoveOptions())
		{
			if(target.location[0] == m.targetSquare.location[0] && target.location[1] == m.targetSquare.location[1])
			{
				return true;
			}
            board.theGame.FreeMove(m);
		}
		return false;
	}
}
