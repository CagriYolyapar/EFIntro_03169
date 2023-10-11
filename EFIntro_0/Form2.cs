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
    public partial class Form2 : Form
    {
        NorthwindEntities _db;
        public Form2()
        {
            InitializeComponent();
            _db = new NorthwindEntities();
        }

        public void CalisanlariListele()
        {
            lstCalisanlar.DataSource = _db.Employees.ToList();
            lstCalisanlar.DisplayMember = "FirstName";
            lstCalisanlar.SelectedIndex = -1;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CalisanlariListele();
        }

        Employee _secilenCalisan;
        private void lstCalisanlar_Click(object sender, EventArgs e)
        {
            if(lstCalisanlar.SelectedIndex > -1)
            {
                _secilenCalisan = lstCalisanlar.SelectedItem as Employee;
                txtIsim.Text = _secilenCalisan.FirstName;
                txtSoyIsim.Text = _secilenCalisan.LastName;
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Employee emp = new Employee();
            emp.FirstName = txtIsim.Text;
            emp.LastName = txtSoyIsim.Text;
            _db.Employees.Add(emp);
            CommitleVeListele();
        }

        private void CommitleVeListele()
        {
            _db.SaveChanges();

            CalisanlariListele();
            txtIsim.Text = txtSoyIsim.Text = string.Empty;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if(_secilenCalisan != null)
            {
                _db.Employees.Remove(_secilenCalisan);
                CommitleVeListele();
                SecileniResetle();
            }
            else
            {
                UyariMesajiCikar();
            }
        }

        private  void UyariMesajiCikar()
        {
            MessageBox.Show("Lütfen önce bir calısan seciniz");
        }

        private void SecileniResetle()
        {
            _secilenCalisan = null;
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if(_secilenCalisan != null)
            {
                _secilenCalisan.FirstName = txtIsim.Text;
                _secilenCalisan.LastName = txtSoyIsim.Text;
                _db.SaveChanges();
                CommitleVeListele();
                SecileniResetle();
            }
            else
            {
                UyariMesajiCikar();
            }
        }
    }
}
