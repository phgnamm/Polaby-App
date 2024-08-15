using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Repositories.Models.NoteModels;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.NoteModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/notes")]
    [ApiController]
    public class NoteController: ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }
        [HttpGet]
        public async Task<IActionResult> GetNotesByFilter([FromQuery] NoteFilterModel noteFilterModel)
        {
            try
            {
                var result = await _noteService.GetNotesAsync(noteFilterModel);

                var metadata = new
                {
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteRequestModel model)
        {
            var result = await _noteService.CreateNoteAsync(model);
            if (!result.Status)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] NoteRequestModel model)
        {
            var result = await _noteService.UpdateNoteAsync(id, model);
            if (!result.Status)
            {
                return NotFound(result.Message);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var result = await _noteService.DeleteNoteAsync(id);
            if (!result.Status)
            {
                return NotFound(result.Message);
            }

            return Ok(result);
        }
    }
}
