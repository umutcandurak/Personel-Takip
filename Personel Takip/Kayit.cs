using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Hesli_Kimlik_Doğrulama
{
    public partial class Kayit : Form
    {
        OleDbConnection bag = new OleDbConnection("Provider=Microfost.Ace.OleDb.12.0;Data Source=veri.accdb");
        OleDbCommand kmt = new OleDbCommand(); // kmt adında komur değişkeni tanımladım
        public Kayit()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string sonuc;
            sonuc = serialPort1.ReadExisting();

            if (sonuc != "")
            {

                label6.Text = sonuc;
            }
        }

        private void Kayit_Load(object sender, EventArgs e)
        {
            serialPort1.PortName = Form1.portismi;
            serialPort1.BaudRate = Convert.ToInt16(Form1.banthizi); 

            if(serialPort1.IsOpen== false)
            {
                try
                {
                    serialPort1.Open();
                    label7.Text = "Bağlantı Sağlandı";
                    label7.ForeColor = Color.Green;
                }
                catch
                {
                    label7.Text = "Bağlantı Sağlanamadı";
                }
            }
            else
            {
                label7.Text = "Bağlantı Sağlanamadı";
                label7.ForeColor = Color.Red;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
            label6.Text = "----------";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text= "";
            comboBox1.Text = "Seçiniz";
            textBox5.Text = "";
            label8.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyaları (jpg) | *.jpg|Tüm Dosyaları | *.*";
            openFileDialog1.InitialDirectory=Application.StartupPath+ "\\foto";
            dosya.RestoreDirectory = true;

            if (dosya.ShowDialog() == DialogResult.OK)
            {
                string di = dosya.SafeFileName;
                textBox5.Text = di;

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label6.Text == "----------" || textBox1.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox2.Text == "" || comboBox1.Text=="Seçiniz" || textBox5.Text =="")
            {
                label8.Text = "Bilgileri Eksiksiz Giriniz";
                label8.ForeColor = Color.Red;
            }
            else
            {
                try
                {


                    bag.Open();
                    kmt.Connection = bag;
                    kmt.CommandText = "INSERT INTO Tablo (Kart ID, Adı Soyadı, Doğum Tarihi, TC Kimlik Numarası, HES Kodu, Test Sonucu, Resim)VALUES ('" + label6.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','"+ textBox5.Text+"','"+ comboBox1.Text + "')";
                    kmt.ExecuteNonQuery();
                    label8.Text = "Kayıt Yapıldı";
                    label8.ForeColor = Color.Green;

                    bag.Close();
                }
                catch
                {
                    bag.Close();    
                    MessageBox.Show("Bu Kart Zaten Kayıtlı");
                }
            }
        


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Kayit_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            serialPort1.Close();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
