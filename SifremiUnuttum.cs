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
    public partial class SifremiUnuttum : Form
    {
        HastaKayitProgramiEntities1 db = new HastaKayitProgramiEntities1();
       
        public SifremiUnuttum()
        {
            InitializeComponent();
        }

        MailGonderici mg = new MailGonderici();
        private void button1_Click(object sender, EventArgs e)
        {
            var durum = db.Kullanicilar.FirstOrDefault(x => x.TcKimlikNo == textBox1.Text && x.Email == textBox2.Text);

            if (durum!=null)
            {
                mg.Microsoft(textBox1.Text, textBox2.Text,textBox3.Text);
                MessageBox.Show("Kullanıcı Şifreniz :" + textBox2.Text + "adresine gönderilmiştir.","Şifre İşlemleri", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Vermiş olduğunuz bilgilerde kullanıcı bulunamamıştır.", "Şifre İşlemleri", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                textBox3.Text = "";
            }
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13 || (int)e.KeyChar == 9)
            {
                textBox3.Focus();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13 || (int)e.KeyChar == 9)
            {
                button1.PerformClick();
            }
        }
    }
}
