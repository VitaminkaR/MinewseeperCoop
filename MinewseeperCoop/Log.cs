namespace MinewseeperCoop
{
    public class Log
    {
        // информация
        private string log;
        private int logSize;

        // устанавливает лог
        public void Set(string info)
        {
            log = info;
        }

        // добавляет к логу
        public void Add(string info)
        {
            if (logSize < Options.LOG_SIZE)
            {
                log += ' ' + info + '\n';
                logSize++;
            }
            else
            {
                Remove();
                logSize = 0;
            }
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
