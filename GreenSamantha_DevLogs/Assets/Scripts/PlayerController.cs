using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.UI;

// sprite sheet from https://pipoya.itch.io/pipoya-free-rpg-character-sprites-32x32

// item sprites from https://opengameart.org/node/113951

public class PlayerController : MonoBehaviour
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

    [SerializeField] private CanvasGroup gameUI;
    [SerializeField] private CanvasGroup battleUI;

    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    // LAYERS
    public LayerMask grassLayer;
    public LayerMask specialGrassLayer;

    // PLAYER GAME DATA (SAVED)
    public float playerHealth = 100.0f;
    public float playerLevel = 1.0f;
    public int playerInventory = 0;
    public int bluePotionsInventory = 0;
    public int greenPotionsInventory = 0;
    public int redPotionsInventory = 0;
    public float playerXPosition = 0.0f;
    public float playerYPosition = 0.0f;

    // MAXIMUM INVENTORY SIZE (SET IN EDITOR)
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

    public ParticleSystem _bloodSplash;

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        Cam_1();
        Ability_3.SetActive(false);

        _promptText.enabled = false;
        _infoText.enabled = false;

        _bloodSplash = transform.GetChild(0).GetComponentInChildren<ParticleSystem>();
        _bloodSplash.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        _bloodSplash.transform.position = new Vector3(-24.0f, 5.0f, 0.0f);

        CheckForFade();

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

    private void CheckForFade()
    {
        if (fadeIn)
        {
            if (gameUI.alpha < 1)
            {
                gameUI.alpha += Time.deltaTime;
                if (gameUI.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
            if (battleUI.alpha < 1)
            {
                battleUI.alpha += Time.deltaTime;
                if (battleUI.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if (gameUI.alpha >= 0)
            {
                gameUI.alpha -= Time.deltaTime;
                if (gameUI.alpha == 0)
                {
                    fadeOut = false;
                }
            }
            if (battleUI.alpha >= 0)
            {
                battleUI.alpha -= Time.deltaTime;
                if (battleUI.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }

    void Cam_1()
    {
        StartCoroutine(fadeToCam1());
    }

    void Cam_2()
    {
        StartCoroutine(fadeToCam2());
    }

    IEnumerator fadeToCam1()
    {
        fadeIn = true;
        yield return new WaitForSeconds(1);
        Camera_1.SetActive(true);
        Camera_2.SetActive(false);
        fadeOut = true;
    }

    IEnumerator fadeToCam2()
    {
        fadeIn = true;
        yield return new WaitForSeconds(1);
        Camera_1.SetActive(false);
        Camera_2.SetActive(true);
        fadeOut = true;
    }

    private void CheckForEncounters()
    { 
        // Check for common enemy types on common grass layer
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 1)
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
            if (UnityEngine.Random.Range(1, 101) <= 1)
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
        _bloodSplash.Play();
        playerLevel += 1.0f;
        Debug.Log("You used an ability and won the battle!");
        Debug.Log("You leveled up to level " + playerLevel);

        StartCoroutine(KillEnemy());
    }

    IEnumerator KillEnemy()
    {
        yield return new WaitForSeconds(2);

        _speed = 2.0f;

        // Switch cameras
        Cam_1();

        // Destroy the enemy prefabs
        Destroy(templateEnemyObj);
        Destroy(specialEnemyObj);
        _bloodSplash.Stop();
    }

    public void SaveData()
    {
        // Get player position
        playerXPosition = transform.position.x;
        playerYPosition = transform.position.y;

        // Get file path
        string path = "Assets/Resources/GameData.txt";

        // Overwrite saved data
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine("playerHealth=" + playerHealth);
        writer.WriteLine("playerLevel=" + playerLevel);
        writer.WriteLine("playerInventory=" + playerInventory);
        writer.WriteLine("redPotionsInventory=" + redPotionsInventory);
        writer.WriteLine("greenPotionsInventory=" + greenPotionsInventory);
        writer.WriteLine("bluePotionsInventory=" + bluePotionsInventory);
        writer.WriteLine("playerXPosition=" + playerXPosition);
        writer.WriteLine("playerYPosition=" + playerYPosition);
        writer.Close();
    }

    public void LoadData()
    {
        // Get file path
        string path = "Assets/Resources/GameData.txt";

        if (File.Exists(path)) // If save file exists
        {
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                Debug.Log("Read line: " + line);
                string[] keyValue = line.Split('=');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    if (key == "playerHealth")
                    {
                        if (float.TryParse(value, out float parsedValue))
                        {
                            playerHealth = parsedValue;
                        }
                    }
                    else if (key == "playerLevel")
                    {
                        if (float.TryParse(value, out float parsedValue))
                        {
                            playerLevel = parsedValue;
                        }
                    }
                    else if (key == "playerInventory")
                    {
                        if (int.TryParse(value, out int parsedValue))
                        {
                            playerInventory = parsedValue;
                        }
                    }
                    else if (key == "redPotionsInventory")
                    {
                        if (int.TryParse(value, out int parsedValue))
                        {
                            redPotionsInventory = parsedValue;
                        }
                    }
                    else if (key == "greenPotionsInventory")
                    {
                        if (int.TryParse(value, out int parsedValue))
                        {
                            greenPotionsInventory = parsedValue;
                        }
                    }
                    else if (key == "bluePotionsInventory")
                    {
                        if (int.TryParse(value, out int parsedValue))
                        {
                            bluePotionsInventory = parsedValue;
                        }
                    }
                    else if (key == "playerXPosition")
                    {
                        if (float.TryParse(value, out float parsedValue))
                        {
                            playerXPosition = parsedValue;
                        }
                    }
                    else if (key == "playerYPosition")
                    {
                        if (float.TryParse(value, out float parsedValue))
                        {
                            playerYPosition = parsedValue;
                        }
                    }
                }
            }
            reader.Close();
        }
        else // If save file doesn't exist or can't be found, log error
        {
            Debug.LogError("GameData file not found: " + path);
        }

        // Set player's current position to position loaded from saved data
        transform.position = new Vector3(playerXPosition, playerYPosition, 0.0f);
    }

    public void NewData()
    {
        // Get file path
        string path = "Assets/Resources/GameData.txt";

        // Overwrite saved data with default values
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine("playerHealth=100");
        writer.WriteLine("playerLevel=1");
        writer.WriteLine("playerInventory=0");
        writer.WriteLine("redPotionsInventory=0");
        writer.WriteLine("greenPotionsInventory=0");
        writer.WriteLine("bluePotionsInventory=0");
        writer.WriteLine("playerXPosition=0");
        writer.WriteLine("playerYPosition=0");
        writer.Close();
    }

    public void ShowFade()
    {
        fadeIn = true;
    }

    public void HideFade()
    {
        fadeOut = true;
    }
}