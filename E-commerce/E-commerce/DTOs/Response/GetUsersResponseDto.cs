namespace E_commerce.DTOs.Response
{
    public class GetUsersResponseDto : SpResponseDto
    {
        public List<UserListItemDto> Users { get; set; } = [];
    }
}
