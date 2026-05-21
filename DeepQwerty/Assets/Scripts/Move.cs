using UnityEngine;
using System.Collections;

public class Move : ScriptableObject
{
    public static ChessGame theGame;
    public PieceModel startPiece;
	public PieceModel capturedPiece;
	public ChessSquareModel startSquare;
	public ChessSquareModel targetSquare;
	public ChessSquareModel capturedSquare;
	public bool takesAPiece;
	public bool deliversCheck;
	public bool illegal;
    public bool castle;
    public bool takesAwayKingside;
    public bool takesAwayQueenside;
    public bool promotes;
    public string promotion;
	public float evalBefore;
	public float evalAfter;
	public float moveGain;

	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Apply()
    {
        ArmyModel army = startPiece.team;
        ChessSquareModel[][] boardSquares = startPiece.theBoard.squares;
        if (castle)
        {
            bool kingside = (startPiece.location[0] == 8);
            bool white = startPiece.white;
            startPiece.MoveSquare(targetSquare);
            if(white)
            {
                if(kingside)
                {
                    boardSquares[5][1].inhabitant.MoveSquare(boardSquares[7][1]);
                }
                else
                {
                    boardSquares[5][1].inhabitant.MoveSquare(boardSquares[3][1]);
                }
            }
            else
            {
                if(kingside)
                {
                    boardSquares[5][8].inhabitant.MoveSquare(boardSquares[7][8]);
                }
                else
                {
                    boardSquares[5][8].inhabitant.MoveSquare(boardSquares[3][8]);
                }
            }
        }
        else
        {
            if (takesAPiece)
            {
                capturedSquare.inhabitant = null;
                ArmyModel capturedArmy = capturedPiece.team;
                capturedArmy.pieces.Remove(capturedPiece);
                capturedPiece.habitat = null;
            }
            startPiece.MoveSquare(targetSquare);
            if (promotes)
            {
                if (promotion.Equals("Queen"))
                {
                    targetSquare.inhabitant = QueenModel.GetQueenModel(startPiece.white, startPiece.location, targetSquare, targetSquare.theBoard);
                }
                else if (promotion.Equals("Rook"))
                {
                    targetSquare.inhabitant = RookModel.GetRookModel(startPiece.white, startPiece.location, targetSquare, targetSquare.theBoard);
                }
                else if (promotion.Equals("Bishop"))
                {
                    targetSquare.inhabitant = BishopModel.GetBishopModel(startPiece.white, startPiece.location, targetSquare, targetSquare.theBoard);
                }
                else if (promotion.Equals("Knight"))
                {
                    targetSquare.inhabitant = KnightModel.GetKnightModel(startPiece.white, startPiece.location, targetSquare, targetSquare.theBoard);
                }
                army.pieces.Remove(startPiece);
                army.pieces.Add(targetSquare.inhabitant);
            }
        }
        targetSquare.inhabitant.theGame = theGame;
        if (takesAwayKingside)
        {
            army.castleKing = false;
        }
        if (takesAwayQueenside)
        {
            army.castleQueen = false;
        }
    }

	public void Reverse()
    {
        ArmyModel army = startPiece.team;
        ChessSquareModel[][] boardSquares = startPiece.theBoard.squares;
        if (castle)
        {
            bool kingside = (startPiece.location[0] == 6);
            bool white = (startPiece.location[1] == 1);
            startPiece.MoveSquare(startSquare);
            if (white)
            {
                if (kingside)
                {
                    boardSquares[7][1].inhabitant.MoveSquare(boardSquares[5][1]);
                }
                else
                {
                    boardSquares[3][1].inhabitant.MoveSquare(boardSquares[5][1]);
                }
            }
            else
            {
                if (kingside)
                {
                    boardSquares[7][8].inhabitant.MoveSquare(boardSquares[5][8]);
                }
                else
                {
                    boardSquares[3][8].inhabitant.MoveSquare(boardSquares[5][8]);
                }
            }
        }
        else
        {
            if (promotes)
            {
                army.pieces.Remove(targetSquare.inhabitant);
                army.pieces.Add(startPiece);
                targetSquare.inhabitant = startPiece;
            }
            startPiece.MoveSquare(startSquare);
            if (takesAPiece)
            {
                capturedSquare.inhabitant = capturedPiece;
                ArmyModel capturedArmy = capturedPiece.team;
                capturedPiece.habitat = capturedSquare;
                capturedArmy.pieces.Add(capturedPiece);
            }
        }
        if (takesAwayKingside)
        {
            army.castleKing = true;
        }
        if (takesAwayQueenside)
        {
            army.castleQueen = true;
        }
    }

