using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEffect : MonoBehaviour
{
    public GameObject StarPrefab;
    public int poolSize = 10; // 오브젝트 풀 크기
    private GameObject[] starPool; // 오브젝트 풀 배열
    private int currentPoolIndex = 0; // 현재 사용 중인 오브젝트 인덱스
    float spawnTime;
    public float defaultTime = 0.05f;

    public GameObject CirclePrefab;
    public int pool2Size = 4;
    GameObject[] circlePool;
    int currentPool2Index = 0;

    void Start()
    {
        // 오브젝트 풀 초기화
        starPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            starPool[i] = Instantiate(StarPrefab, transform);
            starPool[i].SetActive(false);
        }
        circlePool = new GameObject[pool2Size];
        for (int i = 0; i < pool2Size; i++)
        {
            circlePool[i] = Instantiate(CirclePrefab, transform);
            circlePool[i].SetActive(false);
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            TouchCreate();
        }
        if ((Input.GetMouseButton(0) || Input.touchCount > 0) && spawnTime >= defaultTime)
        {
            StarCreate();
            spawnTime = 0;
        }
        spawnTime += Time.deltaTime;
    }

    void StarCreate()
    {
        // 오브젝트 풀에서 사용 가능한 오브젝트 찾기
        GameObject star = starPool[currentPoolIndex];
        star.SetActive(true);

        // 생성 위치 설정
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        star.transform.position = mousePos;
        star.GetComponentInChildren<StarParticle>().Start();

        // 오브젝트 인덱스 업데이트
        currentPoolIndex = (currentPoolIndex + 1) % poolSize;
    }

    void TouchCreate()
    {
        // 오브젝트 풀에서 사용 가능한 오브젝트 찾기
        GameObject star = circlePool[currentPool2Index];
        star.SetActive(true);

        // 생성 위치 설정
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        star.transform.position = mousePos;
        star.GetComponentInChildren<CircleParticle>().Start();

        // 오브젝트 인덱스 업데이트
        currentPool2Index = (currentPool2Index + 1) % pool2Size;
    }
}
