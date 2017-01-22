using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreSubmitter : MonoBehaviour
{

    public static HighscoreSubmitter Instance { get; set; }

    private int _currentScore;

    [SerializeField]
    private Canvas _highscoreCanvas;
    [SerializeField]
    private GameObject _uiHighscoreListGo;
    [SerializeField]
    private GameObject _highscoreElementPrefab;
    [SerializeField]
    private InputField _nameInputField;
    [SerializeField]
    private Text _scoreNumber;
    [SerializeField]
    private Button _submitButton;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

	// Use this for initialization
	void Start ()
	{
	    _submitButton.onClick.AddListener(OnSubmitClick);
        HighscoreList.Instance.OnHighscoresLoadedEvent.AddListener(PopulateHighscores);
	}

    public void DisplayHighscore()
    {
        _highscoreCanvas.enabled = true;
    }

    /// <summary>
    /// Get the score from the WaveSpawner as seconds survived.
    /// </summary>
    public void GetScore()
    {
        SetScore(WaveSpawner.Instance.SecondsSurvived);
    }

    public void SetScore(int score)
    {
        _currentScore = score;
        _scoreNumber.text = score.ToString();
    }

    private void PopulateHighscores()
    {
        // Clear all children.
        for (int i = 0; i < _uiHighscoreListGo.transform.childCount; i++)
        {
            Destroy(_uiHighscoreListGo.transform.GetChild(i).gameObject);
        }

        float totalHighscoreListHeight = 0f;
        int highscoreCount = Math.Min(10, HighscoreList.Instance.Highscores.Count);
        for (int i = 0; i < highscoreCount; i++)
        {
            // Create object.
            var highscoreGo = Instantiate(_highscoreElementPrefab);

            // Place object.
            var rectTransform = highscoreGo.GetComponent<RectTransform>();
            totalHighscoreListHeight += rectTransform.sizeDelta.y;
            highscoreGo.transform.SetParent(_uiHighscoreListGo.transform, false);
            rectTransform.transform.localScale = new Vector3(1, 1, 1);

            // Set strings.
            var highscore = highscoreGo.GetComponent<HighScoreListElement>();
            highscore.Name = HighscoreList.Instance.Highscores[i].Name;
            highscore.Score = HighscoreList.Instance.Highscores[i].Score.ToString();
        }

        // Set content object's height to fit for the amount of elements.
        RectTransform contentRectTransform = _uiHighscoreListGo.GetComponent<RectTransform>();
        VerticalLayoutGroup contentLayoutGroup = _uiHighscoreListGo.GetComponent<VerticalLayoutGroup>();
        float contentHeight = contentLayoutGroup.padding.top + contentLayoutGroup.padding.bottom + totalHighscoreListHeight + (highscoreCount - 1) * contentLayoutGroup.spacing;
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, contentHeight);
    }

    private void OnSubmitClick()
    {
        _submitButton.enabled = false;
        string name = _nameInputField.text;
        int score = _currentScore;

        var highscore = new Highscore(name, score);

        HighscoreList.Instance.AddScore(highscore);

        PopulateHighscores();
    }
}
