using UnityEngine;
using System.Collections;

public class MoveFactory : ScriptableObject
{
    public ArrayList used = new ArrayList();
    public ArrayList available = new ArrayList();
    public ChessGame theGame;

	// Use this for initialization
	void Start ()
    {
        used = new ArrayList();
        available = new ArrayList();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public Move GetMove()
    {
        Move res;
        if(available.Count > 0)
        {
            res = (Move) available[0];
            available.RemoveAt(0);
            res.capturedPiece = null;
            res.capturedSquare = null;
            res.deliversCheck = false;
            res.evalAfter = 0f;
            res.evalBefore = 0f;
            res.illegal = false;
            res.moveGain = 0f;
            res.startPiece = null;
            res.startSquare = null;
            res.takesAPiece = false;
            res.targetSquare = null;
        }
        else
        {
            return ScriptableObject.CreateInstance<Move>();
        }
        return res;
    }

    public void FreeMove(Move freed)
    {
        available.Add(freed);
    }
}
