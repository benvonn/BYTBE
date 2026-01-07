namespace CornHoleRevamp.Models
{
    public class UserRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Passcode { get; set; } = string.Empty;
    }
    public class UpdateBoardDto
    {
        public string Board { get; set; } = string.Empty;
    }

}