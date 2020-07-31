using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookTracker.Models
{
    public enum Genre
    {
        Fiction, Novel, Narrative,ScienceFiction, Mystery,NonFiction,Historical,
        Romance,Thriller,Horror,Fantasy, Memoir,Biography,YoungAdult,Poetry,
        ShortStory,Literacy,Autobiography,Science,Children,Adventure,CrimeFiction,
        Paranormal,Detective,Comedy,Mythology,Spiritual,Philosohy,Literature,Other

    }
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Author { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:00 MM yyyy}", ApplyFormatInEditMode=true)]
        public DateTime PublishedOn { get; set; }

        public int Pages { get; set; }

        public Genre? Genre { get; set; }

        public string Language { get; set; }

        public float Rating { get; set; }

        public ICollection<UserShelf> UserShelves { get; set; }

    }
}
