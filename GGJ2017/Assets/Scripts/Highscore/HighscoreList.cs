using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HighscoreList : MonoBehaviour
{

    public UnityEvent OnHighscoresLoadedEvent;

    public static HighscoreList Instance { get; set; }

    /// <summary>
    /// Don't add highscores to the list returned. Use the <see cref="AddScore"/> method.
    /// </summary>
    public List<Highscore> Highscores { get { return _highscores; } private set { _highscores = value; } }

    [SerializeField]
    private string _fileName = "Highscores";
    [SerializeField]
    private List<Highscore> _highscores;
    private string _filePath;

    private bool _highscoresLoaded;

    public void Awake()
    {
        // Make sure only one highscore list exists.
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        // Get file path.
        _filePath = Application.dataPath + "/" + _fileName + ".json";

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (_highscoresLoaded)
            {
                OnHighscoresLoadedEvent.Invoke();
            }
        };
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
	    _highscoresLoaded = true;
        OnHighscoresLoadedEvent.Invoke();
	}

    public void AddScore(Highscore highscore)
    {
        Highscores.Add(highscore);
        Highscores = Highscores.OrderByDescending(hs => hs.Score).ToList();
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
