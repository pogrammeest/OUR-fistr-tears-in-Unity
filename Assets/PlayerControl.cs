using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script")]
public class PlayerControl : MonoBehaviour
{
    [Header("Players settings")]
    [Tooltip("Horisontal camera sensivity in degrees")]
    public float sensitivityHor = 9.0f;
    [Tooltip("Vertical camera sensivity in degrees")]
    public float sensitivityVert = 9.0f;

    [Space(10)]
    [Tooltip("Minimum vertical camera's border in degrees")]
    public float minimumVert = -87.0f;
    [Tooltip("Maximum vertical camera's border in degrees")]
    public float maximumVert = 87.0f;

    [Space(10)]
    [Tooltip("Player's speed in m/s")]
    public float speed = 6.0f;
    [Tooltip("Player's gravity in m/s")]
    public float gravity = -10f;

    // camera
    private float _rotationX = 0;
    public GameObject _playerCamera;

    // player
    private CharacterController _charController;

    private void Awake()
    {
        _playerCamera = GameObject.FindGameObjectWithTag("Players Camera");

        if (_playerCamera != null)
            Debug.Log("Success");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
