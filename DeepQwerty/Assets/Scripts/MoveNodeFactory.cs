using UnityEngine;
using System.Collections;

public class MoveNodeFactory : ScriptableObject
{
    public ArrayList used = new ArrayList();
    public ArrayList available = new ArrayList();
    public ChessGame theGame;

    // Use this for initialization
    void Start()
    {
        used = new ArrayList();
        available = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public MoveNode GetMoveNode()
    {
        MoveNode res;
        if (available.Count > 0)
        {
            res = (MoveNode) available[0];
            available.RemoveAt(0);
            res.expanded = false;
            res.move = null;
            res.ply = 0;
        }
        else
        {
            return ScriptableObject.CreateInstance<MoveNode>();
        }
        return res;
    }

    public void FreeMoveNode(MoveNode freed)
    {
        available.Add(freed);
    }
}
