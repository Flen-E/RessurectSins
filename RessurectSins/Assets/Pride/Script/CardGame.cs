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


    //클릭한 카드번호 
    string cardNum; 

    //직전의 카드 번호
    string lastNum = "0";

    //스테이지의 전체 카드수
    int cardCnt; //8,12,16,20,24

    //카드 클릭 횟수
    int hitCnt = 0;

    //스테이지 번호
    int stageNum = 1;

    //스테이지 수
    int stateCnt = 5; //2*4,3*4,4*4,5*4,6*4로 5단계를 만들거임

    //게임 시작 시간
    float startTime;

    // 스테이지 경과 시간
    float stageTime;

    char separatorChar = '(';

    //상태를 열거형으로 나눔
    public enum STATE 
    {
        START,HIT,WAIT,IDLE,CLEAR
    }
    static public STATE state = STATE.START;

    
    void Start()
    {
        //시간 초기화
        startTime = stageTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        //왼쪽마우스 버튼 클릭
        if (Input.GetButtonDown("Fire1") && CardGame.state == CardGame.STATE.IDLE)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPos = new Vector2(worldPos.x, worldPos.y);
            Collider2D clickColl = Physics2D.OverlapPoint(clickPos);

            if (clickColl.gameObject.tag == "Card")
            {
                clickColl.transform.DORotate(new Vector3(0, -180, 0), 1f); // 180도 회전해서 오픈함
                cardNum = clickColl.transform.gameObject.name;
                clickColl.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                state = STATE.HIT;
            }
           
            
        }

        switch (state){
            //스테이지 생성
            case STATE.START:
                MakeStage();
                break;

            //같은 그림인지 판정
            case STATE.HIT:
                CheckCard();
                break;

                //스테이지를 클리어하고 다음 스테이지 만듦
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

        //첫번째 카드
        if(lastNum == "0")
        {
            //현재카드 보존
            lastNum = cardNum;
            state = STATE.IDLE;
            return;
        }
        //이미지 찾기
        string card1 = Regex.Replace(cardNum, @"\D", "");
        string  card2 = Regex.Replace(lastNum, @"\D", "");

        int img1 = int.Parse(card1);
        int img2 = int.Parse(card2);

        //같은걸 클릭했을때 대비
        //다른 카드면 카드를 닫게함
        if (img1 != img2 )
        {
            StartCoroutine(CloseTwoCards());
            Debug.Log(lastNum);
            Debug.Log(cardNum);
            lastNum = "0"; //초기화
            
            return;
        }
        //같은 카드면
        hitCnt += 2;

        //카드가 모두 열리면 스테이지 클리어
        if(hitCnt == cardCnt)
        {
            state = STATE.CLEAR;
            return;
        }

        //카드가 남으면 다른 카드 조사
        lastNum = "0";
        state = STATE.IDLE;

    }

    IEnumerator CloseTwoCards()
    {
        //이름 검사
        GameObject card1 = GameObject.Find(lastNum);
        GameObject card2 = GameObject.Find(cardNum);

        //클릭가능하게 교체
        card1.GetComponent<BoxCollider2D>().enabled = true;
        card2.GetComponent<BoxCollider2D>().enabled = true;
        //카드 닫기

        yield return new WaitForSeconds(1f);
        card1.transform.DORotate(new Vector3(0, 360, 0), 0.5f);
        card2.transform.DORotate(new Vector3(0, 360, 0), 0.5f);
        state = STATE.IDLE;

    }
    
    IEnumerator StageClear()
    {
        state = STATE.WAIT;

        yield return new WaitForSeconds(2); //스테이지 클리어시 다음 스테이지 출력하는 대기시간

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
    void ShuffleCard(int num) //좀더 쉽게 짤수 있을거임 좀더 공부해보기 쓸모없이 김
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
        //카드섞기
       
    }
   

    
    void easyStage()
    {
        cardCnt = 8;
        int cardnum = 0; //easyStage마다 초기화
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
