using UnityEngine;

namespace PixelCrew.Components
{
    public class GatherCoinComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToGather;
        static int _totalDemand;
    
        public void GatherObject()
        {
            if (_objectToGather.CompareTag("SilverCoin"))
            {
                _totalDemand += 1;
            }

            if (_objectToGather.CompareTag("GoldenCoin"))
            {
                _totalDemand += 10;
            }

            Debug.Log($"Всего очков: {_totalDemand}" );
        }
    } 
}

