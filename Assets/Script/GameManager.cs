using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button[] idiomButtons;
    public Button[] proverbButtons;
    public TextMeshProUGUI storyText;
    public Button backButton;
    public Button storyBackButton;
    public TMP_Dropdown languageDropdown;
    public GameObject storyPanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI correctPairText;
    public TextMeshProUGUI tryAgainText;

    [Header("Game Settings")]
    public StoryMode selectedMode = StoryMode.Mahabharat;
    public float wrongMatchDisplayTime = 3f;

    private List<GameEntry> allData = new List<GameEntry>();
    private List<GameEntry> currentRoundPairs = new List<GameEntry>();
    private Dictionary<string, GameEntry> proverbToEntryMap = new Dictionary<string, GameEntry>();

    private int selectedIdiomIndex = -1;
    private bool idiomPicked = false;
    private int matchedCount = 0;
    private bool showingStory = false;
    private bool showingWrongMatch = false;
    private int roundStartTime;
    private int totalTimeTaken;
    private bool timerRunning;
     private bool roundComplete = false;

    public enum StoryMode { Ramayan, Mahabharat, Casual }
    public enum Language { en, hi, te, ta, kn, sa }
    public Language selectedLanguage = Language.en;

    [Serializable]
    public class LanguageText
    {
        public string en;
        public string hi;
        public string te;
        public string ta;
        public string kn;
        public string sa;
    }

    [Serializable]
    public class GameEntry
    {
        public LanguageText idioms;
        public LanguageText proverbs;
        public LanguageText stories;
    }

    [Serializable]
    public class GameEntryListWrapper
    {
        public List<GameEntry> entries;
    }

    void Start()
    {
        if (idiomButtons == null || idiomButtons.Length == 0 ||
            proverbButtons == null || proverbButtons.Length == 0 ||
            storyText == null || backButton == null ||
            storyBackButton == null || languageDropdown == null ||
            storyPanel == null || timerText == null ||
            feedbackText == null || correctPairText == null ||
            tryAgainText == null)
        {
            Debug.LogError("Please assign all UI elements in the Inspector.");
            enabled = false;
            return;
        }

        storyPanel.SetActive(false);
        storyBackButton.gameObject.SetActive(false);
        tryAgainText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(true);

        backButton.onClick.AddListener(GoBackToMenu);
        storyBackButton.onClick.AddListener(ReturnToGameFromStory);
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

        LoadJson();

        if (allData.Count == 0)
        {
            Debug.LogError("No data loaded. Check your JSON file.");
            enabled = false;
            return;
        }

        SetupRound();
    }

    void Update()
    {
        if (timerRunning)
        {
            float currentTime = Time.time - roundStartTime;
            timerText.text = $"Time: {currentTime.ToString("F1")}s";
        }
        else if (showingWrongMatch && Time.time - roundStartTime > wrongMatchDisplayTime)
        {
            ReturnToGameFromWrongMatch();
        }
    }

    void LoadJson()
    {
        string fileName = selectedMode switch
        {
            StoryMode.Ramayan => "ramayan_fixed.json",
            StoryMode.Mahabharat => "mahabhart_fixed.json",
            StoryMode.Casual => "Casual_fixed.json",
            _ => "mahabhart_fixed.json"
        };

        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogError("JSON file not found at: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        try
        {
            GameEntryListWrapper wrapper = JsonUtility.FromJson<GameEntryListWrapper>(json);
            allData = wrapper.entries;
        }
        catch (Exception ex)
        {
            Debug.LogError("JSON Parsing failed: " + ex.Message);
        }
    }

    void SetupRound()
    {
        if (timerRunning)
        {
            totalTimeTaken += (int)Time.time - roundStartTime;
            Debug.Log($"Round completed in: {Time.time - roundStartTime:F1} seconds");
            Debug.Log($"Total time: {totalTimeTaken:F1} seconds");
        }

        roundStartTime = (int)Time.time;
        timerRunning = true;
        timerText.text = "Time: 0.0s";

        showingStory = false;
        showingWrongMatch = false;
        storyPanel.SetActive(false);
        backButton.gameObject.SetActive(true);
        storyBackButton.gameObject.SetActive(false);
        tryAgainText.gameObject.SetActive(false);

        idiomPicked = false;
        selectedIdiomIndex = -1;
        matchedCount = 0;
        storyText.text = "Elder: Pick an idiom.";

        currentRoundPairs.Clear();
        proverbToEntryMap.Clear();

        List<GameEntry> uniqueEntries = new List<GameEntry>();
        HashSet<string> usedIdioms = new HashSet<string>();
        foreach (var entry in allData)
        {
            string idiom = GetLocalized(entry.idioms);
            if (!string.IsNullOrWhiteSpace(idiom) && usedIdioms.Add(idiom))
                uniqueEntries.Add(entry);
        }

        if (uniqueEntries.Count < idiomButtons.Length)
        {
            Debug.LogError("Not enough unique idioms.");
            return;
        }

        while (currentRoundPairs.Count < idiomButtons.Length)
        {
            var entry = uniqueEntries[UnityEngine.Random.Range(0, uniqueEntries.Count)];
            if (!currentRoundPairs.Contains(entry))
                currentRoundPairs.Add(entry);
        }

        Shuffle(currentRoundPairs);

        for (int i = 0; i < idiomButtons.Length; i++)
        {
            var btn = idiomButtons[i];
            btn.interactable = true;
            SetButtonColor(btn, Color.white);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = GetLocalized(currentRoundPairs[i].idioms);
            int index = i;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnIdiomSelected(index));
        }

        List<string> proverbOptions = new List<string>();
        foreach (var entry in currentRoundPairs)
        {
            string localizedProverb = GetLocalized(entry.proverbs);
            if (!proverbToEntryMap.ContainsKey(localizedProverb))
            {
                proverbOptions.Add(localizedProverb);
                proverbToEntryMap[localizedProverb] = entry;
            }
        }

        Shuffle(proverbOptions);

        for (int i = 0; i < proverbButtons.Length; i++)
        {
            var btn = proverbButtons[i];
            btn.interactable = true;
            SetButtonColor(btn, Color.white);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = proverbOptions[i];
            string selected = proverbOptions[i];
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnProverbSelected(selected));
        }
    }

    void OnIdiomSelected(int index)
    {
        selectedIdiomIndex = index;
        idiomPicked = true;
        storyText.text = "Child: Pick the matching proverb.";

        for (int i = 0; i < idiomButtons.Length; i++)
        {
            SetButtonColor(idiomButtons[i], i == index ? Color.yellow : Color.white);
        }
    }

    void OnProverbSelected(string selectedProverb)
    {
        if (!idiomPicked)
        {
            storyText.text = "Please pick an idiom first.";
            return;
        }

        if (!proverbToEntryMap.TryGetValue(selectedProverb, out var matchedEntry))
        {
            Debug.LogWarning("Selected proverb not found in map.");
            return;
        }

        var selectedEntry = currentRoundPairs[selectedIdiomIndex];
        bool isCorrect = matchedEntry == selectedEntry;

        if (isCorrect)
        {
            HandleCorrectMatch(selectedProverb, selectedEntry);
        }
        else
        {
            HandleWrongMatch(selectedProverb, selectedEntry);
        }
    }
