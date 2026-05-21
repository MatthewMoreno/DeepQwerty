using UnityEngine;
using System.Collections;

public class BishopModel : PieceModel
{
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

    // Called in Piece's Initialize()
	public static BishopModel GetBishopModel(bool color, int[] loc, ChessSquareModel squ, ChessBoardModel b)
	{
		BishopModel res = ScriptableObject.CreateInstance<BishopModel>();
		res.white = color;
		res.location = loc;
		res.habitat = squ;
		res.theBoard = b;
		int[] spot = new int[2];
		res.moveStructure = new int[28][];    //Holds four diagonals
		for(int i = 0; i < 7; i++)            //First diagonal
		{
			spot[0] = -i - 1;
			spot[1] = i + 1;
			res.moveStructure[i] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)            //Second diagonal
		{
			spot[0] = i + 1;
			spot[1] = i + 1;
			res.moveStructure[i + 7] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)            //Third diagonal
		{
			spot[0] = i + 1;
			spot[1] = -i - 1;
			res.moveStructure[i + 14] = spot;
			spot = new int[2];
		}
		for(int i = 0; i < 7; i++)            //Fourth diagonal
		{
			spot[0] = -i - 1;
			spot[1] = -i - 1;
			res.moveStructure[i + 21] = spot;
			spot = new int[2];
		}
		res.value = 3;
		return res;
	}

	public override ArrayList GetMoveOptions()
    {
        ArrayList res = new ArrayList();
		float before = Evaluator.EvaluatePosition(theBoard);    //Get Evaluation now tofigure out how much the moves benefit us
		int spot = 0;               //Keeps track of location in the move structure
		ChessSquareModel checkSquare;        //Holds the square we're looking at moving to
		for(int i = 1; i < 5; i++)     //Four iterations: one for each diagonal
		{
			while(spot < i * 7)
			{
				checkSquare = findMoveSquare(moveStructure[spot]);   //Gets the square from the chess board
				if(checkSquare == null)     //If it is off the board, move to the next iteration
				{
					spot = i * 7;
				}
				else if(checkSquare.isFree())    //If there is nothing in the square, add it to the list of options
				{
					res.Add(Move.GetMove(this, checkSquare, before));
					spot++;                      //Check the next spot in the move structure
				}
				else if(checkSquare.inhabitant.white != this.white)    //If the piece in the square is an enemy, add the capture
				{
					res.Add(Move.GetMove(this, checkSquare, before));
					spot = i * 7;                //Can't move past the capture, so move to the next iteration
				}
				else                             //If the square holds an ally, move to the next iteration
				{
					spot = i * 7;
				}
			}
		}
		return RemoveIllegalMoves(res);          //Get rid of moves that open us up to check
	}

    //Used for evaluating check and the real value of a piece
	public override ArrayList GetSimpleMoveOptions()
	{
		ArrayList res = new ArrayList();
		int spot = 0;               //Keeps track of location in the move structure
        ChessSquareModel checkSquare;        //Holds the square we're looking at moving to
        for (int i = 1; i < 5; i++)     //Four iterations: one for each diagonal
        {
			while(spot < i * 7)
			{
				checkSquare = findMoveSquare(moveStructure[spot]);   //Gets the square from the chess board
                if (checkSquare == null)     //If it is off the board, move to the next iteration
                {
					spot = i * 7;
				}
				else if(checkSquare.isFree())    //If there is nothing in the square, add it to the list of options
                {
					res.Add(Move.GetMove(this, checkSquare));
					spot++;                      //Check the next spot in the move structure
                }
				else if(checkSquare.inhabitant.white != this.white)    //If the piece in the square is an enemy, add the capture
                {
					res.Add(Move.GetMove(this, checkSquare));
					spot = i * 7;                //Can't move past the capture, so move to the next iteration
                }
                else                             //If the square holds an ally, move to the next iteration
                {
					spot = i * 7;
				}
			}
		}
		return res;
	}

	//Gets a copy of this model to be used for promotions checking
	public override PieceModel Clone()
	{
		return GetBishopModel(white, location, habitat, theBoard);
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
		moveStructure = new int[28][];
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
	}
}
