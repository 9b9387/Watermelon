using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public List<Fruit> fruitPrefabList;
    public Transform spawnPoint;
    public Button playButton;
    public Text scoreLabel;
    /// <summary>
    /// 游戏分数
    /// </summary>
    public int score;

    private Fruit fruit;
    private int fruidId;
    private bool isGameOver;
    /// <summary>
    /// 当前场景中所有的水果对象
    /// </summary>
    private List<Fruit> fruits = new List<Fruit>();
    // Start is called before the first frame update
    void Start()
    {
        fruit = SpawnNextFruit();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver)
        {
            return;
        }

        if(Input.GetMouseButtonUp(0))
        {
            var mousePos = Input.mousePosition;
            var wolrdPos = Camera.main.ScreenToWorldPoint(mousePos);
            var fruitPos = new Vector3(wolrdPos.x, spawnPoint.position.y, 0);
            fruit.gameObject.transform.position = fruitPos;
            fruit.SetSimulated(true);

            fruit = SpawnNextFruit();
        }
    }

    private Fruit SpawnNextFruit()
    {
        var rand = Random.Range(0, fruitPrefabList.Count);
        var prefab = fruitPrefabList[rand].gameObject;
        var pos = spawnPoint.position;

        return SpawnFruit(prefab, pos);
    }

    private Fruit SpawnFruit(GameObject prefab, Vector3 pos)
    {
        var obj = Instantiate(prefab, pos, Quaternion.identity);
        var f = obj.GetComponent<Fruit>();
        f.SetSimulated(false);
        f.id = fruidId++;

        f.OnLevelUp = (a, b) =>
        {
            if(IsFruitExist(a) && IsFruitExist(b))
            {
                var pos1 = a.gameObject.transform.position;
                var pos2 = b.gameObject.transform.position;
                var pos = (pos1 + pos2) * 0.5f;
                RemoveFruit(a);
                RemoveFruit(b);
                AddScore(a.score);
                var fr = SpawnFruit(a.nextLevelPrefab, pos);
                fr.SetSimulated(true);
            }
        };

        f.OnGameOver = () =>
        {
            if (isGameOver == true)
            {
                return;
            }
            OnGameOver();
        };

        fruits.Add(f);
        return f;
    }

    private void OnGameOver()
    {
        isGameOver = true;
        playButton.gameObject.SetActive(true);

        for (int i = 0; i < fruits.Count; i++)
        {
            fruits[i].SetSimulated(false);
            AddScore(fruits[i].score);
            Destroy(fruits[i].gameObject);
        }

        fruits.Clear();
    }

    public void Restart()
    {
        playButton.gameObject.SetActive(false);
        fruit = SpawnNextFruit();

        score = 0;
        scoreLabel.text = "0";

        isGameOver = false;
    }

    private void RemoveFruit(Fruit f)
    {
        for (int i = 0; i < fruits.Count; i++)
        {
            if (fruits[i].id == f.id)
            {
                fruits.Remove(f);
                Destroy(f.gameObject);
                return;
            }
        }
    }

    private bool IsFruitExist(Fruit f)
    {
        for (int i = 0; i < fruits.Count; i++)
        {
            if(fruits[i].id == f.id)
            {
                return true;
            }
        }
        return false;
    }

    private void AddScore(int score)
    {
        this.score += score;
        scoreLabel.text = $"{this.score}";
    }
}
