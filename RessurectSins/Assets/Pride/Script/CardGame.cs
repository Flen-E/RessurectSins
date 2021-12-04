using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Text.RegularExpressions;

public class CardGame : MonoBehaviour
{

    public GameObject[] easyCard = new GameObject[8];
    public GameObject[] normalCard = new GameObject[12];
    public GameObject[] hardCard = new GameObject[16];
    public GameObject[] veryhardCard = new GameObject[20];
    public GameObject[] hellCard = new GameObject[24];


    //Ŭ���� ī���ȣ 
    string cardNum; 

    //������ ī�� ��ȣ
    string lastNum = "0";

    //���������� ��ü ī���
    int cardCnt; //8,12,16,20,24

    //ī�� Ŭ�� Ƚ��
    int hitCnt = 0;

    //�������� ��ȣ
    int stageNum = 1;

    //�������� ��
    int stateCnt = 5; //2*4,3*4,4*4,5*4,6*4�� 5�ܰ踦 �������

    //���� ���� �ð�
    float startTime;

    // �������� ��� �ð�
    float stageTime;

    char separatorChar = '(';

    //���¸� ���������� ����
    public enum STATE 
    {
        START,HIT,WAIT,IDLE,CLEAR
    }
    static public STATE state = STATE.START;

    
    void Start()
    {
        //�ð� �ʱ�ȭ
        startTime = stageTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        //���ʸ��콺 ��ư Ŭ��
        if (Input.GetButtonDown("Fire1") && CardGame.state == CardGame.STATE.IDLE)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPos = new Vector2(worldPos.x, worldPos.y);
            Collider2D clickColl = Physics2D.OverlapPoint(clickPos);

            if (clickColl.gameObject.tag == "Card")
            {
                clickColl.transform.DORotate(new Vector3(0, -180, 0), 1f); // 180�� ȸ���ؼ� ������
                cardNum = clickColl.transform.gameObject.name;
                clickColl.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                state = STATE.HIT;
            }
           
            
        }

