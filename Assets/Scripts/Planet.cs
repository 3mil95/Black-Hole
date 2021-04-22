using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Planet : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public GameObject hit;
    public GameObject planeExp;
    public Sprite[] sprites;
    float angle = 180;
    float radius = 3;

    public CameraHandler ch;


    float angleAcc = 200f;
    //float angleSpeed = 0f;

    float turnMin = 0;
    float turnMax = 0;



    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = sprites[GameScript.gameScript.lives-1];
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Vector3 pos = other.transform.position;
        Destroy(other.gameObject);
        
        pos.z = 0;
        GameObject go = Instantiate(hit, pos, Quaternion.identity);
        go.transform.parent = gameObject.transform;
        Destroy(go, 10f);

        Vector3 dir = transform.position - pos;
        dir.z = 0;
        dir = Vector3.Normalize(dir);
        ch.ScreenShack(dir);
        
        int lives = GameScript.gameScript.LivesAfterTakeDmg();
        if (lives == 0) { 
            Instantiate(planeExp, transform.position, transform.rotation);
            Destroy(gameObject);
            return; 
        }
        spriteRenderer.sprite = sprites[lives-1];
    }

    float getInput() {
        float input = Input.GetAxis("Horizontal");

        if (Input.touchCount > 0 && input == 0) {
            float clickPosX = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x;
            if (clickPosX > 0) {
                input = 1;
            } else if (clickPosX < 0) {
                input = -1;
            } else {
                input = 0;
            }
        }

        return input;

    }
    
    void Update()
    {
        float deltaTime = Time.deltaTime * GameScript.gameScript.timeScale;
        float input = getInput();

        angle += angleAcc * input * deltaTime;
        
        Vector3 pos = transform.position;
        pos = Quaternion.Euler(0, 0, angle) * Vector3.up * radius;
        pos.z = 0;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(0, 0, angle);  

        updateScore();
        debug();  
    }

    void updateScore() {
        if (angle - 180 > turnMax) {
            turnMax = angle - 180;
        } else if (angle - 180 < turnMin) {
            turnMin = angle - 180;
        }

        if (Mathf.Abs(turnMin) > turnMax) {
            GameScript.gameScript.setScore(Mathf.Round(Mathf.Abs(turnMin) / 3.6f) / 100);
            return;
        }
        GameScript.gameScript.setScore(Mathf.Round(turnMax / 3.6f) / 100);
        /*if (input < 0)
            turnStartRigth += input * angleAcc * deltaTime;
        else
            turnStartLeft += input * angleAcc * deltaTime;

        angleSpeed = input * angleAcc; // Time.deltaTime;

        angle += angleSpeed * deltaTime;*/

    }


    private void debug() {
        Vector3 posC = new Vector3(0,0,0);
        Vector3 pos1 = Quaternion.Euler(0, 0, angle) * Vector3.up * (radius + 0.5f);
        Vector3 pos2 = Quaternion.Euler(0, 0, turnMin) * Vector3.up * (radius + 0.5f);
        Vector3 pos3 = Quaternion.Euler(0, 0, turnMax) * Vector3.up * (radius + 0.5f);
        pos1.z = 0;
        pos2.z = 0;
        pos3.z = 0;

        Debug.DrawLine(posC, pos1, Color.white);
        Debug.DrawLine(posC, pos2, Color.green);
        Debug.DrawLine(posC, pos3, Color.green);
    }
}
