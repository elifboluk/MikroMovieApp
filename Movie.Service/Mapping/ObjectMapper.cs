using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Service.Mapping
{
    public static class ObjectMapper // (Lazy Loading) - Static metod olduğu için direkt erişebiliriz.
    {
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() => // Parametre almıyor generic olduğu için, geriye IMapper dönüyor.
        {
            var config = new MapperConfiguration(x => 
            {
                x.AddProfile<MapProfile>(); // MapProfile içerisindeki eşleşmeleri bildirdik. (x==IMapperConfigurationExpression)
            });
            return config.CreateMapper();
        });
        public static IMapper Mapper => lazy.Value; // Sadece datayı alacak & Get 
    }
}

// Object Mapper içerisindeki datayı lazy loading kullanarak istediğimizde memory'e belleğe alacağız. Çağırmadığımız sürece bellekte yer tutmayacak.
// Action delege: Parametre alan ama geriye bir şey dönmeyen metodları işaret eder.
// Object Mapper'ın Mapper metodunu çağırıncaya kadar içerideki kod memory'e yüklenmeyecek.