namespace MinewseeperCoop
{
    public class Log
    {
        // информация
        private string log;

        // устанавливает лог
        public void Set(string info)
        {
            log = info;
        }

        // добавляет к логу
        public void Add(string info)
        {
            log += ' ' + info + '\n';
        }

        // стирает лог
        public void Remove()
        {
            log = "";
        }

        // возварщает лог
        public string Get() => log;
    }
}
