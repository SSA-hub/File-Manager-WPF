using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileManagerWPF.Database
{
    [Table("file_opening_history")]
    public class FileOpeningHistory
    {
        [Key][Column("id")] public int Id { get; set; }
        [Column("filename")] public string Filename { get; set; }
        [Column("visiteddate")] public DateTime VisitedDate { get; set; }

        public FileOpeningHistory(string fileName)
        {
            Id = 0;
            Filename = fileName;
            VisitedDate = DateTime.UtcNow;
        }
    }
}
