using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    //can be modified

    [NonSerialized] public float noteSpeed = 400f;
    [NonSerialized] public float drumTime = .3f;
    [NonSerialized] public float spawnTime = 1f;


    //cannot be modified
    [SerializeField] Transform drum;
    [SerializeField] GameObject notePrefab;
    [SerializeField] Transform canvas;

    [SerializeField] AudioClip[] noteType;

    float drumTimer = 0;
    float spawnTimer = 0;
    private AudioSource sound;
    private Vector3 _mousePos;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        foreach (var note in GameObject.FindGameObjectsWithTag("Note"))
        {
            note.transform.localPosition += Vector3.down * Time.deltaTime * noteSpeed;

            if (note.transform.localPosition.y < -800f)
                Destroy(note);
        }

        if (drumTimer > 0)
        {
            drum.GetComponent<Image>().color = Color.green;
            drumTimer -= Time.deltaTime;
        }
        else drum.GetComponent<Image>().color = Color.blue;

        if (spawnTimer <= 0)
        {
            GameObject note = Instantiate(notePrefab);
            note.transform.parent = canvas;
            note.transform.localPosition = new Vector3(UnityEngine.Random.Range(-810f,810f),700f,0f);
            note.GetComponent<Note>().tuca = noteType[UnityEngine.Random.Range(0,2)];
            spawnTimer = spawnTime + spawnTime * 0.1f * UnityEngine.Random.Range(-5, 5);
            Debug.Log("Spawn");
        }
        else spawnTimer -= Time.deltaTime;
    }

    void Inputs()
    {
        _mousePos = Input.mousePosition - new Vector3(Screen.width / 2f, 0f, 0f);
        drum.localPosition = new Vector3(_mousePos.x, -400f, 0f);
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Z))
        {
            drumTimer = drumTime;
            foreach (var note in GameObject.FindGameObjectsWithTag("Note"))
            {
                if (note.transform.localPosition.y > -450 && note.transform.localPosition.y < -350 && note.transform.localPosition.x > _mousePos.x - 150 && note.transform.localPosition.x < _mousePos.x + 150)
                {
                    sound.clip = note.GetComponent<Note>().tuca;
                    sound.Play();
                    Destroy(note);
                }
                    
            }
        }
    }
}
