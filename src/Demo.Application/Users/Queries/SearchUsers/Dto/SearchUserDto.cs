using Demo.Application.Shared.Dtos;

namespace Demo.Application.Users.Queries.SearchUsers.Dto
{
    public class SearchUserDto : SoftDeleteEntityDto
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
    }
}