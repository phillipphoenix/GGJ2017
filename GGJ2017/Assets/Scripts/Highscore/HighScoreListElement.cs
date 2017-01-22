using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreListElement : MonoBehaviour
{

    public string Name { get { return _nameText.text; } set { _nameText.text = value; } }
    public string Score { get { return _scoreNumber.text; } set { _scoreNumber.text = value; } }

    [SerializeField]
    private Text _nameText;
    [SerializeField]
    private Text _scoreNumber;


}
