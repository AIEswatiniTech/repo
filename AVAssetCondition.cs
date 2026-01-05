using PX.Data;

namespace PPSAAssetValidation
{
    /// <summary>
    /// Defines the list of available asset conditions during verification.
    /// Used by AVVerificationLine.Condition.
    /// </summary>
    public class AVAssetCondition
    {
        public const string Excellent = "EXCLNT";
        public const string Good = "GOOD";
        public const string Fair = "FAIR";
        public const string Poor = "POOR";
        public const string Missing = "MISS";

        // BQL Constants for use in queries and defaults
        public class ExcellentS : PX.Data.BQL.BqlString.Constant<ExcellentS> { public ExcellentS() : base(Excellent) { } }
        public class GoodS : PX.Data.BQL.BqlString.Constant<GoodS> { public GoodS() : base(Good) { } }
        public class FairS : PX.Data.BQL.BqlString.Constant<FairS> { public FairS() : base(Fair) { } }
        public class PoorS : PX.Data.BQL.BqlString.Constant<PoorS> { public PoorS() : base(Poor) { } }
        public class MissingS : PX.Data.BQL.BqlString.Constant<MissingS> { public MissingS() : base(Missing) { } }

        /// <summary>
        /// PXStringListAttribute for the Asset Condition dropdown.
        /// </summary>
        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute()
                : base(
                new string[] { Excellent, Good, Fair, Poor, Missing },
                new string[] { "Excellent", "Good", "Fair", "Poor", "Missing" }) { }
        }
    }
}
