using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    #region CameraBounds
    private Vector3 bottomLeft;
    private Vector3 topRight;
    private float borderOffset = 0.38f;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            MovePlayer(1);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
             MovePlayer(-1);
        }
    }

    public void MovePlayer(int direction)
    {
        if(direction == -1)
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if(direction == 1)
        {
            this.transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        float clampedX = Mathf.Clamp(this.transform.position.x, bottomLeft.x + borderOffset, topRight.x - borderOffset);

        this.transform.position = new Vector3(clampedX, this.transform.position.y, this.transform.position.z);
        
    }
}
