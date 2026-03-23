namespace ProductsCRUD.Application.Common;

public static class PaginationHelper
{
    public static int NormalizePage(int page) => page < 1 ? 1 : page;
    public static int NormalizePageSize(int pageSize) => pageSize is < 1 or > 100 ? 10 : pageSize;
}