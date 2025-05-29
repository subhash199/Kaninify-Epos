namespace EposRetail.Constants
{
    public static class CheckoutConstants
    {
        public static class CssClasses
        {
            public const string Hidden = "d-none";
            public const string Visible = "d-block"; // Add this line
            public const string TableResponsive = "table-responsive";
            public const string BtnDanger = "btn btn-danger";
            public const string BtnSuccess = "btn btn-success";
            public const string BtnWarning = "btn btn-warning";
        }

        public static class Images
        {
            public const string AddIcon = "images/icons/add_24dp_1F1F1F_FILL0_wght400_GRAD0_opsz24.png";
            public const string HideIcon = "images/icons/hide_24dp_1F1F1F_FILL0_wght400_GRAD0_opsz24.png";
            public const string CardIcon = "images/icons/account_balance_wallet_24dp_1F1F1F_FILL0_wght400_GRAD0_opsz24.png";
            public const string CashIcon = "images/icons/money_bag_24dp_1F1F1F_FILL0_wght400_GRAD0_opsz24.png";
        }

        public static class TableHeights
        {
            public const string Default = "height: 512px; overflow-y: auto";
            public const string SmallScreen = "height: 425px; overflow-y: auto";
            public const string Collapsed = "height: 335px; overflow-y: auto";
            public const string SmallCollapsed = "height: 225px; overflow-y: auto";
        }
    }
   
}