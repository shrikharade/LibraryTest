using BookLibrary.Domain.Dto;
using BookLibrary.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;

        public BooksController(ILogger<BooksController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetAll()
        {
           var books = _bookService.GetAll();
           return Ok(books);
        }

        [HttpGet("{id}")]
        public ActionResult<BookDto> GetById([FromRoute(Name = "id")] Guid id)
        {
            var book = _bookService.GetById(id);
            if (book != null)
            {
                return book;
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<BookDto> CreateBook([FromBody, BindRequired] BookDto bookDto)
        {
            var book = _bookService.CreateBook(bookDto);
            return Ok(book);
        }
    }
}