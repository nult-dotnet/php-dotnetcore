using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDesktop.Interfaces.Services;
using BookStoreDesktop.Models;
using BookStoreDesktop.RepositoryPattern;
using BookStoreDesktop.Automapper;
namespace BookStoreDesktop.Services
{
    public class BookService : IBookService
    {
        private readonly UnitOfWork _unitOfWork;
        public BookService()
        {
            this._unitOfWork = new UnitOfWork();
        }

        public bool CheckCategoryId(int categoryId)
        {
            var books = this._unitOfWork.BookRepository.Get(x=>x.CategoryId == categoryId);
            if(books.ToList().Count == 0)
            {
                return true;
            }
            MessageBox.Show("Không thể xóa dữ liệu, có nhiều dữ liệu liên quan", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        public bool CreateBook(BookDTO book)
        {
            Book newBook = new Book();
            ConfigMapper.configMapper().Map(book,newBook);
            var validateName = this._unitOfWork.BookRepository.Get(x=>x.Name==newBook.Name && x.Id != newBook.Id);
            if(validateName.ToList().Count > 0)
            {
                MessageBox.Show("Tên sách đã có, vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            this._unitOfWork.BookRepository.Insert(newBook);
            this._unitOfWork.Save();
            return true;
        }

        public bool DeleteBook(string id)
        {
            Book book = this._unitOfWork.BookRepository.GetByID(id);
            if (book is null)
            {
                MessageBox.Show("Không tìm thấy sách phù hợp, vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            this._unitOfWork.BookRepository.Delete(id);
            this._unitOfWork.Save();
            return true;
        }

        public List<Book> GetAllBook()
        {
            return this._unitOfWork.BookRepository.Get().ToList();
        }

        public Book GetBookById(string id)
        {
            return this._unitOfWork.BookRepository.GetByID(id);
        }

        public List<Book> GetBookByName(string name)
        {
            return this._unitOfWork.BookRepository.Get(x=>x.Name.Contains(name)).ToList();
        }

        public bool UpdateBook(BookDTO book, string id)
        {
            Book findBook = this._unitOfWork.BookRepository.GetByID(id);
            if(book is null)
            {
                MessageBox.Show("Không tìm thấy sách phù hợp, vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            ConfigMapper.configMapper().Map(book,findBook);
            var validateBook = this._unitOfWork.BookRepository.Get(x => x.Name == findBook.Name && x.Id != findBook.Id).ToList();
            if(validateBook.Count > 0)
            {
                MessageBox.Show("Tên sách đã có, vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            this._unitOfWork.BookRepository.Update(findBook);
            this._unitOfWork.Save();
            return true;
        }
    }
}
