using UnityEngine;

public class SpriteController : MonoBehaviour
{

    private Vector2 origin = Vector2.zero;

    public Vector2 Origin
    {
        get
        {
            return origin;
        }
        set
        {
            OriginX = value.x;
            OriginY = value.y;
        }
    }

    public float OriginX
    {
        get
        {
            return origin.x;
        }
        set
        {
            origin.x = value;
            transform.position = new Vector2(
               value + (Width * 0.5f) - (Screen.width * 0.5f),
               transform.position.y
           );
        }
    }

    public float OriginY
    {
        get
        {
            return origin.y;
        }
        set
        {
            origin.y = value;
            transform.position = new Vector2(
               transform.position.x,
               -value - (Height * 0.5f) - (Screen.height * 0.5f)
           );
        }
    }

    public Vector2 Center
    {
        get
        {
            return new Vector2(CenterX, CenterY);
        }
        set
        {
            CenterX = value.x;
            CenterY = value.y;
        }
    }

    public float CenterX
    {
        get
        {
            return origin.x + (Width * 0.5f);
        }
        set
        {
            OriginX = value - (Width * 0.5f);
        }
    }

    public float CenterY
    {
        get
        {
            return origin.y + (Height * 0.5f);
        }
        set
        {
            OriginY = value - (Height * 0.5f);
        }
    }

    public float Width
    { 
        get
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var spriteWidth = spriteRenderer.bounds.size.x;
            return spriteWidth;
        }
    }

    public float Height
    {
        get
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var spriteHeight = spriteRenderer.bounds.size.y;
            return spriteHeight;
        }
    }

    public Vector2 Size
    {
        get
        {
            return new Vector2(Width, Height);
        }
    }

    public float MaxX
    {
        get
        {
            return Origin.x + Width;
        }
    }

    public float MaxY
    {
        get
        {
            return Origin.y + Height;
        }
    }

    public Sprite Sprite
    {
        get
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            return spriteRenderer.sprite;
        }
        set
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }
            spriteRenderer.sprite = value;
        }
    }

    public void Translate(Vector2 diff)
    {
        Origin = new Vector2(Origin.x + diff.x, Origin.y + diff.y);
    }

    public void Scale(Vector2 size)
    {
        transform.localScale = new Vector2(
            size.x / Width / transform.localScale.x,
            size.y / Height / transform.localScale.y
            );
        Origin = Origin;
    }

    public void ScaleAspectFit(float size)
    {
        var scale = size / Mathf.Max(Width / transform.localScale.x, Height / transform.localScale.y);
        transform.localScale = new Vector2(scale, scale);
        Origin = Origin;
    }

    public void ScaleAspectFill(float size)
    {
        var scale = size / Mathf.Min(Width * transform.localScale.x, Height * transform.localScale.y);
        transform.localScale = new Vector2(scale, scale);
        Origin = Origin;
    }

}
