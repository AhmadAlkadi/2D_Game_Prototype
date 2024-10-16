using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerSpawnPoint : MonoBehaviour
{
    public GameObject player;
    public int playerLife = 2;
    private float invTimer;
    public float invTimerLimit = 1f;
    public TextMeshProUGUI lifeCounter;
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        lifeCounter.text = playerLife.ToString();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
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
