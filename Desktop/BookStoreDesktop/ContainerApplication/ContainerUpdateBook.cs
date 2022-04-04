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
using BookStoreDesktop.Interfaces;

namespace BookStoreDesktop.ContainerApplication
{
    public partial class ContainerUpdateBook : Form
    {
        private readonly string _id;
        public string pathImg;
        private readonly ContainerBook _containerBook;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        public ContainerUpdateBook(string id,ContainerBook containerBook)
        {
            InitializeComponent();
            _id = id;
            _containerBook = containerBook;
            _bookService = AutofacInstance.GetInstance<IBookService>();
            _categoryService = AutofacInstance.GetInstance<ICategoryService>();
        }
        public void GetData()
        {
            Book book = this._bookService.GetBookById(_id);
            txtName.Text = book.Name;
            txtAuthor.Text = book.Author;
            txtPrice.Value = book.Price;
            txtQuantity.Value = book.Quantity;
            OpenFileDialog open = new OpenFileDialog();
            this.pathImg = book.ImgPath;
            pictureBox1.Image = new Bitmap(book.ImgPath);
        }
        public void SelectCategory()
        {
            Book book = this._bookService.GetBookById(_id);
            List<Category> listCategory = this._categoryService.GetAllCategory();
            Category fakeCategory = new Category { Name = "---Chọn thể loại sách---" };
            listCategory.Insert(0, fakeCategory);
            int i = 0;
            foreach(Category x in listCategory)
            {
                if(x.Id == book.CategoryId)
                {
                    break;
                }
                else
                {
                    i++;
                }
            }
            selectCategory.DataSource = listCategory;
            selectCategory.ValueMember = "Id";
            selectCategory.DisplayMember = "Name";
            selectCategory.SelectedIndex = i;
        }
        public bool CheckData()
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Tên sách không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtPrice.Value == 0)
            {
                MessageBox.Show("Giá bán không phù hợp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtQuantity.Value == 0)
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
            return true;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ContainerUpdateBook_Load(object sender, EventArgs e)
        {
            GetData();
            SelectCategory();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp;*.png)|*.jpg; *.jpeg; *.gif; *.bmp;*.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(open.FileName);
                this.pathImg = open.FileName;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool checkData = CheckData();
            if (checkData)
            {
                Book book = this._bookService.GetBookById(_id);
                BookDTO newBook = new BookDTO
                {
                    Name = txtName.Text,
                    Author = txtAuthor.Text,
                    CategoryId = Convert.ToInt32(selectCategory.SelectedValue.ToString()),
                    ImgPath = this.pathImg,
                    Price = (int)txtPrice.Value,
                    Quantity = (int)txtQuantity.Value,
                };
                bool reponse = this._bookService.UpdateBook(newBook,this._id);
                if (reponse)
                {
                    if(book.CategoryId != newBook.CategoryId)
                    {
                        Category categoryOld = this._categoryService.GetItemCategory(Convert.ToInt32(book.CategoryId));
                        categoryOld.Quantity -= 1;
                        this._categoryService.UpdateQuantity(categoryOld);
                        Category categoryNew = this._categoryService.GetItemCategory(newBook.CategoryId);
                        categoryNew.Quantity += 1;
                        this._categoryService.UpdateQuantity(categoryNew);
                    }
                    //File.Delete(pathFileName);
                    //File.Copy(this.pathFile, pathFileName,true);
                    this.Close();
                }
                else
                {
                    btnUpdate.Enabled = true;
                }
            }
        }

        private void ContainerUpdateBook_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._containerBook.LoadData();
        }
    }
}