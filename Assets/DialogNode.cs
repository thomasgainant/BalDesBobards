using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogNode {
    public Play play;

    public string id = "";
    public string content = "";
    public string effect = "";

    public List<string> answers = new List<string>();

    public bool alreadyVisited = false;

    public DialogNode(Play play, string id, string content, string effect, List<string> answers)
    {
        this.play = play;
        this.id = id;
        this.content = content;
        this.effect = effect;
        this.answers = answers;

        string answersLog = "";
        foreach(string answer in this.answers)
        {
            answersLog += answer;
        }
        //Debug.Log("new node: "+this.id+", "+this.content+", "+this.effect+", "+ answersLog);
    }

    public void appearInText(UnityEngine.UI.Text text, bool instant)
    {
        if(instant)
        {
            text.text = this.content;
        }
        else
        {
            this.play.appearText(text, this.content);
        }
    }

    public void applyEffect()
    {
        if(this.effect != ""){
            string[] rawValues = this.effect.Split(',');

            int aValue = int.Parse(rawValues[0]);
            int gValue = int.Parse(rawValues[1]);
            int dValue = int.Parse(rawValues[2]);
            int eValue = int.Parse(rawValues[3]);

            foreach (Elector elector in this.play.electors)
            {
                switch (elector.main)
                {
                    case Elector.ElectorCategory.Anar:
                        if(aValue > 0.0f)
                        {
                            elector.sympathy += aValue * 0.05f;
                        }
                        else
                        {
                            elector.sympathy += aValue * 0.2f;
                        }
                        break;
                    case Elector.ElectorCategory.Gauche:
                        if (gValue > 0.0f)
                        {
                            elector.sympathy += gValue * 0.05f;
                        }
                        else
                        {
                            elector.sympathy += gValue * 0.2f;
                        }
                        break;
                    case Elector.ElectorCategory.Droite:
                        if (dValue > 0.0f)
                        {
                            elector.sympathy += dValue * 0.05f;
                        }
                        else
                        {
                            elector.sympathy += dValue * 0.2f;
                        }
                        break;
                    case Elector.ElectorCategory.Extreme:
                        if (eValue > 0.0f)
                        {
                            elector.sympathy += eValue * 0.05f;
                        }
                        else
                        {
                            elector.sympathy += eValue * 0.2f;
                        }
                        break;
                }

                switch (elector.secondary)
                {
                    case Elector.ElectorCategory.Anar:
                        elector.sympathy += aValue * 0.025f;
                        break;
                    case Elector.ElectorCategory.Gauche:
                        elector.sympathy += gValue * 0.025f;
                        break;
                    case Elector.ElectorCategory.Droite:
                        elector.sympathy += dValue * 0.025f;
                        break;
                    case Elector.ElectorCategory.Extreme:
                        elector.sympathy += eValue * 0.025f;
                        break;
                }

                if (elector.sympathy > 1.0f)
                {
                    elector.sympathy = 1.0f;
                }

                if (elector.sympathy < 0.0f)
                {
                    elector.sympathy = 0.0f;
                }
            }
        }
    }
}
