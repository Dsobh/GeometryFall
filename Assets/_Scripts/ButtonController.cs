using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour
{
    private bool isPressed = false;
    
    [SerializeField]
    private int direction;

    private GameObject player;
    // Start is called before the first frame update
    
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(isPressed)
        {
            player.GetComponent<PlayerController>().MovePlayer(direction);
        }
    }

    public void ChangeButtonState()
    {
        isPressed = !isPressed;
    }
}
