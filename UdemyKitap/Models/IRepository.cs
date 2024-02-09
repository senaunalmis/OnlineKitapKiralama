using System.Linq.Expressions;

namespace UdemyKitap.Models
{
    public interface IRepository<T> where T : class
    {
        //T->kitap türü vs
        IEnumerable<T> TumunuGetir(string? includeProps = null);
        T Get(Expression<Func<T, bool>> filtre, string? includeProps = null);
        void Ekle(T entity);
        void Sil(T entity);
        void SilAralık(IEnumerable<T> entities);


    }
}
