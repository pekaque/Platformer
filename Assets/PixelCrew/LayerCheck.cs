using UnityEngine;

namespace PixelCrew
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask _checkedLayer;
        private Collider2D _collider;

        public bool IsTouchingLayer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            IsTouchingLayer = _collider.IsTouchingLayers(_checkedLayer);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            IsTouchingLayer = _collider.IsTouchingLayers(_checkedLayer);
        }
    }

}
