using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighscoreList : MonoBehaviour {

    public static HighscoreList Instance { get; set; }

    public List<Highscore> Highscores { get { return _highscores; } private set { _highscores = value; } }

    [SerializeField]
    private string _fileName = "Highscores";
    [SerializeField]
    private List<Highscore> _highscores;
    private string _filePath;

    public void Awake()
    {
        // Make sure only one highscore list exists.
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Get file path.
        _filePath = Application.dataPath + "/" + _fileName + ".json";
    }

	// Use this for initialization
	void Start () {
	    if (File.Exists(_filePath))
	    {
	        Highscores = JsonUtility.FromJson<ObjectWrapper>(File.ReadAllText(_filePath)).Highscores;
	    }
	    else
	    {
	        Highscores = new List<Highscore>();
	    }
	}

    private void OnApplicationQuit()
    {
        var objWrapper = new ObjectWrapper();
        objWrapper.Highscores = Highscores;
        File.WriteAllText(_filePath, JsonUtility.ToJson(objWrapper));
    }

    private struct ObjectWrapper
    {
        public List<Highscore> Highscores;
    }

}
