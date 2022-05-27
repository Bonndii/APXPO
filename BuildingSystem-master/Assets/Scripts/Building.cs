using UnityEngine;

public class Building : MonoBehaviour
{
    public Renderer MainRenderer;
    public Vector2Int Size = Vector2Int.one;

    public float GoldPrice = 1000f;
    public float WoodPrice = 0f;
    public float IronPrice = 0f;

    public int Level = 1;

    public AnimationCurve GoldUpgradePrice;
    public AnimationCurve WoodUpgradePrice;
    public AnimationCurve IronUpgradePrice;

    public float GoldGain = 5f;
    public float WoodGain = 4f;
    public float IronGain = 4f;
    public float ArmyPower = 200f;

    public enum EType
    {
        TownHall,
        Sawmill,
        Forge,
        Tower
    }

    public EType Type;

    public void SetTransparent(bool available)
    {
        if (available)
        {
            MainRenderer.material.color = Color.green;
        }
        else
        {
            MainRenderer.material.color = Color.red;
        }
    }

    public void SetNormal()
    {
        MainRenderer.material.color = Color.white;
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if ((x + y) % 2 == 0) Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
                else Gizmos.color = new Color(1f, 0.68f, 0f, 0.3f);

                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
            }
        }
    }
}