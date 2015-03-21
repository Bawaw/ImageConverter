using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ImageConverter
{

    public partial class Form1 : Form
    {
        Bitmap newBitmap;
        Image file;
        bool opened = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                file = Image.FromFile(openFileDialog1.FileName);
                newBitmap = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = file;
                opened = true;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = saveFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                if (opened == true)
                {

                    if (saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.Length - 3).ToLower() == "bmp")
	                {
		                 file.Save(saveFileDialog1.FileName,ImageFormat.Bmp);
	                }

                    if (saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.Length - 3).ToLower() == "jpg")
                    {
                        file.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
                    }  
                }

                else
                {
                    MessageBox.Show("You need to open an Image first");
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < newBitmap.Width; x++)
            {
                for (int y = 0; y < newBitmap.Height; y++)
                {
                    Color originalColor = newBitmap.GetPixel(x, y);

                    int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59) + (originalColor.B * .11));
                    Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);

                    newBitmap.SetPixel(x, y, newColor);

                }
            }
            pictureBox1.Image = newBitmap;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            for (int x = 1; x < newBitmap.Width; x++)
            {
                for (int y = 1; y < newBitmap.Height; y++)
                {
                    try
                    {
                        Color prevX = newBitmap.GetPixel(x - 1, y);
                        Color nextX = newBitmap.GetPixel(x + 1, y);
                        Color prevY = newBitmap.GetPixel(x, y - 1);
                        Color nextY = newBitmap.GetPixel(x, y + 1);

                        int avrgR = (int)((prevX.R + nextX.R + nextY.R + prevX.R) / 4);
                        int avrgG = (int)((prevX.G + nextX.G + nextY.G + prevX.G) / 4);
                        int avrgB = (int)((prevX.B + nextX.B + nextY.B + prevX.B) / 4);

                        newBitmap.SetPixel(x, y, Color.FromArgb(avrgB, avrgG, avrgR));
                        
                    }
                    catch (Exception) {}
                }
            }
            pictureBox1.Image = newBitmap;
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = trackBar1.Value.ToString();

            pictureBox1.Image = AdjustBrightness(newBitmap, trackBar1.Value);

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public static Bitmap AdjustBrightness(Bitmap Image, int value)
        {
            Bitmap TempBitmap = Image;

            float FinalValue = (float)value / 255.0f;
            Bitmap newBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);

            Graphics newGraphics = Graphics.FromImage(newBitmap);

            float[][] floatColorMatrix =
            {
                new float [] {1,0,0,0,0},
                new float [] {0,1,0,0,0},
                new float [] {0,0,1,0,0},
                new float [] {0,0,0,1,0},
                new float [] {FinalValue, FinalValue, FinalValue, 1, 1}
            };

            ColorMatrix NewColor = new ColorMatrix(floatColorMatrix);

            ImageAttributes Attributes = new ImageAttributes();

            Attributes.SetColorMatrix(NewColor);
            newGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, GraphicsUnit.Pixel, Attributes);
            
            Attributes.Dispose();
            newGraphics.Dispose();
            return newBitmap;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            for (int x = 0; x < newBitmap.Width; x++)
            {
                for (int y = 0; y < newBitmap.Height; y++)
                {
                    Color pixel = newBitmap.GetPixel(x, y);

                    int red = pixel.R;
                    int blue = pixel.B;
                    int green = pixel.G;

                    newBitmap.SetPixel(x,y,Color.FromArgb(255-blue,255-green,255-red));
                }
            }
            pictureBox1.Image = newBitmap;
        }
    }
}
