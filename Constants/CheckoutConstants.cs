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
            public const string AddIcon = "images/icons/svg/add_24dp_1F1F1F_FILL0_wght400_GRAD0_opsz24.svg";
            public const string HideIcon = "images/icons/svg/hide_24dp_1F1F1F_FILL0_wght400_GRAD0_opsz24.svg";
            public const string CardIcon = "images/icons/svg/account_balance_wallet_24dp_1F1F1F_FILL0_wght400_GRAD0_opsz24.svg";
            public const string CashIcon = "images/icons/svg/money_bag_24dp_1F1F1F_FILL0_wght400_GRAD0_opsz24.svg";
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