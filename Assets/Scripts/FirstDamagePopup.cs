using TMPro;
using UnityEngine;

public class FirstDamagePopup : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float lifetime = 5f;

    public Vector3 startPos;
    public Vector3 targetPos;
    private float timer = 0;

    void Start()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);//улыбаемся в камеру
        startPos = transform.position + new Vector3(0, 2, 0);
        Vector3 endPos = new Vector3(0, 3, 0);
        targetPos = startPos + endPos;
        transform.localScale = Vector3.one;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float fraction = lifetime / 2f;
        if (timer > lifetime) Destroy(gameObject);
        else if (timer / 2 > fraction) text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));
        transform.position = Vector3.Lerp(startPos, targetPos, Mathf.Sin(timer / lifetime));
    }

    public void SetDamageText(int damage)
    {
        text.text = damage.ToString();
    }
}
