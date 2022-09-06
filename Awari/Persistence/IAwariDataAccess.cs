using System;
using System.Threading.Tasks;

namespace Awari.Persistence
{
    public interface IAwariDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal. </param> 
        /// <retruns> A fájlból beolvasott tábla. </retruns>

        AwariTable LoadAsync(String path);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó tábla.</param>
        Task SaveAsync(String path, AwariTable table);
    }
}
