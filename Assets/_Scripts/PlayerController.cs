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

    [SerializeField]
    private GameObject[] lifes;
    private int lifesNumber = 3;

    private int multiplier = 1;

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
    //Score event
    public delegate void _OnScoreChange(int points);
    public static event _OnScoreChange OnScoreChange;
    public UnityEvent OnNewScore;

    //Multiplier Event
    public delegate void _OnMultiplierChange(int multiplier);
    public static event _OnMultiplierChange OnMultiplierChange;
    public UnityEvent OnNewMultiplier;
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

        /*if(timeToChangeCounter >= timeToChange)
        {
            _spriteRenderer.color = colors[Random.Range(0, colors.Length - 1)];
            timeToChange = Random.Range(minimTimeToChange, maxTimeToChange);
            timeToChangeCounter = 0;
        }
        timeToChangeCounter += Time.deltaTime;*/
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

    /// <summary>
    /// Actualiza las vidas visibles en la pantalla del juego
    /// </summary>
    private void UpdateLife()
    {
        if(lifesNumber == 1)
        {
            lifes[0].SetActive(false);
            lifes[1].SetActive(false);
        }else if(lifesNumber == 2)
        {
            lifes[0].SetActive(true);
            lifes[1].SetActive(false);
        }else if(lifesNumber == 3)
        {
            lifes[0].SetActive(true);
            lifes[1].SetActive(true);
        }
        
    }

    /// <summary>
    /// Añade o resta el número de vidas. 
    /// </summary>
    /// <param name="number">Entero con el número de vidas: Valor posible +1 o -1</param>
    private void ChangeLifeNumber(int number)
    {
        lifesNumber += number;
        if(lifesNumber == 0)
        {
            Debug.Log("GAME OVER!");
            //Time.timeScale = 0;
            //Debug.Log("Game Over!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        Color figureColor = other.gameObject.GetComponent<SpriteRenderer>().color;

        if (other.CompareTag("Figure"))
        {
            if (id == other.gameObject.GetComponent<FigureController>().GetFigureId())
            {
                if (CompareColor(figureColor))
                {
                    OnNewScore.Invoke();
                    if(OnScoreChange != null)
                    {
                        OnScoreChange(1 * multiplier);
                    }

                    //Increment multiplier and Change Figure
                    multiplier++;

                    OnNewMultiplier.Invoke();
                    if(OnMultiplierChange != null)
                    {
                        OnMultiplierChange(multiplier);
                    }

                    ChangeForm();
                }
                else
                {
                    //Add a point and restart the multiplier
                    multiplier = 1;
                    OnNewScore.Invoke();
                    OnNewMultiplier.Invoke();
                    if(OnScoreChange != null)
                    {
                        OnScoreChange(1 * multiplier);
                    }

                    if(OnMultiplierChange != null)
                    {
                        OnMultiplierChange(multiplier);
                    }
                    
                    
                    //Change Figure and color
                    _spriteRenderer.color = other.GetComponent<SpriteRenderer>().color;
                    ChangeForm();
                }
            }
            else
            {
                if(CompareColor(figureColor))
                {
                    //Add a point and restart the multiplier
                    multiplier = 1;
                    OnNewScore.Invoke();
                    OnNewMultiplier.Invoke();
                    if(OnScoreChange != null)
                    {
                        OnScoreChange(1 * multiplier);
                    }

                    if(OnMultiplierChange != null)
                    {
                        OnMultiplierChange(multiplier);
                    }

                }else
                {
                    ChangeLifeNumber(-1);
                    UpdateLife();
                    multiplier = 1;
                    OnNewMultiplier.Invoke();
                    
                    if(OnMultiplierChange != null)
                    {
                        OnMultiplierChange(multiplier);
                    }
                }
                
            }
            Destroy(other.gameObject);

        }
    }

}
