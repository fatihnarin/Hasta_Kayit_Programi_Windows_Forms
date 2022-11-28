using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HastaKayıtProgramı.Models;


namespace HastaKayıtProgramı
{
    public partial class Form1 : Form
    {
        HastaKayitProgramiEntities1 db = new HastaKayitProgramiEntities1();
         
        public Form1()
        {
            InitializeComponent();
        }
        private void btnSifremiUnuttum_Click(object sender, EventArgs e)
        {
            SifremiUnuttum sifremiUnuttum = new SifremiUnuttum();
            sifremiUnuttum.Show();
        }
        int hak = 3;
        private void btnGiris_Click(object sender, EventArgs e)
        {
            try
            {
                var Durum1 = db.Kullanicilar.FirstOrDefault(x => x.TcKimlikNo == textBox1.Text && x.Parola == textBox2.Text && x.KullaniciTipi == "Yönetici" && radioButton1.Checked == true);
                var Durum2 = db.Kullanicilar.FirstOrDefault(x => x.TcKimlikNo == textBox1.Text && x.Parola == textBox2.Text && x.KullaniciTipi == "Danışman" && radioButton2.Checked == true);
                var Durum3 = db.Kullanicilar.FirstOrDefault(x => x.TcKimlikNo == textBox1.Text && x.Parola == textBox2.Text && x.KullaniciTipi == "Sekreter" && radioButton3.Checked == true);
                if (Durum1 != null && hak != 0)
                {

                    this.Hide();
                    NrNOtomasyon nrN = new NrNOtomasyon();
                    nrN.textBox1.Text = textBox1.Text;
                    nrN.textBox2.Text = Durum1.Ad;
                    nrN.textBox3.Text = Durum1.Soyad;
                    nrN.comboBox1.Text = Durum1.Durum;
                    nrN.textBox42.Text = Durum1.TelefonNo;
                    nrN.textBox43.Text = Durum1.Email;
                    nrN.richTextBox2.Text = Durum1.Adres;
                    nrN.ShowDialog();
                    this.Close();
                }
                else if (Durum2 != null && hak != 0)
                {
                    this.Hide();
                    NrNOtomasyon nrN = new NrNOtomasyon();
                    nrN.textBox1.Text = textBox1.Text;
                    nrN.textBox2.Text = Durum2.Ad;
                    nrN.textBox3.Text = Durum2.Soyad;
                    nrN.comboBox1.Text = Durum2.Durum;
                    nrN.textBox42.Text = Durum2.TelefonNo;
                    nrN.textBox43.Text = Durum2.Email;
                    nrN.richTextBox2.Text = Durum2.Adres;
                    nrN.ShowDialog();
                    this.Close();
                }
                else if (Durum3 != null && hak != 0)
                {
                    this.Hide();
                    NrNOtomasyon nrN = new NrNOtomasyon();
                    nrN.textBox1.Text = textBox1.Text;
                    nrN.textBox2.Text = Durum3.Ad;
                    nrN.textBox3.Text = Durum3.Soyad;
                    nrN.comboBox1.Text = Durum3.Durum;
                    nrN.textBox42.Text = Durum3.TelefonNo;
                    nrN.textBox43.Text = Durum3.Email;
                    nrN.richTextBox2.Text = Durum3.Adres;
                    nrN.ShowDialog();
                    this.Close();
                }
                else
                {
                    hak--;
                    MessageBox.Show("Hatalı Giriş", "Hasta Kayıt Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox1.Focus();
                }
                label5.Text = Convert.ToString(hak);
                if (hak == 0)
                {
                    MessageBox.Show("Giriş hakkınız doldu. Program kapatılacak", "Hasta Kayıt Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Veritabanı ile bağlantı kurulamatı", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void temizle()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label5.Text = Convert.ToString(hak);
            radioButton1.Checked = true;
            this.StartPosition = FormStartPosition.CenterScreen;// formu ortalama
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            textBox1.MaxLength = 11;
            textBox1.Focus();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            MessageBox.Show("GÜLE GÜLE ...", "Hasta Kayıt Programı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 11)
            {
                errorProvider1.SetError(textBox1, "TC Kimlik No 11 karakter olmalı!");
            }
            else
            {
                errorProvider1.Clear();
            }
        }
        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57) || (int)e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar == 13 || (int)e.KeyChar == 9)
            {
                textBox2.Focus();
            }
        }

       
    }
}
