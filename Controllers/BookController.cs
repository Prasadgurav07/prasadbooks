using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using prasadbooks.Models;


namespace prasadbooks.Controllers
{
    [ApiController]
    [Route("book")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly PbDbContext _db;
     
        public BookController(ILogger<BookController> logger,PbDbContext pbDbContext)
        {
            _logger = logger;
            _db = pbDbContext;


        }

        [HttpPost]
        public IActionResult InsertBook([FromBody] Book book)
        {
            try
            {
               
                    _db.Books.Add(book);
                    _db.SaveChanges();
                
                return Ok(new { Status = "Ok", StatusCode = 200 });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("books")]
        public IActionResult GetBooks()
        {
            try
            {
                List<Book> books = _db.Books.ToList();
                return Ok(books);
            }
            catch (Exception ex) {
               return BadRequest(ex.Message);
            }
         
        }

        [HttpGet]
        [Route("{bookid}")]
        public IActionResult GetBook( int bookid)
        {
            try
            {
                Book book = _db.Books.Find(bookid);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete]
        [Route("delete/{bookid}")]
        public IActionResult deletebook(int bookid)
        {
            try
            {
                Book book = _db.Books.Find(bookid);
                Book book1 = _db.Books.Remove(book).Entity;
                _db.SaveChanges();
                return Ok(book1);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        [Route("getbook")]
        public IActionResult getBookAvailable()
        {
            try
            {
                var book = _db.Books.Where(o => o.AvailableCopies > 0).ToList();
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
