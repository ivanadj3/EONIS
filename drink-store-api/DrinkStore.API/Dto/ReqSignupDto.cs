namespace DrinkStore.API.Dto
{
    public class ReqSignupDto
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

