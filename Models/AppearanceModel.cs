namespace Flexify.Models
{
    public class AppearanceModel : UserModel
    {
        public UserModel User { get; set; }
        public PageModel[] Pages { get; set; }
       public Socials[] Socials { get; set; }
        public PageLayoutModel Layout { get; set; }

        public AppearanceModel(UserModel user, PageModel[] pages, Socials[] sociasl, PageLayoutModel layout ) {
            this.User = user;
            this.Layout = layout;
            this.Pages = pages;
            this.Socials = sociasl;
        }    
    }
}
