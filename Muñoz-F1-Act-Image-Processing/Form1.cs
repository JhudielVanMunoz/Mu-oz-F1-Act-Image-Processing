using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebCamLib;

namespace Muñoz_F1_Act_Image_Processing
{
    public partial class Form1 : Form
    {

        Bitmap loadImage, resultImage, backgroundImage;
        Color pixel;
        private Device webcamDevice;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int gray;

            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int i = 0; i < loadImage.Width; i++)
            {
                for (int j = 0; j < loadImage.Height; j++)
                {
                    pixel = loadImage.GetPixel(i, j);
                    gray = (pixel.R + pixel.G + pixel.B) / 3;
                    resultImage.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            pictureBox2.Image = resultImage;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int i = 0; i < loadImage.Width; i++)
            {
                for (int j = 0; j < loadImage.Height; j++)
                {
                    pixel = loadImage.GetPixel(i, j);
                    resultImage.SetPixel(i, j, pixel);
                }
            }
            pictureBox2.Image = resultImage;
        }

        private void inversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int i = 0; i < loadImage.Width; i++)
            {
                for (int j = 0; j < loadImage.Height; j++)
                {
                    pixel = loadImage.GetPixel(i, j);
                    resultImage.SetPixel(i, j, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                }
            }
            pictureBox2.Image = resultImage;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color sample;
            int gray;

            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int i = 0; i < loadImage.Width; i++)
            {
                for (int j = 0; j < loadImage.Height; j++)
                {
                    pixel = loadImage.GetPixel(i, j);
                    gray = (pixel.R + pixel.G + pixel.B) / 3;
                    resultImage.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            int[] histdata = new int[256];
            for (int i = 0; i < loadImage.Width; i++)
            {
                for (int j = 0; j < loadImage.Height; j++)
                {
                    sample = resultImage.GetPixel(i, j);
                    histdata[sample.R]++;
                }
            }

            Bitmap mydata = new Bitmap(256, 800);

            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 800; j++)
                {
                    mydata.SetPixel(i, j, Color.White);
                }
            }

            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < Math.Min(histdata[i] / 5, 800); j++)
                {
                    mydata.SetPixel(i, 799 - j, Color.Black);
                }
            }
            pictureBox2.Image = mydata;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            resultImage.Save(saveFileDialog1.FileName);
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int gray, red, green, blue;

            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int i = 0; i < loadImage.Width; i++)
            {
                for (int j = 0; j < loadImage.Height; j++)
                {
                    pixel = loadImage.GetPixel(i, j);
                    gray = (pixel.R + pixel.G + pixel.B) / 3;
                    red = Math.Min(255, (int)(gray * 0.393 + gray * 0.769 + gray * 0.189));
                    green = Math.Min(255, (int)(gray * 0.349 + gray * 0.686 + gray * 0.168));
                    blue = Math.Min(255, (int)(gray * 0.272 + gray * 0.534 + gray * 0.131));
                    resultImage.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }
            pictureBox2.Image = resultImage;
        }

        private void loadBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void subtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int x = 0; x < loadImage.Width; x++)
            {
                for (int y =0; y < loadImage.Height; y++)
                {
                    pixel = loadImage.GetPixel(x, y);
                    Color backpixel = backgroundImage.GetPixel(x, y);
                    int grey = (pixel.G + pixel.R + pixel.B) / 3;;
                    int subtractvalue = Math.Abs(grey - greygreen);
                    if (subtractvalue > threshold)
                    {
                        resultImage.SetPixel(x, y, pixel);
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, backpixel);
                    }
                }
            }
            pictureBox2.Image = resultImage;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void useWebcamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Device[] devices = DeviceManager.GetAllDevices();

            if (devices.Length > 0)
            {
                webcamDevice = devices[0];
                webcamDevice.ShowWindow(pictureBox1);
            }
            else
            {
                MessageBox.Show("No webcam devices found.");
            }
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            backgroundImage = new Bitmap(openFileDialog2.FileName);
            pictureBox3.Image = backgroundImage;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loadImage = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loadImage;
        }


    }
}
