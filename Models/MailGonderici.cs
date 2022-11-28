using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HastaKayıtProgramı.Models;

namespace HastaKayıtProgramı.Models
{
    class MailGonderici
    {
        HastaKayitProgramiEntities1 db = new HastaKayitProgramiEntities1();
       
        public void Microsoft(string TcNo , string AliciMail, string GondericiParola)
        {
          
            
            Kullanicilar p = db.Kullanicilar.FirstOrDefault(x => x.Email == AliciMail && x.TcKimlikNo==TcNo);
            string sifre = p.Parola;           
            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = "smtp.outlook.com";
            sc.EnableSsl = true;
            sc.Credentials = new NetworkCredential(AliciMail, GondericiParola);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(AliciMail, "Hasta Kayıt Programı");
            mail.To.Add(AliciMail);
            mail.Subject = "Şifre Talebinde Bulundunuz";
            mail.IsBodyHtml = true;
            mail.Body = $@"{DateTime.Now.ToString()} Tarihinde şifre talebinde bulundunuz. Yeni şifreniz {p.Parola}";
            //sc.Timeout=100;
            sc.Send(mail);

        }
    }
}
