using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sprite sheet from https://pipoya.itch.io/pipoya-free-rpg-character-sprites-32x32


public class PlayerMovement : MonoBehaviour
{

    public float _speed = 2.0f; // increase speed
    public bool _isWalking;
    public int _walkDirection;
    [SerializeField] private Animator _animator; // exposes the animator

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // horizontal parameter
        _animator.SetFloat("Horizontal",Input.GetAxis("Horizontal"));
        _animator.SetFloat("Vertical",Input.GetAxis("Vertical"));

        // Gives back value depending on what arrow key you press (left/right movement)
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        Vector3 vertical = new Vector3(0.0f,Input.GetAxis("Vertical"), 0.0f);
        // y and z-axis stays the same since we're moving it in 2D
        // moving sprite on x-axis
        transform.position = transform.position + horizontal* Time.deltaTime * _speed;
        transform.position = transform.position + vertical * Time.deltaTime * _speed;
        // multiplying by deltaTime helps smooth out the movement
        // multiply by speed to make it move faster
    }
}