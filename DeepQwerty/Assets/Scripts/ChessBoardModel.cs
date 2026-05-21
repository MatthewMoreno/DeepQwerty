using UnityEngine;
using System.Collections;

public class ChessBoardModel : ScriptableObject
{
	public ArmyModel white;
	public ArmyModel black;
	public ChessSquareModel[][] squares = new ChessSquareModel[9][];
    public ChessGame theGame;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public static ChessBoardModel GetChessBoardModel()
	{
		ChessBoardModel res = ScriptableObject.CreateInstance<ChessBoardModel>();
		res.white = ArmyModel.GetArmyModel(true, res);
		res.black = ArmyModel.GetArmyModel(false, res);
		res.squares = new ChessSquareModel[8][];
		for(int i = 1; i < 9; i++)
		{
			res.squares[i] = new ChessSquareModel[9];
		}
		return res;
	}
	
}
