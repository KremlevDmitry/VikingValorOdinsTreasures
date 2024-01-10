using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CubesGame
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private Cube[] _cubes = default;
        private List<Cube> _list = null;
        [SerializeField]
        private Sprite[] _cubeSprites = default;
        private Sprite[] _enableCubeSprites = default;

        [SerializeField]
        private GameObject _startPopup = default;
        [SerializeField]
        private GameObject _winObject = default;
        [SerializeField]
        private GameObject _loseObject = default;
        [SerializeField]
        private Text _rewardText = default;

        [SerializeField]
        private int _height = default;
        [SerializeField]
        private int _width = default;

        [SerializeField]
        private Image _healthImage = default;

        [SerializeField]
        private Sprite[] _enemySprites = default;
        [SerializeField]
        private Image _enemyImage = default;
        [SerializeField]
        private Enemy _enemy = default;


        private void Awake()
        {
            Cube.Height = _cubes[0].transform.position.y - _cubes[_width].transform.position.y;
            _enemy.Game = this;
        }

        private void OnEnable()
        {
            StartGame();
        }

        private void OnDisable()
        {
            FinishGame();
        }

        private void FillList(Cube cube, List<Cube> list)
        {
            if (list.Contains(cube)) { return; }
            list.Add(cube);
            if (cube.X > 0)
            {
                var next = _cubes[cube.Y * _width + (cube.X - 1)];
                if (next.Sprite == cube.Sprite)
                {
                    FillList(next, list);
                }
            }
            if (cube.X < _width - 1)
            {
                var next = _cubes[cube.Y * _width + (cube.X + 1)];
                if (next.Sprite == cube.Sprite)
                {
                    FillList(next, list);
                }
            }
            if (cube.Y > 0)
            {
                var next = _cubes[(cube.Y - 1) * _width + cube.X];
                if (next.Sprite == cube.Sprite)
                {
                    FillList(next, list);
                }
            }
            if (cube.Y < _height - 1)
            {
                var next = _cubes[(cube.Y + 1) * _width + cube.X];
                if (next.Sprite == cube.Sprite)
                {
                    FillList(next, list);
                }
            }
        }

        public void RemoveCubes(Cube currentCube, bool isMe = true)
        {
            FillList(currentCube, _list = new List<Cube>());
            if (_list.Count < 3) { return; }

            if (isMe)
            {
                _healthImage.fillAmount += .01f * _list.Count;
            }
            else
            {
                _healthImage.fillAmount -= .01f * _list.Count;
            }
            CheckWinLose();

            foreach (var cube in _list)
            {
                cube.Sprite = null;
            }

            for (int i = 0; i < _width; i++)
            {
                {
                    int count = 0;
                    for (int j = 0; j < _height; j++)
                    {
                        if (_cubes[j * _width + i].Sprite == null)
                        {
                            count++;
                        }
                    }
                    for (int j = 0; j < count; j++)
                    {
                        _cubes[j * _width + i].transform.position += Cube.Height * count * Vector3.up;
                    }
                    int qwe = 0;
                    for (int j = count; j < _height; j++)
                    {
                        for (int k = qwe; k < _height; k++)
                        {
                            if (_cubes[k * _width + i].Sprite != null)
                            {
                                qwe = k + 1;
                                _cubes[j * _width + i].transform.position = _cubes[k * _width + i].StartPosition.Value;
                                break;
                            }
                        }
                    }
                }
                for (int j = _height - 1; j >= 0; j--)
                {
                    var current = _cubes[j * _width + i];
                    if (current.Sprite == null)
                    {
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (_cubes[k * _width + i].Sprite != null)
                            {
                                (_cubes[k * _width + i].Sprite, current.Sprite) = (current.Sprite, _cubes[k * _width + i].Sprite);
                                break;
                            }
                        }
                    }
                }
            }

            foreach (var cube in _cubes)
            {
                if (cube.Sprite == null)
                {
                    cube.Sprite = GetRandomSprite();
                }
                cube.MoveToStart(true);
            }

            _list = null;
        }

        private void StartGame()
        {
            _enemyImage.sprite = _enemySprites[Random.Range(0, _enemySprites.Length)];
            _healthImage.fillAmount = .5f;

            ResetEnebleSprites();

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    _cubes[i * _width + j].Init(j, i, GetRandomSprite(), this);
                    _cubes[i * _width + j].MoveToStart(false);
                }
            }

            StartCoroutine(Starting());
        }

        private void ResetEnebleSprites()
        {
            var randoms = GenerateRandomArray(4, 0, _cubeSprites.Length - 1);
            _enableCubeSprites = new Sprite[randoms.Length];

            for (int i = 0; i < randoms.Length; i++)
            {
                _enableCubeSprites[i] = _cubeSprites[randoms[i]];
            }
        }

        private void FinishGame()
        {
            _enemy.StopFight();
        }

        private void Win()
        {
            int reward = Troops.Last * 100;
            Wallet.Value += reward;
            _rewardText.text = $"+{reward}";
            FinishGame();

            _winObject.SetActive(true);
        }

        private void Lose()
        {
            Troops.Value -= Troops.Last;
            FinishGame();

            _loseObject.SetActive(true);
        }

        private Sprite GetRandomSprite()
        {
            return _enableCubeSprites[Random.Range(0, _enableCubeSprites.Length)];
        }

        private int[] GenerateRandomArray(int length, int min, int max)
        {
            List<int> availableNumbers = new List<int>();
            for (int i = min; i <= max; i++)
            {
                availableNumbers.Add(i);
            }

            int[] randomArray = new int[length];

            for (int i = 0; i < length; i++)
            {
                int randomIndex = Random.Range(0, availableNumbers.Count);
                randomArray[i] = availableNumbers[randomIndex];
                availableNumbers.RemoveAt(randomIndex);
            }

            return randomArray;
        }

        private IEnumerator Starting()
        {
            _startPopup.SetActive(true);
            yield return new WaitForSeconds(1f);
            _startPopup.SetActive(false);
            _enemy.StartFight();
        }

        public void TryClick()
        {
            int x = Random.Range(0, _width);
            int y = Random.Range(0, _height);
            RemoveCubes(_cubes[y * _width + x], false);
        }

        private void CheckWinLose()
        {
            if (_healthImage.fillAmount <= 0)
            {
                Lose();
            }
            else if (_healthImage.fillAmount >= 1)
            {
                Win();
            }
        }
    }

}
