using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FadeInFadeOutUIScript : MonoBehaviour
{
        [SerializeField] Image fadeImage;

    // Start is called before the first frame update
    void Start()
    {
        // フェードアウト・フェードイン後に何か処理を行いたい場合は、以下のようにSequenceを使用してチェーンさせることができます。
        var sequence = DOTween.Sequence();
        sequence.Append(fadeImage.DOFade(1f, 2f))// フェードアウト
        .AppendInterval(1f)
        .Append(fadeImage.DOFade(0f, 2f))
        .OnComplete(() => {
                  this.gameObject.SetActive(false);
        });




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
