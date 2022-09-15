using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterColor : MonoBehaviour
{
    public float detectRange = 5f;
    public float changeRange = 2f;

    public GameObject coconutMilk;
    public GameObject blueCuracao;
    public float moveSpeed = 2f;

    MeshRenderer render;    //나 자신의 렌더러
    Material materialOrigin; //  나 자신의 원래 material 
    public Material materialPine;       //타겟1와 충돌했을때 center (나자신)가 변하는 색상
    public Material materialBlue;       //타겟2와 충돌했을때 center(나자신) 가 변하는 색상

    public GameObject cocktailPrefab;
    bool finalChange;

    //GameObject fluids;
    GameObject whiskeyGlass;
    GameObject rum;

    Animator  target1Animator;
    Animator  target2Animator;
    Animator rumAnimator;

    float currentTime = 0;

    //소리넣기
    AudioSource audioSource;
    public AudioClip target1ChangeSound;
    public AudioClip target2ChangeSound;
    public AudioClip finalChangeSound;

    public ParticleSystem finalparticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
        materialOrigin = render.material;             //렌더러에 먼저접근을해서  material 을 가져오기 
        audioSource=GetComponent<AudioSource>();

        //fluids = GameObject.Find("Fluid-CenterScript");
        whiskeyGlass = GameObject.Find("WhiskeyGlass");
        rum = GameObject.Find("Rum");

        finalChange = true;
        target1Animator=coconutMilk.GetComponent<Animator>();
        target2Animator=blueCuracao.GetComponent<Animator>();
        rumAnimator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance1 = Vector3.Distance(transform.position, coconutMilk.transform.position);
        float distance2 = Vector3.Distance(transform.position, blueCuracao.transform.position);

        if (render.sharedMaterial == materialOrigin)      // 나자신이 다른물체와 합치기 전 상태-> 가까이 이동 + change1 실행
        {
            if (distance1 < detectRange)    //타겟1이 다가오는 상황
            {
                Debug.Log("타겟1 러프로 이동 해야함");
                coconutMilk.transform.position = Vector3.Lerp(coconutMilk.transform.position, transform.position, moveSpeed * Time.deltaTime);

            }
            if (distance1 < changeRange)
            {
                currentTime += Time.deltaTime;
                //타겟1의 애니메이션 실행 
                audioSource.clip = target1ChangeSound;
                audioSource.Stop();
                audioSource.Play();

                target1Animator.SetTrigger("Milk change");
                //타겟1의 애니메이션 모두실행한다음 렌더러 비활성화 하기 
                float playTime = target1Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

                Debug.Log("플레이타임"+playTime);

                if (currentTime >= playTime*0.95f)
                {
                    coconutMilk.GetComponentInChildren<MeshRenderer>().enabled = false;
                    coconutMilk.SetActive(false);

                    rumAnimator.SetTrigger("Rum");
                    //float rumPlayTime = .GetCurrentAnimatorClipInfo(0)[0].clip.length;

                    //Debug.Log(rumPlayTime);

                    //if (currentTime >= 1.4f)
                    //{
                        //change1 실행 -> 타겟1은 비활성화 /나는 색상만 변화
                        //render.sharedMaterial = materialPine;
                        GetComponent<MeshRenderer>().sharedMaterial = materialPine;
                        //나의 자식인 Fluid에 접근해서 색깔 바꾸기
                        Debug.Log("타겟1으로 색상만 변화 ");

                        currentTime = 0;
                    //}
                }
            }

            if (distance2 < detectRange) //타겟2이 다가오는 상황
            {
                Debug.Log(" 블루 러프로 이동 해야함");
                blueCuracao.transform.position = Vector3.Lerp(blueCuracao.transform.position, transform.position, moveSpeed * Time.deltaTime);
            }
            if (distance2 < changeRange)
            {
                //타겟2의 애니메이션 실행 
                target2Animator.SetTrigger("blue change");
                audioSource.clip = target2ChangeSound;
                audioSource.Stop();
                audioSource.Play();

                currentTime += Time.deltaTime;
                //타겟2의 애니메이션 모두실행한다음 렌더러 비활성화 하기 
                float playTime = target1Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

                Debug.Log(" 불루애니메이션" + playTime);
                 //change1 실행 -> 타겟2은 비활성화 /나는 색상만 변화
                if (currentTime >= playTime * 0.9f)
                {
                    blueCuracao.GetComponentInChildren<MeshRenderer>().enabled = false;
                    blueCuracao.SetActive(false);

                    rumAnimator.SetTrigger("Rum");

                    if (currentTime >= 1.4f)
                    {
                       
                        //render.sharedMaterial = materialBlue;
                        GetComponent<MeshRenderer>().sharedMaterial = materialBlue;
                        Debug.Log("타겟2으로 색상만 변화 ");
                        currentTime = 0;
                    }
                }
            }
        }
      

        //=================================FinalChange==========================================================
        //나 자신의 메테리얼이 오리진이 아닐때 && finalCocktail 트루일때
        //bool값 조건을 먼저 앞에다 달아주기-> 판정이 간단해서 최적화 ! 
        //if (finalCocktail && render.material != materialOrigin)

        if (finalChange && render.sharedMaterial == materialPine) //->먼저 target1으로 색상변화를 의미 
        {
            Debug.Log("먼저 TAGET1으로 색상변화 ");

            if (distance2 <= detectRange)
            {
                Debug.Log("그다음으로 Target 2이 범위 안으로 들어옴");
                blueCuracao.transform.position = Vector3.Lerp(blueCuracao.transform.position, transform.position, moveSpeed * Time.deltaTime);
            }
            if (distance2 < changeRange)
            {
                finalChange = false;
               
                //타겟2의 애니메이션 실행 
                target2Animator.SetTrigger("blue change");
                //타겟1의 애니메이션 모두실행한다음 렌더러 비활성화 하기 
                float playTime = target2Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;


                audioSource.clip = finalChangeSound;
                audioSource.Stop();
                audioSource.Play();

                Invoke("FinalChange", playTime*0.8f);

              
                //나자신이 다른 물체때문에 색깔이 변한 상태 (change1이 실행된 상태)
                //target2가 가까이 이동 + change final 실행 -> 나자신의 렌더러만 비활성화 , 최종칵테일을 한번만 생성하기 
            }

        }


        if (finalChange && render.sharedMaterial == materialBlue) //->먼저 target2으로 색상변화를 의미 
        {
            Debug.Log("먼저 TAGET2으로 색상변화 ");
            if (distance1 <= detectRange)
            {
                Debug.Log("그다음으로 Target 1이 범위 안으로 들어옴");
                coconutMilk.transform.position = Vector3.Lerp(coconutMilk.transform.position, transform.position, moveSpeed * Time.deltaTime);
            }
            if (distance1 < changeRange)
            {
                finalChange = false;
              
                //애니메이션이 모두 끝난 다음에 렌더러를 비활성화 
                //타겟1의 애니메이션 실행 
                target1Animator.SetTrigger("Milk change");
                //타겟1의 애니메이션 모두실행한다음 렌더러 비활성화 하기 
                float playTime = target1Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

                audioSource.clip = finalChangeSound;
                audioSource.Stop();
                audioSource.Play();

                Invoke("FinalChange", playTime * 0.8f);

              
                //나자신이 다른 물체때문에 색깔이 변한 상태 (change1이 실행된 상태)
                //-> 가까이 이동 + change final 실행 -> 나자신의 렌더러만 비활성화 , 최종칵테일을 한번만 생성하기 
            }
        }

    }
   
    public void FinalChange()
    {
        //coconutMilk.GetComponentInChildren<MeshRenderer>().enabled = false;
        //blueCuracao.GetComponentInChildren<MeshRenderer>().enabled = false;

        coconutMilk.SetActive(false);
        blueCuracao.SetActive(false);

        Debug.Log(" 렌더 비활성화");
        whiskeyGlass.GetComponent<MeshRenderer>().enabled = false;
        rum.GetComponent<MeshRenderer>().enabled = false;
        render.enabled = false;
        //최종칵테일 생성/위치지정

        //audioSource.clip = finalChangeSound;
        //audioSource.Stop();
        //audioSource.Play();


        //프로젝트창에있는 prefab을 바로 사용하는것=hiearachy에 올려진 게 아니면 실체(객체) 가없는 상태
        //hiearachy에 올려진 게 아니면 실체(객체) 가없는 상태 ****그래서 생성-> 플레이 
        ParticleSystem finalparticleSystemEffect = Instantiate(finalparticleSystem);
        finalparticleSystemEffect.transform.position=transform.position;
        finalparticleSystemEffect.Play();


        GameObject cocktail = Instantiate(cocktailPrefab);
        cocktail.transform.position = transform.position;

        gameObject.SetActive(false);
        whiskeyGlass.SetActive(false);
        rum.SetActive(false);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, changeRange);
    }
}