        switch (state){
            //�������� ����
            case STATE.START:
                MakeStage();
                break;

            //���� �׸����� ����
            case STATE.HIT:
                CheckCard();
                break;

                //���������� Ŭ�����ϰ� ���� �������� ����
            case STATE.CLEAR:
                StartCoroutine(StageClear());
                break;
            
        }
    }
  

    void OpenCard()
    {
         
    }

    void CheckCard()
    {
        state = STATE.WAIT;

        //ù��° ī��
        if(lastNum == "0")
        {
            //����ī�� ����
            lastNum = cardNum;
            state = STATE.IDLE;
            return;
        }
        //�̹��� ã��
        string card1 = Regex.Replace(cardNum, @"\D", "");
        string  card2 = Regex.Replace(lastNum, @"\D", "");

        int img1 = int.Parse(card1);
        int img2 = int.Parse(card2);

        //������ Ŭ�������� ���
        //�ٸ� ī��� ī�带 �ݰ���
        if (img1 != img2 )
        {
            StartCoroutine(CloseTwoCards());
            Debug.Log(lastNum);
            Debug.Log(cardNum);
            lastNum = "0"; //�ʱ�ȭ
            
            return;
        }
        //���� ī���
        hitCnt += 2;

        //ī�尡 ��� ������ �������� Ŭ����
        if(hitCnt == cardCnt)
        {
            state = STATE.CLEAR;
            return;
        }

        //ī�尡 ������ �ٸ� ī�� ����
        lastNum = "0";
        state = STATE.IDLE;

    }

    IEnumerator CloseTwoCards()
    {
        //�̸� �˻�
        GameObject card1 = GameObject.Find(lastNum);
        GameObject card2 = GameObject.Find(cardNum);

        //Ŭ�������ϰ� ��ü
        card1.GetComponent<BoxCollider2D>().enabled = true;
        card2.GetComponent<BoxCollider2D>().enabled = true;
        //ī�� �ݱ�

        yield return new WaitForSeconds(1f);
        card1.transform.DORotate(new Vector3(0, 360, 0), 0.5f);
        card2.transform.DORotate(new Vector3(0, 360, 0), 0.5f);
        state = STATE.IDLE;

    }
    
    IEnumerator StageClear()
    {
        state = STATE.WAIT;

        yield return new WaitForSeconds(2); //�������� Ŭ����� ���� �������� ����ϴ� ���ð�

        for(int i =1; i <=cardCnt; i++)
        {
            GameObject card = GameObject.Find(i.ToString()+"(Clone)");
            GameObject card2 = GameObject.Find(i.ToString() + "a(Clone)");
            Destroy(card);
            Destroy(card2);
        }

        ++stageNum;
        stageTime = Time.time;
        lastNum = "0";
        hitCnt = 0;

        state = STATE.START;

       
    }

    void MakeStage()
    {
        state = STATE.WAIT;

        if(stageNum == 1)
        {
            easyStage();
        }else if(stageNum == 2)
        {
          normalStage();
        }else if(stageNum == 3)
        {
           hardStage();
        }
        else if (stageNum == 4)
        {
          veryhardStage();
        }
        else if (stageNum == 5)
        {
         hellStage();
        }
    }
    void ShuffleCard(int num) //���� ���� ©�� �������� ���� �����غ��� ������� ��
    {
        switch (num)
        {
            case 1:
                for (int i = 0; i < 10; i++)
                {
                    int n1 = Random.Range(0, cardCnt);
                    int n2 = Random.Range(0, cardCnt);

                    GameObject Rcard = easyCard[n1];
                    easyCard[n1] = easyCard[n2];
                    easyCard[n2] = Rcard;
                }
                break;
            case 2:
                for (int i = 0; i < 15; i++)
                {
                    int n1 = Random.Range(0, cardCnt);
                    int n2 = Random.Range(0, cardCnt);

                    GameObject Rcard = normalCard[n1];
                    normalCard[n1] = normalCard[n2];
                    normalCard[n2] = Rcard;
                }
                break;
            case 3:
                for (int i = 0; i < 25; i++)
                {
                    int n1 = Random.Range(0, cardCnt);
                    int n2 = Random.Range(0, cardCnt);

                    GameObject Rcard = hardCard[n1];
                    hardCard[n1] = hardCard[n2];
                    hardCard[n2] = Rcard;
                }
                break;
            case 4:
                for (int i = 0; i < 30; i++)
                {
                    int n1 = Random.Range(0, cardCnt);
                    int n2 = Random.Range(0, cardCnt);

                    GameObject Rcard = veryhardCard[n1];
                    veryhardCard[n1] = veryhardCard[n2];
                    veryhardCard[n2] = Rcard;
                }
                break;
            case 5:
                for (int i = 0; i < 35; i++)
                {
                    int n1 = Random.Range(0, cardCnt);
                    int n2 = Random.Range(0, cardCnt);

                    GameObject Rcard = hellCard[n1];
                    hellCard[n1] = hellCard[n2];
                    hellCard[n2] = Rcard;
                }
                break;
        }
        //ī�弯��
       
    }
   

    
    void easyStage()
    {
        cardCnt = 8;
        int cardnum = 0; //easyStage���� �ʱ�ȭ
        ShuffleCard(1);
        for (int i = 0; i< 4; i++)
        {
            for(int j= 0; j < 2; j++)
            {

                Instantiate(easyCard[cardnum], new Vector3(0.7f +5.1f*j, 3 - 2 * i, 0), transform.rotation);
                cardnum++;
            }
            
        }

        state = STATE.IDLE;
     
    }

    void normalStage()
    {
        cardCnt = 12;
        int cardnum = 0; 
        ShuffleCard(2);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {

                Instantiate(normalCard[cardnum], new Vector3(0.7f+2.55f*j, 3 - 2 * i, 0), transform.rotation);
                cardnum++;
            }

        }
        state = STATE.IDLE;
    }

    void hardStage()
    {
        cardCnt = 16;
        int cardnum = 0; 
        ShuffleCard(3);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                Instantiate(hardCard[cardnum], new Vector3(0.7f+j*1.7f, 3 - 2 * i, 0), transform.rotation);
                cardnum++;
            }

        }
        state = STATE.IDLE;
    }

    void veryhardStage()
    {
        cardCnt = 20;
        int cardnum = 0; 
        ShuffleCard(4);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {

                Instantiate(veryhardCard[cardnum], new Vector3(-0.15f+1.85f*j, 3 - 2 * i, 0), transform.rotation);
                cardnum++;
            }

        }
        state = STATE.IDLE;
    }

    void hellStage()
    {
        cardCnt = 24;
        int cardnum = 0;
        ShuffleCard(5);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {

                Instantiate(hellCard[cardnum], new Vector3(-1 + j*1.7f, 3 - 2 * i, 0), transform.rotation);
                cardnum++;
            }

        }
        state = STATE.IDLE;
    }

    

    

}
