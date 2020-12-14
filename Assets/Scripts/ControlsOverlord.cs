using UnityEngine;

public class ControlsOverlord : MonoBehaviour
{
    public static ControlsOverlord Instance;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);
    }

    [SerializeField]
    private bool moveLeft = false;
    [SerializeField]
    private bool moveRight = false;
    [SerializeField]
    private bool shoot = false;

    public bool MoveLeft
    {
        get { return moveLeft; }
        set { moveLeft = value; }
    }

    public bool MoveRight
    {
        get { return moveRight; }
        set { moveRight = value; }
    }

    public bool Shoot
    {
        get { return shoot; }
        set { shoot = value; }
    }

    public void SetInputs(bool left, bool right, bool fire)
    {
        // reset inputs
        moveLeft = left == true || moveLeft;
        moveRight = right == true || moveRight;
        shoot = fire == true || shoot;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        moveLeft = false;
        moveRight = false;
        shoot = false;
        // keyboard for testing
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveLeft = true;
            moveRight = false;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveLeft = false;
            moveRight = true;
        }
        else
        {
            moveLeft = false;
            moveRight = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            shoot = true;
        }
#endif


    }
}
