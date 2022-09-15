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

    MeshRenderer render;    //�� �ڽ��� ������
    Material materialOrigin; //  �� �ڽ��� ���� material 
    public Material materialPine;       //Ÿ��1�� �浹������ center (���ڽ�)�� ���ϴ� ����
    public Material materialBlue;       //Ÿ��2�� �浹������ center(���ڽ�) �� ���ϴ� ����

    public GameObject cocktailPrefab;
    bool finalChange;

    //GameObject fluids;
    GameObject whiskeyGlass;
    GameObject rum;

    Animator  target1Animator;
    Animator  target2Animator;
    Animator rumAnimator;

    float currentTime = 0;

    //�Ҹ��ֱ�
    AudioSource audioSource;
    public AudioClip target1ChangeSound;
    public AudioClip target2ChangeSound;
    public AudioClip finalChangeSound;

    public ParticleSystem finalparticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
        materialOrigin = render.material;             //�������� �����������ؼ�  material �� �������� 
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

        if (render.sharedMaterial == materialOrigin)      // ���ڽ��� �ٸ���ü�� ��ġ�� �� ����-> ������ �̵� + change1 ����
        {
            if (distance1 < detectRange)    //Ÿ��1�� �ٰ����� ��Ȳ
            {
                Debug.Log("Ÿ��1 ������ �̵� �ؾ���");
                coconutMilk.transform.position = Vector3.Lerp(coconutMilk.transform.position, transform.position, moveSpeed * Time.deltaTime);

            }
            if (distance1 < changeRange)
            {
                currentTime += Time.deltaTime;
                //Ÿ��1�� �ִϸ��̼� ���� 
                audioSource.clip = target1ChangeSound;
                audioSource.Stop();
                audioSource.Play();

                target1Animator.SetTrigger("Milk change");
                //Ÿ��1�� �ִϸ��̼� ��ν����Ѵ��� ������ ��Ȱ��ȭ �ϱ� 
                float playTime = target1Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

                Debug.Log("�÷���Ÿ��"+playTime);

                if (currentTime >= playTime*0.95f)
                {
                    coconutMilk.GetComponentInChildren<MeshRenderer>().enabled = false;
                    coconutMilk.SetActive(false);

                    rumAnimator.SetTrigger("Rum");
                    //float rumPlayTime = .GetCurrentAnimatorClipInfo(0)[0].clip.length;

                    //Debug.Log(rumPlayTime);

                    //if (currentTime >= 1.4f)
                    //{
                        //change1 ���� -> Ÿ��1�� ��Ȱ��ȭ /���� ���� ��ȭ
                        //render.sharedMaterial = materialPine;
                        GetComponent<MeshRenderer>().sharedMaterial = materialPine;
                        //���� �ڽ��� Fluid�� �����ؼ� ���� �ٲٱ�
                        Debug.Log("Ÿ��1���� ���� ��ȭ ");

                        currentTime = 0;
                    //}
                }
            }

            if (distance2 < detectRange) //Ÿ��2�� �ٰ����� ��Ȳ
            {
                Debug.Log(" ��� ������ �̵� �ؾ���");
                blueCuracao.transform.position = Vector3.Lerp(blueCuracao.transform.position, transform.position, moveSpeed * Time.deltaTime);
            }
            if (distance2 < changeRange)
            {
                //Ÿ��2�� �ִϸ��̼� ���� 
                target2Animator.SetTrigger("blue change");
                audioSource.clip = target2ChangeSound;
                audioSource.Stop();
                audioSource.Play();

                currentTime += Time.deltaTime;
                //Ÿ��2�� �ִϸ��̼� ��ν����Ѵ��� ������ ��Ȱ��ȭ �ϱ� 
                float playTime = target1Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

                Debug.Log(" �ҷ�ִϸ��̼�" + playTime);
                 //change1 ���� -> Ÿ��2�� ��Ȱ��ȭ /���� ���� ��ȭ
                if (currentTime >= playTime * 0.9f)
                {
                    blueCuracao.GetComponentInChildren<MeshRenderer>().enabled = false;
                    blueCuracao.SetActive(false);

                    rumAnimator.SetTrigger("Rum");

                    if (currentTime >= 1.4f)
                    {
                       
                        //render.sharedMaterial = materialBlue;
                        GetComponent<MeshRenderer>().sharedMaterial = materialBlue;
                        Debug.Log("Ÿ��2���� ���� ��ȭ ");
                        currentTime = 0;
                    }
                }
            }
        }
      

        //=================================FinalChange==========================================================
        //�� �ڽ��� ���׸����� �������� �ƴҶ� && finalCocktail Ʈ���϶�
        //bool�� ������ ���� �տ��� �޾��ֱ�-> ������ �����ؼ� ����ȭ ! 
        //if (finalCocktail && render.material != materialOrigin)

        if (finalChange && render.sharedMaterial == materialPine) //->���� target1���� ����ȭ�� �ǹ� 
        {
            Debug.Log("���� TAGET1���� ����ȭ ");

            if (distance2 <= detectRange)
            {
                Debug.Log("�״������� Target 2�� ���� ������ ����");
                blueCuracao.transform.position = Vector3.Lerp(blueCuracao.transform.position, transform.position, moveSpeed * Time.deltaTime);
            }
            if (distance2 < changeRange)
            {
                finalChange = false;
               
                //Ÿ��2�� �ִϸ��̼� ���� 
                target2Animator.SetTrigger("blue change");
                //Ÿ��1�� �ִϸ��̼� ��ν����Ѵ��� ������ ��Ȱ��ȭ �ϱ� 
                float playTime = target2Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;


                audioSource.clip = finalChangeSound;
                audioSource.Stop();
                audioSource.Play();

                Invoke("FinalChange", playTime*0.8f);

              
                //���ڽ��� �ٸ� ��ü������ ������ ���� ���� (change1�� ����� ����)
                //target2�� ������ �̵� + change final ���� -> ���ڽ��� �������� ��Ȱ��ȭ , ����Ĭ������ �ѹ��� �����ϱ� 
            }

        }


        if (finalChange && render.sharedMaterial == materialBlue) //->���� target2���� ����ȭ�� �ǹ� 
        {
            Debug.Log("���� TAGET2���� ����ȭ ");
            if (distance1 <= detectRange)
            {
                Debug.Log("�״������� Target 1�� ���� ������ ����");
                coconutMilk.transform.position = Vector3.Lerp(coconutMilk.transform.position, transform.position, moveSpeed * Time.deltaTime);
            }
            if (distance1 < changeRange)
            {
                finalChange = false;
              
                //�ִϸ��̼��� ��� ���� ������ �������� ��Ȱ��ȭ 
                //Ÿ��1�� �ִϸ��̼� ���� 
                target1Animator.SetTrigger("Milk change");
                //Ÿ��1�� �ִϸ��̼� ��ν����Ѵ��� ������ ��Ȱ��ȭ �ϱ� 
                float playTime = target1Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

                audioSource.clip = finalChangeSound;
                audioSource.Stop();
                audioSource.Play();

                Invoke("FinalChange", playTime * 0.8f);

              
                //���ڽ��� �ٸ� ��ü������ ������ ���� ���� (change1�� ����� ����)
                //-> ������ �̵� + change final ���� -> ���ڽ��� �������� ��Ȱ��ȭ , ����Ĭ������ �ѹ��� �����ϱ� 
            }
        }

    }
   
    public void FinalChange()
    {
        //coconutMilk.GetComponentInChildren<MeshRenderer>().enabled = false;
        //blueCuracao.GetComponentInChildren<MeshRenderer>().enabled = false;

        coconutMilk.SetActive(false);
        blueCuracao.SetActive(false);

        Debug.Log(" ���� ��Ȱ��ȭ");
        whiskeyGlass.GetComponent<MeshRenderer>().enabled = false;
        rum.GetComponent<MeshRenderer>().enabled = false;
        render.enabled = false;
        //����Ĭ���� ����/��ġ����

        //audioSource.clip = finalChangeSound;
        //audioSource.Stop();
        //audioSource.Play();


        //������Ʈâ���ִ� prefab�� �ٷ� ����ϴ°�=hiearachy�� �÷��� �� �ƴϸ� ��ü(��ü) ������ ����
        //hiearachy�� �÷��� �� �ƴϸ� ��ü(��ü) ������ ���� ****�׷��� ����-> �÷��� 
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
