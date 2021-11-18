using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Color[] colors;
    [SerializeField]
    private Sprite[] _sprites;
    private int id = 2;

    #region CameraBounds
    private Vector3 bottomLeft;
    private Vector3 topRight;
    private float borderOffset;
    #endregion

    #region ChangeFormTime
    [SerializeField]
    private float minimTimeToChange = 1;
    [SerializeField]
    private float maxTimeToChange = 3;
    private float timeToChange;
    private float timeToChangeCounter = 0;
    #endregion

    #region Events
    public delegate void _OnScoreChange(int points);
    public static event _OnScoreChange OnScoreChange;
    public UnityEvent OnNewScore;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        _spriteRenderer.color = colors[Random.Range(0, colors.Length-1)];
        ChangeForm();
        borderOffset = this.transform.localScale.x / 2;
        bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        timeToChange = Random.Range(minimTimeToChange, maxTimeToChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MovePlayer(1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MovePlayer(-1);
        }

        if(timeToChangeCounter >= timeToChange)
        {
            ChangeForm();
            timeToChange = Random.Range(minimTimeToChange, maxTimeToChange);
            timeToChangeCounter = 0;
        }
        timeToChangeCounter += Time.deltaTime;
    }

    public void MovePlayer(int direction)
    {
        if (direction == -1)
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if (direction == 1)
        {
            this.transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        float clampedX = Mathf.Clamp(this.transform.position.x, bottomLeft.x + borderOffset, topRight.x - borderOffset);

        this.transform.position = new Vector3(clampedX, this.transform.position.y, this.transform.position.z);

    }

    private bool CompareColor(Color figureColor)
    {
        if (figureColor == _spriteRenderer.color)
        {
            return true;
        }
        return false;
    }

    private void ChangeForm()
    {
        int newIndex = Random.Range(0, 4);
        id = newIndex;
        _spriteRenderer.sprite = _sprites[newIndex];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Figure"))
        {
            if (id == other.gameObject.GetComponent<FigureController>().GetFigureId())
            {
                Color figureColor = other.gameObject.GetComponent<SpriteRenderer>().color;
                if (CompareColor(figureColor))
                {
                    //We add points
                    OnNewScore.Invoke();
                    if(OnScoreChange != null)
                    {
                        OnScoreChange(1);
                    }
                }
                else
                {
                    _spriteRenderer.color = figureColor;
                }
            }
            else
            {
                //We substract points
                OnNewScore.Invoke();
                if(OnScoreChange != null)
                {
                    OnScoreChange(-1);
                }
                //Time.timeScale = 0;
                //Debug.Log("Game Over!");
            }
            Destroy(other.gameObject);

        }
    }

}
