using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VirtualGameObject
{
    //Transform
    public Vector3 mPosition;
    public Quaternion mRotation;
    public Vector3 mScale;
    public float mWidth;
    public float mHeight;

    public Sprite mImage;

    public VirtualGameObject(Vector3 position, Quaternion rotation, float Width, float Height, Sprite image)
    {
        mPosition = position;
        mRotation = rotation;
        mWidth = Width;
        mHeight = Height;
        mImage = image;
    }
}

//구름 하나를 구성하는 오브젝트
public class VirtualObjectManager : MonoBehaviour
{
    public float PartsRateX = 1.51f;
    public float PartsRateY = 0.71f;
    public float ObjectScale = 0.3f;

    public void updatePartsContertRate(GameObject obejct, StoragedCloudData stock)
    {
        RectTransform rectTran = obejct.GetComponent<RectTransform>();

        for (int i = 0; i < obejct.transform.childCount; i++)
        {
            GameObject obejctP;
            obejctP = obejct.transform.GetChild(i).gameObject;

            float newX = rectTran.localPosition.x * PartsRateX / rectTran.rect.width;
            float newY = rectTran.localPosition.y * PartsRateY / rectTran.rect.height;

            obejctP.transform.localPosition = new Vector3(newX, newY, 1.0f);

            // TODO: 파츠의 크기에 따라 LocalScale을 변경해준다.
            obejctP.transform.localScale = new Vector3(ObjectScale, ObjectScale, 0.12f);

            obejctP.GetComponent<SpriteRenderer>().size =
                 new Vector2(obejctP.GetComponent<SpriteRenderer>().size.x * 0.9f, obejctP.GetComponent<SpriteRenderer>().size.y * 0.9f);
        }
    }
    public GameObject OBPrefab; //게임오브젝트 안에 버튼과 이미지 컴포넌트가 있는 Prefab
    public GameObject convertVirtualToGameObject(VirtualGameObject VObject)
    {
        GameObject result;

        result = Instantiate(OBPrefab, VObject.mPosition, VObject.mRotation);

        result.GetComponent<Image>().sprite = VObject.mImage;

        return result;
    }
    public GameObject convertVirtualToGameObjectToSprite(VirtualGameObject VObject)
    {
        GameObject result;

        result = Instantiate(OBPrefab, VObject.mPosition, VObject.mRotation);

        result.GetComponent<SpriteRenderer>().sprite = VObject.mImage;


        return result;
    }

    //씬에 버츄얼 오브젝트를 Instantiate 하는 함수
    public GameObject InstantiateVirtualObjectToScene(StoragedCloudData stock,Vector3 InstancePosition)
    {
        //가상 데이터 들을 게임 오브젝트로 Convert 하여 Instantiate 하는 과정.
        GameObject obejct;
        obejct = convertVirtualToGameObject(stock.mVBase);

        RectTransform rectTran = obejct.GetComponent<RectTransform>();
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, stock.mVBase.mHeight);
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, stock.mVBase.mWidth);


        foreach (VirtualGameObject Vpart in stock.mVPartsList)
        {
            GameObject obejctP;
            obejctP = convertVirtualToGameObject(Vpart);

            obejctP.transform.SetParent(obejct.transform);

            obejctP.transform.localPosition = obejctP.transform.position;

            // 파츠 스케일 변경
            obejctP.transform.localScale = new Vector3(0.7f, 0.7f, 0.12f);
            rectTran = obejctP.GetComponent<RectTransform>();
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Vpart.mHeight);
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Vpart.mWidth);
        }

        obejct.transform.localPosition = Vector3.zero;
        obejct.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        return obejct;
    }

    public GameObject InstantiateVirtualObjectToSceneToSprite(StoragedCloudData stock, Vector3 InstancePosition)
    {
        //가상 데이터 들을 게임 오브젝트로 Convert 하여 Instantiate 하는 과정.
        GameObject obejct;
        obejct = convertVirtualToGameObjectToSprite(stock.mVBase);

        RectTransform rectTran = obejct.GetComponent<RectTransform>();
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, stock.mVBase.mHeight);
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, stock.mVBase.mWidth);

        // 구름의 소팅 레이어를 변경
        obejct.GetComponent<SpriteRenderer>().sortingLayerName = "Cloud";

        //obejct.transform.localScale = new Vector3(176.69f, 176.69f,1.0f);

        obejct.GetComponent<Image>().enabled = false;
        obejct.GetComponent<SpriteRenderer>().enabled = true;

        foreach (VirtualGameObject Vpart in stock.mVPartsList)
        {
            GameObject obejctP;
            obejctP = convertVirtualToGameObjectToSprite(Vpart);

            obejctP.transform.SetParent(obejct.transform);

            obejctP.transform.localPosition = obejctP.transform.position;
            rectTran = obejctP.GetComponent<RectTransform>();
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Vpart.mHeight);
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Vpart.mWidth);

            // 파츠의 소팅 레이어를 변경
            obejctP.GetComponent<SpriteRenderer>().sortingLayerName = "Parts";
            obejctP.GetComponent<SpriteRenderer>().enabled = true;

            float newX = rectTran.localPosition.x * 1.0f / rectTran.rect.width;
            float newY = rectTran.localPosition.y * 1.0f / rectTran.rect.height;

            rectTran.localPosition = new Vector3(newX, newY, 1.0f);

            // TODO: 파츠의 크기에 따라 LocalScale을 변경해준다.
            obejctP.transform.localScale = new Vector3(0.7f, 0.7f, 0.12f);

            obejctP.GetComponent<SpriteRenderer>().size =
                 new Vector2(obejctP.GetComponent<SpriteRenderer>().size.x * 0.9f, obejctP.GetComponent<SpriteRenderer>().size.y * 0.9f);
        }

        obejct.transform.localPosition = InstancePosition;
        obejct.transform.localScale = new Vector3(0.11f, 0.12f, 0.12f);

        return obejct;
    }

    public void realtimeImageToSpriteConverter()
    {

    }
}
