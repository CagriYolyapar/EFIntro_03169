using EFIntro_0.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFIntro_0
{
    public partial class Form1 : Form
    {

        /*
         
         
                       ORM(Object Relational Mapping)

               Veritabanındaki şema yapısını , kullandıgınız programlama dilinde olusturma işlemidir...Class'larınız veritabanınızdaki tablolarla, property'leriniz tablolarınızdaki sütunlarla eşleştirilir...Veritabanınızdaki verileriniz ilgili class'Lardan olusan instance'lar olarak görülür...
         
         
        --Entity Framework

        --Veri erişimi acısından 3 yöntemi vardır...

        a => ModelFirst(Siz manual class yaratarak varolan veritabanınızdaki tablolarla eşleştirmeyi elle yaparsınız)

        b => DBFirst(Database First*)

        c => CodeFirst*


        --DBFirst yaklasımı => Sizin veritabanınız Management Studio'da(Sql Server'da ise) önceden olusturulmustur...Siz projenize bu hazır veritabanını entegre edersiniz...Siz projenize bu hazır veritabanınızı entegre etmek icin EntityFramework'e bir emir verirsiniz...Bu emir karsılıgında  EntityFramework veritabanınızı önce bir sınıf halinde yansıma olarak projenize getirir...Sonra bu veritabanınızdaki tabloları class olarak acar, icerisindeki sütunları property yapar ve verilerinizi de instance olarak getirir...Siz projenizdeki veritabanı işlemlerini veritabanının kendi sınıfından instance alarak yaparken, ilgili verileri eklemek icin tabloların sizin projenizde dönüşmüş oldugu class'lardan instance alarak yaparsınız...
         
         
                  Dezavantajları : DBFirst yaklasımı bir veritabanını en hızlı şekilde projenize entegre edeceginiz bir yaklasımdır...Ancak bunun karsılıgında Entity Framework'un olusturdugu yapı sabittir...(Bir kütüphane kullandıgınız halde siz hakimiyet elinizde kalarak manuel işlemler yapabilirsiniz... Ancak bir kütüphane üzerindeki direkt bir hazır yapıyı kullanıyorsanız o zaman esnekliginizi tamamen kütüphaneye bırakır ve hakimiyetten vazgeciriniz) Bu sabit yapıyı degiştirmek hic saglıklı degildir...Kullanmak istemeyebileceginiz yapıları zoraki icerisine entegre edip sizi onlara bagımlı kılar...DBFirst ile olusturulacak projelerin ancak belirli şartlar gerçekleştirildiginde bu yontemi kabul edecegini bilmeniz gerekir... DBFirst ile olusturulan projelerin ayrıca müsteriye gönderilirken  Veritabanınızın yedegini de alarak ayrı bir şekilde müşteriye kurulumunun yapılması gerekir...


                 Avantajları : Cok hızlı geliştirme yapmanızı saglar. Hazır DB'yi direkt projenize entegre eder...


        --CodeFirst yaklasımı => Sizin veritabanınız yoktur...Veritabanınızı SQL'den de yaratmak istemezsiniz veya belli baslı durumlarda bu olay size hız kazandıracak olmasına ragmen büyük kısıtlılık saglayacagından (DBFirst'e yonlendireceginden dolayı SQL'den yaratmak saglıklı olmaz...Veya Database sisteminizin güvenlik durumundan emin degilseniz güvenlik sistemini kullandıgınız programalama dili üzerinden kurmak istersiniz)...Bu yaklasımda veritabanını C#(veya kullandıgınız programlama dili neyse) kodları ile yaratırsınız...C# icerisinde OOP prensiplerini kullanarak düzgün bir normalizasyon ile class'larınızı tablo olarak gondermek(Migration) CodeFirst yöntemidir...Bütün hakimiyet sizdedir...Her alanda özgürsünüzdür ve her türlü prensibi istediginiz gibi direkt Entity dedigimiz yapılarda kullanabiliriz...


        ---Entity : Veritabanımızdaki tabloları temsil eden sınıflardır...



         
         
         
         
         */



        NorthwindEntities _db;



        public Form1()
        {
            InitializeComponent();
            _db = new NorthwindEntities();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KategorileriListele();
        }

        private void KategorileriListele()
        {
            //DataSource property'si object tipinde veri kabul etse de size verileri listeleyebilmek icin ona List tipinde bir koleksiyon atmalısınız
            lstKategoriler.DataSource = _db.Categories.ToList(); //database'e git, Categories tablosuna gir ve listeye dök...Sonra listeye dökülen yapıları lstKategoriler'in DataSource property'sine aktar...

            //DataSource her zaman sizden List tipinde bir yapı bekler...
            lstKategoriler.DisplayMember = "CategoryName";
            lstKategoriler.SelectedIndex = -1;

            //CommboBox ve ListBox gibi kontrollerin iclerinde tuttugu verilerin sergilenebilmesi adına DisplayMember property'sine sergilenecek verinin property'sini string olarak yazmalısınız...Bu görüntüleme formatlarından sadece bir tanesidie ve sadece bir property sergileyebilir...
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            //Add Demek (Crud, Create) ilgili class'tan yeni bir nesne olusturmak demektir...Bir şey yaratılıyor...

            Category c = new Category();
            c.CategoryName = txtIsim.Text;
            c.Description = txtAciklama.Text;

            _db.Categories.Add(c); //Veritabanında bir degişiklik yaratacak her türlü ifade (add,delete,update) güvenlik önlemi nedeniyle önce transaction dedigimiz bir sistem devreye girer...Transaction, sistemi geri sarmaya hazır bir mekanizmadır...

            CommitleVeListele("Ekleme işlemi");
        }

        private void CommitleVeListele(string islemIsmi)
        {
            _db.SaveChanges();//Bu ifade degişiklikleri kaydedip ilgili transaction'i commitler(yani onaylar)...Bir transaction commitlenmezse yapılan işlem geri sarılır...

            KategorileriListele(); //ListBox'ta güncel kategorileri gözlemleyebilmek icin tekrar bir listeleme yapıyoruz...

            txtAciklama.Text = txtIsim.Text = "";
            MessageBox.Show($"{islemIsmi} gercekleştirildi");
        }

        Category _secilen;

        private void lstKategoriler_Click(object sender, EventArgs e)
        {
            if (lstKategoriler.SelectedIndex > -1) 
            {
                _secilen = lstKategoriler.SelectedItem as Category;
                txtIsim.Text = _secilen.CategoryName;
                txtAciklama.Text = _secilen.Description;
            } 
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if(_secilen != null)
            {
                _db.Categories.Remove(_secilen);
                CommitleVeListele("Silme işlemi");
                SecileniResetle();

            }
            else
            {
                MessageBox.Show("Lutfen önce silmek istediginiz Kategoriyi seciniz");
            }
        }

        private void SecileniResetle()
        {
            _secilen = null;
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if(_secilen != null)
            {
                _secilen.CategoryName = txtIsim.Text;
                _secilen.Description = txtAciklama.Text;
                CommitleVeListele("Guncelleme işlemi");
                SecileniResetle();
            }
            else
            {
                MessageBox.Show("Lutfen güncellenecek Kategoriyi seciniz");
            }
        }
    }
}
