using System.Collections;
using UnityEngine;

public class RusherCollision : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Camera Shake")]
    public Transform trackingSpace;
    public float shakeDuration = 0.3f;
    public float shakeStrength = 0.2f;

    [Header("Knockback")]
    public Transform playerRoot;   // Assign OVRCameraRig or XR Origin here
    public float knockbackStrength = 1.6f;
    public float knockbackUpwardBoost = 0.15f;
    public float knockbackDuration = 0.10f;

    private Vector3 originalShakePos;
    private bool hasTriggered = false;

    private void Start()
    {
        originalShakePos = trackingSpace.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Rusher"))
        {
            hasTriggered = true;
            StartCoroutine(TackleSequence(other.transform.position));
        }
    }

    private IEnumerator TackleSequence(Vector3 hitSourcePos)
    {
        if (audioSource != null)
        {
            audioSource.Play();
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        }

        GlobalVariables.downs += 1;

        yield return StartCoroutine(ApplyKnockback(hitSourcePos));
        yield return StartCoroutine(ImpactShake());
        
        OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.LTouch);
        yield return new WaitForSeconds(.5f);
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);

        GameFlowController.Instance.OnPlayerTackled();
        hasTriggered = false;
    }

    private IEnumerator ImpactShake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 offset = Random.insideUnitSphere * shakeStrength;
            trackingSpace.localPosition = originalShakePos + offset;

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        trackingSpace.localPosition = originalShakePos;
    }

    private IEnumerator ApplyKnockback(Vector3 hitSourcePos)
    {
        if (playerRoot == null)
            yield break;

        float elapsed = 0f;

        Vector3 direction = (playerRoot.position - hitSourcePos).normalized;
        direction.y = 0f;

        Vector3 startPos = playerRoot.position;
        Vector3 targetPos = startPos + direction * knockbackStrength;
        targetPos.y += knockbackUpwardBoost;

        while (elapsed < knockbackDuration)
        {
            float t = elapsed / knockbackDuration;
            playerRoot.position = Vector3.Lerp(startPos, targetPos, t);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        playerRoot.position = targetPos;
    }

}
