using Sudoku.ApiService.Models.DbModels;

namespace Sudoku.ApiService.Repos.Interfaces
{
    public interface ITokenRepositoy
    {
        string CreateJwtToken(UserModel user, List<string> roles);
        public string? ParseTokenToUserId(string token);
    }
}
