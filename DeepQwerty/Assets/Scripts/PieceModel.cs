using UnityEngine;
using System.Collections;

public abstract class PieceModel : ScriptableObject
{
	public int[] location;
	public ChessSquareModel habitat;
	public bool white;
	public ChessBoardModel theBoard;
	public ArrayList moveOptions;
	public int[][] moveStructure;
	public float value;
	public float realValue;
    public ArmyModel team;
    public ChessGame theGame;
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public ChessSquareModel findMoveSquare(int[] path)
	{
		ChessSquareModel res = null;
		int[] newSpot = new int[2];
		newSpot[0] = location[0] + path[0];
		newSpot[1] = location[1] + path[1];
		res = (newSpot[0] > 0 && newSpot[0] <= 8 && newSpot[1] > 0 && newSpot[1] <= 8)? theBoard.squares[newSpot[0]][newSpot[1]] : null;
		return res;
	}

	public void MoveSquare(ChessSquareModel place)
	{
		habitat.removePiece();
		place.addPiece(this);
	}

	public void die()
	{
		habitat.removePiece ();
	}

	public void FindValue()
	{
		realValue = value;
        ArrayList opts = GetSimpleMoveOptions();
        realValue += opts.Count / 10f;
		realValue += GetPositionValue();
        if(theGame == null)
        {
            Debug.Log(location[0] + " " + location[1]);
            Debug.Log(white);
            if (this is BishopModel)
            {
                Debug.Log("Bishop");
            }
            if (this is QueenModel)
            {
                Debug.Log("Queen");
            }
            if (this is KnightModel)
            {
                Debug.Log("Knight");
            }
            if (this is KingModel)
            {
                Debug.Log("King");
            }
            if (this is PawnModel)
            {
                Debug.Log("Pawn");
            }
            if (this is RookModel)
            {
                Debug.Log("Rook");
            }
        }
        foreach(Move m in opts)
        {
            theGame.FreeMove(m);
        }
	}

    public ArrayList RemoveIllegalMoves(ArrayList raw)
    {
        ArrayList res = new ArrayList();
        foreach (Move m in raw)
        {
            if(!m.illegal)
            {
                res.Add(m);
            }
            else
            {
                theGame.FreeMove(m);
            }
        }
        return res;
    }

    public abstract ArrayList GetMoveOptions();

	public abstract ArrayList GetSimpleMoveOptions();

	public abstract PieceModel Clone();

	public abstract float GetPositionValue();

	public abstract void SetMoveStructure();
}
