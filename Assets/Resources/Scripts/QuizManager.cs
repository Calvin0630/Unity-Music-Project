using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour {
    System.Random rand;
    MyProcess chuck;
    Test activeTest;
    int score;
    public GameObject chooseQuizPanel, triadQuizPanel, IntervalQuizPanel, rightResponsePanel, wrongResponsePanel, loadQuizMenuButton, restartButton, scorePanel;
    public GameObject[] intervalQuizResponseButtons;
    // Use this for initialization
    void Start() {
        chuck = new MyProcess();
        rand = new System.Random();
        //UnityEngine.Debug.Log("start: \n" +output+"\n :end");
    }

    // Update is called once per frame
    void Update() {
        
    }

    void StartQuiz() {

    }

    public void StartTriadQuiz() {
        chooseQuizPanel.SetActive(false);
        triadQuizPanel.SetActive(true);
        scorePanel.SetActive(true);
        loadQuizMenuButton.SetActive(true);
        activeTest = new TriadTest();
        activeTest.GenerateAnswer();
        PlayAnswer();
        
    }

    public void StartIntervalQuiz() {
        chooseQuizPanel.SetActive(false);
        IntervalQuizPanel.SetActive(true);
        scorePanel.SetActive(true);
        loadQuizMenuButton.SetActive(true);
        //creates the IntervalTest object which extends test
        activeTest = new IntervalTest();
        activeTest.GenerateAnswer();
        //picks a random # from 0-2 (inclusive)
        int rightAnswerButtonIndex = rand.Next(0, 3);
        //starting at the top and going down
        for (int i=0;i<intervalQuizResponseButtons.Length;i++) {
            intervalQuizResponseButtons[i].SetActive(true);
            if (i== rightAnswerButtonIndex) {
                //set the text to the correct answer
                string rightAnswer = activeTest.GetAnswer();
                intervalQuizResponseButtons[i].GetComponentInChildren<Text>().text = rightAnswer;
            }
            else {
                //generate a false answer and set text
                string falseAnswer = activeTest.GetFalseAnswer();
                intervalQuizResponseButtons[i].GetComponentInChildren<Text>().text = falseAnswer;
            }
        }
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
        //if its an interval test
        if (activeTest is IntervalTest) {
            //take the number the button gave (0-2 starting from top button)
            GameObject button = intervalQuizResponseButtons[Int32.Parse(answer)];
            //and get the text component of the button to submit
            string intervalName = button.GetComponentInChildren<Text>().text;
            Debug.Log(intervalName);
            answer = intervalName;
        }
        bool correct =activeTest.GuessAnswer(answer);
        //update score
        if (correct) {
            rightResponsePanel.SetActive(true);
            score++;
        }
        else {
            wrongResponsePanel.SetActive(true);
            score--;
        }
        //update the score panel
        scorePanel.GetComponentInChildren<Text>().text = "" + score;
        StartCoroutine(DelayQuiz(1.5f));

    }

    public void NewQuiz() {
        activeTest.GenerateAnswer();
        PlayAnswer();
        
    }

    IEnumerator DelayQuiz(float seconds) {
        yield return new WaitForSeconds(seconds);
        NewQuiz();

    }

    public void ReturnToQuizMenu() {
        triadQuizPanel.SetActive(false);
        IntervalQuizPanel.SetActive(false);
        scorePanel.SetActive(false);
        score = 0;
        chooseQuizPanel.SetActive(true);
        loadQuizMenuButton.SetActive(false);
    }
    
    

    void OnApplicationQuit() {
        chuck.Close();
        UnityEngine.Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
