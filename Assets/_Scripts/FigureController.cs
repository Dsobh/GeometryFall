using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureController : MonoBehaviour
{

    [SerializeField]
    private float fallSpeed;
    [SerializeField]
    private int id;
    [SerializeField]
    private GameObject player;
    private float highLimit;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        highLimit = player.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * fallSpeed);
        
        if(this.transform.position.y <= highLimit)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Return figure's identificator
    /// </summary>
    /// <returns>figure id (int)</returns>
    public int GetFigureId()
    {
        return id;
    }
}
