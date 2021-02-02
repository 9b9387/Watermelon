using System;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int id;
    public int score;
    public GameObject nextLevelPrefab;
    public Action<Fruit, Fruit> OnLevelUp;
    public Action OnGameOver;
    private Rigidbody2D rigid;
    /// <summary>
    /// 是否碰到红线
    /// </summary>
    private bool isTouchRedline;
    /// <summary>
    /// 和红线接触的时间
    /// </summary>
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if(isTouchRedline == false)
        {
            return;
        }
        timer += Time.deltaTime;
        if(timer > 3)
        {
            Debug.Log("Game Over");
            OnGameOver?.Invoke();
        }
    }
    public void SetSimulated(bool b)
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        rigid.simulated = b;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject;
        var fruit = obj.GetComponent<Fruit>();
        if (obj.CompareTag("Fruit"))
        {
            if(obj.name == gameObject.name)
            {
                if(nextLevelPrefab != null)
                {
                    OnLevelUp?.Invoke(this, fruit);  
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject;
        if (obj.CompareTag("Redline"))
        {
            Debug.Log("OnTriggerEnter2D Redline");
            isTouchRedline = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var obj = collision.gameObject;
        if (obj.CompareTag("Redline"))
        {
            Debug.Log("OnTriggerExit2D Redline");
            isTouchRedline = false;
            timer = 0;
        }
    }
}
