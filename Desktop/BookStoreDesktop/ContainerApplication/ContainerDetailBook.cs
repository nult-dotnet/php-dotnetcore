using BookStoreDesktop.Interfaces;
using BookStoreDesktop.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookStoreDesktop.Autofac;
using BookStoreDesktop.Models;

namespace BookStoreDesktop.ContainerApplication
{
    public partial class ContainerDetailBook : Form
    {
        private readonly string _id;
        private readonly ContainerBook _containerBook;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        public ContainerDetailBook(string id,ContainerBook containerBook)
        {
            _id= id;
            _containerBook = containerBook;
            InitializeComponent();
            _bookService = AutofacInstance.GetInstance<IBookService>();
            _categoryService = AutofacInstance.GetInstance<ICategoryService>();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ContainerDetailBook_Load(object sender, EventArgs e)
        {
            Book book = this._bookService.GetBookById(_id);
            txtName.Text = book.Name;
            txtPrice.Text = $"{Convert.ToString(book.Price)} VND";
            txtQuantity.Text =$"{Convert.ToString(book.Quantity)} sản phẩm";
            txtAuthor.Text = book.Author;
            txtSold.Text = $"{Convert.ToString(book.Sold)} sản phẩm";
            txtTime.Text = Convert.ToString(book.TimeCreate);
            Category category = this._categoryService.GetItemCategory(Convert.ToInt32(book.CategoryId));
            txtCategory.Text = category.Name;
            OpenFileDialog open = new OpenFileDialog();
            pictureBox1.Image = new Bitmap(book.ImgPath);
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
