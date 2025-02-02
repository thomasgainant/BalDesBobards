using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LocalizationManager : MonoBehaviour {
    public enum LANG{
        SELECTING,
        FR,
        EN,
        DE
    }

    public LANG lang = LANG.SELECTING;

    public Play play;

    public static string MARK_FOR_MAPPING = "__loc__";
    public Dictionary<UnityEngine.UI.Text, string> localizedTextsMapping = new Dictionary<UnityEngine.UI.Text, string>();
    public Dictionary<string, string> currentStrings;

    void Start () {
        this.play = GameObject.FindObjectOfType<Play>();

        UnityEngine.UI.Text[] texts = GameObject.FindObjectsOfType<UnityEngine.UI.Text>();
        foreach(UnityEngine.UI.Text text in texts){
            if(text.text.StartsWith(LocalizationManager.MARK_FOR_MAPPING)){
                this.localizedTextsMapping.Add(text, text.text.Substring(LocalizationManager.MARK_FOR_MAPPING.Length));
            }
        }

        this.updateLang();
    }

    public void setLangToFR(){
        this.setNewLang(LANG.FR);
    }

    public void setLangToDE(){
        this.setNewLang(LANG.DE);
    }

    public void setLangToEN(){
        this.setNewLang(LANG.EN);
    }

    private void setNewLang(LANG newLang){
        this.lang = newLang;
        Debug.Log(this.lang);
        this.updateLang();
    }

    private void updateLang(){
        this.updateLocalizedTexts();

        if(this.lang == LANG.EN){
            this.play.dialogs = Resources.Load<TextAsset>("scenario_en");
        }
        else if(this.lang == LANG.DE){
            this.play.dialogs = Resources.Load<TextAsset>("scenario_de");
        }
        else{
            this.play.dialogs = Resources.Load<TextAsset>("scenario");
        }
    }

    public void updateLocalizedTexts(){
        
        if(this.lang == LANG.EN){
            this.currentStrings = this.strings_en;
        }
        else if(this.lang == LANG.DE){
            this.currentStrings = this.strings_de;
        }
        else{
            this.currentStrings = this.strings_fr;
        }

        foreach(KeyValuePair<UnityEngine.UI.Text, string> entry in this.localizedTextsMapping){
            string content = "";
            if(this.currentStrings.TryGetValue(entry.Value, out content)){
                entry.Key.text = String.Format(content);
            }
            else{
                entry.Key.text = entry.Value;
            }
        }
    }

    public string getLocalizedString(string key){
        string content = "";
        if(this.currentStrings.TryGetValue(key, out content)){
            return String.Format(content);
        }
        else{
            return key;
        }
    }

    public Dictionary<string, string> strings_fr = new Dictionary<string, string>(){
        {"title", "Le Bal des Bobards"},
        {"voters", "électeurs"},
        {"candidacy_majority", "Majorite absolue"},
        {"candidacy_relative", "Majorite relative"},
        {"candidacy_minority", "Candidat minoritaire"},
        {"candidacy_insignificant", "Candidature derisoire"},
        {"result_total_convinced_start", "Vous avez convaincu "},
        {"result_total_convinced_end", " électeurs de voter pour vous."},
        {"result_majority", "Vous êtes certain d'être élu aux prochaines élections présidentielles."},
        {"result_relative", "Vous êtes certain de passer le premier tour des prochaines élections présidentielles."},
        {"result_insignificant", "Vous êtes certain de ne pas passer le premier tour des prochaines élections présidentielles."},
        {"result_voters_start", "Parmi ces électeurs se trouvent :"},
        {"result_voters", @"Électeurs d'extrême gauche
Électeurs de gauche
Électeurs de droite
Électeurs d'extrême droite"},
        {"message", "En politique, ce qui est important, ce n'est pas ce qu'on fait et ce qu'on a fait, c'est de dire aux gens ce qu'ils veulent entendre devant leur café matinal."}
    };
    public Dictionary<string, string> strings_de = new Dictionary<string, string>(){
        {"title", "Der Luegenball"},
        {"voters", "Waehler"},
        {"candidacy_majority", "Qualifizierte Mehrheit"},
        {"candidacy_relative", "Relative Mehrheit"},
        {"candidacy_minority", "Minderheits- kandidat"},
        {"candidacy_insignificant", "Unbedeutende Kandidatur"},
        {"result_total_convinced_start", "Sie haben "},
        {"result_total_convinced_end", " Wähler überzeugt, Sie zu wählen."},
        {"result_majority", "Sie sind sicher, als Kanzler nominiert zu werden."},
        {"result_relative", "Sie sind sich sicher, nach einer Koalition zum Kanzler nominiert zu werden."},
        {"result_insignificant", "Sie müssen unbedingt eine Koalition verhandeln, um als Kanzler nominiert zu werden."},
        {"result_voters_start", "Zu diesen Wählern gehören :"},
        {"result_voters", @"Linksradikalen
Linke Wähler
Rechte Wähler
Rechtsextremisten"},
        {"message", "Bei Politik geht es nicht um was man tut und was man getan hat, sondern um den Leuten zu sagen, was sie bei ihrem Morgenkaffee hören wollen."}
    };
    public Dictionary<string, string> strings_en = new Dictionary<string, string>();
}