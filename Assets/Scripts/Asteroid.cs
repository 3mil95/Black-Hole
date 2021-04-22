using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    float g = 350f;

    
    Vector3 speed = new Vector3(0,5,0);
    // Start is called before the first frame update
    void Start()
    {
        float scale = Random.Range(0.5f, 1.5f);
        transform.localScale = transform.localScale * scale;
        g *= scale;
    }

    // Update is called once per frame
    void Update()
    { 
        float deltaTime = Time.deltaTime * GameScript.gameScript.timeScale;
        Vector3 pos = transform.position;
        float dist = Vector3.Distance(new Vector3(0,0,0), pos);
        if (dist < 1) {
            Destroy(gameObject);
        }
        speed += (Vector3.Normalize(-pos) * g) / Mathf.Pow(dist, 2) * deltaTime; 
        pos += speed * deltaTime;
        pos.z = 0;
        transform.position = pos;
    }
}
