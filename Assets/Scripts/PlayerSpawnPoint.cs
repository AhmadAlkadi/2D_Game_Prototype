/***************************************************************
*file: Turret_Targeting.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 – Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: Allows the player to spawn when they die
*
*References:
*https://docs.unity3d.com/ScriptReference/index.html
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerSpawnPoint : MonoBehaviour
{
    public GameObject player;
    public int playerLife = 2;
    private float invTimer;
    public float invTimerLimit = 1f;
    public TextMeshProUGUI lifeCounter;
    PlayerMovement playerMovement;
    public float spawnYOffset = 3.0f;

    public void SetDeath(int death)
    {
        playerLife = death;
    }

    // Start is called before the first frame update
    void Start()
    {
        lifeCounter.text = playerLife.ToString();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.gameObject.activeSelf == false && playerLife == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }

        invTimer += Time.deltaTime;
        if (invTimer > invTimerLimit)
        {
            playerMovement.setInvincibility(false);
            playerMovement.GetComponent<Collider2D>().excludeLayers ^= LayerMask.GetMask("Enemy");
        }
        else
        {
            playerMovement.setInvincibility(true);
            playerMovement.GetComponent<Collider2D>().excludeLayers |= LayerMask.GetMask("Enemy");
        }

        if (playerMovement.gameObject.activeSelf == false && playerLife != 0)
        {
            transform.position = new Vector3(playerMovement.gameObject.transform.position.x, playerMovement.gameObject.transform.position.y + spawnYOffset);

            invTimer = 0;
            playerLife -= 1;
            lifeCounter.text = playerLife.ToString();

            var playerGun = playerMovement.GetComponentInChildren<gun>();
            playerGun.SetGun(gun.GUN_TYPE.NORMAL);
            playerGun.SetRapidFire(false);

            playerMovement.setStartPosition(this.transform.position);
            playerMovement.setPlayerHit(false);
            playerMovement.gameObject.SetActive(true);
        }
    }
}
