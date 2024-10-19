using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossFight : MonoBehaviour
{
    public int health = 32;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        health -= 1;
        print("boss health: " + health);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2, LoadSceneMode.Single);
        }
    }
}
