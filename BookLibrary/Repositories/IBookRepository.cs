using BookLibrary.Domain.Model;

namespace BookLibrary.Repositories
{
    public interface IBookRepository
    {
        IReadOnlyList<Book> GetAll();

        Book? GetByIdentity(Guid identity);

        Book Put(Book book);
    }
}
