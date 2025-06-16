using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private TextMeshProUGUI lastSaveTimeText;
    [SerializeField] private GameObject saveLoadPanel;

    private void Start()
    {
        if (saveButton != null)
            saveButton.onClick.AddListener(OnSaveButtonClicked);
        
        if (loadButton != null)
            loadButton.onClick.AddListener(OnLoadButtonClicked);

        UpdateLastSaveTime();
    }

    private void Update()
    {
        // Toggle save/load panel with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSaveLoadPanel();
        }
    }

    public void OnSaveButtonClicked()
    {
        SaveManager.Instance.SaveGame();
        UpdateLastSaveTime();
    }

    public void OnLoadButtonClicked()
    {
        if (SaveManager.Instance.HasSaveFile())
        {
            SaveManager.Instance.LoadGame();
        }
        else
        {
            Debug.LogWarning("No save file found!");
        }
    }

    private void UpdateLastSaveTime()
    {
        if (lastSaveTimeText != null)
        {
            DateTime lastSave = SaveManager.Instance.GetLastSaveTime();
            if (lastSave != DateTime.MinValue)
            {
                lastSaveTimeText.text = $"Last Save: {lastSave.ToString("MM/dd/yyyy HH:mm:ss")}";
            }
            else
            {
                lastSaveTimeText.text = "No save file found";
            }
        }
    }

    private void ToggleSaveLoadPanel()
    {
        if (saveLoadPanel != null)
        {
            saveLoadPanel.SetActive(!saveLoadPanel.activeSelf);
        }
    }
} 