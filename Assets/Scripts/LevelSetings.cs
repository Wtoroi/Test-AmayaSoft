using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelSetings", menuName = "MyAssets/LevelSetings")]
public class LevelSetings : ScriptableObject
{
    [SerializeField] private int lines;
    [SerializeField] private int column;

    public int Lines
    {
        get { return lines; }
    }

    public int Column
    {
        get { return column; }
    }
}
