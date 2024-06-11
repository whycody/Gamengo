using UnityEngine;

public class  ParallaxController : MonoBehaviour
{
    private Transform _mainCamera;
    private Vector3 _cameraStartPosition;
    private float _distance;

    private GameObject[] _backgrounds;
    private Material[] _materials;
    private float[] _backSpeed;
    private float _farthestBack;
    
    [Range(0.01f, 0.05f)] public float parallaxSpeed;
    
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    
    private void Start()
    {
        if (Camera.main == null) return;
        _mainCamera = Camera.main.transform;
        _cameraStartPosition = _mainCamera.position;

        var backCount = transform.childCount;
        _materials = new Material[backCount];
        _backSpeed = new float[backCount];
        _backgrounds = new GameObject[backCount];

        for (var i = 0; i < backCount; i++)
        {
            _backgrounds[i] = transform.GetChild(i).gameObject;
            _materials[i] = _backgrounds[i].GetComponent<Renderer>().material;

        }
        
        BackSpeedCalculate(backCount);
    }
    
    private void BackSpeedCalculate(int backCount)
    {
        for (var i = 0; i < backCount; i++)
        {
            if ((_backgrounds[i].transform.position.z - _mainCamera.position.z) > _farthestBack)
            {
                _farthestBack = (_backgrounds[i].transform.position.z) - _mainCamera.position.z;
            }
        }

        for (var i = 0; i < backCount; i++)
        {
            _backSpeed[i] = 1 - (_backgrounds[i].transform.position.z - _mainCamera.position.z) / _farthestBack;
        }
    }

    private void LateUpdate()
    {
        _distance = _mainCamera.position.x - _cameraStartPosition.x;
        transform.position = new Vector3(_mainCamera.position.x, transform.position.y, 0);
        for (var i = 0; i < _backgrounds.Length; i++)
        {
            var speed = _backSpeed[i] * parallaxSpeed;
            _materials[i].SetTextureOffset(MainTex, new Vector2(_distance, 0) * speed);
        }
    }
}
