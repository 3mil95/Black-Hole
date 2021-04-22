using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LifeUI : MonoBehaviour
{
    RawImage[] livesImages;

    [SerializeField]
    private Color lostLifeColor; 
    [SerializeField]
    private Color LifeColor; 
    
    private RectTransform rectTransform;


    void Start()
    {
        livesImages = GetComponentsInChildren<RawImage>(); 
        rectTransform = GetComponent<RectTransform>();

        float height = rectTransform.rect.height;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height * 4);        
    }
 
    
    /*void Update() {
        Debug.Log("apa");
        RectTransform rectTransform = GetComponent<RectTransform>();
        float height = rectTransform.rect.height;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height * 4);    
    }*/

    // Update is called once per frame
    public void SetLifes(int lifes)
    {
        for (int i = 0; i < livesImages.Length; i ++) {
            if (lifes - i > 0) {
                livesImages[i].color = LifeColor;
                continue;
            }
            livesImages[i].color = lostLifeColor;
        }
    }
}
