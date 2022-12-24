using BookLibrary.Domain.Model;

namespace BookLibrary.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContext _bookContext;

        public BookRepository(BookContext bookContext)
        {
            _bookContext = bookContext;
        }

        public IReadOnlyList<Book> GetAll()
        {
            return _bookContext.Books.ToList();
        }

        public Book? GetByIdentity(Guid identity)
        {
            return _bookContext.Books.SingleOrDefault(x => x.Id == identity);
        }

        public Book Put(Book aggregate)
        {
            _bookContext.Books.Add(aggregate);
            _bookContext.SaveChanges();
            return aggregate;
        }
    }
}
