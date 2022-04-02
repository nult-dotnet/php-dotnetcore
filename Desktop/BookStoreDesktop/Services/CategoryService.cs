using BookStoreDesktop.Models;
using BookStoreDesktop.Interfaces;
using BookStoreDesktop.Automapper;
using BookStoreDesktop.Autofac;
using BookStoreDesktop.RepositoryPattern;
namespace BookStoreDesktop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly UnitOfWork _unitOfWork;
        public CategoryService()
        {
            this._unitOfWork = new UnitOfWork();
        }

        public bool CreateCategory(CategoryDTO categoryDTO)
        {
            Category newCategory = new Category();
            ConfigMapper.configMapper().Map(categoryDTO, newCategory);
            var validateName = this._unitOfWork.CategoryRepository.Get(x=>x.Name == newCategory.Name && x.Id != newCategory.Id);
            if(validateName.ToList().Count > 0 )
            {
                return false;
            }
            this._unitOfWork.CategoryRepository.Insert(newCategory);
            this._unitOfWork.Save();
            return true;
        }
        public bool DeleteCategory(int id)
        {
            Category category = this._unitOfWork.CategoryRepository.GetByID(id);
            if(category is null)
            {
                return false;
            }
            this._unitOfWork.CategoryRepository.GetByID(id);
            this._unitOfWork.CategoryRepository.Delete(id);
            this._unitOfWork.Save();
            return true;
        }

        public List<Category> GetAllCategory()
        {
            var listCategory = this._unitOfWork.CategoryRepository.Get();
            return listCategory.ToList();
        }

        public List<Category> GetCategoryByName(string name)
        {
            var listCategory = this._unitOfWork.CategoryRepository.Get(x => x.Name.Contains(name));
            return listCategory.ToList();
        }

        public Category GetItemCategory(int id)
        {
            return this._unitOfWork.CategoryRepository.GetByID(id);
        }

        public bool UpdateCategory(CategoryDTO categoryDTO, int id)
        {
            Category category = this._unitOfWork.CategoryRepository.GetByID(id);
            ConfigMapper.configMapper().Map(categoryDTO, category);
            var validateName = this._unitOfWork.CategoryRepository.Get(x => x.Name == category.Name && x.Id != category.Id);
            if (validateName.ToList().Count > 0)
            {
                return false;
            }
            else
            {
                this._unitOfWork.CategoryRepository.Update(category);
                this._unitOfWork.Save();
                return true;
            }
        }
        public void UpdateQuantity(Category category)
        {
            this._unitOfWork.CategoryRepository.Update(category);
            this._unitOfWork.Save();
        }
    }
}