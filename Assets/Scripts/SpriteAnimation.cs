using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    private float frameTime = 0.3f;
    private float timer = 0;
    private int index = 0;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameTime) {
            timer = 0;
            spriteRenderer.sprite = sprites[index];
            index++;
            if (index >= sprites.Length) {
                index = 0;
            }
        }
    }
}
