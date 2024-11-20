using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance;
	private Rigidbody2D playerRb;
	private Animator anime;
	// private Animator Anime;
	public float speed;


	bool run;


	// Start is called before the first frame update
	void Start()
	{
		GameObject.Find("Player").GetComponentInChildren<Camera>().enabled = true;
		playerRb = GetComponent<Rigidbody2D>();
		anime = GetComponent<Animator>();
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void FixedUpdate()
	{
		Movement();
	}

	void Movement()
	{
		float Hmove = Input.GetAxis("Horizontal");
		float Vmove = Input.GetAxis("Vertical");
		float direction = Input.GetAxisRaw("Horizontal");

		if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{
			run = true;
		}
		else
		{
			run = false;
		}

		playerRb.velocity = new Vector2(Hmove * speed, Vmove * speed);
		anime.SetBool("running", run);

		if (direction != 0)
		{
			transform.localScale = new Vector3(direction, 1, 1);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

	}
}
