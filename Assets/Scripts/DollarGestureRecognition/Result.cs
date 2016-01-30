using UnityEngine;
using System.Collections;

public class Result {

    public string name { get; private set; }
    public float score { get; private set; }

	public Result (string name, float score)
    {
        this.name = name;
        this.score = score;
    }
}
