using System;

namespace Awari.Persistence
{
    public class AwariTable
    {
        /// <summary>
        /// Tablesize = n+2;
        /// n = Number of cups  withouth score cups; 
        /// TableVaules -> Implemented as a Queue 
        /// First n/2 ( 0 - (n/2-1) ) -> Red cups 
        /// n/2 -> Red Score cup
        /// n/2+1 - TableVaules.length-1 ( other from n/2+1 there is n/2 cup that is Blue's )  -> Blue cups
        /// Tablevalues.Length or _tableVaules[n+1] -> Blue score cup 
        /// </summary>
        private Int32 _tableSize;
        private Int32 _n;
        private Int32[] _tableValues;
        




        public Boolean IsRedNull
        {
            get
            {
                Boolean rEmpty = true;
                for (int i = 0; i < (_n / 2); i++)
                {
                    if (_tableValues[i] != 0)
                    {
                        rEmpty = false;
                    }

                }
                return rEmpty;
            }

        }
        public Boolean IsEmpty(Int32 x)
        {
            if (x < 0 || x >= _tableValues.Length)
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            

            return _tableValues[x] == 0;
        }
       

        public Boolean IsBlueNull
        {
            get
            {
                Boolean bEmpty = true;
                for (int i = (_n / 2 + 1); i < (_n + 1); i++)
                {
                    if (_tableValues[i] != 0)
                    {
                        bEmpty = false;
                    }

                }
                return bEmpty;
            }

        }

        public Int32 TableSize { get { return _tableSize; } }

        public Int32 NNumber { get { return _n; } }
        public Int32 ScoreR { get { return _tableValues[(_n / 2)]; } }
        public Int32 ScoreB { get { return _tableValues[(_n + 1)]; } }
        public Int32[] Vaules { get { return _tableValues; } }
        public Int32 this[Int32 x] { get { return GetValue(x); } }

        public AwariTable() : this(8) { }

        public AwariTable(Int32 n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException("A tábla mérete olyan kicsi mint a légynek a....", "tableSize");
            if (n != 4 && n != 8 && n != 12)
                throw new ArgumentOutOfRangeException("n", "Nem 4/8/12 az n");
            _tableSize = n + 2;
            _tableValues = new Int32[_tableSize];
            _n = n;
            for (int i = 0; i < _n / 2; i++)
            {
                _tableValues[i] = 6;
            }
            for (int i = (_n / 2) + 1; i < TableSize - 1; i++)
            {
                _tableValues[i] = 6;
            }

        }

        public Int32 GetValue(Int32 x)
        {
            if (x < 0 || x >= _tableSize)
            {
                throw new ArgumentOutOfRangeException("x", "Nem jó az index bro. What are you doing?! :( ");
            }


            return _tableValues[x];

        }

        public void AddValue(Int32 x, Int32 value)
        {
            if (x < 0 || x >= _tableSize)
            {
                throw new ArgumentOutOfRangeException("x", "Nem jó az index bro. What are you doing?! :( ");
            }

            _tableValues[x] += value;

        }

        public void SetValue(Int32 x, Int32 value)
        {
            if (x < 0 || x >= _tableSize)
            {
                throw new ArgumentOutOfRangeException("x", "Nem jó az index bro. What are you doing?! :( ");
            }

            _tableValues[x] = value;

        }

    }
}