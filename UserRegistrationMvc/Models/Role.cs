namespace UserRegistrationMvc.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RoleUser> RoleUsers { get; set; }
    }
}
