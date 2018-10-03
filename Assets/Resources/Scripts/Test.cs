using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is a Parent class for TriadTest and IntervalTest.
public abstract class Test {
    //maps the name of a scale eg "major" to an int[] of indeces from 0-12 (on the chromatic scale)
    public Dictionary<string, int[]> scales;
    public string answer;
    public System.Random rand;

    public Test() {
        rand = new System.Random();
        scales = new Dictionary<string, int[]>();
        scales.Add("major", new int[] { 0, 2, 4, 5, 7, 9, 11, 12 });
        scales.Add("minor", new int[] { 0, 2, 3, 5, 7, 8, 10, 12 });
    }
    

    //generates the answer
    public abstract void GenerateAnswer();

    // plays the Notes that the player is trying to guess
    public abstract void PlayAnswer(MyProcess chuck);

    // returns true if the guess is correct, otherwise false
    public abstract bool GuessAnswer(string guess);

    //returns the answer
    public abstract string GetAnswer();

    //returns an answer that is false
    public abstract string GetFalseAnswer();

}
