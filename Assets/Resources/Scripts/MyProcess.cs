using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;

public class MyProcess  {
	Process main;
	StreamWriter input;
	List<Process> processList = new List<Process>();

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

        main.StartInfo = startInfo;
		main.Start();
	}

	public string ExecuteCommand(String command, bool getOutput) {
        if (!getOutput) {
            Process tmp = new Process();
            processList.Add(tmp);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "chuck";
            //startInfo.Arguments = "/min "+command;
            startInfo.Arguments = command;
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            tmp.StartInfo = startInfo;
            tmp.Start();
            return "";
        }
        else if (getOutput) {
            return "";
        }
        return "";
	}
	// Use this for initialization
	public void Close () {
        main.CloseMainWindow();
	}

	// Update is called once per frame
	void Update () {

	}
}
