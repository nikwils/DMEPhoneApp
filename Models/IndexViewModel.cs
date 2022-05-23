using System.Collections.Generic;
using DMEPhoneApp.Models;

namespace DMEPhoneApp.Models
{
    public class IndexViewModel
    {
        public IEnumerable<dob> Dobs { get; set; }
        public IEnumerable<name> Names { get; set; }
        public IEnumerable<result> Results { get; set; }
        public IEnumerable<picture> Pictures { get; set; }

        public PageViewModel PageViewModel { get; }
        public FilterViewModel FilterViewModel { get; }
        public SortViewModel SortViewModel { get; }
        public IndexViewModel(IEnumerable<result> results,  
                SortViewModel sortViewModel, PageViewModel pageViewModel, FilterViewModel filterViewModel)
        {
            Results = results;
            SortViewModel = sortViewModel;
            PageViewModel = pageViewModel;
            FilterViewModel = filterViewModel;
        }
    }
}
