using UnityEngine;

namespace Test
{
    public class WheelComponentView : MonoBehaviour
    {
        [SerializeField] private Transform wheelTransform;
        [SerializeField, Range(0, 10)] private int spinSpeed;

        private bool _canSpin;
        
        public void StartSpin()
        {
            _canSpin = true;
        }

        public void StopSpin()
        {
            _canSpin = false;
        }

        private void Update()
        {
            if (_canSpin)
            {
                wheelTransform.transform.Rotate(0f, 0f, spinSpeed + Time.deltaTime, Space.Self);
            }
        }
    }
}