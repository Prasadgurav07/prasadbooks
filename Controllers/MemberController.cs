using Microsoft.AspNetCore.Mvc;
using prasadbooks.Models;
using System.Linq;
using System.Net.WebSockets;

namespace prasadbooks.Controllers
{
    [ApiController]
    [Route("Member")]
    public class MemberController : Controller
    {
        private readonly ILogger <MemberController> logger;
        private readonly PbDbContext dbContext;

        public MemberController(ILogger<MemberController> logger, PbDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }
        [HttpPost]
        [Route("add")]
        public IActionResult addmember([FromBody] Member member)
        {
            try
            {
                dbContext.Members.Add(member);
                dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet]
        [Route("members")]
        public IActionResult members()
        {
            try
            {
                var member = dbContext.Members.ToList();
                return Ok(member);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getmember/{memberid}")]
        public IActionResult member(int memberid)
        {
            try
            {
                var member = dbContext.Members.Find(memberid);
                return Ok(member);
            }
            catch (Exception ex) 
            { 
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("deletemember/{memberid}")]

        public IActionResult deletemember(int memberid)
        {
            try
            {
                var member =dbContext.Members.Find(memberid);
                dbContext.Members.Remove(member);
                dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


    }
}
