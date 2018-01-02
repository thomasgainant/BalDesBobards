using UnityEngine;
using System.Collections;

public class SoundSource : MonoBehaviour {

    public AudioSource source;

    public AudioClip clip;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void play(bool loop)
    {
        StartCoroutine(this.playRoutine(loop));
    }

    private IEnumerator playRoutine(bool loop)
    {
        this.source.clip = this.clip;
        this.source.loop = loop;
        this.source.Play();

        //Debug.Log(this.source.clip);
        //Debug.Log(loop+" - "+this.source.time+"/"+ this.source.clip.length);

        if(loop){
            while (true)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (this.source.time < this.source.clip.length)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        //Debug.Log("delete " + this.source.clip);

        Destroy(this.gameObject);
    }
}
