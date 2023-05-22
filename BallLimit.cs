using UnityEngine;


namespace Oculus.Interaction
{
    public class BallLimit : MonoBehaviour
    {

        [SerializeField]
        private PokeInteractor _pokeInteractor;



        private bool _isTouching;

        protected bool _started = false;


        public bool GetIsTouching()
        {
            return _isTouching;
        }


        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(_pokeInteractor, nameof(_pokeInteractor));
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                _pokeInteractor.WhenStateChanged += HandleStateChanged;
                _pokeInteractor.WhenPassedSurfaceChanged += HandlePassedSurfaceChanged;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                if (_isTouching)
                {
                    UnlockLimit();
                }

                _pokeInteractor.WhenStateChanged -= HandleStateChanged;
                _pokeInteractor.WhenPassedSurfaceChanged -= HandlePassedSurfaceChanged;
            }
        }

        private void HandlePassedSurfaceChanged(bool passed)
        {
            CheckPassedSurface();
        }

        private void HandleStateChanged(InteractorStateChangeArgs args)
        {
            CheckPassedSurface();
        }

        private void CheckPassedSurface()
        {
            if (_pokeInteractor.IsPassedSurface)
            {
                LockLimit();
            }
            else
            {
                UnlockLimit();
            }
        }

        protected virtual void LateUpdate()
        {
            UpdateLimit();
        }

        private void LockLimit()
        {
            _isTouching = true;
        }

        private void UnlockLimit()
        {
            _isTouching = false;
        }

        private void UpdateLimit()
        {
            if (!_isTouching) return;

            
            Vector3 positionDelta = transform.position - _pokeInteractor.Origin;
            Vector3 targetPosePosition = _pokeInteractor.TouchPoint + positionDelta +
                                        _pokeInteractor.Radius *
                                        _pokeInteractor.TouchNormal;
            // update position
            transform.position = targetPosePosition;

        }

        #region Inject

        public void InjectAllBallLimit(PokeInteractor pokeInteractor
        )
        {

            InjectPokeInteractor(pokeInteractor);

        }


        public void InjectPokeInteractor(PokeInteractor pokeInteractor)
        {
            _pokeInteractor = pokeInteractor;
        }


        #endregion
    }
}
