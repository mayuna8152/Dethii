using Dethi02.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dethi02
{
    public partial class frmSanPham : Form
    {
        Model1 model = new Model1();
        public frmSanPham()
        {

            InitializeComponent();
        }
       
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }
            var find = model.SanPhams.FirstOrDefault(s => s.MaSP == txtMaSP.Text);
            if (find == null)
            {
                SanPham sp = new SanPham()
                {
                    MaSP = txtMaSP.Text,
                    TenSP = txtTenSP.Text,
                    NgayNhap = dateTimePicker1.Value,
                    MaLoai = ((LoaiSP)cbLoai.SelectedItem)?.MaLoai
                };
                model.SanPhams.Add(sp);
                model.SaveChanges();
            }

            List<SanPham> ls = model.SanPhams.ToList();
            BindList(ls);

        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
          
            List<SanPham> listSP = model.SanPhams.ToList();
            List<LoaiSP> listLoaiSP = model.LoaiSPs.ToList();
            FillTenLoaiCombobox(listLoaiSP);
            BindList(listSP);
        }
        private void BindList(List<SanPham> sanPhams)
        {
            listView1.Items.Clear();
            foreach (SanPham items in sanPhams)
            {
                var list = new ListViewItem(items.MaSP);
                list.SubItems.Add(items.TenSP);
                list.SubItems.Add(items.NgayNhap.ToString("dd/MM/yyyy"));
                string tenLoai = items.LoaiSP != null ? items.LoaiSP.TenLoai : "";
                list.SubItems.Add(tenLoai);
                listView1.Items.Add(list);
            }
        }
        private void FillTenLoaiCombobox(List<LoaiSP> loaiSPs)
        {
            this.cbLoai.DataSource = loaiSPs;
            this.cbLoai.DisplayMember = "TenLoai";
            this.cbLoai.ValueMember = "MaLoai";
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string maSP = txtMaSP.Text;
            string tenSP = txtTenSP.Text;
            DateTime ngayNhap = dateTimePicker1.Value;
            string maLoai = ((LoaiSP)cbLoai.SelectedItem)?.MaLoai;


            SanPham existingSanPham = model.SanPhams.FirstOrDefault(sp => sp.MaSP == maSP);

            if (existingSanPham != null)
            {

                existingSanPham.TenSP = tenSP;
                existingSanPham.NgayNhap = ngayNhap;
                existingSanPham.MaLoai = maLoai;
                model.SaveChanges();
                List<SanPham> ls = model.SanPhams.ToList();
                BindList(ls);

                txtMaSP.Text = "";
                txtTenSP.Text = "";
                dateTimePicker1.Value = DateTime.Now;
                cbLoai.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Khong tim thay ma san pham trong danh sach.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string maSP = txtTim.Text;
            SanPham foundSanPham = model.SanPhams.FirstOrDefault(sp => sp.MaSP == maSP);

            if (foundSanPham != null)
            {
               
                listView1.Items.Clear();
                ListViewItem newItem = new ListViewItem(foundSanPham.MaSP);
                newItem.SubItems.Add(foundSanPham.TenSP);
                newItem.SubItems.Add(foundSanPham.NgayNhap.ToString());
                newItem.SubItems.Add(foundSanPham.LoaiSP.TenLoai);
                listView1.Items.Add(newItem);
            }
            else
            {
                MessageBox.Show("Item not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                List<SanPham> listSP = model.SanPhams.ToList();
                BindList(listSP);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text) )
            {
                MessageBox.Show("Vui long nhap Ma San Pham can xoa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string maSP = txtMaSP.Text;
            SanPham selectedSanPham = model.SanPhams.FirstOrDefault(sp => sp.MaSP == maSP);

            var confirmResult = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {

                model.SanPhams.Remove(selectedSanPham);
                model.SaveChanges();

              
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.SubItems[0].Text == maSP)
                    {
                        listView1.Items.Remove(item);
                        break;
                    }
                }

                MessageBox.Show("Item deleted successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                List<SanPham> listSP = model.SanPhams.ToList();
                BindList(listSP);
            }
            else
            {
                MessageBox.Show("Item not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];

                string maSP = selectedItem.SubItems[0].Text;

                SanPham selectedSanPham = model.SanPhams.FirstOrDefault(sp => sp.MaSP == maSP);

                if (selectedSanPham != null)
                {
                    txtMaSP.Text = selectedSanPham.MaSP;
                    txtTenSP.Text = selectedSanPham.TenSP;
                    dateTimePicker1.Value = selectedSanPham.NgayNhap;
                    cbLoai.SelectedItem = selectedSanPham.LoaiSP;
                }
            }
        }



    }
}
    