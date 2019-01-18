using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
	public MyProcess chuck;
    System.Random rand;
    //chuck synthVariablesare stored here:
    Dictionary<string, object> synthVariables;
    float synthVolume;
    int lfoActive;
    float attack, delay, sustain, release;
    float reverbActive;
    float reverbMix;
    float delayTime, delayMax;
    int delayActive;
    int synthRootNote;
    //the oscillations per second of the lfo
    float lfoRate;
    public GameObject SynthPanel, LooperPanel;
    private void Awake() {
        chuck = new MyProcess();

    }
    // Use this for initialization
    void Start () {
        rand = new System.Random();
        synthVariables = new Dictionary<string, object>() {
            {"SynthVolume", synthVolume },
            {"attack", attack },
            {"delay", delay },
            {"sustain", sustain },
            {"release", release },
            {"reverbActive", reverbActive },
            {"reverbMix", reverbMix },
            {"delayActive", delayActive },
            {"delayTime", delayTime },
            {"delayMax", delayMax },
            {"synthRootNote", synthRootNote },
        };
        chuck.ExecuteCommand("chuck --status");
        chuck.ExecuteCommand("chuck --status");
        chuck.ExecuteCommand("chuck + Chuck_Scripts//Main.ck:70:.5:76");
        chuck.ExecuteCommand("chuck --status");
        StartCoroutine(UpdateChuckVariables());
        InitChuckVariables();
        printSynthVars();
    }

	// Update is called once per frame
    //this changhes the cubes
	void Update () {
        //Debug.Log(chuck.GetOutput());
	}

    // a function to read the variables from settings.txt on start
    public void InitChuckVariables() {
        using (System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Calvin\Documents\Github\Music Project\Assets\Resources\settings.txt")) {
            string line = file.ReadLine();
            while (line != null) {
                //Debug.Log("line==null " + (line == null));
                //Debug.Log("line: \""+line+"\"");
                string[] words = line.Split(' ');
                if (words.Length == 2) {
                    synthVariables[words[0]] = int.Parse(words[1]);
                    Debug.Log(words[0] + " | " + words[1]);
                }
                line = file.ReadLine();
            }
        }
    }

    //writes the value of chuck synthVariablesinto the file settings.txt
    IEnumerator UpdateChuckVariables () {
        if (SynthPanel.activeSelf == true) {
            using (System.IO.StreamWriter file =
               new System.IO.StreamWriter(@"C:\Users\Calvin\Documents\Github\Music Project\Assets\Resources\settings.txt")) {
                foreach (KeyValuePair<string, object> pair in synthVariables) {
                    //write the line to the file.
                    String line = pair.Key + " " + pair.Value;
                    file.WriteLine(line);

                }
            }
        }
        yield return new WaitForSeconds(.5f);
        StartCoroutine(UpdateChuckVariables());
    }
    //a debug fn that prints synth vars
    public void printSynthVars() {
        Debug.Log("Synth Variables: ");
        foreach (KeyValuePair<string, object> pair in synthVariables) {
            //write the line to the file.
            String line = pair.Key + " " + pair.Value;
            Debug.Log(line);

        }

    }

    public void SetSynthVolume(float volume) {
        synthVolume = volume;
    }
    public void SetLFOActive(bool active) {
        //not active
        if (active) lfoActive = 1;
        //active
        else lfoActive = 0;
    }
    public void SetLFORate(float rate) {
        lfoRate = rate;
    }
    public void SetAttack(float attack_) {
        attack = attack_;
    }
    public void SetDelay(float delay_) {
        delay = delay_;
    }
    public void SetSustain(float sustain_) {
        sustain = sustain_;
    }   
    public void SetRelease(float release_) {
        release = release_;
    }
    public void SetReverbActive(bool b) {
        if (b) reverbActive = 1;
        else reverbActive = 0;
    }
    public void SetReverbMix(float f) {
        reverbMix = f;
    }
    public void SetDelayActive(bool b) {
        if (b) delayActive = 1;
        else delayActive = 0;
    }
    public void SetDelayTime(float f) {
        delayTime = f;
    }
    public void SetDelayMax(float f) {
        delayMax = f;
    }
    public void SetSynthRootNote(string rootNote) {
        synthRootNote = int.Parse(rootNote);
    }

    public void SetSynthPanelActive(bool b) {
        SynthPanel.SetActive(b);
    }

    public void SetLooperPanelActive(bool b) {
        LooperPanel.SetActive(b);
    }

    void OnApplicationQuit() {
		chuck.Close();
		UnityEngine.Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
