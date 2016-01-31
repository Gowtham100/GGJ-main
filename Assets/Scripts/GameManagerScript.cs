using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {
    public static GameManagerScript instance = null;
    public BoardManager boardScript;
    private int level; 



	void Awake()
    {
        if (instance == null)  // if no previous manager
        {
            instance = this;
        } else if (instance != this) // if manager already exists dont make another
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);// makes this not die when a new scene is loaded
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
