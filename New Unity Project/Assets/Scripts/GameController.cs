
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
#if UNITY_ANDROID
    public Button button;
#endif
    int turnNumber;
    int numPlayers = 2;

    [SerializeField]float p1Angle, p1Power, p2Angle, p2Power;
    
    [SerializeField] float angle;
    [SerializeField] float power;
    float missleLife = 10f;

    public enum Turn {Player1, Player2};
    public Turn turn;

    public enum GameState {MAINMENU, SETUP, PLAY, GAMEOVER};
    public GameState gameState;

    //Main Game UI Stuff
    public TMPro.TMP_InputField powerText;
    public TMPro.TMP_InputField angleText;
    public TMPro.TMP_Text playerTurn;

    public Slider powerSlider;

    //Main Menu UI Stuff
    public GameObject mainMenuPanel;
    bool mainMenuFadeComplete;

    //Setup UI stuff
    public GameObject setUpPanel;

    public float Angle { get => angle; }
    public float Power { get => power; }
    public float MissleLife { get => missleLife;}

    public List<Controller> players = new List<Controller>();
    public List<LineRenderer> missleLines = new List<LineRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        turn = Turn.Player1;
        angle = 0f;
        power = 0f;

        playerTurn.text = turn.ToString();

        CollisionController.onHit += ProgressTurn;
        gameState = GameState.MAINMENU;
#if UNITY_ANDROID
        GameObject temp = GameObject.FindGameObjectWithTag("FireButton");
        button = temp.GetComponent<Button>();
        button.onClick.AddListener(Fire);
#endif
    }

    private void Update()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
#endif
    }

    public void SetAngle(float x)
    {
        angle = int.Parse(angleText.text) * Mathf.Deg2Rad;
        switch (turn)
        {
            case Turn.Player1:
                p1Angle = angle * Mathf.Rad2Deg;
                break;
            case Turn.Player2:
                p2Angle = angle * Mathf.Rad2Deg;
                break;
        }
    }
    public void SetPower(float x)
    {
        power = powerSlider.value * 100;
        switch (turn)
        {
            case Turn.Player1:
                p1Power = power;
                break;
            case Turn.Player2:
                p2Power = power;
                break;
        }
    }
    void ProgressTurn()
    {
        turnNumber += 1;
        if(turnNumber > numPlayers - 1)
        {
            turnNumber = 0;
        }
        turn = (Turn)turnNumber;
        playerTurn.text = turn.ToString();
        playerTurn.alpha = 1f;
        StartCoroutine(FadeText(playerTurn));

        switch (turn)
        {
            case Turn.Player1:
                powerText.text = p1Power.ToString();
                angleText.text = p1Angle.ToString();
                angle = p1Angle * Mathf.Deg2Rad;
                power = p1Power;
                break;
            case Turn.Player2:
                powerText.text = p2Power.ToString();
                angleText.text = p2Angle.ToString();
                angle = p2Angle * Mathf.Deg2Rad;
                power = p2Power;
                break;
        }
        
    }

    public void AddToLineRenderer(ref LineRenderer r)
    {
        LineRenderer temp = new LineRenderer();
        temp = r;
        missleLines.Add(temp);
    }

    public void Winner()
    {
        Debug.Log(turn.ToString() + " is the Winner!!");
    }

    public void Fire()
    {
        if (gameState == GameState.PLAY)
        {
            switch (turn)
            {
                case Turn.Player1:
                    if (players[(int)turn] != null)
                    {
                        players[(int)turn].Fire();
                    }
                    break;
                case Turn.Player2:
                    if (players[(int)turn] != null)
                    {
                        players[(int)turn].Fire();
                    }
                    break;
            }
        }
    }

    public void OnStartPressed()
    {
        gameState = GameState.SETUP;
        for (int i = 0; i < mainMenuPanel.transform.childCount; i++)
        {
            mainMenuPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
        StartCoroutine(FadeImage(mainMenuPanel.GetComponent<Image>()));
        StartCoroutine("CheckIfFadeComplete");

    }

    public void SetupYesPressed()
    {
        setUpPanel.SetActive(false);
        gameState = GameState.PLAY;
    }

    IEnumerator FadeText(TMPro.TMP_Text t)
    {
        while (t.alpha > 0)
        {
            t.alpha -= 0.01f;
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine("FadeText");
    }

    IEnumerator FadeImage(Image g)
    {
        Color menu = g.color;
        mainMenuFadeComplete = false;
        if(menu == null)
        {
            yield break;
        }
        while (g.color.a > 0.01f)
        {
            menu.a -= 0.01f;
            g.color = menu;
            yield return new WaitForEndOfFrame();
        }
        mainMenuFadeComplete = true;
        StopCoroutine("FadeGameObject");
    }

    IEnumerator CheckIfFadeComplete()
    {
        while (!mainMenuFadeComplete)
        {
            yield return new WaitForEndOfFrame();
        }
        setUpPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        StopCoroutine("CheckIfFadeComplete");
    }

}
