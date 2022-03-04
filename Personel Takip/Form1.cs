using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;                   // port kütüphanesi
using System.IO.Ports;             // port kütüphanesi
using System.Data.OleDb;           // veri tabanı kütüphanesi

namespace Hesli_Kimlik_Doğrulama
{
    public partial class Form1 : Form
    {
        OleDbConnection bag = new OleDbConnection("Provider=Microfost.Ace.OleDb.12.0;Data Source=veri.accdb");
        OleDbCommand kmt = new OleDbCommand(); // kmt veri tabanından bilgiyi okumak için kullanacağız

        public static string portismi, banthizi;
        string[] ports = SerialPort.GetPortNames();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  // bağlan butonu
        {
            
            
            timer1.Start();
            portismi = comboBox1.Text;
            banthizi = comboBox2.Text;

            try
            {

                serialPort1.PortName = portismi;
                serialPort1.BaudRate = Convert.ToInt16(banthizi);

                serialPort1.Open();

                label1.Text = "Bağlantı Sağlandı";
                label1.ForeColor = Color.Green;
            }
            catch
            {
                serialPort1.Close();
                serialPort1.Open();
                MessageBox.Show("Bağlantı Zaten Açık");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if(serialPort1.IsOpen==true)
            {
                serialPort1.Close();
                label1.Text = "Bağlantı Kesildi";
                label1.ForeColor = Color.Red;

            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();

            }
        }

        private void timer1_Tick(object sender, EventArgs e) //kart uid  sürekli denetlemek için zamanlıyıcı kullanmak
        {
            string sonuc;
            sonuc = serialPort1.ReadExisting();

            if (sonuc !="")
            {

                label2.Text = sonuc;
                bag.Open();
                kmt.Connection = bag;
                kmt.CommandText = "SELECT + FROM Tablo WHERE kid=2'" + sonuc + "'";

                OleDbDataReader oku = kmt.ExecuteReader();
                
                if(oku.Read())
                {
                    DateTime bugun = DateTime.Now;
                    pictureBox1.Image = Image.FromFile("foto\\" + oku["Resim"].ToString());
                    label8.Text = oku["Adı Soyadı"].ToString();
                    label9.Text = oku["Doğum Tarihi"].ToString();
                    label13.Text = oku["TC Kimlik Numarası"].ToString();
                    label15.Text = oku["HES Kodu"].ToString();
                    label17.Text = oku["Test Sonucu"].ToString();
                    label10.Text = bugun.ToShortDateString();
                    label11.Text = bugun.ToLongTimeString();
                    bag.Close();

                    bag.Open();
                    kmt.CommandText = "INSERT INTO Zaman (Adı Soyadı,Tarih,Saat)VALUES ('" + label8.Text + "','" + label10.Text + "','" + label11.Text + "')";
                    kmt.ExecuteReader();
                    bag.Close();

                }
                else
                {
                    pictureBox1.Image = Image.FromFile("foto\\yasak.png"); //hata verirse resim formatını jpg yap
                    label2.Text = "Kart Kayıtlı Değil";
                    label8.Text = "----------";
                    label9.Text = "----------";
                    label10.Text = "----------";
                    label11.Text = "----------";
                    label13.Text = "----------";
                    label15.Text = "----------";
                    label17.Text = "----------";

                }
                bag.Close();
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(portismi==null || banthizi==null)
            {
                MessageBox.Show("Bağlantını Kontrol Et");
            }
            else
            {
                timer1.Stop();
                serialPort1.Close();
                label1.Text = "Bağlantı Kapalı";
                label1.ForeColor = Color.Red;

                Kayit kyt = new Kayit();
                kyt.ShowDialog();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                foreach (string port in ports)
            {
                comboBox1.Items.Add(port); // port ismi tanımlama
            }
            comboBox2.Items.Add("2400");  // okuma bant hızı = 0 İNDEX NUMARASI OLARAK BAŞLIYOR
            comboBox2.Items.Add("3600");  
            comboBox2.Items.Add("4800");
            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("19200");
            comboBox2.Items.Add("115200");

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 3;
        }
    }
}
