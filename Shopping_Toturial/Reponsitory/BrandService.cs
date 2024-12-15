namespace Shopping_Toturial.Reponsitory
{
    public class BrandService
    {
        private readonly DataContext _context;

        public BrandService(DataContext context)
        {
            _context = context;
        }

        public void GetAllBrands()
        {
            var brands = _context.Brands.ToList();

            foreach (var brand in brands)
            {
                Console.WriteLine($"Id: {brand.Id}, Name: {brand.Name}");
            }
        }
    }
}
