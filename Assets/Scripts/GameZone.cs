﻿using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameZone : MonoBehaviour
    {
        public Button scrambleButton;
        public Text diceResult;
        public PlayerManager playerManager;

        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 30;
            SetupUI();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return;

            }
        }

        private void SetupUI()
        {
            Button scrambleButton = this.scrambleButton.GetComponent<Button>();
            scrambleButton.onClick.AddListener(ScrambleClicked);

            Text diceResult = this.diceResult.GetComponent<Text>();
            diceResult.text = "";
        }

        private void ScrambleClicked()
        {
            int dice1 = Random.Range(1, 7);
            int dice2 = Random.Range(1, 7);
            int distance = dice1 + dice2;
            diceResult.GetComponent<Text>().text = distance.ToString();
            playerManager.SetWalkableDistance(distance);
            playerManager.Mode = "moving";
        }
    }
}
