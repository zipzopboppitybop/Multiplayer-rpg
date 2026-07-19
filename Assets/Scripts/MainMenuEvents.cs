using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    [SerializeField] private PlayerSpawner _spawner;
    private UIDocument _document;
    private Button _hostButton;
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _hostButton = _document.rootVisualElement.Q("HostButton") as Button;
        _hostButton.RegisterCallback<ClickEvent>(OnHostClick);
    }
    private void OnHostClick(ClickEvent e)
    {
        _spawner.CallHostGame();
    }
}
