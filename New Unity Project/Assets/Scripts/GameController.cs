
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/************************************************
 *  GameController controls the entire game within this script
 *  It is a simple singleton class to make it globally accessable
 ***********************************************/
public class GameController : MonoBehaviour
{

    //! Simple Singleton of this class
    /*! This allows us to use the GameController from 
     * anywhere within the code base
     * which is nessecary as this class controls everything */
    public static GameController instance;

    //! The fire button
    /*! Used only in android builds */
    public Button button;

    //! turnNumber is used to track whos turn it is
    /*! this is % around numPlayers giving us the player
     * whos turn it is */
    int turnNumber;
    //! Number of players in the game
    /*! done as future proofing incase I wanted to add more players 
     * or even ab AI controlled enemy */
    int numPlayers = 2;

    //! Tracks player 1's angle input
    float p1Angle;
    //! Tracks player 1's power input
    float p1Power;
    //! Tracks player 2's angle input
    float p2Angle;
    //! Tracks player 2's power input
    float p2Power;

    //! angle that is inputed
    /*! Gets set to the value of angle for whos turn it is */
    [SerializeField] float angle;
    //! power that is inputed
    /*! Gets set to the value of power for whos turn it is */
    [SerializeField] float power;

    //! float for how long a missle lives
    float missleLife = 10f;

    //!Enum for tracking turn order
    public enum Turn {Player1, Player2};
    public Turn turn;

    //! Enum for tracking the gameState
    /*! Used for menus, and setup */
    public enum GameState {MAINMENU, SETUP, PLAY, GAMEOVER};
    public GameState gameState;

    //!Panel the contains input fields
    public GameObject inputPanel;

    //! Where the player can input power
    public TMPro.TMP_InputField powerText;
    //! Where the player can input angle;
    public TMPro.TMP_InputField angleText;
    //! Shows whos turn it is 
    public TMPro.TMP_Text playerTurn;

    //! Slider used to set power
    /*! android only */
    public Slider powerSlider;

    //!Reference to the main menu
    /*! allows us to fade the menu within a coroutine */
    public GameObject mainMenuPanel;
    //! Bool to track if menu has faded
    bool mainMenuFadeComplete;

    //!Setup UI stuff
    public GameObject setUpPanel;

    //!Public getter for angle
    public float Angle { get => angle; }
    //!Public getter for power
    public float Power { get => power; }
    //!Public getter for missle life
    public float MissleLife { get => missleLife;}

    //! List of all the player controllers
    public List<Controller> players = new List<Controller>();
    //! List of the line renderers of the missles
    /*! used to keep track of thue renderers even after the 
     * missle has died */
    public List<LineRenderer> missleLines = new List<LineRenderer>();

    //! Start function
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
        button.gameObject.SetActive(false);
#if UNITY_ANDROID
      
        button.onClick.AddListener(Fire);

#endif
    }
    //! Update function
    private void Update()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
#endif
    }

    //!Function to Set the angle, Takes one float as a parameter
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
    //!Function to Set the power, Takes one float as a parameter
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

    //!Private function to progress the turn
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

    //!function to add another line to the Line renderer
    /*! This function takes a reference to another lineRenderer and then
     * adds it to the missleLines list*/
    public void AddToLineRenderer(ref LineRenderer r)
    {
        LineRenderer temp = new LineRenderer();
        temp = r;
        missleLines.Add(temp);
    }

    //! Prints the winner 
    public void Winner()
    {
        Debug.Log(turn.ToString() + " is the Winner!!");
    }

    //!Function the allows the player to fire a missle
    /*! Either called by button click(android) or space bar(pc) */
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

    //!Function for controlling the start button press
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

    //!Function for controlling when yes is pressed on setup screen
    public void SetupYesPressed()
    {
        setUpPanel.SetActive(false);
        inputPanel.SetActive(true);
        gameState = GameState.PLAY;
      
#if UNITY_ANDROID
        powerSlider.gameObject.SetActive(true); 
        button.gameObject.SetActive(true);
#endif
    }

    //!Private coroutine for fading text, Takes a text box as a parameter
    IEnumerator FadeText(TMPro.TMP_Text t)
    {
        while (t.alpha > 0)
        {
            t.alpha -= 0.01f;
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine("FadeText");
    }

    //! Private coroutine for fading an image, Takes an images as a parameter
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

    //! Private coroutine for controlling the fade of menu screens
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
