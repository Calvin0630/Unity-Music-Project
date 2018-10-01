using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuizManager : MonoBehaviour {
    MyProcess chuck;
    Test activeTest;
    public GameObject chooseQuizPanel, triadTestPanel;
    // Use this for initialization
    void Start() {
        chuck = new MyProcess();
        //UnityEngine.Debug.Log("start: \n" +output+"\n :end");
    }

    // Update is called once per frame
    void Update() {
        
    }

    void StartQuiz() {

    }

    public void StartTriadTest() {
        chooseQuizPanel.SetActive(false);
        triadTestPanel.SetActive(true);
        activeTest = new TriadTest();
        activeTest.GenerateAnswer();
        PlayAnswer();
        
    }

    public void PlayAnswer() {
        if(activeTest == null) {
            Debug.LogError("Test isn't initialized!!");
        }
        else {
            activeTest.PlayAnswer(chuck);
        }
    }

    public void GuessAnswer(string answer) {
        activeTest.GuessAnswer(answer);
    }
    

    void OnApplicationQuit() {
        chuck.Close();
        UnityEngine.Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
