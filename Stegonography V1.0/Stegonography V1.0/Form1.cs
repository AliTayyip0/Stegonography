using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stegonography_V1._0
{
    public partial class Form1 : Form
    {

        Bitmap image3;
        string DosyaYolu1="";
        string DosyaYolu2="";
        string DosyaYolu3 = "";

        public Form1()
        {
            InitializeComponent();
        }

#region Taban Dönüşümü

        
        string sayi_dan_binary(int sayi1)
        {
            int sayi, kalan;
            string yazikalan = "";
            sayi = int.Parse(sayi1.ToString());
            while (sayi != 0)
            {
                kalan = sayi % 2;
                sayi = sayi / 2;
                yazikalan = kalan.ToString() + yazikalan;
            }
            for (int i = yazikalan.Length; i < 8; i++)
            {
                yazikalan = "0" + yazikalan;
            }
            return yazikalan;
        }

        int binary_den_sayi(string binary)
        {
            int sayi = 0;
            for (int i = 7; i >= 0; i--)
            {
                if (binary[i].ToString() == "1")
                {
                    sayi += Convert.ToInt32(Math.Pow(2, ((i - 7) * -1)));
                }
            }
            return sayi;
        }


        #endregion

#region fotoğrafı diğerinin ardına gizleme

        void resimleribirlestir()
        {
            try
            {

                Bitmap image1;
                Bitmap image2;
                MessageBox.Show("Resim Gizlenmeye başlıyor");
                
                     image1 = new Bitmap(pictureBox1.Image);
                     image2 = new Bitmap(pictureBox2.Image);
             
                
                image3 = new Bitmap(image1.Width, image1.Height);

                int x, y;

                    for (x = 0; x < image1.Width; x++)
                    {
                        for (y = 0; y < image1.Height; y++)
                        {
                            Color pixelColor1 = image1.GetPixel(x, y);
                            Color pixelColor2 = image2.GetPixel(x, y);

                        #region son 3 bit değişimi

                        // string alpha = sayi_dan_binary(pixelColor2.R).Substring(3,2)+ sayi_dan_binary(pixelColor2.G).Substring(3, 2)+ sayi_dan_binary(pixelColor2.B).Substring(3, 2) +"00";

                           // MessageBox.Show(pixelColor1.GetHue().ToString());
                            

                            string red = sayi_dan_binary(pixelColor1.R).Substring(0, 5);
                            //red = red.Remove(4,3);
                            red += sayi_dan_binary(pixelColor2.R).Substring(0, 3);

                            string green = sayi_dan_binary(pixelColor1.G).Substring(0, 5);
                            //green = green.Remove(4, 3);
                            green += sayi_dan_binary(pixelColor2.G).Substring(0, 3);

                            string blue = sayi_dan_binary(pixelColor1.B).Substring(0, 5);
                            //blue = blue.Remove(4, 3);
                            blue += sayi_dan_binary(pixelColor2.B).Substring(0, 3);
                            

                            #endregion

                            Color newColor = Color.FromArgb(binary_den_sayi(red), binary_den_sayi(green), binary_den_sayi(blue));
                            
                            image3.SetPixel(x, y, newColor);
                        }
                    }

                pictureBox3.Image = image3;

                pictureBox4.Image = image3;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

#endregion
        //psnr değeri yaz!!


#region Gizli fotoğrafı çıkartma


        void cozme(Bitmap image1)
        {
            int x = 0, y = 0;
            MessageBox.Show("Gizli Resim Çıkarılmaya başlıyor");

            for (x = 0; x < image1.Width; x++)
            {
                for (y = 0; y < image1.Height; y++)
                {

                    Color pixelColor1 = image1.GetPixel(x, y);

                    string red = sayi_dan_binary(pixelColor1.R).Substring(5, 3) + "00000";
                    string green = sayi_dan_binary(pixelColor1.G).Substring(5, 3) + "00000";
                    string blue = sayi_dan_binary(pixelColor1.B).Substring(5, 3) + "00000";

                    Color newColor = Color.FromArgb(binary_den_sayi(red), binary_den_sayi(green), binary_den_sayi(blue));

                    image1.SetPixel(x, y, newColor);
                }
            }
            pictureBox5.Image = image1;
        }
       // Orjinal = new Bitmap(pictureBox1.Image);


#endregion



        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Resim Dosyası .png|*.jpg";
            file.ShowDialog();
            DosyaYolu1 = file.FileName;
            pictureBox2.ImageLocation = DosyaYolu1;
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Resim Dosyası .png|*.jpg";
            file.ShowDialog();
             DosyaYolu2 = file.FileName;
            pictureBox1.ImageLocation = DosyaYolu2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image!=null&&pictureBox2.Image!=null)
            {
                if (pictureBox1.Image.Size==pictureBox2.Image.Size)
                {
                    resimleribirlestir();
                }
                else
                {
                    MessageBox.Show("Eşit Boyutta Resimler Giriniz.");
                }
            }
            else
            {
                MessageBox.Show("Seçilmeyen resim var!!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Kaydeymek için
            if (pictureBox3.Image!=null)
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox3.Image.Save(savedialog.FileName + ".jpg", ImageFormat.Jpeg);
                }
            }
       }
        
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Resim Dosyası .png|*.jpg";
            file.ShowDialog();
            DosyaYolu3 = file.FileName;
            pictureBox4.ImageLocation = DosyaYolu3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox4.Image!=null)
            {
                Bitmap image1 = new Bitmap(pictureBox4.Image);
                cozme(image1);
            }
            else
            {
                MessageBox.Show("bir resim seçiniz");
            }
        }
    }
}
