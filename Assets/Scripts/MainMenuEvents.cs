using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    [SerializeField] private PlayerSpawner _spawner;
    private UIDocument _document;
    private Button _hostButton;
    private Button _joinButton;
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
    }
    private void OnHostClick(ClickEvent e)
    {
        _spawner.CallHostGame();
    }

    private void OnJoinClick(ClickEvent e)
    {
        _spawner.CallJoinGame();
    }
}
