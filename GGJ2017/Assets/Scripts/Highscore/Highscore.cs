using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Highscore
{
    public string Name;
    public int Score;

    public Highscore(string name, int score)
    {
        Name = name;
        Score = score;
    }
}
