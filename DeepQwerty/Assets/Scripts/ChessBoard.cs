using UnityEngine;
using System.Collections;

public class ChessBoard : MonoBehaviour
{
	public Army white;
	public Army black;
	public ChessSquare[] squareList = new ChessSquare[64];
	public ChessSquare[][] squares = new ChessSquare[9][];
	public ChessBoardModel model;
    public ChessGame theGame;

	// Use this for initialization
	void Start ()
	{
		for(int j = 1; j < 9; j++)
		{
			squares[j] = new ChessSquare[9];
		}
		for(int i = 1; i < 9; i++)
		{
			for(int j = 1; j < 9; j++)
			{
				squares[i][j] = squareList[(i - 1) * 8 + (j - 1)];
			}
		}
		model = ScriptableObject.CreateInstance<ChessBoardModel>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void Initialize()
	{
		model.squares = new ChessSquareModel[9][];
		for(int i = 1; i < 9; i++)
		{
			model.squares[i] = new ChessSquareModel[9];
			for(int j = 1; j < 9; j++)
			{
				model.squares[i][j] = squares[i][j].model;
			}
		}
		model.white = white.model;
		model.black = black.model;
        model.theGame = theGame;
	}

	public void Apply(Move move)
	{
		int startFile = move.startSquare.location[0];
		int startRank = move.startSquare.location[1];
		int endFile = move.targetSquare.location[0];
		int endRank = move.targetSquare.location[1];
        if (move.capturedPiece != null)
        {
            Piece captured = squares[move.capturedSquare.location[0]][move.capturedSquare.location[1]].inhabitant;
            Army capturedArmy = captured.team;
            capturedArmy.pieces.Remove(captured);
            capturedArmy.model.pieces.Remove(captured.model);
            captured.transform.position = new Vector3(500, 500, 500);
            Object.Destroy(captured);
        }
        squares[startFile][startRank].inhabitant.Move(squares[endFile][endRank]);
        ArrayList opt = null;
        ArmyModel startingArmy = move.startPiece.team;
        if (move.takesAwayKingside)
        {
            startingArmy.castleKing = false;
        }
        if (move.takesAwayQueenside)
        {
            startingArmy.castleQueen = false;
        }
        if (move.castle)
        {
            squares[5][startRank].inhabitant.Move(squares[endFile + endFile - 5][startRank]);
        }
        opt = startingArmy.enemy.GetAllMoveOptions();
        if(opt.Count == 0)
        {
            if (Evaluator.IsCheck(model, move.startPiece.team.enemy.white))
            {
               theGame.CheckMate(true);
            }
            else
            {
               theGame.StaleMate();
            }
        }
        if(move.promotes)
        {
            if(move.promotion.Equals(""))
            {
                Debug.Log("Promoting");
                theGame.queenButton.gameObject.SetActive(true);
                theGame.rookButton.gameObject.SetActive(true);
                theGame.bishopButton.gameObject.SetActive(true);
                theGame.knightButton.gameObject.SetActive(true);
            }
            else
            {
                if (move.promotion.Equals("Queen"))
                {
                    theGame.queenButton.Promote();
                }
                else if (move.promotion.Equals("Rook"))
                {
                    theGame.rookButton.Promote();
                }
                else if (move.promotion.Equals("Bishop"))
                {
                    theGame.bishopButton.Promote();
                }
                else if (move.promotion.Equals("Knight"))
                {
                    theGame.knightButton.Promote();
                }
            }
        }
        foreach(Move theMove in opt)
        {
            theGame.FreeMove(theMove);
        }
    }
}
