using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugFPS : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _fpsText;
    [SerializeField]
    private TextMeshProUGUI _bestfpsText;
    [SerializeField]
    private TextMeshProUGUI _lowestfpsText;

    private int updateInterval = 40;
    int tick;
    private int _bestFps;
    private int _lowestFps;
    private float _currentFPS;
    [SerializeField] private GameObject obj_fps;
    [SerializeField] private bool draw_fps;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        _fpsText.text = "FPS: 0";
        _lowestFps = 100;
        obj_fps.SetActive(draw_fps);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F3)) {
            draw_fps = !draw_fps;
            obj_fps.SetActive(draw_fps);
        }

        tick++;
        _currentFPS = 1f / Time.deltaTime;
        UpdateFPS();
    }

    private void UpdateFPS()
    {
        if (_currentFPS >= _bestFps)
        {
            _bestFps = (int)_currentFPS;
            _bestfpsText.text = $"Best FPS: {_bestFps}";
        }
        if (_lowestFps >= _currentFPS)
        {
            _lowestFps = (int)_currentFPS;
            _lowestfpsText.text = $"Low FPS: {_lowestFps}";
        }
        if (tick % updateInterval==0)
        {
            _fpsText.text = "Curr FPS: " + Mathf.RoundToInt(_currentFPS);
        }
    }

}
