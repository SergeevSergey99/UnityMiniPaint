using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawManager : MonoBehaviour
{
    public Image drawImage;
    public Color currentColor = Color.black;
    public float brushSize = 5f;
    public enum DrawMode { Pixel, Circle }
    public DrawMode drawMode = DrawMode.Pixel;
    
    Color32[] drawImagePixels;
    Camera cam;
    
    private void Start()
    {
        cam = Camera.main;
        Texture2D texture = new Texture2D((int) drawImage.rectTransform.rect.width, (int) drawImage.rectTransform.rect.height);
        for (int x = 0; x < texture.width; x++) {
            for (int y = 0; y < texture.height; y++) {
                texture.SetPixel(x, y, Color.white);
            }
        }
        texture.Apply();
        drawImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        drawImagePixels = drawImage.sprite.texture.GetPixels32();
    }

    public void SetColorRed()
    {
        currentColor = Color.red;
    }
    public void SetColorGreen()
    {
        currentColor = Color.green;
    }
    public void SetColorBlue()
    {
        currentColor = Color.blue;
    }
    public void SetColorBlack()
    {
        currentColor = Color.black;
    }
    public void SetColorWhite()
    {
        currentColor = Color.white;
    }
    public void SetColorYellow()
    {
        currentColor = Color.yellow;
    }
    public void SetDrawModePixel()
    {
        drawMode = DrawMode.Pixel;
    }
    public void SetDrawModeCircle()
    {
        drawMode = DrawMode.Circle;
    }
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 position = new Vector2(mousePosition.x, mousePosition.y);
            if (drawMode == DrawMode.Pixel)
                DrawPixel(position);
            else if (drawMode == DrawMode.Circle)
                DrawCircle(position);
            Apply();
        }
    }
    public void DrawPixel(Vector2 mousePosition)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(drawImage.rectTransform, mousePosition, cam, out localPoint))
        {
            int x = Mathf.RoundToInt(localPoint.x + drawImage.sprite.texture.width / 2f);
            int y = Mathf.RoundToInt(localPoint.y + drawImage.sprite.texture.height / 2f);
            int index = y * drawImage.sprite.texture.width + x;
            if (index >= 0 && index < drawImagePixels.Length)
            {
                drawImagePixels[index] = currentColor;
            }
        }
    }
    public void DrawCircle(Vector2 mousePosition)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(drawImage.rectTransform, mousePosition, cam, out localPoint))
        {
            for (int x = (int)-brushSize; x <= brushSize; x++)
            {
                for (int y = (int)-brushSize; y <= brushSize; y++)
                {
                    int X = Mathf.RoundToInt(localPoint.x + x + drawImage.sprite.texture.width / 2f);
                    int Y = Mathf.RoundToInt(localPoint.y + y + drawImage.sprite.texture.height / 2f);
                    if( (x*x + y*y) <= brushSize*brushSize)
                    {
                        int index = Y * drawImage.sprite.texture.width + X;
                        if (index >= 0 && index < drawImagePixels.Length)
                        {
                            drawImagePixels[index] = currentColor;
                        }
                    }
                }
            }
        }
    }
    
    public void Clear()
    {
        for (int i = 0; i < drawImagePixels.Length; i++)
        {
            drawImagePixels[i] = Color.white;
        }
        Apply();
    }

    public void Apply()
    {
        drawImage.sprite.texture.SetPixels32(drawImagePixels);
        drawImage.sprite.texture.Apply();
    }
}
