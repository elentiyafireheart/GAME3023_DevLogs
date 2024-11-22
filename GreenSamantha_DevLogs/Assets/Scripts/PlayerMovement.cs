using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// sprite sheet from https://pipoya.itch.io/pipoya-free-rpg-character-sprites-32x32

// item sprites from https://opengameart.org/node/113951

public class PlayerMovement : MonoBehaviour
{

    public float _speed = 2.0f; // increase speed
    public bool _isWalking;
    public int _walkDirection;

    public bool hasEncountered;
    [SerializeField]
    private Animator _animator; // exposes the animator
    public GameObject Camera_1;
    public GameObject Camera_2;
    public GameObject Ability_3;

    // LAYERS
    public LayerMask grassLayer;
    public LayerMask specialGrassLayer;

    // PLAYER
    [SerializeField]
    public float playerHealth;
    public float playerLevel = 1.0f;
    public int playerInventory = 0;
    public int bluePotionsInventory = 0;
    public int greenPotionsInventory = 0;
    public int redPotionsInventory = 0;
    [SerializeField]
    public int playerMaxInventory;

    // ENEMY
    public GameObject _enemyTemplatePrefab;
    public GameObject _enemySpecialPrefab;
    GameObject templateEnemyObj;
    GameObject specialEnemyObj;

    // UI
    public TMP_Text _promptText;
    public TMP_Text _infoText;
    public TMP_Text _inventoryText;

    // Start is called before the first frame update
    void Start()
    {
        Cam_1();
        Ability_3.SetActive(false);

        _promptText.enabled = false;
        _infoText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.hasChanged = false;

        // horizontal parameter
        _animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        _animator.SetFloat("Vertical", Input.GetAxis("Vertical"));

        // Gives back value depending on what arrow key you press (left/right movement)
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        Vector3 vertical = new Vector3(0.0f, Input.GetAxis("Vertical"), 0.0f);
        // y and z-axis stays the same since we're moving it in 2D
        // moving sprite on x-axis
        transform.position = transform.position + horizontal * Time.deltaTime * _speed;
        transform.position = transform.position + vertical * Time.deltaTime * _speed;
        // multiplying by deltaTime helps smooth out the movement
        // multiply by speed to make it move faster

        if (transform.hasChanged)
        {
            CheckForEncounters();
        }

        // Reveal the inventory UI element if the player has something in inventory
        if (playerInventory > 0)
        {
            _inventoryText.enabled = true;
        }
        else
        {
            _inventoryText.enabled = false;
        }
        _inventoryText.text = "Inventory: " + playerInventory + "/" + playerMaxInventory;
    }

    void Cam_1()
    {
        Camera_1.SetActive(true);
        Camera_2.SetActive(false);
    }

    void Cam_2()
    {
        Camera_1.SetActive(false);
        Camera_2.SetActive(true);
    }

    private void CheckForEncounters()
    { 
        // Check for common enemy types on common grass layer
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if (Random.Range(1, 101) <= 1)
            {
                if (hasEncountered == false)
                {
                    hasEncountered = true;
                    _speed = 0.0f;
                    templateEnemyObj = Instantiate(_enemyTemplatePrefab);
                    Debug.Log("Encountered an enemy!");
                    Cam_2();
                    if (playerLevel >= 2)
                    {
                        Ability_3.SetActive(true);
                    }
                }
            }
        }
        else
        {
            hasEncountered = false;
        }

        // Check for special enemy types on special grass layer
        if (Physics2D.OverlapCircle(transform.position, 0.2f, specialGrassLayer) != null)
        {
            if (Random.Range(1, 101) <= 1)
            {
                if (hasEncountered == false)
                {
                    hasEncountered = true;
                    _speed = 0.0f;
                    specialEnemyObj = Instantiate(_enemySpecialPrefab);
                    Debug.Log("Encountered an enemy!");
                    Cam_2();
                    if (playerLevel >= 2)
                    {
                        Ability_3.SetActive(true);
                    }
                }
            }
        }
        else
        {
            hasEncountered = false;
        }
    }

    public void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.collider.tag == "BluePotionItem")
        {
            _promptText.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Collected 1 BLUE POTION");
                Destroy(collisionInfo.gameObject);
                playerInventory += 1;
                bluePotionsInventory += 1;
                _promptText.enabled = false;
            }
        }
        if (collisionInfo.collider.tag == "GreenPotionItem")
        {
            _promptText.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Collected 1 GREEN POTION");
                Destroy(collisionInfo.gameObject);
                playerInventory += 1;
                greenPotionsInventory += 1;
                _promptText.enabled = false;
            }
        }
        if (collisionInfo.collider.tag == "RedPotionItem")
        {
            _promptText.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Collected 1 RED POTION");
                Destroy(collisionInfo.gameObject);
                playerInventory += 1;
                redPotionsInventory += 1;
                _promptText.enabled = false;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collisionInfo)
    {
        _promptText.enabled = false;
    }

    public void FleeBattle()
    {
        Debug.Log("You fled from the battle!");

        // Switch cameras
        Camera_1.SetActive(true);
        Camera_2.SetActive(false);

        _speed = 2.0f;

        // Destroy the enemy prefabs
        Destroy(templateEnemyObj);
        Destroy(specialEnemyObj);
    }

    public void UseAbility()
    {
        playerLevel += 1.0f;
        Debug.Log("You used an ability and won the battle!");
        Debug.Log("You leveled up to level " + playerLevel);

        _speed = 2.0f;

        // Switch cameras
        Camera_1.SetActive(true);
        Camera_2.SetActive(false);

        // Destroy the enemy prefabs
        Destroy(templateEnemyObj);
        Destroy(specialEnemyObj);
    }
}