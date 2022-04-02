using BookStoreDesktop.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookStoreDesktop.Models;
using BookStoreDesktop.Interfaces.Services;
using BookStoreDesktop.Autofac;
namespace BookStoreDesktop.ContainerApplication
{
    public partial class ContainerAddBook : Form
    {
        private readonly ICategoryService _categoryService;
        private readonly IBookService _bookService;
        private readonly ContainerBook _containerBook;
        public string pathFile=null;
        public ContainerAddBook(ContainerBook containerBook)
        {
            InitializeComponent();
            this._categoryService = AutofacInstance.GetInstance<ICategoryService>();
            this._bookService = AutofacInstance.GetInstance<IBookService>();
            this._containerBook = containerBook;
        }
        public void SelectCategory()
        {
            List<Category> listCategory = this._categoryService.GetAllCategory();
            Category fakeCategory = new Category { Name = "---Chọn thể loại sách---" };
            listCategory.Insert(0, fakeCategory);
            selectCategory.DataSource = listCategory;
            selectCategory.ValueMember = "Id";
            selectCategory.DisplayMember = "Name";
        }
        public bool CheckData()
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Tên sách không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if(txtPrice.Value == 0)
            {
                MessageBox.Show("Giá bán không phù hợp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if(txtQuantity.Value == 0)
            {
                MessageBox.Show("Số lượng sản phẩm không phù hợp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (String.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                MessageBox.Show("Tên tác giả không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (selectCategory.SelectedValue.ToString() == "0")
            {
                MessageBox.Show("Thể loại sách không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (String.IsNullOrWhiteSpace(this.pathFile))
            {
                MessageBox.Show("Hình ảnh sản phẩm không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp;*.png)|*.jpg; *.jpeg; *.gif; *.bmp;*.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                this.pathFile = open.FileName;
                pictureBox1.Image = new Bitmap(open.FileName);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ContainerAddBook_Load(object sender, EventArgs e)
        {
            SelectCategory();
            txtName.Focus();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ContainerAddBook_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._containerBook.LoadData();
        }
        private void txtPrice_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool checkData = CheckData();
            if (checkData)
            {
                button1.Enabled = false;
                //string pathFolder = Path.Combine(@"D:\xampp\htdocs\php-dotnetcore\Desktop\BookStoreDesktop\BookStoreDesktop", "Images");
                //string[] f = this.pathFile.Split('\\');
                //string fileName = $"{DateTime.Now.ToString("HH-mm-ss-dd-MM-yyyy")}-{f[f.Length - 1]}";
                //string pathFileName = Path.Combine(pathFolder, fileName);
                BookDTO newBook = new BookDTO
                {
                    Name = txtName.Text,
                    Author = txtAuthor.Text,
                    CategoryId = Convert.ToInt32(selectCategory.SelectedValue.ToString()),
                    ImgPath = this.pathFile,
                    Price = (int)txtPrice.Value,
                    Quantity = (int)txtQuantity.Value,
                };
                bool reponse = this._bookService.CreateBook(newBook);
                if (reponse)
                {
                    //File.Delete(pathFileName);
                    //File.Copy(this.pathFile, pathFileName,true);
                    Category category = this._categoryService.GetItemCategory(newBook.CategoryId);
                    category.Quantity += 1;
                    this._categoryService.UpdateQuantity(category);
                    this.Close();
                }
                else
                {
                    button1.Enabled = true;
                }
            }
        }

        private void ContainerAddBook_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
    }
}