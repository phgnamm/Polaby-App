using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.NoteModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.NoteModels;
using Polaby.Services.Models.ResponseModels;
using System.Linq.Expressions;
namespace Polaby.Services.Services
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseDataModel<NoteModel>> CreateNoteAsync(NoteRequestModel model)
        {
            var note = _mapper.Map<Note>(model);
            await _unitOfWork.NoteRepository.AddAsync(note);
            await _unitOfWork.SaveChangeAsync();
            var noteModel = _mapper.Map<NoteModel>(note);
            return new ResponseDataModel<NoteModel>
            {
                Status = true,
                Message = "Note created successfully",
                Data = noteModel
            };
        }

        public async Task<ResponseModel> DeleteNoteAsync(Guid id)
        {
            var note = await _unitOfWork.NoteRepository.GetAsync(id);
            if (note == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Note not found"
                };
            }
            _unitOfWork.NoteRepository.HardDelete(note);
            await _unitOfWork.SaveChangeAsync();
            return new ResponseModel
            {
                Status = false,
                Message = "Note deleted succeefully"
            };
        }

        public async Task<Pagination<NoteModel>> GetNotesAsync(NoteFilterModel model)
        {
            if (model.UserId is null)
            {
                throw new Exception("User ID is required");
            }

            Expression<Func<Note, bool>> filter = note =>
                note.UserId == model.UserId &&
                (model.Date == null || note.Date == model.Date) &&
                (model.Trimester == null || note.Trimester == model.Trimester) &&
                (string.IsNullOrEmpty(model.SearchTerm) || note.Title.Contains(model.SearchTerm));

            var queryResult = await _unitOfWork.NoteRepository.GetAllAsync(
                filter: filter,
                orderBy: notes => notes.OrderBy(note => note.Date),
                pageIndex: model.PageIndex,
                pageSize: model.PageSize
            );

            var noteModels = _mapper.Map<List<NoteModel>>(queryResult.Data);
            return new Pagination<NoteModel>(noteModels, queryResult.TotalCount, model.PageIndex, model.PageSize);
        }

        public async Task<ResponseDataModel<NoteModel>> UpdateNoteAsync(Guid id, NoteRequestModel model)
        {
            var note = await _unitOfWork.NoteRepository.GetAsync(id);
            if (note == null)
            {
                return new ResponseDataModel<NoteModel>
                {
                    Status = false,
                    Message = "Note not found"
                };
            }

            _mapper.Map(model, note);

            note.Date = DateOnly.FromDateTime(DateTime.Now);

            _unitOfWork.NoteRepository.Update(note);
            await _unitOfWork.SaveChangeAsync();

            var updatedNoteModel = _mapper.Map<NoteModel>(note);
            return new ResponseDataModel<NoteModel>
            {
                Status = true,
                Message = "Note updated successfully",
                Data = updatedNoteModel
            };
        }

        public async Task<ResponseDataModel<NoteModel>> GetById(Guid id)
        {
            var note = await _unitOfWork.NoteRepository.GetAsync(id);

            if (note == null)
            {
                return new ResponseDataModel<NoteModel>()
                {
                    Status = false,
                    Message = "Note not found"
                };
            }

            var noteModel = _mapper.Map<NoteModel>(note);

            return new ResponseDataModel<NoteModel>()
            {
                Status = true,
                Message = "Get note successfully",
                Data = noteModel
            };
        }
    }
}
