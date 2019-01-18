using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;

public class MyProcess  {
	public Process main;
	StreamWriter input;
    //
	public List<Process> processList = new List<Process>();

	public MyProcess() {
		main = new Process();
		ProcessStartInfo startInfo = new ProcessStartInfo();
		startInfo.FileName = "chuck";
        startInfo.Arguments = "--loop";
        startInfo.RedirectStandardOutput = false;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardError = false;
        startInfo.UseShellExecute = false;
        startInfo.WindowStyle = ProcessWindowStyle.Minimized;
        startInfo.CreateNoWindow = false;
        //startInfo.CreateNoWindow = true;

        main.StartInfo = startInfo;
		main.Start();
    }

    public string GetOutput() {
        string line = "";
        if (!main.StandardOutput.EndOfStream) line = main.StandardOutput.ReadLine();
        return line;
    }
	public void ExecuteCommand(String command) {
        Process tmp = new Process();
        processList.Add(tmp);
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "chuck";
        //startInfo.Arguments = "/min "+command;
        startInfo.Arguments = command;
        startInfo.WindowStyle = ProcessWindowStyle.Minimized;
        tmp.StartInfo = startInfo;
        tmp.Start();
	}
    //shred name is the name of the shred including file extension
    public void RemoveShred(string shredName) {
        //remove a shred with the above name
    }
	// Use this for initialization
	public void Close () {
        ExecuteCommand("chuck --removeall");
        main.CloseMainWindow();
	}

	// Update is called once per frame
	void Update () {

	}
}
