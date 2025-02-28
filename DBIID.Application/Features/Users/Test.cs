using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using DBIID.Application.Common.Attributes;
    using DBIID.Application.Common.Dtos;
    using MediatR;

namespace DBIID.Application.Features.Users
{

    // GET Request
    [HttpRequest(HttpMethodType.GET, "User/Read/{id}")]
    public class GetUserQuery : IRequest<UserDto>
    {
        public int Id { get; set; }
    }


    // POST Request
    [HttpRequest(HttpMethodType.POST, "User/Create")]
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    // PUT Request
    [HttpRequest(HttpMethodType.PUT, "User/Update/{id}")]
    public class UpdateUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // DELETE Request
    [HttpRequest(HttpMethodType.DELETE, "User/Delete/{id}")]
    public class DeleteUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }


    // GET Request
    [HttpRequest(HttpMethodType.GET, "Group/Read/{id}")]
    public class GetGroupQuery : IRequest<UserDto>
    {
        public int Id { get; set; }
    }


    // POST Request
    [HttpRequest(HttpMethodType.POST, "Group/Create")]
    public class CreateGroupCommand : IRequest<UserDto>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    // PUT Request
    [HttpRequest(HttpMethodType.PUT, "Group/Update/{id}")]
    public class UpdateGroupCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // DELETE Request
    [HttpRequest(HttpMethodType.DELETE, "Group/Delete/{id}")]
    public class DeleteGroupCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

}
