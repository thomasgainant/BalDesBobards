using UnityEngine;
using System.Collections;

public class ElectorPanel : MonoBehaviour{

    public static Color anarColor = Color.black;
    public static Color gaucheColor = Color.red;
    public static Color droiteColor = Color.blue;
    public static Color extremeColor = Color.white;

    public Elector elector;

    public bool deactivated = true;

	// Use this for initialization
	void Start () {
        switch(this.elector.main)
        {
            case Elector.ElectorCategory.Anar:
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color = ElectorPanel.anarColor;
                break;
            case Elector.ElectorCategory.Gauche:
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color = ElectorPanel.gaucheColor;
                break;
            case Elector.ElectorCategory.Droite:
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color = ElectorPanel.droiteColor;
                break;
            case Elector.ElectorCategory.Extreme:
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color = ElectorPanel.extremeColor;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(this.deactivated)
        {
            this.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(
                    this.gameObject.GetComponent<UnityEngine.UI.Image>().color.r,
                    this.gameObject.GetComponent<UnityEngine.UI.Image>().color.g,
                    this.gameObject.GetComponent<UnityEngine.UI.Image>().color.b,
                    0.0f
                );
        }
        else {
            if (this.elector.sympathy >= 0.5f)
            {
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(
                    this.gameObject.GetComponent<UnityEngine.UI.Image>().color.r,
                    this.gameObject.GetComponent<UnityEngine.UI.Image>().color.g,
                    this.gameObject.GetComponent<UnityEngine.UI.Image>().color.b,
                    1.0f
                );
            }
            else
            {
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(
                    this.gameObject.GetComponent<UnityEngine.UI.Image>().color.r,
                    this.gameObject.GetComponent<UnityEngine.UI.Image>().color.g,
                    this.gameObject.GetComponent<UnityEngine.UI.Image>().color.b,
                    0.25f
                );
            }
        }
	}
}
