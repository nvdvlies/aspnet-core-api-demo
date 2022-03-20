using Demo.Application.Shared.Dtos;

namespace Demo.Application.Users.Queries.SearchUsers.Dtos
{
    public class SearchUserDto : SoftDeleteEntityDto
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
    }
}