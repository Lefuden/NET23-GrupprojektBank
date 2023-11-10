namespace NET23_GrupprojektBank.Users
{
    internal class User
    {
        private Guid UserId { get; set; }
        private string Salt { get; set; }
        private string HashedPassword { get; set; }

        public User()
        {
            UserId = Guid.NewGuid();
            Salt = BCrypt.Net.BCrypt.GenerateSalt();

            Console.WriteLine(UserId);
        }
    }
}