	public static Move GetMove(PieceModel sp, ChessSquareModel ts, ChessSquareModel cs, bool tp, bool dc, bool i, bool c, float eb, float ea, float mg)
	{
		Move res = theGame.GetMove();
		res.startPiece = sp;
		res.targetSquare = ts;
		res.capturedSquare = cs;
		res.takesAPiece = tp;
		res.deliversCheck = dc;
		res.illegal = i;
        res.castle = c;
		res.evalBefore = eb;
		res.evalAfter = ea;
		res.moveGain = mg;
		return res;
	}

	public static Move GetMove(PieceModel thePiece, ChessSquareModel checkSquare, float before)
    {
        Move res = theGame.GetMove();
        res.startPiece = thePiece;
        res.startSquare = thePiece.habitat;
        res.targetSquare = checkSquare;
        res.capturedSquare = checkSquare;
        res.evalBefore = before;
        res.takesAPiece = checkSquare.inhabitant != null;
        res.capturedPiece = checkSquare.inhabitant;
        if(res.takesAPiece)
        {
            res.capturedSquare.inhabitant = null;
            ArmyModel army = res.capturedPiece.team;
            army.pieces.Remove(res.capturedPiece);
            res.capturedPiece.habitat = null;
        }
        res.startPiece.MoveSquare(res.targetSquare);
        res.evalAfter = Evaluator.EvaluatePosition(res.startSquare.theBoard);
		res.deliversCheck = Evaluator.IsCheck(res.startSquare.theBoard, !thePiece.white);
		res.illegal = Evaluator.IsCheck(res.startSquare.theBoard, thePiece.white);
        res.castle = false;
        res.promotes = false;
		res.moveGain = res.evalAfter - before;
        res.startPiece.MoveSquare(res.startSquare);
        if(res.takesAPiece)
        {
            res.capturedSquare.inhabitant = res.capturedPiece;
            res.capturedPiece.habitat = res.capturedSquare;
            ArmyModel army = res.capturedPiece.team;
            army.pieces.Add(res.capturedPiece);
        }
        res.takesAwayKingside = LoseKingSide(res.startPiece);
        res.takesAwayQueenside = LoseQueenSide(res.startPiece);
        return res;
	}

    public static Move GetMove(PieceModel thePiece, ChessSquareModel checkSquare, ChessSquareModel captureSquare, float before)
    {
        Move res = theGame.GetMove();
        res.startPiece = thePiece;
        res.startSquare = thePiece.habitat;
        res.targetSquare = checkSquare;
        res.capturedSquare = captureSquare;
        res.evalBefore = before;
        res.takesAPiece = captureSquare.inhabitant != null;
        res.capturedPiece = captureSquare.inhabitant;
        if (res.takesAPiece)
        {
            res.capturedSquare.inhabitant = null;
            ArmyModel army = res.capturedPiece.team;
            army.pieces.Remove(res.capturedPiece);
            res.capturedPiece.habitat = null;
        }
        res.startPiece.MoveSquare(res.targetSquare);
        res.evalAfter = Evaluator.EvaluatePosition(res.startSquare.theBoard);
        res.deliversCheck = Evaluator.IsCheck(res.startSquare.theBoard, !thePiece.white);
        res.illegal = Evaluator.IsCheck(res.startSquare.theBoard, thePiece.white);
        res.castle = false;
        res.promotes = false;
        res.moveGain = res.evalAfter - before;
        res.startPiece.MoveSquare(res.startSquare);
        if (res.takesAPiece)
        {
            res.capturedSquare.inhabitant = res.capturedPiece;
            res.capturedPiece.habitat = res.capturedSquare;
            ArmyModel army = res.capturedPiece.team;
            army.pieces.Add(res.capturedPiece);
        }
        res.takesAwayKingside = LoseKingSide(res.startPiece);
        res.takesAwayQueenside = LoseQueenSide(res.startPiece);
        return res;
    }

