using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public static MainMenuCamera Instance;
    public ParallaxController parallaxController;
    public AnimationCurve transferSpeedCurve;
    public float speedMultiplier;
    public MainMenuSubScene currentScene;


    public Transform leftBound;
    public Transform rightBound;
    public List<ParallaxPanel> panels = new();
    [Range(0, 100)] public int xvalue;
    [Range(0, 100)] public int yvalue;

    private Vector3 _targetPos;
    private float _current;
    private float _target;
    private bool _isTransfering;
    
    private void Awake()
    {
        parallaxController.enabled = false;
        Instance = this;

        foreach (ParallaxPanel panel in panels)
        {
            var position = leftBound.position;
            panel.SetWidth(position.x, rightBound.position.x);
        }

        currentScene.gameObject.SetActive(true);
        
        Vector3 startPos = currentScene.transform.position;
        ApplyCameraPosition(startPos);
        Parallax(startPos);
    }


    public void TransferTo(MainMenuSubScene scene, bool vertical)
    {
        if(_isTransfering) return;
        _targetPos = scene.gameObject.transform.position;
        var position = transform.position;
        _current = vertical ? position.y : position.x;
        _target = vertical ? _targetPos.y : _targetPos.x;
        if (_current > _target) (_current, _target) = (_target, _current);
        StartCoroutine(TransferRoutine(scene));
    }

    private void Parallax(Vector3 newPos)
    {
        foreach (ParallaxPanel panel in panels)
        {
            var t = panel.transform;
            Vector3 panelPosition = t.position;
            panelPosition.x = newPos.x - (newPos.x / panel.Width * xvalue / panel.speedX);
            panelPosition.y = newPos.y - (newPos.y / leftBound.position.y * yvalue / panel.speedY) + panel.offsetY;
            t.position = panelPosition;
        }
    }

    private void ApplyCameraPosition(Vector3 newPos)
    {
        newPos.z = -10;
        transform.position = newPos;
    }

    private IEnumerator TransferRoutine(MainMenuSubScene scene)
    {
        _isTransfering = true;
        scene.gameObject.SetActive(true);

        while (transform.position != new Vector3(_targetPos.x, _targetPos.y, -10))
        {
            _current = Mathf.MoveTowards(_current, _target, speedMultiplier * Time.deltaTime);

            Vector3 newPos = Vector3.Lerp(transform.position, _targetPos, transferSpeedCurve.Evaluate(_current));
            Parallax(newPos);
            ApplyCameraPosition(newPos);
            yield return null;
        }
        
        _isTransfering = false;
        currentScene.gameObject.SetActive(false);
        currentScene = scene;
    }
}