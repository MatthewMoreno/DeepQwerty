using UnityEngine;
using System.Collections;


// A representation of an army used for holding information for the army and decision making for moves
public class ArmyModel : ScriptableObject
{
	public bool white;
	public ArrayList pieces;
	public ChessBoardModel board;
    public bool castleKing;
    public bool castleQueen;
    public ChessGame theGame;
    public ArmyModel enemy;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

    // Pieces will be filled in later in the Army's Initialize() method
	public static ArmyModel GetArmyModel(bool color, ChessBoardModel b)
	{
		ArmyModel res = ScriptableObject.CreateInstance<ArmyModel>();
		res.white = color;
		res.board = b;
		res.pieces = new ArrayList();
        res.castleKing = true;
        res.castleQueen = true;
		return res;
	}
	
    // Returns all legal move options for this army
	public ArrayList GetAllMoveOptions()
	{
		ArrayList res = new ArrayList();

        // Add the legal moves from each PieceModel in the ArmyModel
		foreach(PieceModel p in pieces)
		{
			res.AddRange(p.GetMoveOptions());
		}

        // Add the castle kingside move if available
        if(castleKing)
        {
            Move kingSideCastle = Move.GetMove(this, true);
            if(!kingSideCastle.illegal)
            {
                res.Add(kingSideCastle);
            }
        }

        // Add the castle queenside move if available
        if(castleQueen)
        {
            Move queenSideCastle = Move.GetMove(this, false);
            if(!queenSideCastle.illegal)
            {
                res.Add(queenSideCastle);
            }
        }

		return res;
	}

    // Returns the breadth best moves that this army can make, any additional moves which capture a piece, and any
    // move which delivers check
    public ArrayList GetBestMoveOptions(int breadth)
    {
        ArrayList res = new ArrayList();
        ArrayList opt = GetAllMoveOptions();  // Grabs all available moves. We'll pull the best ones from this.
        ArrayList bestMoves = new ArrayList();
        float targetEval = (white)? 10000f : -10000f;  // The evaluation threshold that a move needs to break to be added

        // Add the first few moves to fill up bestMoves. We'll replace them later if we find better ones
        while(bestMoves.Count < breadth && opt.Count != 0)
        {
            bestMoves.Add(opt[0]);
            if(white)
            {
                targetEval = Mathf.Min(targetEval, ((Move)opt[0]).evalAfter);
            }
            else
            {
                targetEval = Mathf.Max(targetEval, ((Move)opt[0]).evalAfter);
            }
            opt.RemoveAt(0);
        }

        // Loop over the remaining moves in out of all the options and add the good ones in.
        foreach(Move m in opt)
        {
            if((white && m.evalAfter > targetEval) || (!white && m.evalAfter < targetEval))
            {
                // Add the better move in, and remove the worst move. This keeps the number of moves the same.
                bestMoves.Add(m);
                targetEval = RemoveWorst(bestMoves);   // Adjust the target Evaluation to reflect the new list.
            }
            else if(m.takesAPiece || m.deliversCheck)
            {
                res.Add(m);    // Add the moves that deliver check or capture a piece to the final result, which is different from bestMoves
            }
            else
            {
                theGame.FreeMove(m);   // If we don't add the move, then get rid of it.
            }
        }
        res.AddRange(bestMoves);   // Add all the best rated moves to the final result.
        return res;
    }

    // Helper method to remove the worst option from a list of moves in order to add a better move
    // Returns the lowest Evaluation in the new list of moves to be used as the new target evaluation
    private float RemoveWorst(ArrayList best)
    {
        Move worstMove = (Move)best[0];
        float worstScore = worstMove.evalAfter;  // For the moment, this will hold the worst score known in best

        // Find the worst move in the list
        for(int i = 1; i < best.Count; i++)
        {
            if (white && worstScore > ((Move) best[i]).evalAfter)
            {
                worstMove = ((Move)best[i]);
                worstScore = worstMove.evalAfter;
            }
            else if (!white && worstScore < ((Move)best[i]).evalAfter)
            {
                worstMove = ((Move)best[i]);
                worstScore = worstMove.evalAfter;
            }
        }
        best.Remove(worstMove);
        theGame.FreeMove(worstMove);    // Remove the worst move
        worstScore = ((Move) best[0]).evalAfter;    // Now this will hold the new worst score in best, or second worst in the original list
        for (int i = 1; i < best.Count; i++)
        {
            if (white && worstScore > ((Move)best[i]).evalAfter)
            {
                worstScore = ((Move)best[i]).evalAfter;
            }
            else if (!white && worstScore < ((Move)best[i]).evalAfter)
            {
                worstScore = ((Move)best[i]).evalAfter;
            }
        }
        return worstScore;
    }
	
    // Returns all simple move options available for this army. These simple moves only include the starting
    // PieceModel and destination square, and doesn't include castling. It is used by the evaluator for determining
    // if a move gives check.
	public ArrayList getAllSimpleMoveOptions()
	{
		ArrayList res = new ArrayList();
		foreach(PieceModel p in pieces)
		{
			res.AddRange(p.GetSimpleMoveOptions());
		}
		return res;
	}

    public ArmyModel Clone()
    {
        ArmyModel res = ScriptableObject.CreateInstance<ArmyModel>();
        res.board = board;
        res.castleKing = castleKing;
        res.castleQueen = castleQueen;
        res.white = white;
        res.pieces = new ArrayList();
        res.theGame = theGame;
        foreach(PieceModel m in pieces)
        {
            res.pieces.Add(m);
        }
        return res;
    }
}
