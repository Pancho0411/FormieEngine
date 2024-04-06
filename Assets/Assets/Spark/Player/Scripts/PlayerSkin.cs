using UnityEngine;
using System.Collections;

[AddComponentMenu("Freedom Engine/Objects/Player/Player Skin")]
public class PlayerSkin : MonoBehaviour
{
    [Header("Skin Animator")]
    public Animator animator;

    [Header("Transforms")]
    public Transform root;

    [Header("Game Objects")]
    public GameObject skin;

    [Header("Renderers")]
    public SkinnedMeshRenderer skinRenederer;

    private Transform currentActivatedBody;
    private readonly Transform[] mouths = new Transform[4];
    private int currentActivatedMouth;

    private IEnumerator blinkCoroutine;

    private void Start()
    {
        InitializeBody();
    }

    private void InitializeBody()
    {
        skin.transform.localScale = Vector3.one;
        currentActivatedBody = skin.transform;
    }

    public void ActiveBall(bool value = true)
    {
        currentActivatedBody.localScale = Vector3.zero;
        currentActivatedBody.localScale = Vector3.one;
    }

    public void ActiveBoost(bool value = true)
    {
        currentActivatedBody.localScale = Vector3.zero;
        currentActivatedBody.localScale = Vector3.one;
    }

    public void SetEulerY(float angle)
    {
        var euler = root.eulerAngles;
        euler.y = angle;
        root.eulerAngles = euler;
    }

    public void SetEulerZ(float angle)
    {
        var euler = root.eulerAngles;
        euler.z = angle;
        root.eulerAngles = euler;
    }

    public void StartBlinking(float duration)
    {
        blinkCoroutine = Blink(duration);
        StartCoroutine(blinkCoroutine);
    }

    public void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            currentActivatedBody.localScale = Vector3.one;
        }
    }

    private IEnumerator Blink(float duration)
    {
        duration += Time.time;

        while (Time.time < duration)
        {
            yield return new WaitForSeconds(0.1f);
            currentActivatedBody.localScale = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
            currentActivatedBody.localScale = Vector3.one;
        }
    }
}