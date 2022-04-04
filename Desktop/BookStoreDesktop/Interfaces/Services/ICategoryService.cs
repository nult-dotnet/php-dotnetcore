using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDesktop.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreDesktop.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetAllCategory();
        bool CreateCategory(CategoryDTO categoryDTO);
        bool UpdateCategory(CategoryDTO categoryDTO,int id);
        List<Category> GetCategoryByName(string name);
        bool DeleteCategory(int id);
        Category GetItemCategory(int id);
        void UpdateQuantity(Category category);
    }
}