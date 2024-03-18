using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
using static OpenTK.Graphics.OpenGL.GL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KP_Melkonyan_PRI120
{
    public partial class Form1 : Form
    {

        int angle = 5, angleX = -90, angleY = 0, angleZ = 90;
        double translateX = 250, translateY = -40, translateZ = -50;
        int alpha = 0;
        float darkIndex = -0.5f;

        private int imageId;
        //private uint pictureTexture;
        //private uint pictureTexture2;
        //string picture = "picture.jpg";
        //string picture2 = "picture2.jpg";

        float beta = 0;
        float arbuzCoordZ = 45;
        Boolean drop = false;
        Boolean arbuzState = true;

        private float global_time = 0;

        private float[,] camera_date = new float[5, 7];
        private Explosion BOOOOM_1 = new Explosion(1, 10, 1, 100, 100);


        // для main door
        private Boolean isOpenMainDoor;
        private int xMainDoor=-95, yMainDoor=65, zMainDoor=0;
        private double angleMainDoor = 0;
        float humanCoordZ = 0;
        float handsCoordZ = 0;
        float handsCoordX = 0;
        float humanAngle = 0;
        float handsAngle = 0;

        float robotX = 0;
        float robotY= 0;

        private bool isLightOn = false;

        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);
            Gl.glClearColor(255, 255, 255, 1);
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(60, (float)AnT.Width / (float)AnT.Height, 0.1, 900);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            Il.ilGenImages(1, out imageId);
            Il.ilBindImage(imageId);

            //if (Il.ilLoadImage(picture))
            //{
            //    int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
            //    int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
            //    int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
            //    switch (bitspp)
            //    {
            //        case 24:
            //            pictureTexture = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
            //            break;
            //        case 32:
            //            pictureTexture = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
            //            break;
            //    }
            //}
            //if (Il.ilLoadImage(picture2))
            //{
            //    int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
            //    int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
            //    int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
            //    switch (bitspp)
            //    {
            //        case 24:
            //            pictureTexture2 = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
            //            break;
            //        case 32:
            //            pictureTexture2 = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
            //            break;
            //    }
            //}
            Il.ilDeleteImages(1, ref imageId);

            RenderTimer.Start();
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            global_time += (float)RenderTimer.Interval / 1000;
            Draw();
        }

        private void AnT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                translateX = translateX + Math.Sin(((90 - angleZ) * Math.PI) / 180);
                translateY = translateY - Math.Cos(((90 - angleZ) * Math.PI) / 180);
            }
            if (e.KeyCode == Keys.D)
            {
                translateX = translateX - Math.Sin(((90 - angleZ) * Math.PI) / 180);
                translateY = translateY + Math.Cos(((90 - angleZ) * Math.PI) / 180);
            }
            if (e.KeyCode == Keys.W)
            {
                translateX = translateX - 2*Math.Cos(((angleZ-90)* Math.PI) / 180);
                translateY = translateY + 2 * Math.Sin(((angleZ-90) * Math.PI) / 180);
            }
            if (e.KeyCode == Keys.S)
            {
                translateX = translateX + Math.Cos(((angleZ - 90) * Math.PI) / 180);
                translateY = translateY - Math.Sin(((angleZ - 90) * Math.PI) / 180);
            }

            if (e.KeyCode == Keys.Q)
            {
                angleZ -= angle;
            }
            if (e.KeyCode == Keys.E)
            {
                angleZ += angle;
            }

            if (e.KeyCode == Keys.R)
            {
                //if (yMainDoor<=65 && yMainDoor>=35 && translateX-250>=-130 && translateX-250 <= -105 && translateY+40 <= 100 && translateY +40 >= -50)
                //{
                if (yMainDoor>30)
                {
                    yMainDoor -= 5;
                }
                    
               // }
            }
            if (e.KeyCode == Keys.T)
            {
                //if (translateX - 250 <= -80 && translateX - 250 >= -200 && translateY + 40 >= -10 && translateY + 40 <= 60)
                //{
                    if (isLightOn)
                    {
                        darkIndex = -0.5f;
                        isLightOn = false;
                    }
                    else
                    {
                        darkIndex = 0f;
                        isLightOn = true;
                    }
                //}
            }
            if (e.KeyCode == Keys.NumPad8 && robotX <= 180)
            {
                robotX += 1;
            }
            if (e.KeyCode == Keys.NumPad2 && robotX >= -80)
            {
                robotX -= 1;
            }
            if (e.KeyCode == Keys.NumPad4 && robotY <= 180)
            {
                robotY += 1;
            }
            if (e.KeyCode == Keys.NumPad6 && robotY >= 5)
            {
                robotY -= 1;
            }
        }

        private void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            //Gl.glClearColor(255, 255, 255, 1);
            //Gl.glClearColor(0.82f, 0.7f, 0.3f, 1); // бамбук
            Gl.glClearColor(0.66f, 0.98f, 0.99f, 1);
            Gl.glLoadIdentity();
            Gl.glPushMatrix();
            Gl.glRotated(angleX, 1, 0, 0); Gl.glRotated(angleY, 0, 1, 0); Gl.glRotated(angleZ, 0, 0, 1);
            Gl.glTranslated(translateX, translateY, translateZ);
            //Gl.glColor3f(0.07f, 0.04f, 0.56f);
            BOOOOM_1.Calculate(global_time);

            
            Gl.glPushMatrix();

            // земля зеленая
            Gl.glPushMatrix();
            Gl.glColor3f(0f, 0.5f, 0.0f); //зеленый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(1000, 500, 0);
            Gl.glVertex3d(-1000, 500, 0);
            Gl.glVertex3d(-1000, -500, 0);
            Gl.glVertex3d(1000, -500, 0);
            Gl.glEnd();

            //Циферблат
            Gl.glPushMatrix();
            Gl.glTranslated(90, 199, 55);
            Gl.glColor3f(0.72f + darkIndex, 0.79f + darkIndex, 0.25f + darkIndex);
            //Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(0, 0, 0);
            Gl.glVertex3d(0, 0, 15);
            Gl.glVertex3d(15, 0, 15);
            Gl.glVertex3d(15, 0, 0);
            Gl.glEnd();
            //Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glPopMatrix();

            //Стрелки
            Gl.glPushMatrix();
            Gl.glTranslated(97.5, 197, 62.5);
            Gl.glColor3f(0, 0, 0);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glLineWidth(1f);
            Gl.glRotatef(alpha, 0, 1, 0);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3d(0, 0, 0);
            Gl.glVertex3d(0, 0, 3.5);
            Gl.glEnd();
            alpha += 12;
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(97.5, 197, 62.5);
            Gl.glColor3f(0, 0, 0);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glLineWidth(2f);
            Gl.glRotatef(beta, 0, 1, 0);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3d(0, 0, 0);
            Gl.glVertex3d(0, 0, 2);
            Gl.glEnd();
            beta += 1;
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glPopMatrix();

            //Фрактал дракон
            Gl.glPushMatrix();
            Gl.glTranslated(-400, 10, 0);
            Gl.glScaled(0.5, 0.3, 0.6);
            Gl.glRotated(0, 0, 1, 0);
            Gl.glRotated(0, 0, 0, 1);
            Gl.glColor3f(0f, 0f, 0f);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glLineWidth(1f);
            Gl.glBegin(Gl.GL_LINES);
            dragonFractal(0, 100, 200, 100, 0, "yellow");
            Gl.glEnd();
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glPopMatrix();
            //Фрактал дракон
            Gl.glPushMatrix();
            Gl.glTranslated(-400, 100, 0);
            Gl.glScaled(0.5, 0.3, 0.6);
            Gl.glRotated(0, 0, 1, 0);
            Gl.glRotated(0, 0, 0, 1);
            Gl.glColor3f(0f, 0f, 0f);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glLineWidth(1f);
            Gl.glBegin(Gl.GL_LINES);
            dragonFractal(0, 100, 200, 100, 0,"blue");
            Gl.glEnd();
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glPopMatrix();
            //Фрактал дракон
            Gl.glPushMatrix();
            Gl.glTranslated(-400, -100, 0);
            Gl.glScaled(0.5, 0.3, 0.6);
            Gl.glRotated(0, 0, 1, 0);
            Gl.glRotated(0, 0, 0, 1);
            Gl.glColor3f(0f, 0f, 0f);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glLineWidth(1f);
            Gl.glBegin(Gl.GL_LINES);
            dragonFractal(0, 100, 200, 100, 0, "red");
            Gl.glEnd();
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glPopMatrix();

            // прилавок горизонтальная часть стола
            Gl.glColor3f(0.82f+darkIndex, 0.7f+darkIndex, 0.3f + darkIndex); // бамбук
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(160, 80, 15);
            Gl.glVertex3d(160, 195, 15);
            Gl.glVertex3d(120, 195, 15);
            Gl.glVertex3d(120, 80, 15);

            // прилавок вертикально справа
            Gl.glVertex3d(160, 80, 15);
            Gl.glVertex3d(160, 80, 0);
            Gl.glVertex3d(120, 80, 0);
            Gl.glVertex3d(120, 80, 15);

            // прилавок вертикально спереди
            Gl.glVertex3d(120, 80, 15);
            Gl.glVertex3d(120, 80, 0);
            Gl.glVertex3d(120, 195, 0);
            Gl.glVertex3d(120, 195, 15);

            // прилавок вертикально слева
            Gl.glVertex3d(160, 195, 15);
            Gl.glVertex3d(160, 195, 0);
            Gl.glVertex3d(120, 195, 0);
            Gl.glVertex3d(120, 195, 15);

            // прилавок вертикально сзаи
            Gl.glVertex3d(160, 195, 15);
            Gl.glVertex3d(160, 195, 0);
            Gl.glVertex3d(160, 80, 0);
            Gl.glVertex3d(160, 80, 15);
            Gl.glEnd();

            // крыша дома
            // дальний треугольник
            //Gl.glColor3f(0.5f + darkIndex, 0f + darkIndex, 1f + darkIndex); //фиолетовый
            Gl.glColor3f(0.82f, 0.7f, 0.3f); // бамбук
            Gl.glBegin(Gl.GL_TRIANGLES);
            Gl.glVertex3d(200, 200, 100);
            Gl.glVertex3d(200, -10, 100);
            Gl.glVertex3d(150, 105, 300);
            Gl.glEnd();
            // ближний треугольник
            Gl.glPushMatrix();
            Gl.glColor3f(0.82f, 0.7f, 0.3f); // бамбук
            Gl.glBegin(Gl.GL_TRIANGLES);
            Gl.glVertex3d(-100, 200, 100);
            Gl.glVertex3d(-100, -10, 100);
            Gl.glVertex3d(150, 105, 300);
            Gl.glEnd();
            // лево треугольник
            Gl.glPushMatrix();
            Gl.glColor3f(0.82f, 0.7f, 0.3f); // бамбук
            Gl.glBegin(Gl.GL_TRIANGLES);
            Gl.glVertex3d(200, 200, 100);
            Gl.glVertex3d(-100, 200, 100);
            Gl.glVertex3d(150, 105, 300);
            Gl.glEnd();
            // право треугольник
            Gl.glPushMatrix();
            Gl.glColor3f(0.82f, 0.7f, 0.3f); // бамбук
            Gl.glBegin(Gl.GL_TRIANGLES);
            Gl.glVertex3d(200, -10, 100);
            Gl.glVertex3d(-100, -10, 100);
            Gl.glVertex3d(150, 105, 300);
            Gl.glEnd();

            

            // море вокруг 1
            Gl.glPushMatrix();
            Gl.glColor3f(0f, 1f, 1f); //цвет моря
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(2000, 500, 1);
            Gl.glVertex3d(300, 500, 1);
            Gl.glVertex3d(300, -500, 1);
            Gl.glVertex3d(2000, -500, 1);
            Gl.glEnd();
            // море вокруг 2
            Gl.glPushMatrix();
            Gl.glColor3f(0f, 1f, 1f); //цвет моря
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(2000, -500, 0);
            Gl.glVertex3d(2000, -1000, 0);
            Gl.glVertex3d(-2000, -1000, 0);
            Gl.glVertex3d(-2000, -500, 0);
            Gl.glEnd();
            // море вокруг 3
            Gl.glPushMatrix();
            Gl.glColor3f(0f, 1f, 1f); //цвет моря
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-1000, 500, 0);
            Gl.glVertex3d(-1000, -500, 0);
            Gl.glVertex3d(-2000, -500, 0);
            Gl.glVertex3d(-2000, 500, 0);
            Gl.glEnd();
            // море вокруг 4
            Gl.glPushMatrix();
            Gl.glColor3f(0f, 1f, 1f); //цвет моря
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-2000, 500, 0);
            Gl.glVertex3d(-2000, 1000, 0);
            Gl.glVertex3d(2000, 1000, 0);
            Gl.glVertex3d(2000, 500, 0);
            Gl.glEnd();

            //Стена основная
            Gl.glPushMatrix();
            Gl.glColor3f(0.4f + darkIndex, 0.2f + darkIndex, 0.0f + darkIndex); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(200, 200, 0);
            Gl.glVertex3d(200, -10, 0);
            Gl.glVertex3d(200, -10, 100);
            Gl.glVertex3d(200, 200, 100);
            Gl.glEnd();
            Gl.glPushMatrix();
            Gl.glColor3f(0.4f, 0.2f, 0.0f); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(201, 200, 0);
            Gl.glVertex3d(201, -10, 0);
            Gl.glVertex3d(201, -10, 100);
            Gl.glVertex3d(201, 200, 100);
            Gl.glEnd();

            // стена правая
            Gl.glPushMatrix();
            Gl.glColor3f(0.4f + darkIndex, 0.2f + darkIndex, 0.0f + darkIndex); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-100, -10, 0);
            Gl.glVertex3d(200, -10, 0);
            Gl.glVertex3d(200, -10, 100);
            Gl.glVertex3d(-100, -10, 100);
            Gl.glEnd();
            Gl.glPushMatrix();
            Gl.glColor3f(0.4f, 0.2f, 0.0f); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-100, -11, 0);
            Gl.glVertex3d(200, -11, 0);
            Gl.glVertex3d(200, -11, 100);
            Gl.glVertex3d(-100, -11, 100);
            Gl.glEnd();

            // стена левая
            Gl.glPushMatrix();
            Gl.glColor3f(0.4f + darkIndex, 0.2f + darkIndex, 0.0f + darkIndex); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-100, 200, 0);
            Gl.glVertex3d(200, 200, 0);
            Gl.glVertex3d(200, 200, 100);
            Gl.glVertex3d(-100, 200, 100);
            Gl.glEnd();
            Gl.glPushMatrix();
            Gl.glColor3f(0.4f, 0.2f, 0.0f); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-100, 201, 0);
            Gl.glVertex3d(200, 201, 0);
            Gl.glVertex3d(200, 201, 100);
            Gl.glVertex3d(-100, 201, 100);
            Gl.glEnd();

            // стена сзади с вырезом для двери
            Gl.glPushMatrix();
            Gl.glColor3f(0.4f + darkIndex, 0.2f + darkIndex, 0.0f + darkIndex); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-100, 200, 0);
            Gl.glVertex3d(-100, 65, 0);
            Gl.glVertex3d(-100, 65, 100);
            Gl.glVertex3d(-100, 200, 100);
            Gl.glEnd();

            Gl.glPushMatrix();
            Gl.glColor3f(0.4f, 0.2f, 0.0f); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-101, 200, 0);
            Gl.glVertex3d(-101, 65, 0);
            Gl.glVertex3d(-101, 65, 100);
            Gl.glVertex3d(-101, 200, 100);
            Gl.glEnd();

            Gl.glPushMatrix();
            Gl.glColor3f(0.4f + darkIndex, 0.2f + darkIndex, 0.0f + darkIndex); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-100, 65, 70);
            Gl.glVertex3d(-100, 65, 100);
            Gl.glVertex3d(-100, 30, 100);
            Gl.glVertex3d(-100, 30, 70);
            Gl.glEnd();

            Gl.glPushMatrix();
            Gl.glColor3f(0.4f, 0.2f, 0.0f); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-101, 65, 70);
            Gl.glVertex3d(-101, 65, 100);
            Gl.glVertex3d(-101, 30, 100);
            Gl.glVertex3d(-101, 30, 70);
            Gl.glEnd();

            Gl.glPushMatrix();
            Gl.glColor3f(0.4f + darkIndex, 0.2f + darkIndex, 0.0f + darkIndex); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-100, 30, 100);
            Gl.glVertex3d(-100, -10, 100);
            Gl.glVertex3d(-100, -10, 0);
            Gl.glVertex3d(-100, 30, 0);
            Gl.glEnd();

            Gl.glPushMatrix();
            Gl.glColor3f(0.4f, 0.2f, 0.0f); //коричневый
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-101, 30, 100);
            Gl.glVertex3d(-101, -10, 100);
            Gl.glVertex3d(-101, -10, 0);
            Gl.glVertex3d(-101, 30, 0);

            Gl.glEnd();

            //дверь
            //DrawDoor(-95, -105, 65, 30, 0, 70);
            //Gl.glRotated(0.1, 1, 0, 0); Gl.glRotated(2, 0, 1, 0); Gl.glRotated(angleZ, 0, 0, 1);
            

            // пол
            Gl.glPushMatrix();
            Gl.glColor3f(0.8f + darkIndex, 0.5f + darkIndex, 0.2f + darkIndex); //
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(200, -10, 5);
            Gl.glVertex3d(-100, -10, 5);
            Gl.glVertex3d(-100, 200, 5);
            Gl.glVertex3d(200, 200, 5);
            Gl.glEnd();

            // потолок
            Gl.glPushMatrix();
            Gl.glColor3f(1f + darkIndex, 0.8f + darkIndex, 0.7f + darkIndex); //
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(200, -10, 100);
            Gl.glVertex3d(-100, -10, 100);
            Gl.glVertex3d(-100, 200, 100);
            Gl.glVertex3d(200, 200, 100);
            Gl.glEnd();


            DrawDoor(xMainDoor, xMainDoor - 10, yMainDoor, yMainDoor - 35, zMainDoor, zMainDoor + 70,true);
            DrawDoor(xMainDoor-1, xMainDoor-1 - 10, yMainDoor, yMainDoor - 35, zMainDoor, zMainDoor + 70,false);

            // выключатель света
            if (isLightOn)
            {
                Gl.glPushMatrix();
                Gl.glColor3f(1f + darkIndex, 1f + darkIndex, 1f + darkIndex); //
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glVertex3d(-70, -7, 50);
                Gl.glVertex3d(-70, -7, 40);
                Gl.glVertex3d(-80, -7, 40);
                Gl.glVertex3d(-80, -7, 50);
                Gl.glEnd();
            } else
            {
                Gl.glPushMatrix();
                Gl.glColor3f(1f + darkIndex, 1f + darkIndex, 1f + darkIndex); //
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glVertex3d(-70, -9, 50);
                Gl.glVertex3d(-70, -9, 40);
                Gl.glVertex3d(-80, -9, 40);
                Gl.glVertex3d(-80, -9, 50);
                Gl.glEnd();
            }

            // robot пылесос
            Gl.glPushMatrix();
            Gl.glColor3f(1f+darkIndex, 1f+darkIndex, 1f + darkIndex);
            Gl.glTranslated(robotX, robotY, 0);
            Glut.glutSolidCylinder(10,10, 50, 50);
            Gl.glPopMatrix();

            // sun
            Gl.glPushMatrix();
            Gl.glColor3f(0.97f, 0.67f, 0.28f); // закатное солнце
            Gl.glTranslated(400, -600, 0);
            Glut.glutSolidSphere(200, 50, 50);

            // пальма
            Gl.glPushMatrix();
            Gl.glColor3f(0.5f, 0.3f, 0.03f); // пальма
            Gl.glTranslated(-400, 500, 0);
            Glut.glutSolidCylinder(10, 200, 50,50);
            Gl.glPopMatrix();
            Gl.glPushMatrix();
            Gl.glColor3f(0f, 0.5f, 0.0f);
            Gl.glTranslated(0, -100, 50);
            Glut.glutSolidCone(70, 180, 50, 50);
            Gl.glPopMatrix();

            // арбуз пирамида
            drawArbuz(154, 88, 22, 7, 7);
            drawArbuz(140, 109, 22,5,7);
            drawArbuz(126, 123, 22,3,7);

            // продавец


            if (drop == true && arbuzState == true)
            {
                //одиночный арбуз для падения
                arbuzCoordZ -= 10;
                ArbuzOne(81, 101, arbuzCoordZ, 7);
                if (arbuzCoordZ <= -5)
                {
                    BOOOOM_1.SetNewPosition(100, 140, 80);
                    BOOOOM_1.SetNewPower(5000);
                    BOOOOM_1.Boooom(global_time);
                    arbuzCoordZ = -10;
                    arbuzState = false;
                }
            } else
            {
                ArbuzOne(81, 101, arbuzCoordZ, 7);
            }
            //DrawHuman(0, 100, 100, 0);
            handsAngle = 90;
            
            DrawHuman(humanAngle, 100, 100, humanCoordZ);
            //humanAngle = 90;
            //DrawHuman(humanAngle, 100, 100, humanCoordZ);
            DrawHands(handsAngle, 90 + handsCoordX, 100, handsCoordZ);

            

            Gl.glPopMatrix();
            Gl.glFlush();
            AnT.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            drop = true;
            AnT.Focus();
            button1.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                angleX = -90; angleY = 0; angleZ = 90;
                translateX = 250; translateY = -40; translateZ = -50;
            }

            if (comboBox1.SelectedIndex == 1)
            {
                angleX = -90; angleY = 0; angleZ = 90;
                translateX = 50; translateY = -80; translateZ = -50;
            }

            if (comboBox1.SelectedIndex == 2)
            {
                angleX = -90; angleY = 0; angleZ = -90;
                translateX = -180; translateY = -120; translateZ = -50;
            }

            if (comboBox1.SelectedIndex == 3)
            {
                angleX = -90; angleY = 0; angleZ = 120;
                translateX = 250; translateY = 100; translateZ = -50;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form help = new Help();
            help.ShowDialog();
        }

        private void DrawDoor(int x1, int x2, int y1,int y2,int z1,int z2,bool isLight)
        {
            Gl.glPushMatrix();
            if (isLight)
            {
                Gl.glColor3f(0.2f + darkIndex, 0.2f + darkIndex, 0.2f + darkIndex);
            } else
            {
                Gl.glColor3f(0.2f , 0.2f , 0.2f );
            }
            
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(x1, y1, z1);
            Gl.glVertex3d(x1, y2, z1);
            Gl.glVertex3d(x1, y2, z2);
            Gl.glVertex3d(x1, y1, z2);

            Gl.glVertex3d(x2, y1, z1);
            Gl.glVertex3d(x2, y2, z1);
            Gl.glVertex3d(x2, y2, z2);
            Gl.glVertex3d(x2, y1, z2);

            Gl.glVertex3d(x2, y1, z2);
            Gl.glVertex3d(x1, y1, z2);
            Gl.glVertex3d(x1, y2, z2);
            Gl.glVertex3d(x2, y2, z2);

            Gl.glVertex3d(x2, y1, z1);
            Gl.glVertex3d(x1, y1, z1);
            Gl.glVertex3d(x1, y1, z2);
            Gl.glVertex3d(x2, y1, z2);

            Gl.glVertex3d(x2, y2, z1);
            Gl.glVertex3d(x1, y2, z1);
            Gl.glVertex3d(x1, y2, z2);
            Gl.glVertex3d(x2, y2, z2);


            Gl.glEnd();

        }

        private static uint MakeGlTexture(int Format, IntPtr pixels, int w, int h)
        {

            uint texObject;
            Gl.glGenTextures(1, out texObject);
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texObject);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);

            switch (Format)
            {

                case Gl.GL_RGB:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, w, h, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;

                case Gl.GL_RGBA:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, w, h, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;

            }
            return texObject;
        }

        void dragonFractal(int x1, int y1, int x2, int y2, int i,string color)
        {
            if (i == 15)
            {
                if (color=="yellow")
                {
                    Gl.glColor3f(1f, 1f, 0f);
                } else if (color=="blue")
                {
                    Gl.glColor3f(0f, 0.46f, 0.74f);
                } else if (color=="red")
                {
                    Gl.glColor3f(0.93f, 0.29f, 0.33f);
                }
                
                Gl.glVertex2i(x1, y1);
                Gl.glVertex2i(x2, y2);
            }
            else
            {
                int x = (x1 + x2) / 2 + (y2 - y1) / 2;
                int y = (y1 + y2) / 2 - (x2 - x1) / 2;
                dragonFractal(x, y, x1, y1, i + 1, color);
                dragonFractal(x, y, x2, y2, i + 1, color);
            }
        }

        //нарисовать арбуз
        // указываются точки первого арбуза справа и количество арбузов в первом ряду, необходимо указывать обязательно нечетное число
        private void drawArbuz(float x, float y, float z,int count,float radius)
        {
            float startY = y;
            float startZ = z;
            for (int i=count;i>=0;i--)
            {
                y = (count-i)*7+startY;
                //if (z!=startZ)
                //{
                //    y = startY + 7;
                //}
                for (int k=0;k<=i;k++)
                {
                    Gl.glPushMatrix();
                    if (darkIndex == -0.5)
                    {
                        Gl.glColor3f(0f, 0.5f + darkIndex + 0.25f, 0.0f); //зеленый
                    }
                    else
                    {
                        Gl.glColor3f(0f, 0.5f, 0.0f); //зеленый
                    }

                    Gl.glTranslated(x, y, z);
                    Gl.glRotated(90, 0, 1, 0);
                    Gl.glRotated(90, 0, 0, 1);
                    Glut.glutSolidSphere(radius, 100, 100);
                    Gl.glPopMatrix();
                    Gl.glPushMatrix();
                    Gl.glColor3f(0.08f + darkIndex, 0.08f + darkIndex, 0.08f + darkIndex); // пальма
                    Gl.glTranslated(x, y, z);
                    Gl.glRotated(90, 0, 1, 0);
                    Gl.glRotated(90, 0, 0, 1);
                    Glut.glutWireSphere(radius + 0.1, 11, 11);
                    Gl.glPopMatrix();
                    y += 14;
                }
                z += 10;
            }
        }

        private void ArbuzOne(float x, float y, float z,float radius)
        {
            Gl.glPushMatrix();
            if (darkIndex == -0.5)
            {
                Gl.glColor3f(0f, 0.5f + darkIndex + 0.25f, 0.0f); //зеленый
            }
            else
            {
                Gl.glColor3f(0f, 0.5f, 0.0f); //зеленый
            }

            Gl.glTranslated(x, y, z);
            Gl.glRotated(90, 0, 1, 0);
            Gl.glRotated(90, 0, 0, 1);
            Glut.glutSolidSphere(radius, 100, 100);
            Gl.glPopMatrix();
            Gl.glPushMatrix();
            Gl.glColor3f(0.08f + darkIndex, 0.08f + darkIndex, 0.08f + darkIndex); // пальма
            Gl.glTranslated(x, y, z);
            Gl.glRotated(90, 0, 1, 0);
            Gl.glRotated(90, 0, 0, 1);
            Glut.glutWireSphere(radius + 0.1, 11, 11);
            Gl.glPopMatrix();
        }

        private void DrawHuman(float angle, float x, float y, float z)
        {
            Gl.glPushMatrix();

            // Голова
            Gl.glPushMatrix();
            Gl.glTranslatef(x, y, z + 55);
            Gl.glRotatef(angle, 0, 0, 1);
            Gl.glColor3f(1, 0.8f, 0.6f);
            Glut.glutSolidSphere(7, 20, 20);
            Gl.glPopMatrix();

            //Шея
            Gl.glPushMatrix();
            Gl.glTranslatef(x, y, z + 42);
            Gl.glRotatef(angle, 0, 0, 1);
            Gl.glColor3f(1, 0.8f, 0.6f);
            Glut.glutSolidCone(4, 20, 20, 20);
            Gl.glPopMatrix();

            // Тело
            Gl.glPushMatrix();
            Gl.glTranslatef(x, y, z + 32);
            Gl.glRotatef(angle, 0, 0, 1);
            Gl.glColor3f(1 + darkIndex, 0.25f + darkIndex, 0.5f + darkIndex);
            Gl.glScaled(0.55, 1, 2);
            Glut.glutSolidCube(15);
            Gl.glPopMatrix();

            //Правая шортина
            Gl.glPushMatrix();
            Gl.glTranslatef(x, y + 5, z + 14);
            Gl.glRotatef(angle, 0, 0, 1);
            Gl.glColor3f(0 + darkIndex, 0 + darkIndex, 0 + darkIndex);
            Gl.glScaled(1, 0.8, 2);
            Glut.glutSolidCube(7);
            Gl.glPopMatrix();

            //Левая шортина
            Gl.glPushMatrix();
            Gl.glTranslatef(x, y - 5, z + 14);
            Gl.glRotatef(angle, 0, 0, 1);
            Gl.glColor3f(0 + darkIndex, 0 + darkIndex, 0 + darkIndex);
            Gl.glScaled(1, 0.8, 2);
            Glut.glutSolidCube(7);
            Gl.glPopMatrix();

            // Правая нога
            Gl.glPushMatrix();
            Gl.glTranslatef(x, y + 6, z + 9);
            Gl.glRotatef(angle, 0, 0, 1);
            Gl.glColor3f(1, 0.8f, 0.6f);
            Gl.glScaled(0.5, 0.5, 1.75);
            Glut.glutSolidCube(7);
            Gl.glPopMatrix();

            // Левая нога
            Gl.glPushMatrix();
            Gl.glTranslatef(x, y - 6, z + 9);
            Gl.glRotatef(angle, 0, 0, 1);
            Gl.glColor3f(1, 0.8f, 0.6f);
            Gl.glScaled(0.5, 0.5, 1.75);
            Glut.glutSolidCube(7);
            Gl.glPopMatrix();
        }

        private void DrawHands(float angle, float x, float y, float z)
        {
            Gl.glPushMatrix();
            

            // Правая рука
            Gl.glPushMatrix();
            Gl.glTranslatef(x, y + 8, z + 45);
            Gl.glRotatef(angle, 0, 1, 0);
            Gl.glColor3f(1, 0.8f, 0.6f);
            Gl.glScaled(0.5, 0.5, 2.5);
            Glut.glutSolidCube(7);
            Gl.glPopMatrix();

            // Левая рука
            Gl.glPushMatrix();
            Gl.glTranslatef(x, y - 8, z + 45);
            Gl.glRotatef(angle, 0, 1, 0);
            Gl.glColor3f(1, 0.8f, 0.6f);
            Gl.glScaled(0.5, 0.5, 2.5);
            Glut.glutSolidCube(7);
            Gl.glPopMatrix();

            Gl.glPopMatrix();
        }
    }
}
