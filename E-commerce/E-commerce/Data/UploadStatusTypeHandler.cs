using E_commerce.Enums;
using System.Data;
using static Dapper.SqlMapper;

namespace E_commerce.Data
{
    public class UploadStatusTypeHandler : StringTypeHandler<UploadStatus>
    {
        protected override UploadStatus Parse(string value)
        {
            if (!Enum.TryParse(value, true, out UploadStatus status))
            {
                throw new DataException($"Unknown upload status '{value}'.");
            }
            return status;
        }

        protected override string Format(UploadStatus value)
        {
            return value.ToString();
        }
    }
}
