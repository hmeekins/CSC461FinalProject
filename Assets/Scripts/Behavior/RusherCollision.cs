using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class RusherCollision : MonoBehaviour
{
    public bool HasTriggered = false;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;

    [Header("Camera Shake")]
    [SerializeField] private Transform _trackingSpace;
    [SerializeField] private float _shakeDuration = 0.15f;
    [SerializeField] private float _shakeStrength = 0.1f;

    [Header("Knockback")]
    [SerializeField] private Transform _playerRoot;
    [SerializeField] private float _knockbackStrength = 1.6f;
    [SerializeField] private float _knockbackUpwardBoost = 0.15f;
    [SerializeField] private float _knockbackDuration = 0.10f;

    private Vector3 _originalShakePos;
    private AudioFade _audioFade;

    private void Start()
    {
        _originalShakePos = _trackingSpace.localPosition;
        GameObject stadiumObject = GameObject.FindGameObjectWithTag("Stadium");
        _audioFade = stadiumObject.GetComponentInParent<AudioFade>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GlobalVariables.ballThrown) {
            if (HasTriggered) return;

            if (other.CompareTag("Rusher"))
            {
                _audioFade.FadeOut(1, 4f);
                HasTriggered = true;
                GameData.RegisterTackle();
                CsvLogger.SaveEventData("Tackle", GameData.PlayNum);

                StartCoroutine(TackleSequence(other.transform.position));
            }
        }
    }

    private IEnumerator TackleSequence(Vector3 hitSourcePos)
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
            AudioSource.PlayClipAtPoint(_audioSource.clip, transform.position);
        }
        
        GlobalVariables.tackled = true;
        yield return StartCoroutine(ApplyKnockback(hitSourcePos));
        yield return StartCoroutine(ImpactShake());
        
        OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.LTouch);
        yield return new WaitForSeconds(.5f);
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
        GlobalVariables.downs += 1;
        GameFlowController.Instance.OnPlayerTackled();  
    }

    private IEnumerator ImpactShake()
    {
        float elapsed = 0f;

        while (elapsed < _shakeDuration)
        {
            Vector3 offset = Random.insideUnitSphere * _shakeStrength;
            _trackingSpace.localPosition = _originalShakePos + offset;

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        _trackingSpace.localPosition = _originalShakePos;
    }

    private IEnumerator ApplyKnockback(Vector3 hitSourcePos)
    {
        if (_playerRoot == null)
            yield break;

        float elapsed = 0f;

        Vector3 direction = (_playerRoot.position - hitSourcePos).normalized;
        direction.y = 0f;

        Vector3 startPos = _playerRoot.position;
        Vector3 targetPos = startPos + direction * _knockbackStrength;
        targetPos.y += _knockbackUpwardBoost;

        while (elapsed < _knockbackDuration)
        {
            float t = elapsed / _knockbackDuration;
            _playerRoot.position = Vector3.Lerp(startPos, targetPos, t);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        _playerRoot.position = targetPos;
    }

}
