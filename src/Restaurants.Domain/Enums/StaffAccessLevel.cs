namespace Restaurants.Domain.Enums;

[Flags]
public enum StaffAccessLevel
{
    None = 0,           // 000
    ReadOnly = 1 << 0,  // 001
    ReadUpdate = 1 << 1, // 010
    ReadDelete = 1 << 2, // 100
    ReadUpdateDelete = ReadOnly | ReadUpdate | ReadDelete
}