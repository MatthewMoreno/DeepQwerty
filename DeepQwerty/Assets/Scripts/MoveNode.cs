using UnityEngine;
using System.Collections;

public class MoveNode : ScriptableObject
{
    public Move move;
    public int ply;
    public bool expanded;
    public float score;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public static MoveNode GetMoveNode(Move m, int p, bool e, MoveNodeFactory factory)
    {
        MoveNode res = factory.GetMoveNode();
        res.move = m;
        res.ply = p;
        res.expanded = e;
        return res;
    }
}
