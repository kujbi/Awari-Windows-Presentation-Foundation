using System;
using System.Threading.Tasks;
using System.IO;

namespace Awari.Persistence
{
    public class AwariFileDataAccess : IAwariDataAccess
    {

        ///<summary>
        /// Fájl betöltés.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott tábla.</returns>

        public AwariTable LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line =  reader.ReadLine();
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    Int32 n = Int32.Parse(numbers[0]); // beolvassuk a tábla méretét
                    AwariTable table = new AwariTable(n); // létrehozzuk a táblát

                    for (int i = 0; i < n + 2; i++)
                    {

                        table.SetValue(i, int.Parse(numbers[i + 1]));
                    }


                    return table;
                }
            }
            catch
            {
                throw new AwariDataException();
            }

        }

        ///<summary>
        /// Fájl mentés
        /// </summary>
        /// <param name="path">Elérési útvonal</param>
        /// <param name="table">A fájlba kiírt tábla.</param>
        public async Task SaveAsync(String path, AwariTable table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    writer.Write(table.NNumber + " ");
                    for (Int32 i = 0; i < table.TableSize; i++)
                    {
                        await writer.WriteAsync(table.GetValue(i) + " ");
                    }
                }
            }
            catch
            {
                throw new AwariDataException();
            }
        }
    }
}
