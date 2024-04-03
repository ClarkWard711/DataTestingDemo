using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float time = 2;
    Vector3 speed = new Vector3(7, 0, 0);
    public int factor = 1;
    // 开始
    void Start()
    {
        StartCoroutine(delay());
    }
    IEnumerator delay()
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            factor *= -1;
        }

    }
    // 更新
    void Update()
    {
        transform.position += (speed * factor) * Time.deltaTime;
    }
}
