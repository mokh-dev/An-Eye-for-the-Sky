using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeOption : MonoBehaviour
{
    [SerializeField] private Vector3 hiddenOptionPos;
    [SerializeField] private Vector3 shownObjectPos;
    [SerializeField] private float animDuration;
    [SerializeField] private AudioSource upgradeSound;


    public int upgradeNum;
    public int optionNum;
    private GameController gc;
    private BoxCollider2D boxCollider;


    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }


    public void DisplayOption()
    {
        StartCoroutine(DisplayOptionAnim());
    }

    private IEnumerator DisplayOptionAnim()
    {
        Vector3 startPos = hiddenOptionPos;
        float timeElapsed = 0;

        while (timeElapsed < animDuration)
        {
            transform.position = Vector3.Lerp(startPos, shownObjectPos, timeElapsed / animDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = shownObjectPos;
        boxCollider.enabled = true;
    }

    private IEnumerator HideOptionAnim()
    {
        boxCollider.enabled = false;
        
        Vector3 startPos = shownObjectPos;
        float timeElapsed = 0;

        while (timeElapsed < animDuration)
        {
            transform.position = Vector3.Lerp(startPos, hiddenOptionPos, timeElapsed / animDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = hiddenOptionPos;

        gc.ShopHidden();
        gameObject.SetActive(false);
    }

    public void HideShop()
    {
        StartCoroutine(HideOptionAnim());
    }

    public void PickShop()
    {
        boxCollider.enabled = false;
        transform.position = hiddenOptionPos;
        //gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            upgradeSound.Play();
            gc.ActivateUpgrade(upgradeNum, optionNum);
        }
    }
}
