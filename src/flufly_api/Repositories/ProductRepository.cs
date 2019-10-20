namespace FNCPLT.FinancialProfile.Repository.FinancePartner
{
    public class FinancePartnerRepository
    { 

        private static string CreateWhereStatementForOrder(
            IEnumerable<string> searchRegions,
            IEnumerable<string> searchProductTypes,
            IEnumerable<string> searchCountries,
            IEnumerable<string> searchCurrencies,
            IEnumerable<string> searchProductCategories,
            IEnumerable<string> fluflyProducts,
            IEnumerable<string> customerTypes)
        {
            StringBuilder sqlWhere = new StringBuilder();
            List<string> queries = new List<string>();
            queries.Add(CreateQueryForOrderField(searchRegions, "'productPlacement'->'region'"));
            queries.Add(CreateQueryForOrderField(searchCountries, "'productPlacement'->'country'"));
            queries.Add(CreateQueryForOrderFieldArray(productTypes, "'fluflyProductDetails'->'fluflyProductTypes'"));
            queries.Add(CreateQueryForOrderField(productCategories, "'fluflyProductDetails'->'fluflyProductCategory'"));
            queries.Add(CreateQueryForOrderField(customerTypes, "'customerDetails'->'customerType'"));
            queries.Add(CreateQueryForOrderField(currencies, "'productMarket'->'currency'"));

            var queriesNotEmpty = queries.Where(query => query.Length != 0);
            if (queriesNotEmpty.Count() != 0)
            {
                int index = 0;
                sqlWhere.Append("WHERE ");

                foreach (var query in queriesNotEmpty)
                {
                    if (index++ > 0)
                    {
                        sqlWhere.Append(" AND ");
                    }
                    sqlWhere.Append(query);
                }
            }

            return sqlWhere.ToString();
        }

    }
}
