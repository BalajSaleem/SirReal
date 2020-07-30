using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    // Start is called before the first frame update

    Transform playerPostion;
    Camera cam;
    public Transform bossFightPoint;
    public GameObject enemySpawner;
    public GameObject playerLight;


    Transform leftWall;
    Transform rightWall;

    Vector2 screenBounds;
    void Start()
    {
        cam = Camera.main;
        screenBounds = cam.ScreenToViewportPoint(new Vector2(Screen.width, Screen.height));
        playerPostion = GameObject.FindGameObjectWithTag("Player").transform;

        leftWall = cam.transform.Find("leftWall").gameObject.transform;
        rightWall = cam.transform.Find("rightWall").gameObject.transform;

        leftWall.position = cam.ViewportToWorldPoint(new Vector3(0,1,0));
        rightWall.position = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));



    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(playerPostion.position, bossFightPoint.position) <= 7f)
        {
            cam.GetComponent<CameraMove>()._target = bossFightPoint;
            leftWall.gameObject.SetActive(true);
            rightWall.gameObject.SetActive(true);
            playerPostion.Find("EnemySpawner").gameObject.SetActive(false);
            enemySpawner.SetActive(false);
            playerLight.SetActive(false);

        }

    }
}
