using UnityEngine;
using System.Collections;

public class SpriteAnimation {

    public SpriteRenderer renderer;
    public Sprite[] sprites;
    public int currentFrame;
    public float speed;//In ms per frame

    public SpriteAnimation(GameObject rendererObj, Sprite[] sprites, int startingFrame, float speed)
    {
        this.renderer = rendererObj.GetComponent<SpriteRenderer>();
        this.sprites = sprites;
        this.currentFrame = startingFrame;
        this.speed = speed;
    }

    public IEnumerator update()
    {
        while(this.currentFrame < this.sprites.Length)
        {
            this.renderer.sprite = this.sprites[this.currentFrame];
            this.currentFrame++;
            yield return new WaitForSeconds(this.speed/1000.0f);
        }
    }	
}
