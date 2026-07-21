using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class HealthEvents : MonoBehaviour
{
    [SerializeField] private Sprite _heartSprite;
    private Health _hp;
    private UIDocument _document;
    private VisualElement _healthBar;
    private void OnEnable()
    {
        PlayerEvents.OnLocalPlayerSpawned += Initialize;

        if (PlayerEvents.LocalPlayerHealth != null)
        {
            Initialize(PlayerEvents.LocalPlayerHealth);
        }
    }

    private void OnDisable()
    {
        PlayerEvents.OnLocalPlayerSpawned -= Initialize;
        Uninitialize();
    }
    void Start()
    {
        // root document
        _document = GetComponent<UIDocument>();
        _healthBar = _document.rootVisualElement.Q("HealthBar");

        if (_healthBar == null)
        {
            Debug.Log("No health bar!");
        }
    }

    public void Initialize(Health localPlayerHealth)
    {
        Uninitialize();

        _hp = localPlayerHealth;

        _hp.OnHealthCreated.AddListener(SetHealth);
        _hp.OnDamagePlayer.AddListener(SetHealth);
        _hp.OnHealPlayer.AddListener(SetHealth);
        _hp.OnPlayerDeath.AddListener(GameOver);

        SetHealth(_hp.CurrentHealth);
        Debug.Log("Added player");
    }

    public void Uninitialize()
    {
        if (_hp == null) return;

        _hp.OnHealthCreated.RemoveListener(SetHealth);
        _hp.OnDamagePlayer.RemoveListener(SetHealth);
        _hp.OnHealPlayer.RemoveListener(SetHealth);
        _hp.OnPlayerDeath.RemoveListener(GameOver);
    }
    private void SetHealth(int health)
    {
        if (_healthBar == null) return;

        _healthBar.Clear();

        for (int i = 0; i < health; i++)
        {
            Image heartImage = new Image();
            heartImage.name = "HeartIcon";
            heartImage.style.width = 30;
            heartImage.style.height = 30;
            heartImage.sprite = _heartSprite;

            _healthBar.Add(heartImage);
        }

        Debug.Log($"I set the health to {health} in the ui!");
    }
    private void GameOver()
    {
        Debug.Log("Game Over");
    }
}
