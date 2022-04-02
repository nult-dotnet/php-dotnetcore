using BookStoreDesktop.Interfaces;
using BookStoreDesktop.Services;
using BookStoreDesktop.Autofac;
using AutoMapper;
using BookStoreDesktop.BookStoreDatabase;
using BookStoreDesktop.Models;
using BookStoreDesktop.Automapper;
using BookStoreDesktop.Interfaces.Services;

namespace BookStoreDesktop
{
    public partial class Form1 : Form
    {
        private readonly ICategoryService _categoryService;
        private readonly IBookService _bookService;
        public string flag;
        public int index;
        public Form1()
        {
            InitializeComponent();
            this._categoryService = AutofacInstance.GetInstance<ICategoryService>();
            this._bookService = AutofacInstance.GetInstance<IBookService>();
        }
        public void LockControll()
        {
            txtName.Enabled = false;
            txtSrearch.Enabled = true;
            btnAdd.Enabled = true;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            txtName.Text = null;
            btnAdd.Focus();
            dataCategory.Enabled = true;
        }
        public void UnlockControll()
        {
            txtName.Enabled = true;
            btnAdd.Enabled = false;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtSrearch.Enabled = true;
            dataCategory.Enabled = false;
            txtName.Focus();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LockControll();
            LoadData();
        }
        public void LoadData()
        {
            List<Category> listCategory = this._categoryService.GetAllCategory();
            if(listCategory.Count == 0)
            {
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
                btnAdd.Focus();
            }
            dataCategory.DataSource = listCategory;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            flag = "add";
            UnlockControll();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            flag = "update"; 
            txtName.Text = dataCategory.Rows[index].Cells[1].Value.ToString();
            UnlockControll();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LockControll();
        }
        public bool CheckData()
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên loại sách","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                txtName.Focus();
                return false;
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool checkData = CheckData();
            if (checkData)
            {
                if(flag == "add")
                {
                    btnSave.Enabled = false;
                    CategoryDTO category = new CategoryDTO { Name = txtName.Text };
                    bool result = this._categoryService.CreateCategory(category);
                    if (result)
                    {
                        LockControll();
                        LoadData();
                    }
                    else
                    {
                        txtName.Focus();
                        UnlockControll();
                        MessageBox.Show("Tên loại sách đã có, vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    } 
                }
                else
                {
                    int Id = Convert.ToInt32(dataCategory.Rows[index].Cells[0].Value.ToString());
                    CategoryDTO category = new CategoryDTO { Name = txtName.Text };
                    bool result = this._categoryService.UpdateCategory(category,Id);
                    if (result)
                    {
                         LockControll();
                         LoadData();
                    }
                    else
                    {
                        txtName.Focus();
                        MessageBox.Show("Tên loại sách đã có, vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private void dataCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn có muốn xóa loại sản phẩm này?","Cảnh báo",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                
                int Id = Convert.ToInt32(dataCategory.Rows[index].Cells[0].Value.ToString());
                bool checkCategoryId = this._bookService.CheckCategoryId(Id);
                if (checkCategoryId)
                {
                    this._categoryService.DeleteCategory(Id);
                    LoadData();
                }
            }
        }
        private void dataCategory_SelectionChanged(object sender, EventArgs e)
        {
            if(dataCategory.CurrentCell != null)
            {
                this.index = dataCategory.CurrentCell.RowIndex;
            }
        }
        private void txtSrearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSrearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSrearch.Text))
            {
                LoadData();
            }
            else
            {
                List<Category> listSearch = this._categoryService.GetCategoryByName(txtSrearch.Text);
                dataCategory.DataSource = listSearch;
            }
        }
    }
}