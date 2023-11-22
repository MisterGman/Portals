using System.Collections;
using System.Collections.Generic;
using _Scripts.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioType = _Scripts.Audio.AudioType;

namespace _Scripts.Input
{
    public class DrawControl : MonoBehaviour
    {
        #region Variables

        [field: SerializeField]
        private LineRenderer lineRendererPrefab;

        [field: SerializeField, 
                Range(1, 4), 
                Tooltip("Define how many mirrored drawing lines player sees during drawing")]
        private int numberOfMirrors;
    
        /// <summary>
        /// New input system singleton
        /// </summary>
        private InputManager _inputManager;
        
        /// <summary>
        /// AudioManager singleton
        /// </summary>
        private AudioManager _audioManager;

        /// <summary>
        /// List of line renderers. First line is what player draws
        /// </summary>
        private readonly List<LineRenderer> _currentLines = new();
        
        /// <summary>
        /// Positions of points for each line renderer
        /// </summary>
        private readonly List<List<Vector3>> _points = new();

        /// <summary>
        /// Coroutine stopper
        /// </summary>
        private bool _coroutineRunning;
        
        /// <summary>
        /// When we have 2 lines - the line has negative values for x and y
        /// So it has unique calculation. This prevents calculation errors
        /// </summary>
        private bool _secondMirrorPassed;

        private Transform _transform;

        #endregion

        private void Awake()
        {
            _transform = transform;
            
            _inputManager = InputManager.Instance;
            _audioManager = AudioManager.Instance;
            
            _inputManager.Enable();
            _audioManager.SwitchSong(AudioType.BgdMusicTwo);
        }

        private void OnEnable()
        {
            _inputManager.OnPressDown += DrawStart;
            _inputManager.OnPressUp += DrawEnd;
            _inputManager.OnGoBackPress += GoBackToMainMenu;
        }

        private void OnDisable()
        {
            _inputManager.OnPressDown -= DrawStart;
            _inputManager.OnPressUp -= DrawEnd;
            _inputManager.OnGoBackPress -= GoBackToMainMenu;

            _inputManager.Disable();
        }
    
        /// <summary>
        /// On finger/mouse button down
        /// </summary>
        /// <param name="position">Start position</param>
        private void DrawStart(Vector3 position)
        {
            Debug.Log("Draw start");

            for (int i = 0; i < numberOfMirrors; i++)
            {
                _currentLines.Add(Instantiate(lineRendererPrefab, transform, true));
                _points.Add(new List<Vector3>());
            }

            _coroutineRunning = true;

            _audioManager.PlaySoundOnce(AudioType.StartDragging);
            _audioManager.PlaySoundLoop(AudioType.DuringDragging,true);
            StartCoroutine(DrawTrail());
        }

        /// <summary>
        /// On finger/mouse button hold
        /// </summary>
        /// <returns></returns>
        private IEnumerator DrawTrail()
        {
            while (_coroutineRunning)
            {
                var curLinePosition = _inputManager.CurrentPosition();

                _points[0].Add(curLinePosition);
                _currentLines[0].positionCount = _points[0].Count;
                _currentLines[0].SetPositions(_points[0].ToArray());

                var x = curLinePosition.x;
                var y = curLinePosition.y;
                for (int i = 1; i < numberOfMirrors; i++)
                {
                    var values = RotateValues(x, y, i);
                
                    _points[i].Add(new Vector3(values.x, values.y, curLinePosition.z));
                    _currentLines[i].positionCount = _points[i].Count;
                    _currentLines[i].SetPositions(_points[i].ToArray());
                }
                
                yield return null;
            }
        }

        /// <summary>
        /// On finger/mouse button up
        /// </summary>
        /// <param name="position">End position</param>
        private void DrawEnd(Vector3 position)
        {
            _coroutineRunning = false;
            _currentLines.Clear();

            for (int i = 0; i < numberOfMirrors; i++) 
                _points[i].Clear();

            _audioManager.PlaySoundLoop(AudioType.DuringDragging,false);
            _audioManager.PlaySoundOnce(AudioType.StopDragging);
            
            if(_transform.childCount > 10)
                _audioManager.SwitchSong(AudioType.BgdMusicThree);
            
            Debug.Log("Draw end");
        }

        private Vector2 RotateValues(float x, float y, int index)
        {
            switch (index+1) //Added +1 to get readable info
            {
                case 2:
                    x = -x;
                    y = -y;
                    _secondMirrorPassed = true;
                    break;
                case 3:
                case 4:
                    if (_secondMirrorPassed)
                    {
                        x = -x;
                        y = -y;
                        _secondMirrorPassed = false;
                    }

                    var z = x;
                    x = -y;
                    y = z;
                    break;
            }
            
            return new Vector2(x, y);
        }

        private void GoBackToMainMenu()
        {
            SceneManager.LoadSceneAsync(sceneBuildIndex: 0);
        }
    }
}
