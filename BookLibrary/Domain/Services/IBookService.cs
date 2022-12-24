using BookLibrary.Domain.Dto;

namespace BookLibrary.Domain.Services
{
    public interface IBookService
    {
        /// <summary>
        /// return book by id.
        /// </summary>
        BookDto GetById(Guid identity);

        /// <summary>
        /// return all the books.
        /// </summary>
        IReadOnlyList<BookDto> GetAll();

        /// <summary>
        /// return save a book
        /// </summary>
        BookDto CreateBook(BookDto book);
    }
}
