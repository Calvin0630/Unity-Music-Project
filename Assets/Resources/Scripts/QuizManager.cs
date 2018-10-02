using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour {
    MyProcess chuck;
    Test activeTest;
    int score;
    public GameObject chooseQuizPanel, triadTestPanel, rightResponsePanel, wrongResponsePanel, restartButton, scorePanel;
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
        scorePanel.SetActive(true);
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
        bool correct =activeTest.GuessAnswer(answer);
        restartButton.SetActive(true);
        if (correct) {
            rightResponsePanel.SetActive(true);
            score++;
        }
        else {
            wrongResponsePanel.SetActive(true);
            score--;
        }
        scorePanel.GetComponentInChildren<Text>().text = "" + score;
    }

    public void NewQuiz() {
        rightResponsePanel.SetActive(false);
        wrongResponsePanel.SetActive(false);
        restartButton.SetActive(false);
        activeTest.GenerateAnswer();
        
    }
    

    void OnApplicationQuit() {
        chuck.Close();
        UnityEngine.Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
