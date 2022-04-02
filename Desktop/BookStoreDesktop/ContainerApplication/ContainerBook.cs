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
using BookStoreDesktop.Interfaces;
using BookStoreDesktop.Autofac;
using BookStoreDesktop.Interfaces.Services;
namespace BookStoreDesktop.ContainerApplication
{
    public partial class ContainerBook : Form
    {
        private readonly ICategoryService _categoryService;
        private readonly IBookService _bookService;
        public string id;
        public ContainerBook()
        {
            this._bookService = AutofacInstance.GetInstance<IBookService>();
            this._categoryService = AutofacInstance.GetInstance<ICategoryService>();
            InitializeComponent();
        }
        public void LockControll()
        {
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
            btnDetail.Enabled = true;
        }
        public void LoadData()
        {
            List<Book> books = this._bookService.GetAllBook();
            if(books.Count == 0)
            {
                btnAdd.Focus();
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
                btnDetail.Enabled = false;
            }
            dataBook.DataSource = books;
        }
        private void ContainerBook_Load(object sender, EventArgs e)
        {
            LockControll();
            LoadData(); 
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ContainerAddBook addBook = new ContainerAddBook(this);
            addBook.ShowDialog();
        }

        private void dataBook_SelectionChanged(object sender, EventArgs e)
        {
            if(dataBook.CurrentCell != null)
            {
                int index = dataBook.CurrentCell.RowIndex;
                this.id = dataBook.Rows[index].Cells[0].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn muốn xóa sản phẩm này","Thông báo",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Book findBook = this._bookService.GetBookById(this.id);
                Category category = this._categoryService.GetItemCategory(Convert.ToInt32(findBook.CategoryId));
                category.Quantity -= 1;
                this._categoryService.UpdateQuantity(category);
                //File.Delete(findBook.ImgPath);
                this._bookService.DeleteBook(this.id);
                LoadData();
            }
        }
        private void dataBook_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ContainerUpdateBook containerUpdateBook = new ContainerUpdateBook(this.id,this);
            containerUpdateBook.ShowDialog();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            ContainerDetailBook containerDetailBook = new ContainerDetailBook(this.id,this);
            containerDetailBook.Show();
        }

        private void txtSrearch_KeyUp(object sender, KeyEventArgs e)
        {
            List<Book> listBook = this._bookService.GetBookByName(txtSrearch.Text);
            dataBook.DataSource = listBook;
        }

        private void bookBindingSource2_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
