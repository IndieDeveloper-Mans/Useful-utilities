using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public abstract class TimelineControl : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] protected PlayableDirector _playableDirector;
    [Space]
    [SerializeField] protected Player _player;
    [Space]
    [SerializeField] protected Animator _playerAnimator;
    [Space]
    [SerializeField] protected bool _timelineIsActive;
    [Space]
    [SerializeField] protected bool _playOnce;
    [Space]
    [SerializeField] protected bool _timelineIsPlayed;

    [Header("Activation Control")]
    [SerializeField] protected bool _playerWithinCollider;
    [Space]
    [SerializeField] protected bool _checkIfPlayerIsGrounded = true;
    [Space]
    [SerializeField] protected bool _activateByKey;
    [Space]
    [SerializeField] protected KeyCode _activateKeyCode;

    [Header("Dynamic Bindings Control")]
    [SerializeField] protected bool _useDynamicBinding;

    WaitForSeconds _timelineDuration;

    protected virtual void Start()
    {
        _playableDirector = GetComponentInParent<PlayableDirector>();

        _timelineDuration = new WaitForSeconds((float)_playableDirector.duration);
    }

    private void Update()
    {
        if (_activateByKey)
        {
            if (Input.GetKeyDown(_activateKeyCode))
            {
                CheckIfCanPlayTimeline();
            }     
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            _playerWithinCollider = true;

            if (_player == null)
            {
                _player = player;

                _playerAnimator = player.GetComponent<Animator>();

                DynamicAnimatorBinding(_playerAnimator, "Player");
            }

            if (_playableDirector != null && !_activateByKey)
            {
                if (_checkIfPlayerIsGrounded)
                {
                    if (_player.PlayerIsGrounded())
                    {
                        CheckIfCanPlayTimeline();
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            _playerWithinCollider = false;
        }
    }

    public IEnumerator StartTimelineRoutine()
    {
        Debug.Log(_playableDirector.name + " Starts");

        _timelineIsActive = true;

        _playableDirector.Play();

        yield return _timelineDuration;

        _timelineIsPlayed = true;

        _timelineIsActive = false;

        Debug.Log(_playableDirector.name + " Ends");

        Debug.Log(_playableDirector.name + " Duration Is: " + (float)_playableDirector.duration);
    }

    public void CheckIfCanPlayTimeline()
    {
        if (!_timelineIsActive)
        {
            if (_playOnce && _timelineIsPlayed)
            {
                Debug.Log("Timeline Is already played once");

                return;
            }
            else
            {
                StartCoroutine(StartTimelineRoutine());
            }
        }
        else
        {
            Debug.Log(_playableDirector.name + " Is Working, Wait Till It Ends");
        }
    }

    public void DynamicAnimatorBinding(Animator animator, string trackName)
    {
        if (_useDynamicBinding)
        {
            foreach (var playableAssetOutput in _playableDirector.playableAsset.outputs)
            {
                if (playableAssetOutput.streamName.Equals(trackName))
                {
                    _playableDirector.SetGenericBinding(playableAssetOutput.sourceObject, animator);
                }
            }
        }
    }
}