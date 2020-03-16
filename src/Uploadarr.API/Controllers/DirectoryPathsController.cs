using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uploadarr.Common;
using Uploadarr.Data;

namespace Uploadarr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectoryPathsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public DirectoryPathsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/DirectoryPaths
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DirectoryPath>>> GetDirectoryPath()
        {
            return await _context.DirectoryPath.ToListAsync();
        }

        // GET: api/DirectoryPaths/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DirectoryPath>> GetDirectoryPath(int id)
        {
            var directoryPath = await _context.DirectoryPath.FindAsync(id);

            if (directoryPath == null)
            {
                return NotFound();
            }

            return directoryPath;
        }

        // PUT: api/DirectoryPaths/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDirectoryPath(int id, DirectoryPath directoryPath)
        {
            if (id != directoryPath.Id)
            {
                return BadRequest();
            }

            _context.Entry(directoryPath).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectoryPathExists(id))
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

        // POST: api/DirectoryPaths
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<DirectoryPath>> PostDirectoryPath(DirectoryPath directoryPath)
        {
            _context.DirectoryPath.Add(directoryPath);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDirectoryPath", new { id = directoryPath.Id }, directoryPath);
        }

        // DELETE: api/DirectoryPaths/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DirectoryPath>> DeleteDirectoryPath(int id)
        {
            var directoryPath = await _context.DirectoryPath.FindAsync(id);
            if (directoryPath == null)
            {
                return NotFound();
            }

            _context.DirectoryPath.Remove(directoryPath);
            await _context.SaveChangesAsync();

            return directoryPath;
        }

        private bool DirectoryPathExists(int id)
        {
            return _context.DirectoryPath.Any(e => e.Id == id);
        }
    }
}