    public static Move GetMove(ArmyModel army, bool kingside)
    {
        Move res = theGame.GetMove();
        res.castle = true;
        res.takesAPiece = false;
        ChessSquareModel kingStart = null;
        ChessSquareModel kingFinish = null;
        ChessSquareModel rookStart = null;
        ChessSquareModel rookFinish = null;
        PieceModel king = null;
        PieceModel rook = null;
        ChessBoardModel b = army.board;
        res.illegal = Evaluator.IsCheck(b, army.white);
        if (army.white)
        {
            kingStart = b.squares[5][1];
            king = kingStart.inhabitant;
            if(kingside)
            {
                kingFinish = b.squares[7][1];
                rookStart = b.squares[8][1];
                rookFinish = b.squares[6][1];
                res.illegal = res.illegal || rookFinish.inhabitant != null || kingFinish.inhabitant != null;
            }
            else
            {
                kingFinish = b.squares[3][1];
                rookStart = b.squares[1][1];
                rookFinish = b.squares[4][1];
                res.illegal = res.illegal || b.squares[2][1].inhabitant != null || kingFinish.inhabitant != null || rookFinish.inhabitant != null;
            }
        }
        else
        {
            kingStart = b.squares[5][8];
            king = kingStart.inhabitant;
            if (kingside)
            {
                kingFinish = b.squares[7][8];
                rookStart = b.squares[8][8];
                rookFinish = b.squares[6][8];
                res.illegal = res.illegal || rookFinish.inhabitant != null || kingFinish.inhabitant != null;
            }
            else
            {
                kingFinish = b.squares[3][8];
                rookStart = b.squares[1][8];
                rookFinish = b.squares[4][8];
                res.illegal = res.illegal || b.squares[2][8].inhabitant != null || kingFinish.inhabitant != null || rookFinish.inhabitant != null;
            }
        }
        rook = rookStart.inhabitant;
        res.startPiece = rook;
        res.targetSquare = rookFinish;
        res.startSquare = rookStart;
        res.evalBefore = Evaluator.EvaluatePosition(b);
        if (!res.illegal)
        {
            king.MoveSquare(rookFinish);
            res.illegal = Evaluator.IsCheck(b, army.white) || res.illegal;
            king.MoveSquare(kingFinish);
            rook.MoveSquare(rookFinish);
            res.evalAfter = Evaluator.EvaluatePosition(b);
            res.deliversCheck = Evaluator.IsCheck(b, !army.white);
            res.illegal = Evaluator.IsCheck(b, army.white) || res.illegal;
            king.MoveSquare(kingStart);
            rook.MoveSquare(rookStart);
        }
        res.promotes = false;
        res.takesAwayKingside = army.castleKing;
        res.takesAwayQueenside = army.castleQueen;
        return res;
    }

    public static Move GetMove(PieceModel thePiece, ChessSquareModel checkSquare)
    {
        Move res = theGame.GetMove();
        res.startPiece = thePiece;
		res.targetSquare = checkSquare;
        res.castle = false;
		return res;
	}

