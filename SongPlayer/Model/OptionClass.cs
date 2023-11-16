namespace SongPlayer.Model
{
    public class OptionClass
    {
        public int Id { get; set; }
        public string OptionName { get; set; }



        public List<OptionClass> GetOptionsList()
        {
            List<OptionClass> OptionsList = new List<OptionClass>()
             {

                new OptionClass() { Id = 1, OptionName = "Favoriye Ekle" },
                new OptionClass() { Id = 2, OptionName ="Şarkıyı Gönder"},
                new OptionClass() { Id = 3, OptionName = "Sil" }
             };

            return OptionsList;
        }
    }
}
