using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Model;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogEntriesController : ControllerBase
    {
        private readonly BlogContext _context;

        public BlogEntriesController(BlogContext context)
        {
            _context = context;
        }

        // GET: api/BlogEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogEntry>>> GetBlogEntry()
        {
            return await _context.BlogEntry.ToListAsync();
        }

        // GET: api/BlogEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogEntry>> GetBlogEntry(int id)
        {
            var blogEntry = await _context.BlogEntry.FindAsync(id);

            if (blogEntry == null)
            {
                return NotFound();
            }

            return blogEntry;
        }

        // PUT: api/BlogEntries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogEntry(int id, BlogEntry blogEntry)
        {
            if (id != blogEntry.EntryId)
            {
                return BadRequest();
            }

            _context.Entry(blogEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogEntryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BlogEntries
        [HttpPost]
        public async Task<ActionResult<BlogEntry>> PostBlogEntry(BlogEntry blogEntry)
        {
            _context.BlogEntry.Add(blogEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlogEntry", new { id = blogEntry.EntryId }, blogEntry);
        }

        // DELETE: api/BlogEntries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BlogEntry>> DeleteBlogEntry(int id)
        {
            var blogEntry = await _context.BlogEntry.FindAsync(id);
            if (blogEntry == null)
            {
                return NotFound();
            }

            _context.BlogEntry.Remove(blogEntry);
            await _context.SaveChangesAsync();

            return blogEntry;
        }

        private bool BlogEntryExists(int id)
        {
            return _context.BlogEntry.Any(e => e.EntryId == id);
        }
    }
}
