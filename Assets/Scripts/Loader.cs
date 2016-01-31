using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject gameManager;

    void Awake()
    {
        if (GameManagerScript.instance == null)
        {
            Instantiate(gameManager);
        } 
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
