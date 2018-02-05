using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneName
{
    public enum SceneNames
    {
        Title,
        WorldMap,
        azito,
        Battle,
        Kazan,
		Credit,
		Prologue
    }

    public const string Title    = "Title";
	public const string Prologue = "Prologue";
    public const string WorldMap = "WorldMap";
    public const string Base     = "Base";
    public const string Battle   = "Battle";
    public const string Credit   = "Credit";
}
