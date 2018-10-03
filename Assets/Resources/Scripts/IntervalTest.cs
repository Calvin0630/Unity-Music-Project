using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IntervalTest : Test {
    //a list of intervals starting at 1 semitone, going up by one until it reaches 12
    public List<string> intervalStrings = new List<string>() {
        "Minor second",
        "Major second",
        "Minor third",
        "Major third",
        "Perfect fourth",
        "Diminished fifth",
        "Perfect fifth",
        "Minor sixth",
        "Major sixth",
        "Minor seventh",
        "Major seventh",
        "Perfect octave",
    };
    //answer is the name of the interval in the above list
    //the answer string is in the base class

    public override void GenerateAnswer() {
        answer = intervalStrings.ElementAt(rand.Next(0, 12));
    }

    public override bool GuessAnswer(string guess) {
        if (answer==null) Debug.LogError("answer in null!!");

        if (guess==answer) {
            return true;
        }
        else {
            return false;
        }
    }

    public override void PlayAnswer(MyProcess chuck) {
        chuck.ExecuteCommand("chuck + PlayInterval.ck:70:.8:63:0:"+intervalStrings.IndexOf(answer));
    }

    public override string GetAnswer() {
        if (answer == null) {
            Debug.LogError("answer not set!!!");
            return null;
        }
        return answer;
    }

    public override string GetFalseAnswer() {
        string result;
        do {
            result = intervalStrings.ElementAt(rand.Next(0, 12));
        } while (result == answer);
        return result;
    }

}
