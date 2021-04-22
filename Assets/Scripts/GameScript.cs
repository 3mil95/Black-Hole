using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{

    public float timeScale { get; private set; } = 0;

    bool started = false;
    //bool isGameOver = false;
    public GameObject asteroid;
    float spawnRate = 1;
    float nextSpawn = 0;
    float radius = 10;
    public int lives { get; private set; } = 3;

    public float score = 0;

    static public GameScript gameScript;

    public GameScript() {
        gameScript = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int LivesAfterTakeDmg() {
        lives--;
        if (lives <= 0) {
            GameScript.gameScript.GameOver();
        }
        UIHandler.main.SetLife(lives);
        return lives;
    }

    private void GameOver() {
        bool newHiScore = setHiScore(score); 
        UIHandler.main.openMenu(score, PlayerPrefs.GetFloat("HiScore"), newHiScore);
    }

    public void setScore(float newScore) {
        score = newScore;
        UIHandler.main.setScore(score);
    }

    private bool setHiScore(float score) {
        float hiScore = PlayerPrefs.GetFloat("HiScore");
        if(score > hiScore) {
            PlayerPrefs.SetFloat("HiScore", score);
            PlayerPrefs.Save ();
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 || Input.GetAxisRaw("Horizontal") != 0) && !started) {
            timeScale = 1;
        }

        nextSpawn -= Time.deltaTime * timeScale;
        if (nextSpawn <= 0) {
            spawn();
            nextSpawn = spawnRate;
        }
        
    }

    void spawn() {
        float angle = Random.Range(0,360f);
        Vector3 pos = Quaternion.Euler(0,0,angle) * Vector3.up * radius;
        pos.z = 0;
        Instantiate(asteroid, pos, Quaternion.identity);
    }
}
