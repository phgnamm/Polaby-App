using Polaby.Repositories.Models.NoteModels;
using Polaby.Services.Common;
using Polaby.Services.Models.NoteModels;
using Polaby.Services.Models.ResponseModels;
namespace Polaby.Services.Interfaces
{
    public interface INoteService
    {
        Task<ResponseDataModel<NoteModel>> CreateNoteAsync(NoteRequestModel model);
        Task<ResponseDataModel<NoteModel>> UpdateNoteAsync(Guid id, NoteRequestModel model);
        Task<ResponseModel> DeleteNoteAsync(Guid id);
        Task<Pagination<NoteModel>> GetNotesAsync(NoteFilterModel model);
        Task<ResponseDataModel<NoteModel>> GetById(Guid id);
    }
}
