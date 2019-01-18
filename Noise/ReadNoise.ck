//recommended args: (bpm, gain, rootNote)
//arguements separated by a colon
int bpm;
//the time(seconds) of one beat
float beat;
//a number between 0 and 1 that sets the volume
float volume;
//the midi int of the root note
int rootNote;

if(me.args() == 3) {
    Std.atoi(me.arg(0)) =>bpm;
    60/Std.atof(me.arg(0)) => beat;
    Std.atof(me.arg(1)) => volume;
    Std.atoi(me.arg(2)) => rootNote;
}
else {
    <<<"Fix your args","">>>;
    <<<"","Expected: bpm:volume:rootNote">>>;
    me.exit();
}
PitShift shift =>dac;
MidiOscillator mOsc;
mOsc.init(shift, bpm, volume, 47);
me.dir()+"/../Noise/noise-perlin6.txt"=>string fileName;
IntArray noiseValues;
[0,2,7,2,4,2,7,12,14]@=> int shiftValues[];
FileIO fin;
// open a file
fin.open( fileName, FileIO.READ );
if( !fin.good() ) {
    5::second=> now;
    cherr <= "can't open file: " <= fileName <= " for reading..." <= IO.newline();
    me.exit();
}
//for changing the pitch shift value
0=>int i;
//read line by line
//each new line has one integer number
while(fin.more()) {
    fin.readLine()=>string tmp;
    Std.atoi(tmp) => int UwU;
    noiseValues.add(UwU);
    if (noiseValues.size()>7) {
        noiseValues.print();
        i++;
        if (i> shiftValues.cap()-1) 0 =>i;
        shiftValues[i]=>shift.shift;
        mOsc.playNotes(noiseValues.elements, beat/16);
        noiseValues.clear();

    }
}
fin.close();
noiseValues.print();

private class MidiOscillator {
    // 100:.2:60
    //recommended args: (bpm, gain, rootNote)
    //arguements separated by a colon
    int bpm;
    //the time(seconds) of one beat
    float beat;
    //a number between 0 and 1 that sets the volume
    float volume;
    //the midi int of the root note
    int rootNote;
    //a sin osc for each midi note
    SinOsc oscillators[128];
    //an adsr filter for each note
    //finalEnvolope used to be an adsr and proceeds preADSR which is why it is named that way
    ADSR preAdsr[128];
    Envelope finalEnvelope;
    Gain audioSource;
    Gain phaseOne;
    NRev reverbEffect;
    Delay delayEffect;
    Gain gain;
    float attack, delay, sustain, release;
    int reverbActive, delayActive;
    //a list of all the active notes
    IntArray activeNotes;


