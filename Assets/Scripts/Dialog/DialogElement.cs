using System;
using System.Collections.Generic;

[Serializable]
public class DialogElement
{
    public DialogSide speakingSide;
    public string text;

    public string SpeakerName(DialogMember[] members) => members[(int) speakingSide].name;
}
