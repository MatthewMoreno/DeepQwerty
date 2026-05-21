using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChessGame : MonoBehaviour
{
	public ChessBoard gameBoard;
	public Army white;
	public Army black;
	public bool whiteIsPlayer;
	public bool blackIsPlayer;
	public bool whitesTurn;
	public int ply;
    public int breadth;
	private bool started;
    public Piece selectedPiece;
    public bool gameOver;
    public MoveFactory moveFactory;
    public MoveNodeFactory moveNodeFactory;
    public Promotion queenButton;
    public Promotion rookButton;
    public Promotion bishopButton;
    public Promotion knightButton;

	// Use this for initialization
	void Start ()
	{
		GameObject obj = GameObject.Find("MenuController");
		MenuController mController = obj.GetComponent<MenuController>();
		whiteIsPlayer = mController.whiteIsPlayer;
		blackIsPlayer = mController.blackIsPlayer;
		Destroy(mController);
		started = false;
        selectedPiece = null;
        gameOver = false;
        moveFactory = ScriptableObject.CreateInstance<MoveFactory>();
        moveNodeFactory = ScriptableObject.CreateInstance<MoveNodeFactory>();
        Move.theGame = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!started)
		{
            started = true;
			white.Initialize();
			black.Initialize();
			gameBoard.Initialize();
			for(int i = 1; i < 9; i++)
			{
				for(int j = 1; j < 9; j++)
				{
					gameBoard.squares[i][j].Initialize();
				}
			}
			foreach(Piece p in white.pieces)
			{
				p.Initialize();
			}
			foreach(Piece p in black.pieces)
			{
				p.Initialize();
			}
			Initialize();
		}
        if(!gameOver)
        {
            if (!whiteIsPlayer && whitesTurn)
            {
                MakeMove();
            }
            else if (!blackIsPlayer && !whitesTurn)
            {
                MakeMove();
            }
        }
	}
	
	private void Initialize()
	{
		whitesTurn = true;
		if(blackIsPlayer && !whiteIsPlayer)
		{
			GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            mainCamera.transform.position = new Vector3(17.5f, 30, 40);
			mainCamera.transform.rotation = new Quaternion(0f, .866025f, -.5f, 0f);
		}
	}
	
	public void TakeTurn()
	{
		if((whitesTurn && !whiteIsPlayer) || (!whitesTurn && !blackIsPlayer))
		{
			MakeMove();
		}
	}
	
	public void MakeMove()
	{
		ArmyModel yourArmy = (whiteIsPlayer)? black.model : white.model;
		ArmyModel opponentsArmy = yourArmy.enemy;
		ArrayList Moves = yourArmy.GetBestMoveOptions(breadth);
        ArrayList theStack = new ArrayList();
        MoveNode[] bestMoves = new MoveNode[ply];
        foreach(Move m in Moves)
        {
            theStack.Add(MoveNode.GetMoveNode(m, ply - 1, false, moveNodeFactory));
        }
        MoveNode nextNode = null;
		Move nextMove = null;
		Move move = null;
		while(theStack.Count > 0)
		{
			nextNode = (MoveNode) theStack[theStack.Count - 1];
            nextMove = nextNode.move;
            if(!nextNode.expanded)
            {
                nextNode.expanded = true;
                if (nextNode.ply > 0)
                {
                    move = nextMove;
                    nextMove.Apply();
                    yourArmy = (move.startPiece.white)? black.model : white.model;
                    opponentsArmy = (move.startPiece.white)? white.model : black.model;
                    Moves = yourArmy.GetBestMoveOptions(breadth);
                    foreach (Move m in Moves)
                    {
                        theStack.Add(MoveNode.GetMoveNode(m, nextNode.ply - 1, false, moveNodeFactory));
                    }
                }
                else
                {
                    nextNode.score = nextMove.evalAfter;
                    if (bestMoves[nextNode.ply] == null)
                    {
                        bestMoves[nextNode.ply] = nextNode;
                    }
                    else
                    {
                        if (nextNode.move.startPiece.white && nextNode.score > bestMoves[nextNode.ply].score)
                        {
                            moveNodeFactory.FreeMoveNode(bestMoves[nextNode.ply]);
                            moveFactory.FreeMove(bestMoves[nextNode.ply].move);
                            bestMoves[nextNode.ply] = nextNode;
                        }
                        else if (!nextNode.move.startPiece.white && nextNode.score < bestMoves[nextNode.ply].score)
                        {
                            moveNodeFactory.FreeMoveNode(bestMoves[nextNode.ply]);
                            moveFactory.FreeMove(bestMoves[nextNode.ply].move);
                            bestMoves[nextNode.ply] = nextNode;
                        }
                        else
                        {
                            moveNodeFactory.FreeMoveNode(nextNode);
                            moveFactory.FreeMove(nextNode.move);
                        }
                    }
                    theStack.RemoveAt(theStack.Count - 1);
                }
            }
			else
			{
				nextMove.Reverse();
				theStack.RemoveAt(theStack.Count - 1);
                if(bestMoves[nextNode.ply - 1] == null)
                {
                    if(nextMove.deliversCheck)
                    {
                        nextNode.score = (nextMove.startPiece.white) ? 10000f : -10000f;
                    }
                    else
                    {
                        nextNode.score = 0f;
                    }
                }
                else
                {
                    nextNode.score = bestMoves[nextNode.ply - 1].score;
                    moveNodeFactory.FreeMoveNode(bestMoves[nextNode.ply - 1]);
                    moveFactory.FreeMove(bestMoves[nextNode.ply - 1].move);
                }
                bestMoves[nextNode.ply - 1] = null;
                if(bestMoves[nextNode.ply] == null)
                {
                    bestMoves[nextNode.ply] = nextNode;
                }
                else
                {
                    if (nextNode.move.startPiece.white && nextNode.score > bestMoves[nextNode.ply].score)
                    {
                        moveNodeFactory.FreeMoveNode(bestMoves[nextNode.ply]);
                        moveFactory.FreeMove(bestMoves[nextNode.ply].move);
                        bestMoves[nextNode.ply] = nextNode;
                    }
                    else if(!nextNode.move.startPiece.white && nextNode.score < bestMoves[nextNode.ply].score)
                    {
                        moveNodeFactory.FreeMoveNode(bestMoves[nextNode.ply]);
                        moveFactory.FreeMove(bestMoves[nextNode.ply].move);
                        bestMoves[nextNode.ply] = nextNode;
                    }
                    else
                    {
                        moveNodeFactory.FreeMoveNode(nextNode);
                        moveFactory.FreeMove(nextNode.move);
                    }
                }
			}
		}
		gameBoard.Apply(bestMoves[ply - 1].move);
        if(!bestMoves[ply - 1].move.promotes)
        {
            whitesTurn = !whitesTurn;
        }
        moveNodeFactory.FreeMoveNode(bestMoves[ply - 1]);
        moveFactory.FreeMove(bestMoves[ply - 1].move);
        System.GC.Collect();
    }

    public void CheckMate(bool winner)
    {
        Debug.Log("Checkmate");
        gameOver = true;
    }

    public void StaleMate()
    {
        Debug.Log("Stalemate");
        gameOver = true;
    }

    public void FreeMove(Move m)
    {
        moveFactory.FreeMove(m);
    }

    public Move GetMove()
    {
        return moveFactory.GetMove();
    }
}
