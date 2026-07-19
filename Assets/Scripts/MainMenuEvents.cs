using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
public class MainMenuEvents : MonoBehaviour
{
    [SerializeField] private PlayerSpawner _spawner;
    private UIDocument _document;
    private Button _hostButton;
    private Button _joinButton;
    private Button _quitButton;
    private List<Button> _menuButtons = new List<Button>();
    private void Awake()
    {
        // root document
        _document = GetComponent<UIDocument>();

        // host button
        _hostButton = _document.rootVisualElement.Q("HostButton") as Button;
        _hostButton.RegisterCallback<ClickEvent>(OnHostClick);

        // join button
        _joinButton = _document.rootVisualElement.Q("JoinButton") as Button;
        _joinButton.RegisterCallback<ClickEvent>(OnJoinClick);

        // quit button
        _quitButton = _document.rootVisualElement.Q("QuitButton") as Button;
        _quitButton.RegisterCallback<ClickEvent>(OnQuitClick);

        // register callbacks for all buttons
        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }
    private void OnHostClick(ClickEvent e)
    {
        _spawner.CallHostGame();
    }
    private void OnJoinClick(ClickEvent e)
    {
        _spawner.CallJoinGame();
    }
    private void OnQuitClick(ClickEvent e)
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    private void OnAllButtonsClick(ClickEvent e)
    {
        Debug.Log("I was clicked");
    }
}
