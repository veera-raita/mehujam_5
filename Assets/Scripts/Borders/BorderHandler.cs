using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BorderHandler : MonoBehaviour
{
    private const float dimThreshold = 5.0f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject topBorder;
    [SerializeField] private GameObject bottomBorder;
    [SerializeField] private GameObject topPanel1;
    [SerializeField] private GameObject topPanel2;
    private Image topImg1;
    private Image topImg2;
    [SerializeField] private GameObject bottomPanel1;
    [SerializeField] private GameObject bottomPanel2;
    private Image botImg1;
    private Image botImg2;
    [SerializeField] private float halfBorderWidth;
    private float bottomAdjustment = 0.635f;
    private float topAdjustment = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        topImg1 = topPanel1.GetComponent<Image>();
        topImg2 = topPanel2.GetComponent<Image>();
        botImg1 = bottomPanel1.GetComponent<Image>();
        botImg2 = bottomPanel2.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(player.transform.position.x, transform.position.y);
        DimScreen();
    }

    private void DimScreen()
    {
        float distToTop = topBorder.transform.position.y - halfBorderWidth - player.transform.position.y - topAdjustment;
        float distToBot = topBorder.transform.position.y - halfBorderWidth + player.transform.position.y - bottomAdjustment;
        float alpha = distToBot < distToTop ? 1 - distToBot / dimThreshold : 1 - distToTop / dimThreshold;

        if (distToTop < 5.0f)
        {
            topImg1.color = new Color(0, 0, 0, alpha);
            topImg2.color = new Color(0, 0, 0, alpha);
        }
        else if (distToBot < 5.0f)
        {
            botImg1.color = new Color(0, 0, 0, alpha);
            botImg2.color = new Color(0, 0, 0, alpha);
        }
        else
        {
            
            topImg1.color = Color.clear;
            topImg2.color = Color.clear;
            botImg1.color = Color.clear;
            botImg2.color = Color.clear;
        }
    }
}
