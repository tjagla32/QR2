using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Imaging;
using QRCoder;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;


namespace ReloadPage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void RenderQrCode(string TEXT)
        {
         
            QRCodeGenerator.ECCLevel eccLevel = 0;
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(TEXT, eccLevel))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {

                        var tmp = qrCode.GetGraphic(3, Color.Black, Color.White,
                            GetIconBitmap(), 0);
                        pictureBoxQRCode.BackgroundImage = tmp;

                        pictureBoxQRCode.ToString();

                        this.pictureBoxQRCode.Size = new System.Drawing.Size(pictureBoxQRCode.Width, pictureBoxQRCode.Height);

                        //Set the SizeMode to center the image.
                        this.pictureBoxQRCode.SizeMode = PictureBoxSizeMode.CenterImage;

                        pictureBoxQRCode.SizeMode = PictureBoxSizeMode.CenterImage;
                    }
                }
            }
        }

        public void saveToFile(string read, string write)
        {

            //var lines = File.ReadAllLines(read);

            //lines = lines.Skip(1).ToArray();
            //File.AppendAllLines(write, lines);

          
            //textBox2.Text += line.Substring(0, line.IndexOf(";"));
 


            var lines = File.ReadLines(read);
            int row = 0;
            int row2 = 0;
            foreach (var line in lines)
            {
                if (row != 0)
                {
                    textBox2.Text += line.Substring(0, line.IndexOf(";"));
                    //SKU[row2] = textBox2.Text;
                    row2++;
                    textBox2.Text += Environment.NewLine;
                }
                row++;
                
            }

        }

        private Bitmap GetIconBitmap()
        {
            Bitmap img = null;
            if (iconPath.Text.Length > 0)
            {
                try
                {
                    img = new Bitmap(iconPath.Text);
                }
                catch (Exception)
                {
                }
            }
            return img;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            RenderQrCode(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
            var lines = File.ReadLines("C:/Users/Tomek/Desktop/CSVK-Baterie.csv");
            int row = 0;

            foreach (var line in lines)
            {

                if (row != 0)
                {
                    var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0f, 0f, 0f, 0f);
                    //textBox2.Text = line.Substring(0, line.IndexOf(";"));
                    RenderQrCode(line.Substring(0, line.IndexOf(";")));

                    PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(@"C:\Users\Tomek\Desktop\BaterieQR\" + line.Substring(0, line.IndexOf(";")) + ".pdf", FileMode.Create));
                    doc.Open();
                    doc.SetMargins(0f, 0f, 0f, 0f);

                    Bitmap qr_image = new Bitmap(pictureBoxQRCode.BackgroundImage);
                    qr_image = addTextToImage(qr_image, line.Substring(0, line.IndexOf(";")));


                    iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(qr_image, System.Drawing.Imaging.ImageFormat.Bmp);


                    PdfPTable table = new PdfPTable(6);

                    float tmp = 24.75F;
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.DefaultCell.FixedHeight = iTextSharp.text.Utilities.MillimetersToPoints(tmp);
                    table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.DefaultCell.PaddingTop = 1;
                    table.DefaultCell.PaddingBottom = 1;

                    for (int j = 0; j < 72; j++)
                    {
                        pdfImage.ScaleAbsolute(30, 25);
                        table.AddCell(pdfImage);
                    }


                    doc.Add(table);
                    doc.Close();
                }
                row++;


            }

            

            textBox1.Text = "chyba działa";
        }
        private Bitmap addTextToImage(Bitmap image, string text)
        {
            Bitmap tmp;
            using (Bitmap newbitmap = new Bitmap(image))
            {
                PointF firstLocation = new PointF(0, 0);
                using (Graphics graphics = Graphics.FromImage(newbitmap))
                {
                    using (System.Drawing.Font arialFont = new System.Drawing.Font("Arial", 5))
                    {
                        //graphics.DrawImage(newbitmap, 0, 0, iTextSharp.text.Utilities.MillimetersToPoints(80), iTextSharp.text.Utilities.MillimetersToPoints(50));
                        graphics.DrawString(text, arialFont, Brushes.Black, firstLocation);
                        
                        //graphics.DrawString(secondText, arialFont, Brushes.Red, secondLocation);
                    }
                }
                tmp = new Bitmap(newbitmap);
                tmp.SetResolution(300, 240);
            }
            return tmp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
