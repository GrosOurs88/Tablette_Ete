using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour
{
    [Header("***General***")]
    public List<GameObject> objectsToDisableAtStart = new List<GameObject>();
    private int numberOfTypeToRestart = 5;
    private int actualType = 0;
    private float timeToCheckForRestart = 3f;
    private float timer = 0f;
    public static ManagerScript instance;

    [Header("***Main***")]
    public VideoPlayer videoPlayer;
    public Image blackScreen;
    public RawImage rawImage;
    public Button buttonClose;
    public VideoClip VideoCorsairs;
    public VideoClip videoCaptainSelkirk;
    public VideoClip videoConstellation;
    public Canvas canvasCode;
    public Canvas canvasTresor;
    public Canvas canvasConstellation;
    public Texture texture;
    private bool isCodeCanvasActive = false;
    private bool isChestCanvasActive = false;
    private bool isConstellationCanvasActive = false;

    [Header("***Code***")]
    public TMP_InputField codeInputField;
    private TouchScreenKeyboard keyboard;
    public string code1;
    public string code2;
    private List<char> charListCode1 = new List<char>();
    private List<char> charListCode2 = new List<char>();
    private List<char> charListCodeInputField = new List<char>();
    public GameObject wrongOrderPopUp;
    public GameObject wrongCodePopUp;
    private int codeTryNumber = 5;
    public TextMeshProUGUI remainingCodeTry;
    public Button validateCodeButton;
    public VideoClip videoCodeFound;
    public VideoClip videoCodeNotFound;

    [Header("***Tresor***")]
    public GameObject panelMap;
    private List<GameObject> chestButtonsList = new List<GameObject>();
    public VideoClip videoChestFound;
    public VideoClip videoChestNotFound;
    public GameObject chestValidationPanel;
    public GameObject textOuEstLeTresor;
    private bool isTheChestOnTheLastCaseChecked;
    [HideInInspector]
    public GameObject ButtonSelected;

    [Header("***Constellation***")]
    public VideoClip videoConstellationFound;
    public VideoClip videoConstellationNotFound;
    public GameObject wrongConstellationPopUp;
    private int constellationTryNumber = 5;
    public TextMeshProUGUI remainingConstellationTry;
    [Tooltip("Toujours mettre le chiffre le plus petit à gauche")]
    public List<string> starsCombinationsToReproduce = new List<string>();
    public List<string> starsCombinations = new List<string>(); //private
    public List<GameObject> links = new List<GameObject>(); //private
    public GameObject firstStarSelected = null; //[HideInInspector] 
    public GameObject secondStarSelected = null; //[HideInInspector] 
    public GameObject starLinkPrefab;
    public GameObject starLinksParentFolder;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (Transform tr in panelMap.transform)
        {
            foreach(Transform trChild in tr.transform)
            {
                chestButtonsList.Add(trChild.gameObject);
            }
        }

        foreach(GameObject gO in objectsToDisableAtStart)
        {
            gO.SetActive(false);
        }

        rawImage.color = new Color(1, 1, 1, 1);

        foreach(char c in code1)
        {
            charListCode1.Add(c);
        }

        foreach (char c in code2)
        {
            charListCode2.Add(c);
        }
    }

    void Update()
    {
        if(videoPlayer != null &&
           videoPlayer.isPrepared &&
           videoPlayer.isPlaying == false)
        {
            StopVideo();
        }

        if(actualType > 0)
        {
            timer += Time.deltaTime;
            if(timer >= timeToCheckForRestart)
            {
                timer = 0f;
                actualType = 0;
            }
            if(actualType >= numberOfTypeToRestart)
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

        if(codeInputField.text.Length == 4)
        {
            validateCodeButton.interactable = true;
        }
        else
        {
            validateCodeButton.interactable = false;
        }
    }

    public void LaunchVideoCorsairs()
    {
        StartCoroutine(VideoStart(VideoCorsairs));
    }

    public void LaunchVideoCaptainSelkirk()
    {
        StartCoroutine(VideoStart(videoCaptainSelkirk));
    }

    public void LaunchVideoConstellation()
    {
        StartCoroutine(VideoStart(videoConstellation));
    }

    public void LaunchVideoCodeFound()
    {
        StartCoroutine(VideoStart(videoCodeFound));
    }

    public void LaunchVideoCodeNotFound()
    {
        StartCoroutine(VideoStart(videoCodeNotFound));
    }

    public void LaunchVideoChestFound()
    {
        StartCoroutine(VideoStart(videoChestFound));
    }

    public void LaunchVideoChestNotFound()
    {
        StartCoroutine(VideoStart(videoChestNotFound));
    }

    public void LaunchVideoConstellationNotFound()
    {
        StartCoroutine(VideoStart(videoConstellationNotFound));
    }

    public void LaunchVideoConstellationFound()
    {
        StartCoroutine(VideoStart(videoConstellationFound));
    }

    public void StopVideo()
    {
        rawImage.gameObject.SetActive(false);
        buttonClose.gameObject.SetActive(false);

        videoPlayer.Stop();
        videoPlayer.clip = null;

        if (isCodeCanvasActive == true)
        {
            DeactiveCanvasCode();
            isCodeCanvasActive = false;
        }

        if (isChestCanvasActive == true)
        {
            DeactiveCanvasTresor();
            isChestCanvasActive = false;
        }

        if (isConstellationCanvasActive == true)
        {
            DeactiveCanvasConstellation();
            isConstellationCanvasActive = false;
        }

        blackScreen.gameObject.SetActive(false);
    }

    public void ActiveCanvasCode()
    {
        EnableCanvas(canvasCode);
        isCodeCanvasActive = true;
    }

    public void ActiveCanvasTresor()
    {
        EnableCanvas(canvasTresor);
        isChestCanvasActive = true;
    }

    public void ActiveCanvasConstellation()
    {
        EnableCanvas(canvasConstellation);
        isConstellationCanvasActive = true;
    }

    public void DeactiveCanvasCode()
    {
        codeInputField.text = "";
        DisableCanvas(canvasCode);
        DeactivateKeyboard();
    }

    public void DeactiveCanvasTresor()
    {
        if(ButtonSelected != null)
        {
            ButtonSelected.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            ButtonSelected = null;
        }

        foreach (GameObject gO in chestButtonsList)
        {
            gO.GetComponent<Button>().interactable = true;
        }

        chestValidationPanel.gameObject.SetActive(false);
        textOuEstLeTresor.gameObject.SetActive(true);

        DisableCanvas(canvasTresor);
    }

    public void DeactiveCanvasConstellation()
    {
        DisableCanvas(canvasConstellation);
    }

    public void ActivateKeyboard()
    {
         keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    public void DeactivateKeyboard()
    {
        if (keyboard != null)
        {
            keyboard.active = false;
        }
    }

    public void EnableChestValidationPanel()
    {
        textOuEstLeTresor.gameObject.SetActive(false);
        chestValidationPanel.gameObject.SetActive(true);

        foreach (GameObject gO in chestButtonsList)
        {
            gO.GetComponent<Button>().interactable = false;
        }
    }

    public void DisableChestValidationPanel()
    {
        chestValidationPanel.gameObject.SetActive(false);
        textOuEstLeTresor.gameObject.SetActive(true);
        isTheChestOnTheLastCaseChecked = false;
        ButtonSelected.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        ButtonSelected = null;

        foreach (GameObject gO in chestButtonsList)
        {
            gO.GetComponent<Button>().interactable = true;
        }
    }

    public void DisableChestValidationPanelWhenChestChecked()
    {
        chestValidationPanel.gameObject.SetActive(false);
        textOuEstLeTresor.gameObject.SetActive(true);
        ButtonSelected = null;
    }

    public void GoodLocationSelected()
    {
        isTheChestOnTheLastCaseChecked = true;
    }

    public void CheckChestEmplacment()
    {
        if(isTheChestOnTheLastCaseChecked)
        {
            LaunchVideoChestFound();

            foreach (GameObject gO in chestButtonsList)
            {
                gO.GetComponent<Button>().interactable = false;
            }

            isTheChestOnTheLastCaseChecked = false;

            ButtonSelected.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            LaunchVideoChestNotFound();

            foreach (GameObject gO in chestButtonsList)
            {
                gO.GetComponent<Button>().interactable = true;
            }

            ButtonSelected.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }

        DisableChestValidationPanelWhenChestChecked();
    }

    public void EnableCanvas(Canvas _canvas)
    {
        _canvas.gameObject.SetActive(true);
    }

    public void DisableCanvas(Canvas _canvas)
    {
        _canvas.gameObject.SetActive(false);
    }

    public static bool CompareLists<T>(List<T> aListA, List<T> aListB)
    {
        if (aListA == null || aListB == null || aListA.Count != aListB.Count)
        {
            return false;
        }

        if (aListA.Count == 0)
        {
            return true;
        }

        Dictionary<T, int> lookUp = new Dictionary<T, int>();

        for (int i = 0; i < aListA.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListA[i], out count))
            {
                lookUp.Add(aListA[i], 1);
                continue;
            }
            lookUp[aListA[i]] = count + 1;
        }

        for (int i = 0; i < aListB.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListB[i], out count))
            {
                return false;
            }
            count--;

            if (count <= 0)
            {
                lookUp.Remove(aListB[i]);
            }

            else
            {
                lookUp[aListB[i]] = count;
            }
        }

        return lookUp.Count == 0;
    }

    private void DebugVisualReset()
    {
        videoPlayer.targetTexture.DiscardContents();
        videoPlayer.targetTexture.Release();
        Graphics.Blit(texture, videoPlayer.targetTexture);
    }
    
    public void CheckCode()
    {
        charListCodeInputField.Clear();
        codeInputField.text = codeInputField.text.ToUpper();

        foreach (char c in codeInputField.text.ToString())
        {
            charListCodeInputField.Add(c);
        }

        if (codeInputField.text == code1 || codeInputField.text == code2)
        {
            LaunchVideoCodeFound();
            codeInputField.text = "";
            codeTryNumber = 5;
            remainingCodeTry.text = codeTryNumber.ToString();
        }

        else if(CompareLists(charListCodeInputField, charListCode1) || CompareLists(charListCodeInputField, charListCode2))
        {
            StartCoroutine(WrongOrderPopUp());
            codeTryNumber--;
        }

        else
        {
            StartCoroutine(WrongCodePopUp());
            codeTryNumber--;
        }

        remainingCodeTry.text = codeTryNumber.ToString();

        if(codeTryNumber == 0)
        {
            LaunchVideoCodeNotFound();
            codeTryNumber = 5;
            remainingCodeTry.text = codeTryNumber.ToString();
        }
    }

    public void SelectFirstStar(GameObject _starSelected)
    {
        firstStarSelected = _starSelected;
    }

    public void SelectSecondStar(GameObject _starSelected)
    {
        secondStarSelected = _starSelected;
    }

    public void ResetFirstStar()
    {
        firstStarSelected = null;
    }

    public void MatchStars()
    {
        string newCombination = "";

        if(firstStarSelected.GetComponent<ButtonStarScript>().starIndex < secondStarSelected.GetComponent<ButtonStarScript>().starIndex)
        {
            newCombination = firstStarSelected.GetComponent<ButtonStarScript>().starIndex.ToString() + "-" + secondStarSelected.GetComponent<ButtonStarScript>().starIndex.ToString();
        }
        else
        {
            newCombination = secondStarSelected.GetComponent<ButtonStarScript>().starIndex.ToString() + "-" + firstStarSelected.GetComponent<ButtonStarScript>().starIndex.ToString();
        }

        if(starsCombinations.Contains(newCombination))
        {
            starsCombinations.Remove(newCombination);
            RemoveStarsLink(newCombination);
        }
        else
        {
            starsCombinations.Add(newCombination);
            AddStarsLink();
        }

        firstStarSelected.GetComponent<ButtonStarScript>().ResetStar();
        secondStarSelected.GetComponent<ButtonStarScript>().ResetStar();

        firstStarSelected = null;
        secondStarSelected = null;
    }

    public void AddStarsLink() //---------------------------------
    {
        GameObject link = Instantiate(starLinkPrefab, starLinksParentFolder.transform);

        Vector3 startPos = firstStarSelected.transform.TransformPoint(firstStarSelected.GetComponent<RectTransform>().rect.center);
        Vector3 endPos = secondStarSelected.transform.TransformPoint(firstStarSelected.GetComponent<RectTransform>().rect.center);

        Vector3 startPosModified = new Vector3(startPos.x, startPos.y, startPos.z+0.5f);
        Vector3 endPosModified = new Vector3(endPos.x, endPos.y, endPos.z + 0.5f);

        link.GetComponent<LineRenderer>().SetPosition(0, startPosModified);
        link.GetComponent<LineRenderer>().SetPosition(1, endPosModified);

        if(firstStarSelected.GetComponent<ButtonStarScript>().starIndex < secondStarSelected.GetComponent<ButtonStarScript>().starIndex)
        {
            link.GetComponent<StarLinkScript>().linkIndexes = firstStarSelected.GetComponent<ButtonStarScript>().starIndex.ToString() + "-" + secondStarSelected.GetComponent<ButtonStarScript>().starIndex.ToString();
        }
        else
        {
            link.GetComponent<StarLinkScript>().linkIndexes = secondStarSelected.GetComponent<ButtonStarScript>().starIndex.ToString() + "-" + firstStarSelected.GetComponent<ButtonStarScript>().starIndex.ToString();
        }

        links.Add(link);
    }

    public void RemoveStarsLink(string _name)
    {
        for (int i=0; i<links.Count; i++)
        {
            if(links[i].GetComponent<StarLinkScript>().linkIndexes == _name)
            {
                Destroy(links[i]);
                links.RemoveAt(i);
            }
        }
    }

    public void ResetConstellation()
    {
        foreach(GameObject gO in links)
        {
            Destroy(gO);
        }
        links.Clear();

        starsCombinations.Clear();
    }

    public void CheckConstellation()
    {
        int goodIndexes = 0;

        if (starsCombinations.Count == starsCombinationsToReproduce.Count)
        {
            foreach (string s in starsCombinations)
            {
                if (starsCombinationsToReproduce.Contains(s))
                {
                    goodIndexes += 1;

                    codeTryNumber = 5;
                    remainingConstellationTry.text = codeTryNumber.ToString();
                }
            }

            if(goodIndexes == starsCombinationsToReproduce.Count)
            {
                LaunchVideoConstellationFound();

                ResetConstellation();
                constellationTryNumber = 5;
                canvasConstellation.gameObject.SetActive(false);
            }

            else
            {
                StartCoroutine(WrongConstellationPopUp());
                constellationTryNumber--;
            }
        }

        else
        {
            StartCoroutine(WrongConstellationPopUp());
            constellationTryNumber--;
        }

        remainingConstellationTry.text = constellationTryNumber.ToString();

        if (constellationTryNumber == 0)
        {
            LaunchVideoConstellationNotFound();
            constellationTryNumber = 5;
            remainingConstellationTry.text = constellationTryNumber.ToString();

            canvasConstellation.gameObject.SetActive(false);
        }
    }

    public void TypeToRestart()
    {
        actualType++;
    }

    IEnumerator VideoStart(VideoClip _clip)
    {
        blackScreen.gameObject.SetActive(true);

        videoPlayer.clip = _clip;

        DebugVisualReset();

        rawImage.gameObject.SetActive(true);

        videoPlayer.Play();

        yield return new WaitForSeconds(1f);

        buttonClose.gameObject.SetActive(true);

        yield return null;
    }

    IEnumerator WrongOrderPopUp()
    {
        codeInputField.text = "";
        wrongOrderPopUp.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        wrongOrderPopUp.gameObject.SetActive(false);
    }

    IEnumerator WrongCodePopUp()
    {
        codeInputField.text = "";
        wrongCodePopUp.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        wrongCodePopUp.gameObject.SetActive(false);
    }

    IEnumerator WrongConstellationPopUp()
    {
        wrongConstellationPopUp.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        wrongConstellationPopUp.gameObject.SetActive(false);
    }
}
