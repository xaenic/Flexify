namespace Flexify.Models
{
    public class AppearanceModel : UserModel
    {
        public UserModel User { get; set; }
        public PageModel[] page { get; set; }
        public Socials[] Socials { get; set; }
        public Theme theme { get; set; }
        public PostModel[] posts { get; set; }


        public AppearanceModel(UserModel user, PageModel[] page, Socials[] sociasl, Theme theme, PostModel[] posts)
        {
            this.User = user;
            this.theme = theme;
            this.page = page;
            this.Socials = sociasl;
            this.posts = posts;
        }
    }
}