    public static Move GetMove(PieceModel thePiece, ChessSquareModel checkSquare, float before, string type)
    {
        Move res = theGame.GetMove();
        res.startPiece = thePiece;
        res.startSquare = thePiece.habitat;
        res.targetSquare = checkSquare;
        res.capturedSquare = checkSquare;
        res.evalBefore = before;
        res.takesAPiece = checkSquare.inhabitant != null;
        res.capturedPiece = checkSquare.inhabitant;
        res.promotion = type;
        if (res.takesAPiece)
        {
            res.capturedSquare.inhabitant = null;
            ArmyModel army = res.capturedPiece.team;
            army.pieces.Remove(res.capturedPiece);
            res.capturedPiece.habitat = null;
        }
        res.startPiece.MoveSquare(res.targetSquare);
        //ArmyModel yourArmy = thePiece.team.Clone();
        ArrayList cloneArmy = (ArrayList) thePiece.team.pieces.Clone();
        if (type.Equals("Queen"))
        {
            checkSquare.inhabitant = QueenModel.GetQueenModel(thePiece.white, res.startPiece.location, checkSquare, checkSquare.theBoard);
            cloneArmy.Remove(thePiece);
            cloneArmy.Add(checkSquare.inhabitant);
        }
        else if (type.Equals("Rook"))
        {
            checkSquare.inhabitant = RookModel.GetRookModel(thePiece.white, res.startPiece.location, checkSquare, checkSquare.theBoard);
            cloneArmy.Remove(thePiece);
            cloneArmy.Add(checkSquare.inhabitant);
        }
        else if (type.Equals("Bishop"))
        {
            checkSquare.inhabitant = BishopModel.GetBishopModel(thePiece.white, res.startPiece.location, checkSquare, checkSquare.theBoard);
            cloneArmy.Remove(thePiece);
            cloneArmy.Add(checkSquare.inhabitant);
        }
        else if (type.Equals("Knight"))
        {
            checkSquare.inhabitant = KnightModel.GetKnightModel(thePiece.white, res.startPiece.location, checkSquare, checkSquare.theBoard);
            cloneArmy.Remove(thePiece);
            cloneArmy.Add(checkSquare.inhabitant);
        }
        PieceModel addedPiece = checkSquare.inhabitant;        //added
        addedPiece.theGame = theGame;
        ArrayList temp = thePiece.team.pieces;                 //added
        thePiece.team.pieces = cloneArmy;                      //added
        /**
        ArmyModel temp = thePiece.team;
        if(thePiece.white)
        {
            res.startSquare.theBoard.white = yourArmy;
        }
        else
        {
            res.startSquare.theBoard.black = yourArmy;
        }*/
        res.evalAfter = Evaluator.EvaluatePosition(res.startSquare.theBoard);
        res.deliversCheck = Evaluator.IsCheck(res.startSquare.theBoard, !thePiece.white);
        res.illegal = Evaluator.IsCheck(res.startSquare.theBoard, thePiece.white);
        res.castle = false;
        res.promotes = true;
        res.moveGain = res.evalAfter - before;
        /**
        if (thePiece.white)
        {
            res.startSquare.theBoard.white = temp;
        }
        else
        {
            res.startSquare.theBoard.black = temp;
        }*/
        yourArmy.pieces.Add(thePiece);          //added
        yourArmy.pieces.Remove(addedPiece);     //added
        checkSquare.inhabitant = thePiece;
        res.startPiece.MoveSquare(res.startSquare);
        if (res.takesAPiece)
        {
            res.capturedSquare.inhabitant = res.capturedPiece;
            res.capturedPiece.habitat = res.capturedSquare;
            ArmyModel army = res.capturedPiece.team;
            army.pieces.Add(res.capturedPiece);
        }
        res.takesAwayKingside = LoseKingSide(res.startPiece);
        res.takesAwayQueenside = LoseQueenSide(res.startPiece);
        return res;
    }

    private static bool LoseKingSide(PieceModel m)
    {
        if (!m.team.castleKing)
        {
            return false;
        }
        if (m is KingModel)
        {
            return true;
        }
        if (m is RookModel && m.location[0] == 8)
        {
            return true;
        }
        return false;
    }

    private static bool LoseQueenSide(PieceModel m)
    {
        if (!m.team.castleQueen)
        {
            return false;
        }
        if (m is KingModel)
        {
            return true;
        }
        if (m is RookModel && m.location[0] == 1)
        {
            return true;
        }
        return false;
    }

    public void PrintMove()
    {
        if(startPiece.white)
        {
            Debug.Log("White");
        }
        else
        {
            Debug.Log("Black");
        }
        if(startPiece is PawnModel)
        {
            Debug.Log("Pawn");
        }
        if (startPiece is RookModel)
        {
            Debug.Log("Rook");
        }
        if (startPiece is KnightModel)
        {
            Debug.Log("Knight");
        }
        if (startPiece is BishopModel)
        {
            Debug.Log("Bishop");
        }
        if (startPiece is KingModel)
        {
            Debug.Log("King");
        }
        if (startPiece is QueenModel)
        {
            Debug.Log("Queen");
        }
        Debug.Log(startSquare.location[0]);
        Debug.Log(startSquare.location[1]);
        Debug.Log(targetSquare.location[0]);
        Debug.Log(targetSquare.location[1]);
        Debug.Log(evalAfter);
    }
}
