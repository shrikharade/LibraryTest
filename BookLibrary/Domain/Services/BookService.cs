using AutoMapper;
using BookLibrary.Domain.Dto;
using BookLibrary.Domain.Model;
using BookLibrary.Repositories;

namespace BookLibrary.Domain.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public BookDto CreateBook(BookDto book)
        {
            ValidateBookDto(book);
            _logger.LogTrace("Validate Book Dto Sucessfully");
            var bookModel = _mapper.Map<Book>(book);

            _bookRepository.Put(bookModel);
            _logger.LogTrace("Saved Book Sucessfully");
            return _mapper.Map<BookDto>(bookModel);
        }

        public IReadOnlyList<BookDto> GetAll()
        {
            var books = _bookRepository.GetAll();
            return _mapper.Map<IReadOnlyList<BookDto>>(books);
        }

        public BookDto GetById(Guid identity)
        {
            var book = _bookRepository.GetByIdentity(identity);
            return _mapper.Map<BookDto>(book);
        }

        private void ValidateBookDto(BookDto book)
        {
            if (book == null)
            {
                _logger.LogError("Book Validation Error, Can't Create Book");
                throw new ArgumentNullException("Argument can't be null");
            }

            if (string.IsNullOrWhiteSpace(book.Name) || string.IsNullOrWhiteSpace(book.AuthorName))
            {
                _logger.LogError($"Book dto contain invalid data, BookName: {book.Name}, BookAuthor: {book.AuthorName}");
                throw new BadHttpRequestException("Values can't be empty");
            }
        }
    }
}
