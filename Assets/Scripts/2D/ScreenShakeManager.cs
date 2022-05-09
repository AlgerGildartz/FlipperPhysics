using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class ScreenShakeManager : MonoBehaviour
    {
        public static ScreenShakeManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        [SerializeField]
        private float _shakeDuration;
        [SerializeField]
        private float _shakeAmplitude;

        private Transform _camera;
        private Vector3 _cameraInitPos;

        /// <summary>
        /// Récupère le transform de la camera et sa position initiale
        /// </summary>
        private void Start()
        {
            _camera = Camera.main.transform;
            _cameraInitPos = _camera.position;
        }

        /// <summary>
        /// Lance la coroutine de ScreenShake
        /// </summary>
        public void ScreenShake()
        {
            StartCoroutine(CoroutineShake());
        }

        /// <summary>
        /// Coroutine de ScreenShake
        /// </summary>
        private IEnumerator CoroutineShake()
        {
            float temp = 0;

            while (temp <= _shakeDuration)
            {
                _camera.position = _cameraInitPos + Random();
                yield return null;
                temp += Time.deltaTime;
            }
            _camera.position = _cameraInitPos;
        }

        /// <summary>
        /// Renvoie un Vector3 avec une amplitude random sur les 3 axes
        /// </summary>
        private Vector3 Random()
        {
            Vector3 tempResult = Vector3.zero;
            tempResult.x = UnityEngine.Random.Range(-_shakeAmplitude, _shakeAmplitude);
            tempResult.y = UnityEngine.Random.Range(-_shakeAmplitude, _shakeAmplitude);
            tempResult.z = UnityEngine.Random.Range(-_shakeAmplitude, _shakeAmplitude);
            return tempResult;
        }
    }