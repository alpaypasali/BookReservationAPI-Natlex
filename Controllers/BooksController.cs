using BookStoreData.Bussiness.Abstract;
using BookStoreData.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BookReservationAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class BooksController : ControllerBase
    {

        private readonly ILogger<BooksController> _logger;
        private readonly BookStoreContext _context;
        private IBookService _bookService;

        public BooksController(ILogger<BooksController> logger, BookStoreContext context, IBookService bookService)
        {
            _logger = logger;
            _context = context;
            _bookService = bookService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns all books in database. </returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var book = await _bookService.GetAll();
            return Ok(book);
        }

        /// <summary>
        /// Getting a book by id
        /// </summary>
        /// <param name="id">Id book</param>
        /// <returns></returns>
        [HttpGet("getBook")]
        public async Task<IActionResult> Get([FromQuery(Name = "id")] int id)
        {
            var book = await _bookService.Get(id);
            return Ok(book);
        }

        /// <summary>
        /// Get reserved books
        /// </summary>
        /// <returns>Returns reserved books in database. </returns>
        [HttpGet("getReservedBooks")]
        public async Task<IActionResult> GetReservedBooks()
        {
            var books = await _bookService.GetReserved();
            return Ok(books);
        }
        /// <summary>
        /// Get not reserved books
        /// </summary>
        /// <returns>Returns reserved books in database. </returns>
        [HttpGet("getNotReservedBooks")]
        public async Task<IActionResult> GetNotReservedBooks()
        {
            var books = await _bookService.GetNotReserved();
            return Ok(books);
        }

        /// <summary>
        /// Create new book
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Created book Id</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookModel model)
        {
            if (!ModelState.IsValid)
                return new EmptyResult();

            var book = await _bookService.Create(model);

            return Ok(new { Id = book.Id });
        }

        /// <summary>
        /// This method is intended for book reservation
        /// </summary>
        /// <param name="id">Id book, which needs to be reserved</param>
        /// <param name="comment">Comment to the reserved book</param>
        /// <returns></returns>
        [HttpPost("reserveBook")]

        public async Task<IActionResult> ReserveBook(int id, string? comment)
        {
            var result = await _bookService.Reserve(id, comment);
            if (result == BookStatus.NotFound)
                return StatusCode(404, "This book was not found");

            if (result == BookStatus.IsReserved)
                return StatusCode(500, "This book has already been reserved");

            return Ok(result);
        }
    }
}
