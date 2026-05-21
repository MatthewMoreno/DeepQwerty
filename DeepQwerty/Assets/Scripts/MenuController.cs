using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class MenuController : MonoBehaviour
{
	public Button computerButton;
	public Button humanButton;
	public Button whiteButton;
	public Button blackButton;
	
	public bool whiteIsPlayer;
	public bool blackIsPlayer;
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void SelectColor()
	{
		humanButton.GetComponent<RectTransform>().Translate(0, -30, 0);
		whiteButton.GetComponent<RectTransform>().Translate(0, -110, 0);
		blackButton.GetComponent<RectTransform>().Translate(0, -140, 0);
		computerButton.GetComponent<RectTransform>().Translate(0, 100, 0);
	}

	public void StartGameAI(bool white) 
	{
		DontDestroyOnLoad(this);
		whiteIsPlayer = white;
		blackIsPlayer = !white;
		SceneManager.LoadScene("Game");
	}

	public void StartGamePlayer() 
	{
		print("StartingGameAgainstHuman");
		DontDestroyOnLoad(this);
		whiteIsPlayer = true;
		blackIsPlayer = true;
		SceneManager.LoadScene("Game");
	}
}
