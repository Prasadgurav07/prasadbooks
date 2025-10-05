using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using prasadbooks.Models;
using System.Net;

namespace prasadbooks.Controllers
{
    [ApiController]
    [Route("borrow")]
    public class BorrowController : Controller
    {
        private readonly ILogger<BorrowController> _logger;
        private readonly PbDbContext db;




        public BorrowController(ILogger<BorrowController> logger, PbDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        [HttpPost]
        [Route("addborrow")]
        public IActionResult addborrow([FromBody] BorrowRecord borrow, int bookid)
        {
            try
            {

                var book = db.Books.Where(o => o.BookId == bookid && o.AvailableCopies > 0).FirstOrDefault();
                if (book != null)
                {
                    book.AvailableCopies -= 1;
                    db.BorrowRecords.Add(borrow);
                    db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return Ok("Book is not available");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        [Route("borrow")]
        public IActionResult borrowlist()
        {
            try
            {
                var borrow = db.BorrowRecords.ToList();
                return Ok(borrow);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }


        [HttpGet]
        [Route("borrowid/{borrowid}")]
        public IActionResult borrowid(int borrowid)
        {
            try
            {
                var borrow = db.BorrowRecords.FirstOrDefault();
                return Ok(borrow);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("delete/{borrowid}")]
        public IActionResult deleteborrow(int borrowid)
        {
            try
            {
                var borrow = db.BorrowRecords.Find(borrowid);
                db.BorrowRecords.Remove(borrow);
                db.SaveChanges();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("returnbook/{borrowid}")]
        public IActionResult returnBook(int borrowid)
        {
            try
            {
                BorrowRecord borrowerid = db.BorrowRecords.Where(o => o.BorrowId == borrowid).First();

                if (borrowerid != null)
                {
                    var bookid = borrowerid.BookId;

                    var book1 = db.Books.Find(bookid);
                    if (book1 != null)
                    {
                        book1.AvailableCopies += 1;
                        borrowerid.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
                        borrowerid.IsReturned = true;
                        db.SaveChanges();

                    }
                    else
                    {
                        return Ok("Something went wrong");
                    }

                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok();
            }

        }

        [HttpGet]
        [Route("top5BorrowedBooks")]
        public IActionResult top5BorrowedBooks()
        {
            try
            {
                var top5 = (from br in db.BorrowRecords
                            join b in db.Books
                            on br.BookId equals b.BookId into bookGroup
                            from b in bookGroup.DefaultIfEmpty() // LEFT JOIN
                            group b by b.Title into g
                            orderby g.Count() descending
                            select new
                            {
                                Title = g.Key,
                                BookAmount = g.Count()
                            })
              .Take(5)
              .ToList();

                return Ok(top5);
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }
        [HttpGet]
        [Route("OverdueBorrow")]
        public IActionResult GetOverdueBooks()
        {
            var overdueList = db.BorrowRecords
                .FromSqlRaw("EXEC Sp_OverdueButNotReturn")
                .ToList();

            return Ok(overdueList);

        }

    }
}
