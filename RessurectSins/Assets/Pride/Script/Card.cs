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
        if (90 <= gameObject.transform.eulerAngles.y && 270 > gameObject.transform.eulerAngles.y) //90도보다 같거나 커지고 270도 보다 작을때 이미지 뒷면으로 교체
        {

            cardImage.sprite = frontSprite;

        }
        if (270 <= gameObject.transform.eulerAngles.y || 0>= gameObject.transform.eulerAngles.y) //270도 넘어갈때
        {
           
            cardImage.sprite = backSprite;
        }
    }


}