    fun void init(UGen output, int bpm_, float volume_, int rootNote_) {

        bpm_ =>bpm;
        60/(bpm_ $ float) => beat;
        volume_ => volume;
        rootNote_ => rootNote;
        //audioSource=>phaseOne=>finalEnvelope=>gain=>output;
        audioSource=>phaseOne;
        phaseOne =>gain;
        0.2=>attack;
        0.1=>delay;
        0.5=>sustain;
        0.1=>release;
        gain=>output;
        volume => gain.gain;
        if (volume==0)<<<"VOLUME IS ZERO","">>>;
        //an array of adsr settings: AttackTime, DelayTime, Sustain, Release
        [beat/2, beat, beat/8, beat/8] @=> float preAdsrSettings[];
        //adsr.set(adsrSettings[0]::second, adsrSettings[1]::second, adsrSettings[2], adsrSettings[3]::second);

        //instantiate the elements in the the array
        for (0=>int i;i<oscillators.cap();i++) {
            oscillators[i] =>preAdsr[i]=> audioSource;
            //apply setting to the adsr
            Std.mtof(i) => oscillators[i].freq;
            1 => oscillators[i].gain;

        }
        //listens for key presses to play notes
        spork~listenForEvents();
        //sets adsr values every 20 ms
        setADSR();
    }
    //a function to set the volume
    fun void setVolume(float _volume) {
        _volume=>volume;
        volume=>gain.gain;
    }
    //uses public variables to change the value instead of passing the var in the func
    fun void setADSR() {
        if (sustain ==0)<<<"Sustain is zero!! you wont get any sound from the Osc","">>>;
        //<<<"ADSR: ",attack, " ", delay," ", sustain, " ", release>>>;
        for (0=>int i;i<preAdsr.cap();i++) {
            preAdsr[i].set(attack::second, delay::second, sustain, release::second);
        }
    }
    //im using a float as a boolean cause im dumb
    fun void setReverbActive(int f) {
        if (f!=reverbActive) {
            if (f==0) {
                <<<"disconnecting"," reverb">>>;
            }
            else if (f==1) {
                <<<"connecting"," reverb">>>;
            }
        }
        f=>reverbActive;
    }
    fun void setReverbMix(float f) {
        //f=>reverb.mix;
    }
    fun void setRootNote(int rootNote_) {
        if (rootNote==rootNote_) return;
        else {
            rootNote_=>rootNote;
            <<<"rootNote is now: ",rootNote>>>;
            for (0=>int i;i<oscillators.cap();i++) {
                //apply setting to the adsr
                Std.mtof(i) => oscillators[i].freq;
                1 => oscillators[i].gain;

            }
        }
    }

    fun void setDelayActive(int active) {
        if (delayActive==1){

        }
        else if (delayActive==0) {

        }
    }
    fun void setDelayTime(float delayTime){}
    fun void setDelayMax(float delayMax){}
    //a function for debugging
    fun void listenForEvents() {
        // which keyboard
        0 => int device;
        // HID
        Hid hi;
        HidMsg msg;
        // open keyboard (get device number from command line)
        if( !hi.openKeyboard( device ) ) me.exit();
        IntArray keys;
        //z,a,q go up by fifths. with 7 frets on each row
        keys.add([16,17,18,19,20,21,22,23,24,25,
            30,31,32,33,34,35,36,37,38,
            44,45,46,47,48,49,50]);
        IntArray notes;
        //the notes that's index corresponds to the index of keys
        notes.add([10,11,12,13,14,15,16,17,18,19,
            5,6,7,8,9,10,11,13,14,
            0,1,2,3,4,5,6]);
        IntArray activeNotes;
        // infinite event loop
        while( true )
        {
            //starts at z, a is 1 fifth up, q, a second fifth up.
            [1]
            @=>int keyboardLayout[];
            // wait for event
            hi => now;

            // get message
            while( hi.recv( msg ) )
            {
                // if the button is pressed down
                if( msg.isButtonDown() )
                {
                    keys.indexOf(msg.which)=> int index;
                    //if its a valid key
                    if (index!=-1) {
                        notes.get(index) => int note;
                        //if the note isnt active
                        if (activeNotes.contains(note)==0) {
                            activeNotes.add(note);
                            notesOn([note]);
                        }
                        //<<<"down "+ notes.get(index),"">>>;

                    }
                    //<<< msg.which,"">>>;


                    80::ms => now;
                }
                else //its been released
                {
                    keys.indexOf(msg.which)=> int index;
                    //if its a valid key
                    if (index!=-1) {
                        notes.get(index) => int note;
                        //if the note is active
                        if (activeNotes.contains(note)!=-1) {
                            activeNotes.remove(note);
                            notesOff([note]);
                        }
                        //<<<"up "+ notes.get(index),"">>>;
                    }
                }
                //(1/(mOsc.activeNotes.size() $ float))=>mOsc.gain.gain;
            }
        }

    }

    //notes is an array of ints that are the offset from rootNote of the notes to play
    fun void notesOn(int notes[]) {
        for(0=>int i;i<notes.cap();i++) {
            activeNotes.add(notes[i]);
            preAdsr[rootNote+notes[i]].keyOn();
        }
        //if (activeNotes.size()>0)finalEnvelope.keyOn();
        //activeNotes.print();
        (1/(activeNotes.size()$float))=>audioSource.gain;
    }

