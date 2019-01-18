//This script starts playing a loop and write the name of the file followed by the shred ID that is playing it to ActiveLoops.txt
if (me.args()!=1)<<<"not enough args","">>>;
else {
    <<<"Opening: ",me.arg(0),"\".wav\"">>>;
    <<<"Current Directory: ",me.dir()>>>;
    me.arg(0)=>string name;
    //first read the contents of ActiveLoops.txt
    me.dir()+"/ActiveLoops.txt"=>string fileName;
    //read the contents into the following string then append the name and ShredID
    string fileText;

    FileIO fin;
    // open a file
    fin.open( fileName, FileIO.READ );
    // ensure it's ok
    if( !fin.good() ) {
        5::second=> now;
        cherr <= "can't open file: " <= fileName <= " for reading..." <= IO.newline();
        me.exit();
    }
    //read line by line
    while(fin.more()) {
        fin.readLine()=>string tmp;
        <<<tmp,"">>>;
        fileText+tmp+"\n"=>fileText;
    }
    fin.close();
    //append the name and shread id to the file
    fileText + me.arg(0)+ " "+me.id()=>fileText;
    //now write the string back to the file

    FileIO fout;
    // open a file
    fout.open( fileName, FileIO.WRITE );
    // ensure it's ok
    if( !fout.good() ) {
        cherr <= "can't open file: " <= fileName <= " for reading..." <= IO.newline();
        5::second=>now;
        me.exit();
    }

    fout<=fileText;
    fout.close();
    me.dir()+"../Loops/"+name +".wav"=>string loopPath;
    SndBuf buf=>dac;
    while (true) {
        loopPath =>buf.read;
        (buf.length()/buf.rate())=>now;
    }

}
