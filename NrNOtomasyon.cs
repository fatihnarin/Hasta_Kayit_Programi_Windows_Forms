using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HastaKayıtProgramı.Models;
//Regex kütüphanesi
using System.Text.RegularExpressions;
//Giriş-çıkış iş
using System.IO;

namespace HastaKayıtProgramı.Models
{
    public partial class NrNOtomasyon : Form
    {
        public NrNOtomasyon()
        {
            InitializeComponent();
        }

        HastaKayitProgramiEntities1 db = new HastaKayitProgramiEntities1();
        private void NrNOtomasyon_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'hastaKayitProgramiDataSet1.Kullanicilar' table. You can move, or remove it, as needed.
            this.kullanicilarTableAdapter.Fill(this.hastaKayitProgramiDataSet1.Kullanicilar);
            // TODO: This line of code loads data into the 'hastaKayitProgramiDataSet.AktifRandevular' table. You can move, or remove it, as needed.

            Grid1Doldur();
            // grid2Doldur();
            grid4Doldur();
            grid5Doldur();
            var durum = db.Kullanicilar.FirstOrDefault(x => x.TcKimlikNo == textBox1.Text);

            if (durum.KullaniciTipi == "Yönetici")
            {
                tabControl1.TabPages.Remove(tabPage3);
            }
            else if (durum.KullaniciTipi == "Danışman")
            {
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage6);
                var durum1 = db.Kullanicilar.FirstOrDefault(x => x.TcKimlikNo == textBox1.Text);
                string danisan_ = "";
                danisan_ = durum1.Ad + " " + durum1.Soyad;
                grid2Doldur(danisan_);

            }
            else if (durum.KullaniciTipi == "Sekreter")
            {
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage6);
                tabControl1.TabPages.Remove(tabPage3);
            }

        }
        
        // **************************TABPAGE1 START********************************
        private void btnBilgilerimiGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                var durum = db.Kullanicilar.FirstOrDefault(x => x.TcKimlikNo == textBox1.Text);
                DialogResult dialogResult = MessageBox.Show("Bilgilerinizi güncellemek istediğinize eminmisiniz?", "DİKKAT", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    if (comboBox1.Text != "" && textBox42.Text.Length == 42 && textBox43.Text.Length > 8 && richTextBox2.Text.Length > 20)
                    {
                        durum.Durum = comboBox1.Text;
                        durum.TelefonNo = textBox42.Text;
                        durum.Email = textBox43.Text;
                        durum.Adres = richTextBox2.Text;
                        db.SaveChanges();
                        MessageBox.Show("Güncelleşme gercekleşti.", "DURUM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Tüm alanları eksiksiz doldurun", "Günceleşme Gerçekleşmedi!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                comboBox1.Text = durum.Durum;
                textBox42.Text = durum.TelefonNo;
                textBox43.Text = durum.Email;
                richTextBox2.Text = durum.Adres;
            }
            catch (Exception)
            {

                MessageBox.Show("Veritabanıyla bağlantı kurulamadı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnSifreDegistir_Click(object sender, EventArgs e)
        {
            try
            {
                var durum = db.Kullanicilar.FirstOrDefault(x => x.TcKimlikNo == textBox1.Text && x.Parola == textBox18.Text);
                if (durum != null)
                {
                    if (textBox4.Text == textBox5.Text)
                    {
                        if (progressBar1.Value >= 70)
                        {
                            durum.Parola = textBox4.Text;
                            db.SaveChanges();
                            MessageBox.Show("Şifreniz başarı ile değiştirilmiştir.");
                        }
                        else
                        {
                            MessageBox.Show("Parola güçü %70'ten küçük olamaz!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Şifre tekrarı aynı değil", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Mevcut şifrenizi hatalı girdiniz!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {

            }

        }
        //******************PAROLA DEĞİŞİKLİK İŞLEMLERİ START********************
        int parola_skor = 0;
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string parola_seviyesi = "";
            int kucuk_harf_skoru = 0, buyuk_harf_skoru = 0, rakam_skoru = 0, sembol_skoru = 0;
            string sifre = textBox4.Text;
            //Regex kütüphanesi İngilizce karakterleri baz altığından Türkçe karakterleri ingilizce karakterlere çevirir.

            string düzeltirmis_sifre = "";
            düzeltirmis_sifre = sifre;
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('İ', 'I');
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('ı', 'i');
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('Ç', 'C');
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('ç', 'c');
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('Ş', 'Ş');
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('ş', 's');
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('Ğ', 'G');
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('ğ', 'g');
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('Ü', 'U');
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('ü', 'u');
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('Ö', 'O');
            düzeltirmis_sifre = düzeltirmis_sifre.Replace('ö', 'o');
            if (sifre != düzeltirmis_sifre)
            {
                sifre = düzeltirmis_sifre;
                textBox5.Text = sifre;
                MessageBox.Show("Paroladaki Türkçe karakterler İngilizce karakterlere dönüştürülmüştür!");
            }
            //1 kücük harf10 puan, 2 ve üzeri 20 puan
            int az_karakter_sayısı = sifre.Length - Regex.Replace(sifre, "[a-z]", "").Length;
            kucuk_harf_skoru = Math.Min(2, az_karakter_sayısı) * 10;

            //1 büyük harf 10 puan, 2 ve üzeri 20 puan
            int AZ_karakter_sayısı = sifre.Length - Regex.Replace(sifre, "[A-Z]", "").Length;
            buyuk_harf_skoru = Math.Min(2, AZ_karakter_sayısı) * 10;

            //1rakam  10 puan, 2 ve üzeri 20 puan
            int rakam_sayisi = sifre.Length - Regex.Replace(sifre, "[0-9]", "").Length;
            rakam_skoru = Math.Min(2, rakam_sayisi) * 10;

            //1sembol  10 puan, 2 ve üzeri 20 puan
            int sembol_sayisi = sifre.Length - az_karakter_sayısı - AZ_karakter_sayısı - rakam_sayisi;
            sembol_skoru = Math.Min(2, sembol_sayisi) * 10;

            parola_skor = kucuk_harf_skoru + buyuk_harf_skoru + rakam_skoru + sembol_skoru;

            if (sifre.Length == 9)
                parola_skor += 10;
            if (sifre.Length >= 10)
                parola_skor += 20;

            if (kucuk_harf_skoru == 0 || buyuk_harf_skoru == 0 || rakam_skoru == 0 || sembol_skoru == 0)
                label66.Text = "Büyük harf, küçük harf, rakam ve sembol mutlaka kullanmalısın!";
            if (kucuk_harf_skoru != 0 && buyuk_harf_skoru != 0 && rakam_skoru != 0 && sembol_skoru != 0)
                label66.Text = "";
            if (parola_skor < 70)
                parola_seviyesi = "kabul edilemez!";
            else if (parola_skor == 70 || parola_skor == 80)
                parola_seviyesi = "Güçlü";
            else if (parola_skor == 90 || parola_skor == 100)
                parola_seviyesi = "Çok Güçlü";

            label67.Text = "%" + Convert.ToString(parola_skor);
            label68.Text = parola_seviyesi;
            progressBar1.Value = parola_skor;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text != textBox4.Text)
            {
                errorProvider1.SetError(textBox5, "Parola tekrarı eşleşmiyor!");
            }
            else
            {
                errorProvider1.Clear();

            }
        }
        //******************PAROLA DEĞİŞİKLİK İŞLEMLERİ FINISH********************
        // **************************TABPAGE1 FINISH********************************

        // **************************TABPAGE2 START********************************
        private void btnDanisanKayitAra_Click(object sender, EventArgs e)
        {
            try
            {
                var durum = db.Danisanlar.FirstOrDefault(x => x.TcKimlikNo == textBox25.Text);
                if (durum != null)
                {
                    textBox25.Enabled = false;
                    textBox26.Text = durum.Ad;
                    textBox27.Text = durum.Soyad;
                    textBox28.Text = durum.Telefon;
                    textBox29.Text = durum.Email;
                    richTextBox4.Text = durum.Adres;
                    comboBox7.Text = durum.Cinsiyet;
                    dateTimePicker1.Value = durum.DogumTarihi.Date;
                    btnDanisanKayitGuncelle.Enabled = true;
                    btnDanisanKaydet.Enabled = false;
                    btnDanisanKayitSil.Enabled = true;

                }
                else
                {
                    MessageBox.Show(textBox25.Text + "Tc Kimlik Numaralı Kayıt Bulunamadı.Yeni Kayıt yapmalısınız", "DURUM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Veritabanıyla bağlantı kurulamadı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void btnDanisanKayitTemizle_Click(object sender, EventArgs e)
        {
            temizledanisankayit();
        }
        private void btnDanisanKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var durum = db.Danisanlar.FirstOrDefault(x => x.TcKimlikNo == textBox25.Text);
                if (durum == null)
                {
                    if (textBox25.TextLength == 11 && textBox26.TextLength >= 2 && textBox27.TextLength >= 2 && textBox28.TextLength == 11
                        && textBox29.TextLength >= 8 && richTextBox4.TextLength >= 20 && comboBox7.Text != "")
                    {
                        if (dateTimePicker1.Value < DateTime.Today)
                        {
                            Danisanlar yeni = new Danisanlar();
                            yeni.TcKimlikNo = textBox25.Text;
                            yeni.Ad = textBox26.Text;
                            yeni.Soyad = textBox27.Text;
                            yeni.Telefon = textBox28.Text;
                            yeni.Email = textBox29.Text;
                            yeni.Adres = richTextBox4.Text;
                            yeni.DogumTarihi = dateTimePicker1.Value.Date;
                            yeni.Cinsiyet = comboBox7.Text;
                            db.Danisanlar.Add(yeni);
                            db.SaveChanges();
                            MessageBox.Show(textBox25.Text + " Tc Kimlik Nolu danışan kaydı veritabanına kaydedildi.", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            temizledanisankayit();
                        }
                        else
                        {
                            MessageBox.Show("Doğum tarihi ileri bir tarih olamaz");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tüm alanları eksiksiz doldurun!!", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show(textBox25.Text + "Tc Kimlik Numaralı Kayıt Zaten Var!!!", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Veritabanıyla bağlantı kurulamadı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void btnDanisanKayitSil_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Kayıtı Silmek İstediğinizden eminmisiniz", "Dikkat Kayıt Silinecek", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                {
                    var durum1 = db.AktifRandevular.FirstOrDefault(x => x.TcKimlikNo == textBox25.Text);
                    var durum2 = db.DanisanMuayeneTablosu.FirstOrDefault(x => x.TcKimlikNo == textBox25.Text);
                    if (durum1 != null)
                    {
                        MessageBox.Show("Aktif Randevusu olan hastanın kaydı silinemez!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (durum2 != null)
                    {
                        MessageBox.Show("Daha önce muayene olan hastanın kaydı silinemez", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        var sil = db.Danisanlar.Where(w => w.TcKimlikNo == textBox25.Text).FirstOrDefault();
                        db.Danisanlar.Remove(sil);
                        db.SaveChanges();
                        MessageBox.Show(textBox15.Text + "Tc kimlik numaralı hasta kaydı silindi.");
                        temizledanisankayit();
                    }
                }
                else
                {
                    temizledanisankayit();
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Veritabanıyla bağlantı kurulamadı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void btnDanisanKayitGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox25.TextLength == 11 && textBox26.TextLength >= 2 && textBox27.TextLength >= 2 && textBox28.TextLength == 11
                    && textBox29.TextLength >= 8 && richTextBox4.TextLength >= 20 && comboBox7.Text != "")
                {
                    if (dateTimePicker1.Value <= DateTime.Now)
                    {
                        var durum = db.Danisanlar.FirstOrDefault();
                        durum.Ad = textBox26.Text;
                        durum.Soyad = textBox27.Text;
                        durum.Telefon = textBox28.Text;
                        durum.Email = textBox29.Text;
                        durum.Adres = richTextBox4.Text;
                        durum.DogumTarihi = dateTimePicker1.Value;
                        durum.Cinsiyet = comboBox7.Text;
                        db.SaveChanges();
                        MessageBox.Show("Güncelleşme gercekleşti.", "DURUM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        temizledanisankayit();

                    }
                    else
                    {
                        MessageBox.Show("Doğum tarihi ileri bir tarih olamaz");
                    }
                }
                else
                {
                    MessageBox.Show("Tüm alanları eksiksiz doldurun!!", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Veritabanıyla bağlantı kurulamadı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void temizledanisankayit()
        {
            textBox25.Text = "";
            textBox26.Text = "";
            textBox27.Text = "";
            textBox28.Text = "";
            textBox29.Text = "";
            richTextBox4.Text = "";
            comboBox7.Text = "";
            textBox25.Enabled = true;
            textBox25.Focus();
            dateTimePicker1.Value = DateTime.Now;
            btnDanisanKaydet.Enabled = true;
            btnDanisanKayitGuncelle.Enabled = false;
            btnDanisanKayitSil.Enabled = false;
        }
        private void btnRandevuKayitAra_Click(object sender, EventArgs e)
        {
            try
            {
                var durum = db.Danisanlar.FirstOrDefault(x => x.TcKimlikNo == textBox31.Text);
                if (durum != null)
                {
                    textBox31.Enabled = false;
                    textBox32.Text = durum.Ad;
                    textBox33.Text = durum.Soyad;
                    btnRandevuKayitTemizle.Enabled = true;
                    btnRandevuKaydet.Enabled = true;
                    btnRandevuKayitAra.Enabled = false;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    dateTimePicker2.Enabled = true;
                    danismanCombobax(comboBox3);
                }
                else
                {
                    MessageBox.Show(textBox25.Text + "Tc Kimlik Numaralı Kayıt Bulunamadı.Yeni Kayıt yapmalısınız", "DURUM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Veritabanıyla bağlantı kurulamadı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void comboBox3_MouseClick(object sender, MouseEventArgs e)
        {
            danismanCombobax(comboBox3);
        }
        private void danismanCombobax(object sender)
        {
            ((ComboBox)sender).Items.Clear();
            var oku = db.Kullanicilar.Where(w => w.KullaniciTipi == "Danışman" && w.Durum == "Aktif").ToList();
            foreach (var item in oku)
            {
                string a = item.Ad + " " + item.Soyad;
                ((ComboBox)sender).Items.Add(a);
            }
        }
        private void btnRandevuKayitTemizle_Click(object sender, EventArgs e)
        {
            randevuTemizle();
        }
        private void randevuTemizle()
        {
            textBox31.Text = "";
            textBox32.Text = "";
            textBox33.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            dateTimePicker2.Value = DateTime.Now;
            textBox31.Enabled = true;
            btnRandevuKayitTemizle.Enabled = false;
            btnRandevuKaydet.Enabled = false;
            btnRandevuKayitAra.Enabled = true;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            dateTimePicker2.Enabled = false;
        }
        private void btnRandevuKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var durum = db.AktifRandevular.FirstOrDefault(x => x.TcKimlikNo == textBox31.Text && x.DanismanAdiSoyadi == comboBox3.Text);
                if (durum == null)
                {
                    if (comboBox3.Text != "" && comboBox4.Text != "")
                    {
                        if (dateTimePicker2.Value < DateTime.Today)
                        {
                            MessageBox.Show(" Randevu tarihi geçmiş zaman olamaz", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            AktifRandevular yeni = new AktifRandevular();
                            yeni.TcKimlikNo = textBox31.Text;
                            yeni.Ad = textBox32.Text;
                            yeni.Soyad = textBox33.Text;
                            yeni.DanismanAdiSoyadi = comboBox3.Text;
                            yeni.RandevuTarihi = dateTimePicker2.Value.Date;
                            yeni.RandevuSaati = comboBox4.Text;
                            db.AktifRandevular.Add(yeni);
                            db.SaveChanges();
                            MessageBox.Show(textBox31.Text + " Tc Kimlik Nolu danışan randevu kaydı yapıldı.", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Grid1Doldur();
                            randevuTemizle();
                        }
                    }
                    else
                    {
                        MessageBox.Show(" Tüm alanları eksiksiz doldurun", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(textBox31.Text + " Tc kimlik nolu kullanıcının aynı danışman ile aktif " +
                        "randevusu zaten var", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Veritabanıyla bağlantı kurulamadı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void btnRandevuKayitSil_Click(object sender, EventArgs e)
        {
            try
            {
                var durum = db.AktifRandevular.FirstOrDefault(x => x.TcKimlikNo == textBox31.Text && x.DanismanAdiSoyadi == comboBox3.Text);
                if (durum != null)
                {
                    DialogResult dialogResult = MessageBox.Show(textBox31 + " Tc Kimlik Nolu hastanın " + comboBox3.Text + " danışanla olan aktif randevusunu silmek istediğinize emin misiniz?", "DİKKAT", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var sil = db.AktifRandevular.Where(w => w.TcKimlikNo == textBox31.Text && w.DanismanAdiSoyadi == comboBox3.Text).FirstOrDefault();
                        db.AktifRandevular.Remove(sil);
                        db.SaveChanges();
                        MessageBox.Show(textBox31 + " Tc Kimlik Nolu hastanın " + comboBox3.Text + " danışanla olan aktif randevusunu silindi.");
                        grid4Doldur();
                        randevuTemizle();
                    }
                    else
                    {
                        textBox15.Text = "";
                        textBox16.Text = "";
                        textBox17.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Girelen bilgilerde randevu kaydı bulunamadı");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Veritabanıyla bağlantı kurulamadı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void btnSilinecekRandevuSec_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.TabIndex > -1)
                {
                    danismanCombobax(comboBox3);
                    textBox31.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    textBox32.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    textBox33.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    comboBox3.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    dateTimePicker2.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    comboBox4.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    btnRandevuKayitTemizle.Enabled = true;
                    btnRandevuKayitAra.Enabled = false;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Bir hata ile karşılaşıldı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // **************************TABPAGE2 FINISH********************************

        // **************************TABPAGE3 START********************************
        private void btnHastaKabul_Click(object sender, EventArgs e)
        {
            if (dataGridView2.TabIndex>-1)
            {
                textBox34.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                textBox35.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
                textBox36.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
                textBox34.Enabled = false;
                textBox35.Enabled = false;
                textBox36.Enabled = false;
                grid3Doldur();
            }
            
        }
        private void btnHastaGelmedi_Click(object sender, EventArgs e)
        {
            if (dataGridView2.TabIndex > -1)
            {
                textBox34.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                textBox35.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
                textBox36.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
           
            }
            muayeneKaydet("Hasta muayeneye gelmedi");
            randevukaydettemizle();
            Grid1Doldur();
            grid3Doldur();
        }
        private void btnHastaRet_Click(object sender, EventArgs e)
        {
            if (dataGridView2.TabIndex > -1)
            {
                textBox34.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                textBox35.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
                textBox36.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();

            }
            muayeneKaydet("Muayene danişman tarafından retedildi");
            randevukaydettemizle();
            Grid1Doldur();
            grid3Doldur();
        }
        private void btnMuayeneKaydet_Click(object sender, EventArgs e)
        {
            muayeneKaydet(richTextBox1.Text);
            randevukaydettemizle();
            Grid1Doldur();
            grid3Doldur();

        }
        private void randevukaydettemizle()
        {
            textBox34.Text = "";
            textBox35.Text = "";
            textBox36.Text = "";
            richTextBox1.Text = "";
        }

        private void muayeneKaydet(string a)
        {
            try
            {
                string danisman_ = textBox2.Text + " " + textBox3.Text;
                var durum1 = db.Danisanlar.FirstOrDefault(x => x.TcKimlikNo == textBox34.Text);
                var durum2 = db.AktifRandevular.FirstOrDefault(x => x.TcKimlikNo == textBox34.Text && x.DanismanAdiSoyadi == danisman_);

                DanisanMuayeneTablosu yeni = new DanisanMuayeneTablosu();
                yeni.TcKimlikNo = textBox34.Text;
                yeni.Ad = textBox35.Text;
                yeni.Soyad = textBox36.Text;
                yeni.Cinsiyet =durum1.Cinsiyet;
                yeni.Yas = Convert.ToByte(DateTime.Today.Year - durum1.DogumTarihi.Year);
                yeni.MuayeneSonucları = a;
                yeni.DanismanAdi = textBox2.Text;
                yeni.DanismanSoyadi = textBox3.Text;
                yeni.MuayeneTarihi = durum2.RandevuTarihi;
                yeni.MuayeneSaati = durum2.RandevuSaati;
                db.DanisanMuayeneTablosu.Add(yeni);
                db.SaveChanges();
                MessageBox.Show(textBox34.Text + " Tc Kimlik Nolu hastanın muayenesi veritabanına kaydedildi.", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RandevuSil();
            }
            catch (Exception)
            {

                MessageBox.Show("Muayene kaydı yapılamadı !!");
            }
        }
        private void RandevuSil()
        {
            try
            {
                string danisman_ = textBox2.Text + " " + textBox3.Text;
                var sil = db.AktifRandevular.FirstOrDefault(x => x.TcKimlikNo == textBox34.Text && x.DanismanAdiSoyadi == danisman_);
                db.AktifRandevular.Remove(sil);
                db.SaveChanges();
                MessageBox.Show(textBox15.Text + "Tc kimlik numaralı hastanın aktif rendevusu silindi");
                grid2Doldur(danisman_);
            }
            catch (Exception)
            {

                MessageBox.Show(textBox15.Text + "Tc kimlik numaralı hastanın aktif rendevusu veritabanından silinemedi.");
            }

        }
        // **************************TABPAGE3 FINISH********************************

        // **************************TABPAGE5 START********************************
        private void btnKullaniciKayit_Click_1(object sender, EventArgs e)
        {
            try
            {
                var durum = db.Kullanicilar.FirstOrDefault(x => x.TcKimlikNo == textBox8.Text);
                if (textBox8.Text.Length == 11 && textBox9.Text.Length >= 2 && textBox10.Text.Length >= 2 && comboBox2.Text != null &&
                   textBox11.Text.Length == 11 && textBox12.Text.Length >= 5 && textBox13.Text.Length >= 8 && richTextBox3.Text.Length >= 20)
                {
                    if (durum == null)
                    {

                        Kullanicilar yeni = new Kullanicilar();
                        yeni.TcKimlikNo = textBox8.Text;
                        yeni.Ad = textBox9.Text;
                        yeni.Soyad = textBox10.Text;
                        yeni.KullaniciTipi = comboBox2.Text;
                        yeni.TelefonNo = textBox11.Text;
                        yeni.Email = textBox12.Text;
                        yeni.Parola = textBox13.Text;
                        yeni.Adres = richTextBox3.Text;
                        db.Kullanicilar.Add(yeni);
                        db.SaveChanges();
                        MessageBox.Show(textBox8.Text, "TC kimlik numaları kullanıcı veri tabanına kaydedildi.");
                        grid4Doldur();
                        temizle();

                    }
                    else
                    {
                        MessageBox.Show(textBox8.Text, "TC kimlik numaralı kayıt zaten var");
                        temizle();
                    }
                }
                else
                {
                    MessageBox.Show("Tüm alanların doldurunuz!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Veritabanıyla bağlantı kurulamadı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void temizle()
        {
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            richTextBox3.Text = "";
            comboBox2.Text = "";
            comboBox8.Text = "";
        }
        private void btnSilinecekKullaniciAra_Click(object sender, EventArgs e)
        {
            try
            {
                var durum = db.Kullanicilar.FirstOrDefault(x => x.TcKimlikNo == textBox15.Text);
                if (durum != null)
                {
                    textBox16.Text = durum.Ad;
                    textBox17.Text = durum.Soyad;
                    btnKullaniciKayitSil.Enabled = true;
                }
                else
                {
                    textBox15.Text = "";
                    btnKullaniciKayitSil.Enabled = false;
                    MessageBox.Show(textBox15.Text, "Tc Kimlik numaralı kullanıcı bulunamadı");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Veritabanıyla bağlantı kurulamadı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnKullaniciKayitSil_Click(object sender, EventArgs e)
        {
            try
            {
                var durum = db.Kullanicilar.FirstOrDefault(x => x.TcKimlikNo == textBox15.Text && x.KullaniciTipi == "Yönetici");
                if (durum == null)
                {
                    DialogResult dialogResult = MessageBox.Show("Kayıtı Silmek İstediğinizden eminmisiniz", "Dikkat Kayıt Silinecek", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                    {

                        var sil = db.Kullanicilar.Where(w => w.TcKimlikNo == textBox15.Text).FirstOrDefault();
                        db.Kullanicilar.Remove(sil);
                        db.SaveChanges();
                        MessageBox.Show(textBox15.Text + "Tc kimlik numaralı kullanıcı kayıtı silindi.");
                        textBox15.Text = "";
                        textBox16.Text = "";
                        textBox17.Text = "";
                        grid4Doldur();
                    }
                    else
                    {
                        textBox15.Text = "";
                        textBox16.Text = "";
                        textBox17.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Yönetici kullanıcı kayıtı silinemez", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox15.Text = "";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Veritabanıyla bağlantı kurulamadı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnSilineceKullaniciSec_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView4.TabIndex > -1)
                {
                    textBox15.Text = dataGridView4.CurrentRow.Cells[1].Value.ToString();
                    textBox16.Text = dataGridView4.CurrentRow.Cells[2].Value.ToString();
                    textBox17.Text = dataGridView4.CurrentRow.Cells[3].Value.ToString();                    
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Bir hata ile karşılaşıldı!!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // **************************TABPAGE5 FINISH********************************
       
        // **************************TABPAGE6 START********************************
        private void button26_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Çıkmak istediğinize eminmisiniz?", "ÇIKIŞ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();

            }
        }
        // **************************TABPAGE FINISH********************************
    
        //****************************************************************************
        private void SayiMi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57) || (int)e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void HalfMi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true ||
                char.IsSeparator(e.KeyChar) == true)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void AdSoyad_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length < 2)
            {
                errorProvider1.SetError(((TextBox)sender), "Ad veya soyad en az 2 karakterli olmalı !");
            }
            else
            {
                errorProvider1.Clear();
            }
        }
        private void Tc_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length < 11)
            {
                errorProvider1.SetError((TextBox)sender, "Tc Kimlik No 11 karakterli olmalı !");
            }
            else
            {
                errorProvider1.Clear();
            }
        }
        private void Tel_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length < 11)
            {
                errorProvider1.SetError((TextBox)sender, "Telefon no 11 karakterli olmalı !");
            }
            else
            {
                errorProvider1.Clear();
            }
        }
        //*******************************************************************************
        
        //********************DATAGRIDDOLDUR START************************
        private void Grid1Doldur()
        {
            try
            {
                dataGridView1.DataSource = db.AktifRandevular.ToList();
            }
            catch (Exception Hata)
            {
                MessageBox.Show("Sunucuya ulaşılmıyor !" + Hata.Message.ToString());
            }
        }
        private void grid2Doldur(string a)
        {
            try
            {
                dataGridView2.DataSource = db.AktifRandevular.Where(w => w.DanismanAdiSoyadi == a).ToList();
            }
            catch (Exception Hata)
            {
                MessageBox.Show("Sunucuya ulaşılmıyor !" + Hata.Message.ToString());
            }
        }
        private void grid3Doldur()
        {
            try
            {
                dataGridView3.DataSource = db.DanisanMuayeneTablosu.Where(w => w.TcKimlikNo == textBox34.Text).ToList();
            }
            catch (Exception Hata)
            {
                MessageBox.Show("Sunucuya ulaşılmıyor !" + Hata.Message.ToString());
            }
        }
        private void grid4Doldur()
        {
            try
            {

                dataGridView4.DataSource = db.Kullanicilar.ToList();
            }
            catch (Exception Hata)
            {
                MessageBox.Show("Sunucuya ulaşılmıyor !" + Hata.Message.ToString());
            }
        }
        private void grid5Doldur()
        {
            try
            {
                dataGridView5.DataSource = db.DanisanMuayeneTablosu.ToList();
            }
            catch (Exception Hata)
            {
                MessageBox.Show("Sunucuya ulaşılmıyor !" + Hata.Message.ToString());
            }
        }
        //********************DATAGRIDDOLDUR FINISH************************


        //******************LINQ İşlemleri START******************************       
        private void DataGrid1LinQ_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.AktifRandevular.Where(p => p.TcKimlikNo.Contains(textBox6.Text) && p.Ad.Contains(textBox7.Text) && p.Soyad.Contains(textBox14.Text)).ToList();
        }
        private void DataGrid4LinQ_TextChanged(object sender, EventArgs e)
        {
            dataGridView4.DataSource = db.Kullanicilar.Where(p => p.Ad.Contains(textBox38.Text) && p.TcKimlikNo.Contains(textBox37.Text) && p.Soyad.Contains(textBox39.Text)).ToList();
        }
        private void DataGrid5LinQ_TextChanged(object sender, EventArgs e)
        {
            dataGridView5.DataSource = db.DanisanMuayeneTablosu.Where(p => p.TcKimlikNo.Contains(textBox19.Text) && p.Ad.Contains(textBox20.Text) && p.Soyad.Contains(textBox21.Text)&&
            p.Cinsiyet.Contains(textBox22.Text) && p.DanismanAdi.Contains(textBox23.Text)&& p.DanismanSoyadi.Contains(textBox24.Text)).ToList();
        }
        //*******************LINQ İşlemleri FINISH******************************       
    }
}
