using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
    public class UIEventListener : MonoBehaviour
    {

        //---------------------------------------------------------------------
        // Editor
        //---------------------------------------------------------------------

        [Header("Text")] 
        [SerializeField] private Text _moneyText;
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _enemiesLeftText;

        //---------------------------------------------------------------------
        // Events
        //---------------------------------------------------------------------

        public void OnCreepDisappeared(GameObject creep)
        {
            var creepEnemyScript = creep.GetComponent<EnemyScript>();

            if (creepEnemyScript.Happy)
            {
                var score = int.Parse(_scoreText.text) + 1;
                var money = int.Parse(_moneyText.text) + creepEnemyScript.Money;
                
                _scoreText.text = score.ToString();
                _moneyText.text = money.ToString();
            }
            else
            {
                var enemiesLeft = int.Parse(_enemiesLeftText.text) - 1;
                _enemiesLeftText.text = enemiesLeft.ToString();
            }
        }
    }
}