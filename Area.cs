using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;

namespace TrabalhoFinal;
class Area
{
    public Rectangle area;
    public Texture2D texture;
    public int cube, face;
    public string id;

    public Area(string id,Texture2D texture, Rectangle rect, int cube, int face)
    {
        this.id = id;
        this.texture = texture;
        area = rect;
        this.cube = cube;
        this.face = face;
    }
    public Area(string id,Rectangle rect, int cube, int face)
    {
        this.id = id;
        area = rect;
        this.cube = cube;
        this.face = face;
    }
}