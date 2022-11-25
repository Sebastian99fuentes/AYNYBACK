using Microsoft.AspNetCore.Mvc;
using Backend_AYNI.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_AYNI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumController : Controller
    {
        private readonly DatabaseContext context;

        public ForumController(DatabaseContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<ForumModel>>> Get()
        {
            var forums = await context.Forums.Include(o => o.owner).ToListAsync();
            if (!forums.Any())
                return NotFound();
            return forums;
        }

        [HttpGet("id")]
        public async Task<ActionResult<ICollection<ForumModel>>> GetForum(string id)
        {
            var forum = await context.Forums.Where(e => e.Id.Equals(id)).FirstOrDefaultAsync();
            if (forum == null)
                return NotFound();
            return Ok(forum);
        }
        [HttpGet(template: ApiRoutes.Forum.Padre)]
        public async Task<ActionResult<ICollection<ForumModel>>> GetPorPadre(string id)
        {
            var forum = await context.Forums.Where(e => e.fatherId.Equals(id)).FirstOrDefaultAsync();
            if (forum == null)
                return NotFound();
            return Ok(forum);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(ForumModel forum)
        {
            var created = context.Forums.Add(forum);
            await context.SaveChangesAsync();
            return CreatedAtAction("GetForum", new { id = forum.Id }, created.Entity);
        }

        [HttpPut("id")]
        public async Task<ActionResult> Put(string id, ForumModel forum)
        {
            var existe = await Existe(id);

            if (!existe)
                return NotFound();

            forum.Id = id;
            context.Forums.Update(forum);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(string id)
        {
            var existe = await Existe(id);

            if (!existe)
                return NotFound();


            var forum = await context.Forums.FindAsync(id);
            context.Forums.Remove(forum);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> Existe(string id)
        {
            return await context.Forums.AnyAsync(f => f.Id == id);
        }
    }
}
