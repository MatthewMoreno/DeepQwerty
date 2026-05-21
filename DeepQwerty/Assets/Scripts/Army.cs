using UnityEngine;
using System.Collections;


// A real Army physically in the game
public class Army : MonoBehaviour
{
	public bool white;
	public ArrayList pieces;
	public ChessBoard board;
	public ArmyModel model;
    public ChessGame theGame;
    public Army enemy;

    // Use this for initialization
    void Start ()
	{
		model = ScriptableObject.CreateInstance<ArmyModel>();
	}
	
    // For setting up the ArmyModel
	public void Initialize()
	{
		Piece[] p = GetComponentsInChildren<Piece>();
		pieces = new ArrayList();
		model.pieces = new ArrayList();
		for(int i = 0; i < p.Length; i++)
		{
			pieces.Add(p[i]);
			model.pieces.Add(p[i].model);
		}
		model.board = board.model;
		model.white = white;
        model.castleKing = true;
        model.castleQueen = true;
        model.theGame = theGame;
        model.enemy = enemy.model;   // Model is assigned in Start(), so this won't be null
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
