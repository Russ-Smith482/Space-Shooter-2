using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    
    
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;

    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Image _ammoImg;
    [SerializeField]
    private Sprite[] _ammoSprites;
    [SerializeField]
    private Text _noAmmoText;


    // Start is called before the first frame update
    void Start()
    {

        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }
    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }

    }
    
    public void UpdateAmmo(int playerAmmo)
    {

        _ammoText.text = "AMMO COUNT: " + playerAmmo.ToString();

        _ammoImg.sprite = _ammoSprites[playerAmmo];

        if (playerAmmo == 0)
        {
            NoAmmoSequence();
        }

    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        _restartText.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlickerRoutine()

    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);

        }
    }

    void NoAmmoSequence()
    {
        _ammoText.gameObject.SetActive(false);
        _noAmmoText.gameObject.SetActive(true);
        StartCoroutine(NoAmmoFlickerRoutine());
        
    }
    
    IEnumerator NoAmmoFlickerRoutine()

    {
        while (true)
        {
            _noAmmoText.text = "NO AMMO";
            yield return new WaitForSeconds(0.5f);
            _noAmmoText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void AmmoReset()
    {
        _noAmmoText.gameObject.SetActive(false);
        _ammoText.gameObject.SetActive(true);
        _ammoText.text = "AMMO COUNT: 15"; 
        _ammoImg.sprite = _ammoSprites[14];
    }


}

