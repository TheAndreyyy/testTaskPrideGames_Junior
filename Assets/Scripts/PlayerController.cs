using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] float playerSpeed = 10;

    [Header("Targeting/Grenade Settings")]
    [SerializeField] public GameObject selectedBorder1;
    [SerializeField] public GameObject selectedBorder2;
    [SerializeField] public GameObject selectedBorder3;
    [SerializeField] public Text grenade0Text;
    [SerializeField] public Text grenade1Text;
    [SerializeField] public Text grenade2Text;
    public float grenadeCount0 = 0;
    public float grenadeCount1 = 0;
    public float grenadeCount2 = 0;
    [SerializeField] GameObject targetGraphicPrefab;
    [SerializeField] GameObject pointOfThrow;
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] float initialAngle = 45;
    public TrajectoryRenderer trajectory;
    private GameObject spawnedTargetGraphicPrefab;
    public int selectedGrenadeNumber = 0;
    public bool targetingState = false;
    [Range(3, 100)] public int countOfPointsInTrajectory = 3;
    public LineRenderer lineRenderer;

    //12.09
    //начать делать+

    //13.09
    //взрыв у гранат+                       
    //подбор гранат+
    //инвентарь (хранение гранат)+
    //отрисовка траектории полета гранаты+
    //переключение гранат+
    //спавн гранат+
    //хп у врагов

    //доп. хп и у игрока
    //»» дл€ врагов

    private void Update()
    {
        PressedKeys();
        TargetingState();
        Targeting();
        ThrowGrenadeLogic();
    }

    private void FixedUpdate()
    {
        MovementLogic();
    }

    private void MovementLogic()
    {
        if (!targetingState)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            transform.Translate(movement * playerSpeed * Time.fixedDeltaTime);
        }
    }

    private void Targeting()
    {
        if (targetingState)
        {
            Ray rayToCursor = Camera.main.ScreenPointToRay(Input.mousePosition);//указаываем цель, куда стрел€ть
            RaycastHit hit;
            if (Physics.Raycast(rayToCursor, out hit, 50))//пиу, стрел€ем, если куда-то попали
            {
                if (hit.transform.tag == "Ground")//попали в землю
                {
                    //отрисовка траектории
                    TrajectoryRenderer(pointOfThrow.transform.position, ShootLogic(pointOfThrow.transform, hit.point));
                    if (spawnedTargetGraphicPrefab == null)//спрайта на земле нет, спавним
                    {
                        spawnedTargetGraphicPrefab = Instantiate(targetGraphicPrefab, hit.point, targetGraphicPrefab.transform.rotation);
                    }
                    else
                    {
                        if (spawnedTargetGraphicPrefab.activeSelf == false)
                        {
                            spawnedTargetGraphicPrefab.gameObject.SetActive(true);
                        }
                        spawnedTargetGraphicPrefab.transform.position = hit.point;
                    }
                }
            }
            else
            {
                if (spawnedTargetGraphicPrefab != null)
                {
                    spawnedTargetGraphicPrefab.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (spawnedTargetGraphicPrefab != null)
            {
                spawnedTargetGraphicPrefab.gameObject.SetActive(false);
            }
            lineRenderer.enabled = false;
        }
    }

    public void ThrowGrenadeLogic()
    {
        Ray rayToCursor = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(rayToCursor, out hit, 60))
        {
            if (hit.transform.tag == "Ground")
            {
                if (targetingState == false && Input.GetKeyUp(KeyCode.Mouse0))
                {
                    bool canShoot = false;
                    Color32 generateColor = Color.black;
                    switch (selectedGrenadeNumber)
                    {
                        case 0:
                            if (grenadeCount0 > 0)
                            {
                                generateColor = Color.red;
                                grenadeCount0--;
                                grenade0Text.text = grenadeCount0.ToString();
                                canShoot = true;
                            }
                            break;
                        case 1:
                            if (grenadeCount1 > 0)
                            {
                                generateColor = Color.green;
                                grenadeCount1--;
                                grenade1Text.text = grenadeCount1.ToString();
                                canShoot = true;
                            }
                            break;
                        case 2:
                            if (grenadeCount2 > 0)
                            {
                                generateColor = Color.blue;
                                grenadeCount2--;
                                grenade2Text.text = grenadeCount2.ToString();
                                canShoot = true;
                            }
                            break;
                    }
                    if (canShoot)
                    {
                        GameObject thrownGrenade = Instantiate(grenadePrefab, pointOfThrow.transform.position, grenadePrefab.transform.rotation);
                        thrownGrenade.GetComponent<GrenadeScript>().colorOfThisGrenade = selectedGrenadeNumber;
                        thrownGrenade.GetComponent<Renderer>().material.color = generateColor;
                        //рандомно берем цвет из списка уже представленных цветов
                        //тут надо передать значение isThrown, иначе не сработает взрыв (в заспавненный √ќ)
                        thrownGrenade.GetComponent<Rigidbody>().velocity = ShootLogic(thrownGrenade.transform, hit.point);
                    }
                }
            }
        }
    }

    private void TargetingState()
    {
        targetingState = Input.GetKey(KeyCode.Mouse0) ? true : false;
    }

    private Vector3 ShootLogic(Transform origin, Vector3 target)
    {
        //этот кусок кода вз€л почти полностью с примеров по работе с баллистикой, но пришлось его малость переделать, так что скопипиздил не 100%
        //modified        
        Vector3 p = target;
        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;
        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(origin.transform.position.x, 0, origin.transform.position.z);
        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = origin.transform.position.y - p.y;
        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) /*+ yOffset*/));
        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        Vector3 cross = Vector3.Cross(Vector3.forward, planarTarget - planarPostion);//
        if (cross.y < 0) angleBetweenObjects = -angleBetweenObjects;//
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
        // Fire!
        return finalVelocity;//сила в виде направлени€ вектора
        //конец скопипизденного кода
    }

    private void TrajectoryRenderer(Vector3 origin, Vector3 speed)
    {
        lineRenderer.enabled = true;
        Vector3[] points = new Vector3[countOfPointsInTrajectory];
        float timeOfStep = 4f / countOfPointsInTrajectory;//полет в край экрана занимает чуть меньше 4 секунд, поэтому вз€л с запасом
                                                          //(по-хорошему можно и просчитать максимальное врем€ полета, чтобы всегда было впритык, при том и в бќльшую сторону)
        lineRenderer.positionCount = countOfPointsInTrajectory;
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * timeOfStep;
            Vector3 tempTransform = new Vector3(transform.position.x * 2, transform.position.y, transform.position.z * 2);
            points[i] = tempTransform - origin + speed * time + Physics.gravity * time * time / 2f;
            if (points[i].y < 0)
            {
                lineRenderer.positionCount = i + 1;
                break;
            }
        }
        lineRenderer.SetPositions(points);
    }

    private void PressedKeys()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && selectedGrenadeNumber > 0)
        {
            selectedGrenadeNumber--;
            ChangeSelectedGrenade();
        }
        else
        if (Input.GetKeyDown(KeyCode.RightArrow) && selectedGrenadeNumber < 2)
        {
            selectedGrenadeNumber++;
            ChangeSelectedGrenade();
        }
    }

    private void ChangeSelectedGrenade()
    {
        selectedBorder1.SetActive(false);
        selectedBorder2.SetActive(false);
        selectedBorder3.SetActive(false);
        switch (selectedGrenadeNumber)
        {
            case 0:
                selectedBorder1.SetActive(true);
                break;
            case 1:
                selectedBorder2.SetActive(true);
                break;
            case 2:
                selectedBorder3.SetActive(true);
                break;
        }
    }
}
