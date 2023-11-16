using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongPlayer.Model
{
   
    public class OptionPictureClass
    {
        public int Id { get; set; }
        public string OptionName { get; set; }



        public List<OptionClass> GetOptionsList()
        {
            List<OptionClass> OptionsList = new List<OptionClass>()
             {

                new OptionClass() { Id = 1, OptionName = "Üst Alana Ekle" },
                new OptionClass() { Id = 2, OptionName = "Alt Alana Ekle"},
                new OptionClass() { Id = 3, OptionName = "Kimim Ben Bölümüne Ekle" },
                new OptionClass() { Id = 4, OptionName = "Resmi Sil" }
             };

            return OptionsList;
        }
    }
}