void HandleCorrectMatch(string selectedProverb, GameEntry selectedEntry)
{
    timerRunning = false;
    int roundTime = (int)Time.time - roundStartTime;
    totalTimeTaken += roundTime;
    timerText.text = $"Time: {roundTime:F1}s (Total: {totalTimeTaken:F1}s)";

    showingStory = true;
    showingWrongMatch = false;
    feedbackText.text = "Correct!";
    feedbackText.color = Color.green;
    correctPairText.text = $"{GetLocalized(selectedEntry.idioms)} → {GetLocalized(selectedEntry.proverbs)}";

    // ✅ Always show the story
    storyText.text = GetLocalized(selectedEntry.stories);

    // 🎉 Add congrats below story on 4th match
    if (matchedCount == 3) // 0-based index → 4th match
    {
        storyText.text += "\n\n<color=#FFD700><b>🎉 Congratulations! You matched all 4 cards!</b></color>";
        // mark that round is complete
        roundComplete = true;
    }

    // Show story panel
    storyPanel.SetActive(true);
    backButton.gameObject.SetActive(false);
    storyBackButton.gameObject.SetActive(true);
    tryAgainText.gameObject.SetActive(false);

    // Disable matched idiom
    idiomButtons[selectedIdiomIndex].interactable = false;
    SetButtonColor(idiomButtons[selectedIdiomIndex], Color.green);

    // Disable matched proverb
    foreach (var btn in proverbButtons)
    {
        if (btn.GetComponentInChildren<TextMeshProUGUI>().text == selectedProverb)
        {
            btn.interactable = false;
            SetButtonColor(btn, Color.green);
            break;
        }
    }

    matchedCount++;
}

    void HandleWrongMatch(string selectedProverb, GameEntry correctEntry)
    {
        timerRunning = false;
        showingWrongMatch = true;
        showingStory = false;
        roundStartTime = (int)Time.time;

        feedbackText.text = "Wrong Match!";
        feedbackText.color = Color.red;
        correctPairText.text = $"Correct: {GetLocalized(correctEntry.idioms)} → {GetLocalized(correctEntry.proverbs)}";
        storyText.text = "";
        tryAgainText.gameObject.SetActive(true);

        storyPanel.SetActive(true);
        backButton.gameObject.SetActive(false);
        storyBackButton.gameObject.SetActive(false);

        foreach (var btn in proverbButtons)
        {
            if (btn.GetComponentInChildren<TextMeshProUGUI>().text == selectedProverb)
            {
                SetButtonColor(btn, Color.red);
            }
        }
        SetButtonColor(idiomButtons[selectedIdiomIndex], Color.yellow);
    }

    void ReturnToGameFromWrongMatch()
    {
        showingWrongMatch = false;
        storyPanel.SetActive(false);
        backButton.gameObject.SetActive(true);
        tryAgainText.gameObject.SetActive(false);

        foreach (var btn in proverbButtons)
        {
            SetButtonColor(btn, Color.white);
        }

        roundStartTime = (int)Time.time;
        timerRunning = true;

        storyText.text = "Child: Pick the matching proverb.";
    }

    void ReturnToGameFromStory()
{
    showingStory = false;
    storyPanel.SetActive(false);
    storyBackButton.gameObject.SetActive(false);

    // ✅ If round is complete → restart new round
    if (roundComplete)
    {
        roundComplete = false;
        SetupRound();
        return;
    }

    backButton.gameObject.SetActive(true);
    roundStartTime = (int)Time.time;
    timerRunning = true;

    storyText.text = idiomPicked ?
        "Child: Pick the matching proverb." :
        "Elder: Pick an idiom.";
}


    void OnLanguageChanged(int index)
    {
        selectedLanguage = (Language)index;
        SetupRound();
    }

    string GetLocalized(LanguageText text)
    {
        return selectedLanguage switch
        {
            Language.hi => text.hi,
            Language.te => text.te,
            Language.ta => text.ta,
            Language.kn => text.kn,
            Language.sa => text.sa,
            _ => text.en
        };
    }

    void SetButtonColor(Button btn, Color color)
    {
        Image img = btn.GetComponent<Image>();
        if (img != null)
            img.color = color;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }

    void GoBackToMenu()
    {
        if (timerRunning)
        {
            totalTimeTaken += (int)Time.time - roundStartTime;
            Debug.Log($"Total play time: {totalTimeTaken:F1} seconds");
            Debug.Log("Game Over");
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseStory()
    {
        Debug.Log("Closing story");
    }
}