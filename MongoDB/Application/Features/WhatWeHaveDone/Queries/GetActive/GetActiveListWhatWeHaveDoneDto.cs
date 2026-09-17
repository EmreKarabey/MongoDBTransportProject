namespace Application.Features.WhatWeHaveDone.Queries.GetActive
{
    public class GetActiveListWhatWeHaveDoneDto
    {
        public string WhatWeHaveDoneId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public bool IsActive { get; set; }
    }
}
