using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IDataPersistence
{
    [Header("Engine Variables")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] islandPointers;
    public GameObject pointTo { get; private set; }
    [SerializeField] private GameObject pointer;
    [SerializeField] private InputReader reader;
    private int lastIsland = 0;

    [Header("Tutorial Objects")]
    [SerializeField] private GameObject fadeInPanel;
    private Image fadeInImg;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI titleBG;
    [SerializeField] private TextMeshProUGUI tut1;
    [SerializeField] private TextMeshProUGUI tut1BG;
    [SerializeField] private TextMeshProUGUI tut2;
    [SerializeField] private TextMeshProUGUI tut2BG;
    [SerializeField] private TextMeshProUGUI tut3;
    [SerializeField] private TextMeshProUGUI tut3BG;
    public bool introRunning { get; private set; } = true;
    private Coroutine tutorial = null;
    private bool tutPlayed = false;
    private bool tutRunning = false;

    [Header("Const Variables")]
    private const float fadeInTime = 3.0f;
    private const float tutFadeTime = 1.0f;
    private const float tutReadTime = 5.0f;

    //singleton
    public static GameManager instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        Destroy(this);
        else
        instance = this;
        fadeInImg = fadeInPanel.GetComponent<Image>();

        StartCoroutine(LoadIn());
    }

    // Update is called once per frame
    void Update()
    {
        if (pointTo != null)
        RotatePointer();
        RunTutorial();
    }

    private IEnumerator LoadIn()
    {
        fadeInPanel.SetActive(true);
        float takenTime = 0;
        while (takenTime < fadeInTime)
        {
            takenTime += Time.deltaTime;
            fadeInImg.color = new Color(0, 0, 0, 1 - takenTime / fadeInTime);
            if (takenTime > fadeInTime * 0.5f)
            {
                title.color = new Color(0, 0, 0, 1 - (takenTime - 1.5f) / fadeInTime);
                titleBG.color = new Color(1, 1, 1, 1 - (takenTime - 1.5f) / fadeInTime);
            }
            yield return null;
        }

        while (takenTime < fadeInTime * 1.5f)
        {
            takenTime += Time.deltaTime;
            title.color = new Color(0, 0, 0, 1 - (takenTime - 1.5f) / fadeInTime);
            titleBG.color = new Color(1, 1, 1, 1 - (takenTime - 1.5f) / fadeInTime);
            yield return null;
        }

        fadeInPanel.SetActive(false);
        reader.SetIslandMovement();
        introRunning = false;
    }

    private void RunTutorial()
    {
        if (tutPlayed) return;
        if (tutRunning) return;
        if (introRunning) return;
        tutorial = StartCoroutine(PlayTutorial());
    }

    private void TutorialFunctions(float _takenTime, bool fadeIn, TextMeshProUGUI _tut, TextMeshProUGUI _tutBG)
    {
        if (fadeIn)
        {
            _tut.color = new Color(0, 0, 0, _takenTime / tutFadeTime);
            _tutBG.color = new Color(1, 1, 1, _takenTime / tutFadeTime);
        }
        else
        {
            _tut.color = new Color(0, 0, 0, 1 - _takenTime / tutFadeTime);
            _tutBG.color = new Color(1, 1, 1, 1 - _takenTime / tutFadeTime);
        }
    }

    private IEnumerator PlayTutorial()
    {
        tutRunning = true;
        tut1BG.gameObject.SetActive(true);
        float takenTime = 0;
        while (takenTime < tutFadeTime)
        {
            takenTime += Time.deltaTime;
            TutorialFunctions(takenTime, true, tut1, tut1BG);
            yield return null;
        }

        yield return new WaitForSeconds(tutReadTime);
        
        takenTime = 0;
        while (takenTime < tutFadeTime)
        {
            takenTime += Time.deltaTime;
            TutorialFunctions(takenTime, false, tut1, tut1BG);
            yield return null;
        }
        tut1BG.gameObject.SetActive(false);
        tut2BG.gameObject.SetActive(true);

        takenTime = 0;
        while (takenTime < tutFadeTime)
        {
            takenTime += Time.deltaTime;
            TutorialFunctions(takenTime, true, tut2, tut2BG);
            yield return null;
        }

        yield return new WaitForSeconds(tutReadTime);

        takenTime = 0;
        while (takenTime < tutFadeTime)
        {
            takenTime += Time.deltaTime;
            TutorialFunctions(takenTime, false, tut2, tut2BG);
            yield return null;
        }
        tut2BG.gameObject.SetActive(false);
        tut3BG.gameObject.SetActive(true);

        takenTime = 0;
        while (takenTime < tutFadeTime)
        {
            takenTime += Time.deltaTime;
            TutorialFunctions(takenTime, true, tut3, tut3BG);
            yield return null;
        }

        yield return new WaitForSeconds(tutReadTime);

        takenTime = 0;
        while (takenTime < tutFadeTime)
        {
            takenTime += Time.deltaTime;
            TutorialFunctions(takenTime, false, tut3, tut3BG);
            yield return null;
        }
        tut3BG.gameObject.SetActive(false);

        tutPlayed = true;
    }

    private void RotatePointer()
    {
        Vector2 dir = (player.transform.position - pointTo.transform.position).normalized;
        float zRotation = Vector2.SignedAngle(Vector2.up, dir);
        pointer.transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    public void UpdatePointer(int _islandNumber)
    {
        pointer.SetActive(true);
        pointTo = GameManager.instance.islandPointers[_islandNumber];
    }

    public void SaveLastIsland(int _islandNumber)
    {
        lastIsland = _islandNumber;
    }

    public void HidePointer()
    {
        pointer.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        reader.SetIslandMovement();
    }

    public void SaveData(ref GameData data)
    {
        if (lastIsland != 0)
        data.position = islandPointers[lastIsland-1].transform.position;
    }
}
