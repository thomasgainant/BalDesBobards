using UnityEngine;
using System.Collections;

public class Elector {

    //Anar, Gauche, droite ou Extreme
    //Une catégorie primaire, une secondaire
    public enum ElectorCategory
    {
        Anar,
        Gauche,
        Droite,
        Extreme
    }

    public ElectorCategory main;
    public ElectorCategory secondary;

    public float sympathy = 0.0f;

    public Elector()
    {
        float randomMain = Random.Range(0.0f, 1.0f);
        if(randomMain < 0.2f){
            this.main = ElectorCategory.Anar;
        }
        else if(randomMain >= 0.2f && randomMain <= 0.5f)
        {
            this.main = ElectorCategory.Gauche;
        }
        else if(randomMain > 0.5f && randomMain <= 0.8f)
        {
            this.main = ElectorCategory.Droite;
        }
        else if(randomMain > 0.8f)
        {
            this.main = ElectorCategory.Extreme;
        }

        int randomSecondary = Random.Range(0, 4);
        if (randomSecondary == 0)
        {
            this.secondary = ElectorCategory.Anar;
        }
        else if(randomSecondary == 1)
        {
            this.secondary = ElectorCategory.Gauche;
        }
        else if(randomSecondary == 2)
        {
            this.secondary = ElectorCategory.Droite;
        }
        else if(randomSecondary == 3)
        {
            this.secondary = ElectorCategory.Extreme;
        }

        this.sympathy = Random.Range(0.0f, 0.25f);
    }
	
}
