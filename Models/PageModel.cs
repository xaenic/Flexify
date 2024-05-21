﻿namespace Flexify.Models
{
    public class PageModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Clicks { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}
