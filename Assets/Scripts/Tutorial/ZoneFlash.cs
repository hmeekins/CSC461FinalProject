using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneFlash : MonoBehaviour
{
    private Renderer _zoneRenderer;
    private MaterialPropertyBlock _mpb;

    [SerializeField] private Color _baseColor = Color.red;
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private AudioSource _chime;

    [SerializeField] private float _baseEmission = 0.5f;
    [SerializeField] private float _flashEmission = 4.0f;
    [SerializeField] private float _flashDuration = 0.2f;

    void Start()
    {
        _zoneRenderer = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();

        SetEmission(_baseColor, _baseEmission);
    }

    public void TriggerFlash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        _chime.Play();
        SetEmission(_flashColor, _flashEmission);

        yield return new WaitForSeconds(_flashDuration);

        SetEmission(_baseColor, _baseEmission);
    }

    private void SetEmission(Color color, float intensity)
    {
        _zoneRenderer.GetPropertyBlock(_mpb);
        _mpb.SetColor("_EmissionColor", color * intensity);
        _zoneRenderer.SetPropertyBlock(_mpb);
    }
}