    fun void notesOff(int notes[]) {
        for(0=>int i;i<notes.cap();i++) {
            activeNotes.remove(notes[i]);
            preAdsr[rootNote+notes[i]].keyOff();
        }
        if (activeNotes.size() == 0) {
            //finalEnvelope.keyOff();
        }
        //activeNotes.print();
        if (activeNotes.size()>1) (1/(activeNotes.size()+1)$float)=>audioSource.gain;
        else 1=>audioSource.gain;
    }

    fun void playNotes(int notes[], float duration) {
        for (0=>int i;i<notes.cap();i++) {
            notesOn([notes[i]]);
            (3*duration/4)::second=>now;
            notesOff([notes[i]]);
            (duration/4)::second=>now;
        }
    }

    fun void wait(float duration) {
        duration::second=>now;
    }
}

private class IntArray {
    /*
    functions:
    add: int[] or int
    remove: int[] or int
    get: int index
    indexOf:int element
    contains returns 0 if no, 1 if yes
    print: void, prints the array
    size return the size of the array

    */
    int elements[];
    //add an array of ints
    fun void add(int newElements[]) {
        for (0=>int i;i<newElements.cap();i++) {
            add(newElements[i]);
        }
    }
    //add a single int
    fun void add(int element) {
        //if its empty
        if (elements==null) {
            [element]@=> elements;
        }
        else { //creates a new array and appends the new element
            elements.cap()+1=>int newArraySize;
            int newElements[newArraySize];
            for (0=>int i;i<newArraySize;i++) {
                //if its at the end
                if (i==newArraySize-1) {
                    //append element
                    element=>newElements[i];
                }
                else {
                    elements[i]=>newElements[i];
                }
            }
            newElements@=>elements;
        }
    }
    //removes a list of numbers
    fun void remove(int removeThis[]) {
        for (0=>int i;i<removeThis.cap();i++) {
            remove(removeThis[i]);
        }
    }
    //removes the element parameter (not the index)
    fun void remove(int element) {
        indexOf(element)=> int index;
        //if the element isnt here
        if (index==-1) {
            <<<"the element you're trying to remove isn't here","">>>;
        }
        else {//the element is in the array
            int newElements[];
            if (elements.cap()>1) { //if the array is bigger than 1
                int newElements[elements.cap()-1];
                for (0=>int i;i<elements.cap();i++) {
                    if (i==index) {
                        continue;
                    }
                    if (i>index) {
                        elements[i]=>newElements[i-1];
                    }
                    else {
                        elements[i]=>newElements[i];
                    }
                }
                newElements@=>elements;
            }
            else if(elements.cap()==1){
                 null@=>elements;
            }
        }

    }
    //returns the element @ index
    fun int get(int index) {
        if (index >=elements.cap()) <<<"invalid index","">>>;
        else return elements[index];
    }
    //returns the index of the element
    //if not found returns -1
    fun int indexOf(int element) {
        //int
        -1=>int contains;
        if (elements!= null) {
            for (0=>int i;i<elements.cap();i++) {
                if (elements[i]==element) {
                    i=>contains;
                    return contains;
                }
            }
        }
        return contains;
    }

    fun int contains(int element) {
        indexOf(element)=>int result;
        if (result ==-1) return 0;
        else return 1;
    }
    fun void print() {
        ""=>string result;
        if(elements==null) {
            <<<"[]","">>>;
            return;
        }
        for (0=>int i;i<elements.cap();i++) {
            //if its the last element
            if(i==elements.cap()-1) {
                //dont put a comma
                elements[i]+"" +=>result;
            }
            else {
                //do
                elements[i] + "," +=>result;
            }
        }
        <<<("["+result+"]"),"">>>;
    }
    fun int size() {
        if(elements==null) return 0;

        else return elements.cap();
    }
    fun void clear() {
        null@=>elements;
    }
}
