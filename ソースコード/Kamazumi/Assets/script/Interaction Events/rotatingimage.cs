using DG.Tweening;
using UnityEngine;

public class rotatingimage : InteractionEvents
{
    public bool isTouched;
    int angle;
    public int password = 10;
    public int rotationValue = 10;
    int maxAngle = 350;

    // Start is called before the first frame update
    void Start()
    {
        isTouched = false;
        angle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(angle == password)
        {
            isTouched = true;
        }
        else
        {
            isTouched = false;
        }

    }

    public override void interactionEvents()
    {
        transform.DOBlendableRotateBy(new Vector3(0, rotationValue, 0), 1f);
        angle += rotationValue;
        if (angle > maxAngle)
        {
            angle = 0;
        }
    }

    }
