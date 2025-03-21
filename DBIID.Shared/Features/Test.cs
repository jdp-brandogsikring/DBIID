using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBIID.Application.Shared.Attributes;
using DBIID.Application.Shared.Dtos;
using DBIID.Shared.Results;
using MediatR;

namespace DBIID.Application.Features.Users
{



    //// DELETE Request
    //[HttpRequest(HttpMethodType.DELETE, "User/Delete/{id}")]
    //public class DeleteUserCommand : IRequest<bool>
    //{
    //    public int Id { get; set; }
    //}


    //// GET Request
    //[HttpRequest(HttpMethodType.GET, "Group/Read/{id}")]
    //public class GetGroupQuery : IRequest<UserDto>
    //{
    //    public int Id { get; set; }
    //}


    //// POST Request
    //[HttpRequest(HttpMethodType.POST, "Group/Create")]
    //public class CreateGroupCommand : IRequest<UserDto>
    //{
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //}

    //// POST Request
    //[HttpRequest(HttpMethodType.POST, "Group/Create")]
    //public class CreatesGroupCommand : IRequest<UserDto>
    //{
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //}

    //// PUT Request
    //[HttpRequest(HttpMethodType.PUT, "Group/Update/{id}")]
    //public class UpdateGroupCommand : IRequest<bool>
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

    //// DELETE Request
    //[HttpRequest(HttpMethodType.DELETE, "Group/Delete/{id}")]
    //public class DeleteGroupCommand : IRequest<bool>
    //{
    //    public int Id { get; set; }
    //}


}
