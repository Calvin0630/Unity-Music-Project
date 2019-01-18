if (me.args()<1)<<<"not enough args","">>>;
else {
    //the arg is the name of the wav file (minus the .wav extension)
    //read Active loops.txt to get the shred#
    me.arg(0)=>string name;
    int shreadToKill;
    //first read the contents of ActiveLoops.txt
    me.dir()+"/ActiveLoops.txt"=>string fileName;
    //read the contents into the following string then append the name and ShredID
    string newFileText;

    FileIO fin;
    // open a file
    fin.open( fileName, FileIO.READ );
    // ensure it's ok
    if( !fin.good() ) {
        cherr <= "can't open file: " <= fileName <= " for reading..." <= IO.newline();
        5::second=>now;
        me.exit();
    }
    //read line by line
    while(fin.more()) {
        fin.readLine()=>string line;
        if (line.length() ==0) continue;
        line.trim();
        line.find(" ")=>int spaceIndex;
        line.substring(0,spaceIndex)=>string loopName;
        //takes a substring from the spaceIndex+1 to end
        line.substring(spaceIndex+1)=>string loopShredID;
        if (loopName ==name ) {
            Std.atoi(loopShredID)=>shreadToKill;
            Machine.remove(shreadToKill);
        }
        else {
            line+"\n"=>newFileText;
        }
    }
    fin.close();

    //need to rewrite the file so it doesnt contain the removed loop
    FileIO fout;
    fout.open( fileName, FileIO.WRITE);
    if( !fout.good() ) {
        cherr <= "can't open file: " <= fileName <= " for reading..." <= IO.newline();
        5::second=>now;
        me.exit();
    }

    fout<=newFileText;
    fout.close();
}
