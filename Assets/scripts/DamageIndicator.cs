using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] Text text;

    [SerializeField] float time, floatingScale;
    public static DamageIndicator Instance = null;

    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        transform.Translate(Vector2.up * floatingScale *  Time.deltaTime);

        if (time > 0.65)
        {
            Debug.Log("1");
            Destroy(gameObject);
        }
    }

    public void IndicateDamage(float damage, Vector2 pos, Color color)
    {
        DamageIndicator indicator = Instantiate(this, pos, Quaternion.identity);
        indicator.text.text = Mathf.Round(damage).ToString();
        indicator.text.color = color;
    }

}
