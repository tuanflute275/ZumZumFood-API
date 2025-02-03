namespace ZumZumFood.Application.Abstracts
{
    public interface ICodeService
    {
        Task<ResponseObject> GetAllPaginationAsync(CodeQuery codeQuery);
        Task<ResponseObject> GetByCodeIdAsync(string codeId);
        Task<ResponseObject> SaveCodeAsync(CodeModel model);
        Task<ResponseObject> UpdateCodeAsync(string codeId, CodeUpdateModel model);
        Task<ResponseObject> DeleteCodeAsync(string codeId);

        // codeValues
        Task<ResponseObject> GetListCodeValueByCodeId(string codeId);
        Task<ResponseObject> GetByCodeIdAndCodeValueAsync(string codeId, string codeValue);
        Task<ResponseObject> SaveCodeValueAsync(CodeValueModel model);
        Task<ResponseObject> UpdateCodeValueAsync(string codeId, string codeValue, CodeValueUpdateModel model);
        Task<ResponseObject> DeleteCodeValueAsync(string codeId, string codeValue);


        // location
        Task<ResponseObject> GetLocation(int parentId);
    }
}
