namespace DMEPhoneApp.Models
{
    public class SortViewModel
    {
        public SortState LastNameSort { get; } // значение для сортировки по имени
        public SortState DateOfBirthSort { get; }    // значение для сортировки по возрасту
        public SortState Current { get; }     // текущее значение сортировки

        public SortViewModel(SortState sortOrder)
        {
            LastNameSort = sortOrder == SortState.LastNameAsc ? SortState.LastNameDesc : SortState.LastNameAsc;
            DateOfBirthSort = sortOrder == SortState.DateOfBirthAsc ? SortState.DateOfBirthDesc : SortState.DateOfBirthAsc;
            Current = sortOrder;
        }
    }
}