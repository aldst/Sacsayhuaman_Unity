using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

[System.Serializable]
public class Texts
{
    public List<string> titles;
    public List<string> phrases;

    public string instructionTitle;
    public string instructionDescription;
    public string instructionInstruction;
    public string instructionBtnContinueLabel;
}
public class FixedTexts
{
    public string questionTitle;
    public string questionBody;
    public string questionOption1;
    public string questionOption2;

    public string menuTitle;
    public string menuVolumeLabel;
    public string menuLanguageLabel;
    public string menuLanguageOptEs;
    public string menuLanguageOptEn;
    public string menuBtnReturnLabel;
    public string menuBtnLeaveTourLabel;

    public string instruction;
}


public enum Language { english, spanish };
public class PanelManager : MonoBehaviour
{
    public GameObject player;

    public GameObject warningPanel;

    public GameObject instructionPanel;
    public Text instructionTitle;
    public Text instructionDescription;
    public Text instructionInstruction;
    public Text instructionBtnContinueLabel;

    public GameObject textPanel;
    public Text textTitle;
    public Text textDescription;
    public Text textInstruction;

    public GameObject imagePanel;
    public Text imageTitle;
    public Text imageDescription;
    public Text imageInstruction;
    public Image image;
    public List<Sprite> sprites = new List<Sprite>();

    public GameObject questionPanel;
    public Text questionTitle;
    public Text questionBody;
    public Text questionOptYes;
    public Text questionOptNo;
    public Button afirmacion;
    public Button negacion;

    public GameObject menuPanel;
    public Text menuTitle;
    public Text menuVolumeLabel;
    public Text menuLanguageLabel;
    public Text menuLanguageOptEs;
    public Text menuLanguageOptEn;
    public Text menuBtnReturnLabel;
    public Text menuBtnLeaveTourLabel;

    public Toggle toggleSpanish;
    public Toggle toggleEnglish;

    private bool isPanelActive = false;
    private string activePanelType = "-";
    private Text activeTitle;
    private Text activeDescription;
    private Text activeInstruction;


    public GameObject cursorMano;

    private AudioSource audioSource;

    private bool previousResponse;
    private string path;
    private string jsonString;

    public TourSoundingManager tourSoundingManager;

    private Dictionary<Language, Texts> textsDic = new Dictionary<Language, Texts>();
    private Dictionary<Language, Texts> imagesDescriptionsDic = new Dictionary<Language, Texts>();
    private Dictionary<Language, Texts> instructionsDescriptionsDic = new Dictionary<Language, Texts>();
    private Dictionary<Language, FixedTexts> fixedTextsDic = new Dictionary<Language, FixedTexts>();

    [HideInInspector]
    public Language currentLanguage = Language.english;

    private int interactionNumber;

    public void Start()
    {
        path = Path.Combine(Application.streamingAssetsPath, "Text.json");
        jsonString = File.ReadAllText(path);
        textsDic.Add(Language.spanish, JsonUtility.FromJson<Texts>(jsonString));
        path = Path.Combine(Application.streamingAssetsPath, "TextEn.json");
        jsonString = File.ReadAllText(path);
        textsDic.Add(Language.english, JsonUtility.FromJson<Texts>(jsonString));



        path = Path.Combine(Application.streamingAssetsPath, "FixedText.json");
        jsonString = File.ReadAllText(path);
        fixedTextsDic.Add(Language.spanish, JsonUtility.FromJson<FixedTexts>(jsonString));
        path = Path.Combine(Application.streamingAssetsPath, "FixedTextEn.json");
        jsonString = File.ReadAllText(path);
        fixedTextsDic.Add(Language.english, JsonUtility.FromJson<FixedTexts>(jsonString));


        path = Path.Combine(Application.streamingAssetsPath, "Images.json");
        jsonString = File.ReadAllText(path);
        imagesDescriptionsDic.Add(Language.spanish, JsonUtility.FromJson<Texts>(jsonString));
        path = Path.Combine(Application.streamingAssetsPath, "ImagesEn.json");
        jsonString = File.ReadAllText(path);
        imagesDescriptionsDic.Add(Language.english, JsonUtility.FromJson<Texts>(jsonString));

        path = Path.Combine(Application.streamingAssetsPath, "Instructions.json");
        jsonString = File.ReadAllText(path);
        instructionsDescriptionsDic.Add(Language.spanish, JsonUtility.FromJson<Texts>(jsonString));
        path = Path.Combine(Application.streamingAssetsPath, "InstructionsEn.json");
        jsonString = File.ReadAllText(path);
        instructionsDescriptionsDic.Add(Language.english, JsonUtility.FromJson<Texts>(jsonString));

        Cursor.visible = false;
        cursorMano.SetActive(false);


        HandleLanguageChange(Language.spanish.ToString());
    }

