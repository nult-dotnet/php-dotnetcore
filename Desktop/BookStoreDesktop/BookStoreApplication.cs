using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookStoreDesktop.ContainerApplication;
namespace BookStoreDesktop
{
    public partial class BookStoreApplication : Form
    {
        public BookStoreApplication()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            ContainerBook book = new ContainerBook();
            book.TopLevel = false;
            container.Controls.Clear();
            container.Controls.Add(book);
            book.Show();
        }

        private void btnCategory_Click_1(object sender, EventArgs e)
        {
            Form1 category = new Form1();
            category.TopLevel = false;
            container.Controls.Clear();
            container.Controls.Add(category);
            category.Show();
        }

        private void container_Paint(object sender, PaintEventArgs e)
        {

        }
        private void BookStoreApplication_Load(object sender, EventArgs e)
        {
            btnCategory.Focus();
            ContainerBook book = new ContainerBook();
            book.TopLevel = false;
            container.Controls.Clear();
            container.Controls.Add(book);
            book.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ContaineRole role = new ContaineRole();
            role.TopLevel = false;
            container.Controls.Clear();
            container.Controls.Add(role);
            role.Show();
        }
    }
}
