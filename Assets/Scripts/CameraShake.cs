 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _cameraStartPos;
    // Start is called before the first frame update
    void Start()
    {
        _cameraStartPos = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CameraShakeCoroutine(float duration, float magnitude)
    {
        float _elapsedTime = 0.0f;
        while (_elapsedTime < duration)
        {
            float xOffset = Random.Range(-1, 1) * magnitude;
            float yOffset = Random.Range(-1, 1) * magnitude;

            transform.position = new Vector3(xOffset, yOffset, _cameraStartPos.z);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = _cameraStartPos;
    }
}