    public void Update()
    {
        if (IsMouseOverUI())
        {
            cursorMano.transform.position = Input.mousePosition;
            cursorMano.SetActive(true);
        }
        else
        {
            Cursor.visible = false;
            cursorMano.SetActive(false);
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void panelControl(string type, string number)
    {
        if (number != "-") { interactionNumber = int.Parse(number) - 1; }
        switch (type)
        {
            case "instructions":
                instructionTitle.text = instructionsDescriptionsDic[currentLanguage].instructionTitle;
                instructionDescription.text = instructionsDescriptionsDic[currentLanguage].instructionDescription;
                instructionInstruction.text = instructionsDescriptionsDic[currentLanguage].instructionInstruction;
                instructionBtnContinueLabel.text = instructionsDescriptionsDic[currentLanguage].instructionBtnContinueLabel;
                instructionPanel.SetActive(true);

                isPanelActive = true;
                activePanelType = "text";
                activeTitle = textTitle;
                break;
            case "sound":
                audioSource = GetComponent<AudioSource>();
                audioSource.Play();
                break;
            case "text":
                textTitle.text = textsDic[currentLanguage].titles[interactionNumber];
                textDescription.text = textsDic[currentLanguage].phrases[interactionNumber];
                textInstruction.text = fixedTextsDic[currentLanguage].instruction;
                textPanel.SetActive(true);

                isPanelActive = true;
                activePanelType = "text";
                activeTitle = textTitle;
                activeDescription = textDescription;
                activeInstruction = textInstruction;
                break;
            case "image":
                image.sprite = sprites[interactionNumber];
                imageTitle.text = imagesDescriptionsDic[currentLanguage].titles[interactionNumber];
                imageDescription.text = imagesDescriptionsDic[currentLanguage].phrases[interactionNumber];
                imageInstruction.text = fixedTextsDic[currentLanguage].instruction;
                imagePanel.SetActive(true);

                isPanelActive = true;
                activePanelType = "image";
                activeTitle = imageTitle;
                activeDescription = imageDescription;
                activeInstruction = imageInstruction;
                break;
            case "question":
                player.GetComponent<PlayerController>().speed = 0;
                questionPanel.SetActive(true);
                break;
            case "release":
                disabelPanels();
                break;
            /*
            case "response":
                if (previousResponse)
                {
                    textDescription.text = "Nos alegramos que así sea.";
                }
                else
                {
                    textDescription.text = "Por favor comantanos como podemos mejorar.";
                }
                textPanel.transform.position = position + new Vector3(0, 1.8f, 15.0f);
                textPanel.SetActive(true);
                break;
            case "warning":
                warningPanel.transform.position = position + new Vector3(0, 1.5f, 3.0f);
                warningPanel.SetActive(true);
                break;
            */
            default:
                break;
        }
    }
    public void showManu()
    {
        this.menuPanel.SetActive(true);
    }
    public void onButtonReturnTour()
    {
        menuPanel.SetActive(false);
    }
    public void HandleLanguageChange(string language)
    {
        currentLanguage = (Language)System.Enum.Parse(typeof(Language), language);


        questionTitle.text = fixedTextsDic[currentLanguage].questionTitle;
        questionBody.text = fixedTextsDic[currentLanguage].questionBody;
        questionOptNo.text = fixedTextsDic[currentLanguage].questionOption1;
        questionOptYes.text = fixedTextsDic[currentLanguage].questionOption2;

        menuTitle.text = fixedTextsDic[currentLanguage].menuTitle;
        menuVolumeLabel.text = fixedTextsDic[currentLanguage].menuVolumeLabel;
        menuLanguageLabel.text = fixedTextsDic[currentLanguage].menuLanguageLabel;
        menuBtnReturnLabel.text = fixedTextsDic[currentLanguage].menuBtnReturnLabel;
        menuBtnLeaveTourLabel.text = fixedTextsDic[currentLanguage].menuBtnLeaveTourLabel;

        menuLanguageOptEn.text = fixedTextsDic[currentLanguage].menuLanguageOptEn;
        menuLanguageOptEs.text = fixedTextsDic[currentLanguage].menuLanguageOptEs;

        if (isPanelActive)
        {
            if (activeTitle)
            {
                panelControl(activePanelType, (interactionNumber + 1).ToString());
            }
        }

        tourSoundingManager.HandleLanguageChange(currentLanguage);
    }
    public void HandleAnswer(bool response)
    {
        previousResponse = response;
        imagePanel.SetActive(false);
        questionPanel.SetActive(false);
        textPanel.SetActive(false);
        player.GetComponent<PlayerController>().speed = player.GetComponent<PlayerController>().originalSpeed;
        player.GetComponent<PlayerController>().started = true;
    }
    public void disabelPanels()
    {
        instructionPanel.SetActive(false);
        textPanel.SetActive(false);
        imagePanel.SetActive(false);
        questionPanel.SetActive(false);
        warningPanel.SetActive(false);
        menuPanel.SetActive(false);

        isPanelActive = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
