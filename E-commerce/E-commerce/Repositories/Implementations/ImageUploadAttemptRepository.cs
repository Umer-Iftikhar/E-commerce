using Dapper;
using E_commerce.Constants;
using E_commerce.Data;
using E_commerce.DTOs.Request;
using E_commerce.DTOs.Response;
using E_commerce.Repositories.Interfaces;
using System.Data;

namespace E_commerce.Repositories.Implementations
{
    public class ImageUploadAttemptRepository : IImageUploadAttemptRepository
    {
        private readonly DapperContext _context;

        public ImageUploadAttemptRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<ApiResponseDto> CreateImageUploadAttemptAsync(CreateImageUploadAttemptRequestDto request)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<ApiResponseDto>(
                StoredProcedures.CreateImageUploadAttempt,
                new
                {
                    request.UploadToken,
                    request.TempFileName,
                    request.ExpiresAt
                },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<GetExpiredImageUploadsResponseDto> GetExpiredUploadsAsync()
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetExpiredImageUploads,
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadSingleAsync<ApiResponseDto>();

            var expiredUploads = (await multi.ReadAsync<ExpiredImageUploadDto>()).ToList();

            return new GetExpiredImageUploadsResponseDto
            {
                ResponseCode = response.ResponseCode,
                ResponseMessage = response.ResponseMessage,
                ExpiredUploads = expiredUploads
            };
        }

        public async Task<GetImageUploadAttemptResponseDto> GetUploadAttemptByTokenAsync(Guid uploadToken)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                StoredProcedures.GetImageUploadAttemptByToken,
                new
                {
                    UploadToken = uploadToken
                },
                commandType: CommandType.StoredProcedure);

            var response = await multi.ReadSingleAsync<ApiResponseDto>();

            var upload = await multi.ReadSingleOrDefaultAsync<ImageUploadAttemptDto>();

            return new GetImageUploadAttemptResponseDto
            {
                ResponseCode = response.ResponseCode,
                ResponseMessage = response.ResponseMessage,
                Upload = upload
            };
        }

        public async Task<ApiResponseDto> MarkUploadCompletedAsync(Guid uploadToken)
        {
            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<ApiResponseDto>(
                StoredProcedures.MarkImageUploadCompleted,
                new
                {
                    UploadToken = uploadToken
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<ApiResponseDto> MarkUploadExpiredAsync(Guid uploadToken)
        {
            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<ApiResponseDto>(
                StoredProcedures.MarkImageUploadExpired,
                new
                {
                    UploadToken = uploadToken
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
