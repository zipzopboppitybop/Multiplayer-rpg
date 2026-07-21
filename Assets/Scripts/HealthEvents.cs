using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class HealthEvents : MonoBehaviour
{
    [SerializeField] private Health _hp;
    private UIDocument _document;

    void Start()
    {
        // root document
        _document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        _hp.OnHealthCreated.AddListener((health) => SetHealth(health));
        _hp.OnDamagePlayer.AddListener((health) => SetHealth(health));
        _hp.OnHealPlayer.AddListener((health) => SetHealth(health));
        _hp.OnPlayerDeath.AddListener(GameOver);
    }

    private void OnDisable()
    {
        _hp.OnHealthCreated.RemoveListener(SetHealth);
        _hp.OnDamagePlayer.RemoveListener(SetHealth);
        _hp.OnHealPlayer.RemoveListener(SetHealth);
        _hp.OnPlayerDeath.RemoveListener(GameOver);
    }

    private void Awake()
    {
        //// host button
        //_hostButton = _document.rootVisualElement.Q("HostButton") as Button;
        //_hostButton.RegisterCallback<ClickEvent>(OnHostClick);

        //// join button
        //_joinButton = _document.rootVisualElement.Q("JoinButton") as Button;
        //_joinButton.RegisterCallback<ClickEvent>(OnJoinClick);

        //// quit button
        //_quitButton = _document.rootVisualElement.Q("QuitButton") as Button;
        //_quitButton.RegisterCallback<ClickEvent>(OnQuitClick);

        //// register callbacks for all buttons
        //_menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        //for (int i = 0; i < _menuButtons.Count; i++)
        //{
        //    _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        //}
    }
    private void SetHealth(int health)
    {
        Debug.Log(health);
    }
    private void GameOver()
    {
        Debug.Log("Game Over");
    }
}
