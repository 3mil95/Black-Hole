using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    // Start is called before the first frame update    
    public static UIHandler main{ get; private set; } 

    [SerializeField]
    private Text scoreText; 

    [SerializeField]
    private LifeUI lifeUI;


    [Space]
    [SerializeField]
    private Canvas menu;

    [SerializeField]
    private Text manuScore;
    [SerializeField]
    private Text manuHiScore;

    


    private void Start() {
        menu.enabled = false;
    }

    public UIHandler() {
        main = this;
    }

    public void SetLife(int life) {
        lifeUI.SetLifes(life);
    }

    public void setScore(float score) {
        scoreText.text = "Score: " + score.ToString();
    }

    public void restartLevel() {
        SceneManager.LoadScene(0);
    }

    public void openMenu(float score, float hiScore, bool isNewHiScore) {
        manuScore.text = score.ToString();  
        manuHiScore.text = "Hi-Score: " + hiScore.ToString();
        menu.enabled = true;
    }
}
