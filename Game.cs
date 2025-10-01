using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;

namespace TrabalhoFinal;

public class Engine : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private float width { get; }
    private float height { get; }

    //
    private Texture2D[,] textureCubes = new Texture2D[2, 5];
    private Texture2D textureSeta;
    private int cube = 0, face = 0;
    // areas 
    Rectangle areaSetaDireita, areaSetaEsquerda, areaSetaCima, areaSetaBaixo;
    Area[] areas = new Area[2];
    // faces
    int lastFace;
    // debug
    bool showAreas = false;
    
    public Engine()
    {
        _graphics = new GraphicsDeviceManager(this);
        width = 1000f;
        height = 800f;
        _graphics.PreferredBackBufferWidth = (int)width;   // tamXura
        _graphics.PreferredBackBufferHeight = (int)height;   // altura

        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        //sprites
        string[,] locals = {
            {
                "data/labterm/saida.jpg",
                "data/labterm/me.jpg",
                "data/labterm/miniArmario.jpg",
                "data/labterm/pcsNormais.jpg",
                "data/labterm/tetoJunino.jpg"
            },
            {
                "data/labterm/geladeira.jpg",
                "data/labterm/cameras.jpg",
                "data/labterm/janPeq.jpg",
                "data/labterm/superPcs.jpg",
                "data/labterm/tetoNormal.jpg"
            }
        };
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                // using var str = new FileStream(locals[i, j], FileMode.Open);
                // textureCubes[0] = new Texture2D[6];
                textureCubes[i, j] = ReloadTexture(locals[i, j]);
            }
        }
        Console.WriteLine(Directory.GetCurrentDirectory());

        textureSeta = Texture2D.FromStream(GraphicsDevice, new FileStream("data/seta.png", FileMode.Open));
    }
   private Texture2D ReloadTexture(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return Texture2D.FromStream(GraphicsDevice, stream);
    }


    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        setAreas();

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // pega position
        EntryDevices.Update();
        Point mousePos = new Point(EntryDevices.x, EntryDevices.y);
        if (EntryDevices.enter)
        {
            Console.WriteLine("Real: " + mousePos.X + ", " + mousePos.Y + "; Relativa ao centro: " + (mousePos.X - width / 2) + ", " + (mousePos.Y - height / 2));
        }       


        // clique
        if (EntryDevices.mleft)
        {
            // textureCubes[1,4] = ReloadTexture("data/labterm/tetoNormal.jpg");
            if (areaSetaDireita.Contains(mousePos))
            {
                if (face == 4) face = lastFace;
                if (face < 3) face++; else face = 0;
                Console.WriteLine("Clicou em cima da seta direita!");
            }
            else if (areaSetaEsquerda.Contains(mousePos))
            {
                if (face == 4) face = lastFace;
                if (face > 0) face--; else face = 3;
                Console.WriteLine("Clicou em cima da seta esquerda!");
            }
            else if (areaSetaCima.Contains(mousePos))
            {

                if (face < 4)
                {
                    lastFace = face;
                    face = 4;
                }
                else
                {
                    face = (lastFace + 2) % 4;
                }
                Console.WriteLine(lastFace + " cima");
            }
            else if (areaSetaBaixo.Contains(mousePos))
            {
                face = lastFace;
                Console.WriteLine("Clicou em cima da seta de baixo!");
            }
            else
            {
                processAreas(mousePos);
            }
        }

        if (EntryDevices.space) showAreas = !showAreas;
        // process areas
        // ac cima >  baixo
        // ac baixo > cima

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        //
        // 400,295
        int tamX = 4080 / 5;
        int tamY = 3060 / 5;
        int padX = 0, padY = 0; // equal and 5
        float zoom = 0.2f;
        float rotation = 0f;
        Vector2 position = Vector2.Zero;

        // TODO: Add your drawing code here
        if (face == 4)
        {
            position = new Vector2(width / 2 - tamX / 2, height / 2 - tamY / 2);
            if (lastFace % 2 == 1)
            {
                rotation = MathHelper.PiOver2 * ((lastFace - 1));
                position = new Vector2(width / 2 - tamX / 2 + tamX * (lastFace - lastFace % 2) / 2, height / 2 - tamY / 2 + tamY * (lastFace - lastFace % 2) / 2);

                // position = new Vector2(height / 2 - tamY / 2, width / 2 - tamX / 2);
            }
            Console.WriteLine("rot.: " + rotation);

        }
        else
        {
            position = new Vector2(width / 2 - tamX / 2, height / 2 - tamY / 2);
        }
        var flip = SpriteEffects.None;  // flipDrawSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        _spriteBatch.Draw(textureCubes[cube, face], position, null, Color.White, rotation, Vector2.Zero, zoom, flip, 0); // 192 x 96

        setas(tamX, tamY, padX, padY, zoom, rotation);
        showareas(showAreas);
        // flip = SpriteEffects.FlipHorizontally;
        // rotation = MathHelper.PiOver2;
        //
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void setas(int tamX, int tamY, int padX, int padY, float zoom, float initRotation)
    {
        // direita
        padX = 5;
        padY = 5;
        int tamY2 = 20;
        int tamX2 = 17;
        int marginX = 5;
        var rotation = 0f;
        var flip = SpriteEffects.None;
        zoom = 1f;

        var position = new Vector2(width / 2 + tamX / 2 - tamX2 - marginX, height / 2 - tamY2 / 2);
        var cut = new Rectangle(padX, padY, tamX2, tamY2);
        areaSetaDireita = new Rectangle(
            (int)position.X,
            (int)position.Y,
            cut.Width,
            cut.Height
        );
        _spriteBatch.Draw(textureSeta, position, cut, Color.White, rotation, Vector2.Zero, zoom, flip, 0);

        // esquerda
        flip = SpriteEffects.FlipHorizontally;
        position = new Vector2(width / 2 - tamX / 2 + marginX, height / 2 - tamY2 / 2);
        cut = new Rectangle(padX, padY, tamX2, tamY2);
        areaSetaEsquerda = new Rectangle(
            (int)position.X,
            (int)position.Y,
            cut.Width,
            cut.Height
        );
        _spriteBatch.Draw(textureSeta, position, cut, Color.White, rotation, Vector2.Zero, zoom, flip, 0);

        // cima
        flip = SpriteEffects.None;
        rotation = -MathHelper.PiOver2;
        position = new Vector2(width / 2 - tamY2 / 2, height / 2 - tamY / 2 + tamX2 + marginX);
        cut = new Rectangle(padX, padY, tamX2, tamY2);
        areaSetaCima = new Rectangle(
            (int)position.X,
            (int)(position.Y - tamY2 + (tamY2 - tamX2)),
            cut.Height,
            cut.Width
        );
        _spriteBatch.Draw(textureSeta, position, cut, Color.White, rotation, Vector2.Zero, zoom, flip, 0);

        if (face == 4)
        {
            // baixo
            flip = SpriteEffects.None;
            rotation = MathHelper.PiOver2;
            position = new Vector2(width / 2 + tamY2 / 2, height / 2 + tamY / 2 - tamX2 - marginX);
            cut = new Rectangle(padX, padY, tamX2, tamY2);
            areaSetaBaixo = new Rectangle(
                (int)position.X - tamX2 - (tamY2 - tamX2),
                (int)(position.Y),
                cut.Height,
                cut.Width
            );
            _spriteBatch.Draw(textureSeta, position, cut, Color.White, rotation, Vector2.Zero, zoom, flip, 0);
        }
        else
        {
            areaSetaBaixo = new Rectangle(0, 0, 0, 0);
        }
    }

    private void showareas(bool show)
    {
        if (!show) return;
        // debug areas
        Texture2D rect = new Texture2D(GraphicsDevice, 1, 1);
        rect.SetData(new[] { Color.White });

        // direita
        _spriteBatch.Draw(rect, areaSetaDireita, Color.Red * 0.5f);
        // esquerda
        _spriteBatch.Draw(rect, areaSetaEsquerda, Color.Red * 0.5f);
        // cima
        _spriteBatch.Draw(rect, areaSetaCima, Color.Red * 0.5f);
        // baixo
        _spriteBatch.Draw(rect, areaSetaBaixo, Color.Red * 0.5f);

        foreach (Area area in areas)
        {
            if (area.cube == cube && area.face == face)
            {
                _spriteBatch.Draw(rect, new Vector2(width / 2 + area.area.X, height / 2 + area.area.Y), area.area, Color.Blue * 0.5f);
            }
        }
    }

    private void setAreas()
    {

        areas[0] = new Area("cubo1", new Rectangle(11, -125, 145, 340), 0, 2);
        areas[1] = new Area("cubo0", new Rectangle(-167, -109, 150, 325), 1, 0);
    }
    private void processAreas(Point mousePos)
    {
        foreach (Area area in areas)
        {
            if (area.cube == cube && area.face == face)
            {
                Rectangle realArea = new Rectangle(
                    (int)(width / 2 + area.area.X),
                    (int)(height / 2 + area.area.Y),
                    area.area.Width,
                    area.area.Height
                );
                if (realArea.Contains(mousePos))
                {
                    // Console.WriteLine("Clicou na area " + area.id + " do cubo " + area.cube + " face " + area.face);
                    switch (area.id)
                    {
                        case "cubo0":
                            cube = 0;
                            face = 0;
                            break;
                        case "cubo1":
                            cube = 1;
                            face = 2;
                            break;
                        default:
                            Console.WriteLine("id deconhecido: "+area.id);
                            break;
                    }
                }
            }
        }
    }
}
