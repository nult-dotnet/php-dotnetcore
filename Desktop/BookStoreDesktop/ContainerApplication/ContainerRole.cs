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
    public partial class ContaineRole : Form
    {
        public string flag;
        public int index;
        private readonly IRoleService _roleService;
        public ContaineRole()
        {
            InitializeComponent();
            this._roleService = AutofacInstance.GetInstance<IRoleService>();
        }
        public void LockControll()
        {
            txtName.Enabled = false;
            txtSrearch.Enabled = true;
            btnAdd.Enabled = true;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnDelete.Enabled = false;
            txtName.Text = null;
            dataRole.Enabled = true;
            btnAdd.Focus();
        }
        public void UnLockControll()
        {
            txtName.Enabled = true;
            btnAdd.Enabled = false;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtSrearch.Enabled = true;
            dataRole.Enabled = false;
            txtName.Focus();
        }
        public bool CheckData()
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Tên chức vụ không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        public void LoadData()
        {
            List<Role> listRole = this._roleService.GetAllRole();
            if(listRole.Count == 0)
            {
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
            }
            dataRole.DataSource = listRole;
        }
        private void ContaineRole_Load(object sender, EventArgs e)
        {
            LockControll();
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            flag = "Add";
            UnLockControll();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool checkData = CheckData();
            if (checkData)
            {
                if (flag == "Add")
                {
                    RoleDTO newRole = new RoleDTO { Name = txtName.Text };
                    bool reponse = this._roleService.CreateRole(newRole);
                    if (reponse)
                    {
                        LockControll();
                        LoadData();
                    }
                    else
                    {
                        txtName.Focus();
                    }
                }else if(flag == "Update")
                {
                    RoleDTO role = new RoleDTO { Name = txtName.Text };
                    int id = Convert.ToInt32(dataRole.Rows[index].Cells[0].Value.ToString());
                    bool reponse = this._roleService.UpdateRole(role, id);
                    if (reponse)
                    {
                        LockControll();
                        LoadData();
                    }
                    else
                    {
                        txtName.Focus();
                    }
                }
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtName.Text = null;
            LockControll();
        }

        private void txtSrearch_KeyUp(object sender, KeyEventArgs e)
        {
            if(txtSrearch.Text == null)
            {
                LoadData();
            }
            else
            {
                List<Role> findRoleByname = this._roleService.GetRoleByName(txtSrearch.Text);
                dataRole.DataSource = findRoleByname;
            }
        }

        private void dataRole_SelectionChanged(object sender, EventArgs e)
        {
            if(dataRole.CurrentCell != null)
            {
                this.index = dataRole.CurrentCell.RowIndex;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn có muốn xóa chức vụ này","Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dataRole.Rows[index].Cells[0].Value.ToString());
                this._roleService.DeleteRole(id);
                LoadData();
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            flag = "Update";
            txtName.Text = dataRole.Rows[index].Cells[1].Value.ToString();
            UnLockControll();
        }
    }
}