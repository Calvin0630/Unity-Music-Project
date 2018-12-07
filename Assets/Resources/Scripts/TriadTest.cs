
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class TriadTest : Test {
    List<int> answerList;

    //answer is either "major" or "minor"

    // Use this for initialization
    public TriadTest () {
        answerList = new List<int>();

    }
   
    public override void GenerateAnswer() {
        answerList.Clear();
        //this is a list of notes that are in the major in minor scales. This is necasay cause if the triad is in both scales, the user cant tell the difference
        List<int> intersection = scales["minor"].Intersect(scales["major"]).ToList<int>();
        //decides between major and minor
        //answer is either "major" or "minor"
        if (rand.Next(0,2) == 0) {
            answer = "major";
        }
        else {
            answer = "minor";
        }
        do {
            answerList.Clear();
            //a for loop to generate 3 random number in ascending order from 0-8
            for (int i = 0; i < 3; i++) {
                answerList.Add(scales[answer][rand.Next(0, 8)]);
                answerList.Sort();
            }  //checks if there are repetitions
        } while (answerList.ElementAt(0) == answerList.ElementAt(1) || answerList.ElementAt(0) == answerList.ElementAt(2) || answerList.ElementAt(2) == answerList.ElementAt(1)
                //checks if the answer is identical in both scales ie 0,1,3
                || answerList.Except(intersection).Count() < 2 );
    }

    public override void PlayAnswer(MyProcess chuck) {
        chuck.ExecuteCommand("chuck + Chuck_Scripts//PlayInterval.ck:70:.8:63:" + answerList.ElementAt(0) + ":" + answerList.ElementAt(1) + ":" + answerList.ElementAt(2));
    }


    public override bool GuessAnswer(string guess) {
        if (guess == answer) return true;
        else return false;
    }
    public override string GetAnswer() {
        if(answer ==null) {
            Debug.LogError("answer not set!!!");
            return null;
        }
        return answer;
    }

    public override string GetFalseAnswer() {
        throw new NotImplementedException();
    }
}
