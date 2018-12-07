if (me.args()<1)<<<"not enough args","">>>;
else {
    std.atoi(me.arg(0))=>int killThisShred;
    Machine.remove(killThisShred);
}
