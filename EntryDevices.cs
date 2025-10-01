using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;

namespace TrabalhoFinal;

static class EntryDevices
{

    // interactions
    public static MouseState mouseAtual, mouseAnterior;
    public static KeyboardState tecladoAtual, tecladoAnterior;
    public static bool enter = false,space = false;
    public static bool mleft = false, mright = false;
    public static int x=0,y=0;

    public static void Update()
    {
        mouseAnterior = mouseAtual;
        mouseAtual = Mouse.GetState();
        tecladoAnterior = tecladoAtual;
        tecladoAtual = Keyboard.GetState();

        updateKeyboard();
        updateMouse();
    }
    private static void updateKeyboard()
    {
        if (tecladoAnterior != null && tecladoAnterior.IsKeyUp(Keys.Enter) && tecladoAtual.IsKeyDown(Keys.Enter))
        {
            enter = true;
        }
        else
        {
            enter = false;
        }
        if (tecladoAnterior != null && tecladoAnterior.IsKeyUp(Keys.Space) && tecladoAtual.IsKeyDown(Keys.Space))
        {
            space = true;
        }
        else
        {
            space = false;
        }
    }
    private static void updateMouse()
    {
        if (mouseAnterior != null && mouseAnterior.LeftButton == ButtonState.Released && mouseAtual.LeftButton == ButtonState.Pressed)
        {
            mleft = true;
        }
        else
        {
            mleft = false;
        }
        if (mouseAnterior != null && mouseAnterior.RightButton == ButtonState.Released && mouseAtual.RightButton == ButtonState.Pressed)
        {
            mright = true;
        }
        else
        {
            mright = false;
        }
        x = mouseAtual.X;
        y = mouseAtual.Y;
    }
}