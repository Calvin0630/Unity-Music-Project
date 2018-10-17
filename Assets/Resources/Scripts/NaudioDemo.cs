using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
/*
using NAudio.CoreAudioApi;
using NAudio.Wave;
*/
using System.Threading;
using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
//using WinformsVisualization.Visualization;
using CSCore.DSP;
using CSCore.Streams;

public class NaudioDemo{
    //output bytes is a queue that stores speaker output data.
    public Queue<byte> outputBytes;
    public NaudioDemo() {
        using (WasapiCapture capture = new WasapiLoopbackCapture()) {
            //if nessesary, you can choose a device here
            //to do so, simply set the device property of the capture to any MMDevice
            //to choose a device, take a look at the sample here: http://cscore.codeplex.com/

            //initialize the selected device for recording
            capture.Initialize();

            //create a wavewriter to write the data to
            using (WaveWriter w = new WaveWriter("dump.wav", capture.WaveFormat)) {
                //setup an eventhandler to receive the recorded data
                capture.DataAvailable += (s, e) =>
                {
                    //save the recorded audio
                    w.Write(e.Data, e.Offset, e.ByteCount);
                };

                //start recording
                capture.Start();

                Thread.Sleep(1000);

                //stop recording
                capture.Stop();
            }
        }

    }



    public void GetSpeakerOutput() {

    }
	
}
