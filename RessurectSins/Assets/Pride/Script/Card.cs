using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Card : MonoBehaviour
{
    public SpriteRenderer cardImage;
    public Sprite frontSprite;
    public Sprite backSprite;

    private void Start()
    {

        cardImage = gameObject.GetComponent<SpriteRenderer>();


    }

    private void Update()
    {


        if (gameObject.transform.rotation.y >= 360)
        {

            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        CardImageChange();
    }

    public void CardImageChange()
    {
        if (90 <= gameObject.transform.eulerAngles.y && 270 > gameObject.transform.eulerAngles.y) //90������ ���ų� Ŀ���� 270�� ���� ������ �̹��� �޸����� ��ü
        {

            cardImage.sprite = frontSprite;

        }
        if (270 <= gameObject.transform.eulerAngles.y || 0>= gameObject.transform.eulerAngles.y) //270�� �Ѿ��
        {
           
            cardImage.sprite = backSprite;
        }
    }


}
