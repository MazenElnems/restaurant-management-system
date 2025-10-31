using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Restaurants.Application.Commands.Users.UnAssignToRole;

public class UnAssignRoleFromUserCommand : IRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    [Required]
    public string Role { get; set; }
}
