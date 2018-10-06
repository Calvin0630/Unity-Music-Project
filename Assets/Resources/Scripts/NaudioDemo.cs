using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Threading;

public class NaudioDemo{
    //output bytes is a queue that stores speaker output data.
    public Queue<byte> outputBytes;
    public NaudioDemo() {
        var outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "NAudio");
        Directory.CreateDirectory(outputFolder);
        var outputFilePath = Path.Combine(outputFolder, "recorded.wav");
        var capture = new WasapiLoopbackCapture();
        var writer = new WaveFileWriter(outputFilePath, capture.WaveFormat);
        capture.DataAvailable += (s, a) =>
        {
            writer.Write(a.Buffer, 0, a.BytesRecorded);
            if (writer.Position > capture.WaveFormat.AverageBytesPerSecond * 20) {
                capture.StopRecording();
            }
        };
        capture.RecordingStopped += (s, a) =>
        {
            writer.Dispose();
            writer = null;
            capture.Dispose();
        };
        capture.StartRecording();
        while (capture.CaptureState != NAudio.CoreAudioApi.CaptureState.Stopped) {
            Thread.Sleep(500);
        }



    }
    

    public void GetSpeakerOutput() {

    }
	
}
