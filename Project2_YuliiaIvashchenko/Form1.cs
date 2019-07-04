using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace Project2_YuliiaIvashchenko
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap b = new Bitmap("flower.jpg");
        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Bitmap d;
        bool newpic = false;
        private void button1_Click(object sender, EventArgs e)
        {
            d = new Bitmap(b.Width, b.Height);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an image file.";
            ofd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
            ofd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader streamReader = new StreamReader(ofd.FileName);
                d = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                streamReader.Close();

                pictureBox1.BackgroundImage = d;
            }
            newpic = true;
            d = new Bitmap(pictureBox1.BackgroundImage);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (newpic == false)
                pictureBox1.BackgroundImage = new Bitmap("flower.jpg");
            else
                pictureBox1.BackgroundImage = d;
        }

        //Thresholding(also average dithering) : each pixel value is compared against a fixed 
        //threshold.This may be the simplest dithering algorithm there is, but it results in 
        //immense loss of detail and contouring (c) Wikipedia

        //AVERAGE DITHERING consists of choosing a certain constant gray level, 
        //in  particular the average value of image pixels, and using it as a 
        //global threshold in deciding whether a pixel should be quantized to 0 or to 1. 
        //All pixels whose intensity level lies above the average value (the threshold) 
        //are quantized to 1; all others get a value of 0.

        private void button2_Click(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.BackgroundImage);
            Bitmap d = new Bitmap(b.Width, b.Height);

            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    Color oc = b.GetPixel(x, y);

                    //grayscale
                    //int ret = (int)(oc.R * 0.299 + oc.G * 0.578 + oc.B * 0.114);
                    int ret = (int)(oc.R);
                    int avg = (oc.R + oc.G + oc.B) / 3;
                    if (ret > avg)
                    {
                        ret = 255;
                    }
                    else
                    {
                        ret = 0;
                    }

                    Color nc = Color.FromArgb(oc.A, ret, ret, ret);
                    d.SetPixel(x, y, nc);
                }
            }

            pictureBox1.BackgroundImage= d;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int centroidNums = Convert.ToInt32(textBox1.Text);
            Bitmap picBitmap = new Bitmap(pictureBox1.BackgroundImage);

            ImageSegmentation ImageSeg = new ImageSegmentation();
            Bitmap resPicBitmap = ImageSeg.Compute(picBitmap, centroidNums);

            pictureBox1.BackgroundImage = resPicBitmap;
        }

    }
}
