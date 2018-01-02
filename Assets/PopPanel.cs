using UnityEngine;
using System.Collections;

public class PopPanel : MonoBehaviour {

    public UnityEngine.UI.Text contentText;

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        this.gameObject.GetComponent<RectTransform>().offsetMax = Vector2.zero;

        this.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color.r,
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color.g,
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color.b,
                0.0f
        );

        StartCoroutine(this.handleMovement());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator handleMovement()
    {
        float ttl = 2.0f;

        while(ttl > 0.0f)
        {
            //Debug.Log(ttl);
            Vector2 lerp = Vector2.Lerp(this.gameObject.GetComponent<RectTransform>().offsetMin, new Vector2(0.0f, 150.0f), 0.95f * Time.deltaTime);
            this.gameObject.GetComponent<RectTransform>().offsetMin = lerp;
            this.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(lerp.x, lerp.y);
            ttl -= Time.deltaTime;

            this.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color.r,
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color.g,
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color.b,
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color.a + 0.5f * Time.deltaTime
            );

            yield return new WaitForEndOfFrame();
        }

        while(this.gameObject.GetComponent<UnityEngine.UI.Image>().color.a > 0.0f)
        {
            this.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color.r,
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color.g,
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color.b,
                this.gameObject.GetComponent<UnityEngine.UI.Image>().color.a - 0.5f*Time.deltaTime
            );

            yield return new WaitForEndOfFrame();
        }

        Destroy(this.gameObject);

        yield return null;
    }
}
