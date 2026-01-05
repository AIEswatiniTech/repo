using PX.Data;

namespace PPSAAssetValidation
{
    /// <summary>
    /// Defines the list of available statuses for the Asset Verification document (AVVerification.Status).
    /// </summary>
    public class AVVerificationStatus
    {
        public const string OnHold = "HOLD"; // Initial state
        public const string Initial = "INIT"; // Assets loaded after Initialize
        public const string AssetCountInProgress = "INPGRS";
        public const string Balanced = "BALANC"; // Ready for release (after count completion)
        public const string InReview = "REVIEW";
        public const string Released = "RLSD"; // Final state after processing
        public const string Completed = "CMPLTD";
        public const string Cancelled = "CANCEL";

        // BQL Constants for use in queries and defaults
        public class OnHoldS : PX.Data.BQL.BqlString.Constant<OnHoldS> { public OnHoldS() : base(OnHold) { } }
        public class InitialS : PX.Data.BQL.BqlString.Constant<InitialS> { public InitialS() : base(Initial) { } }
        public class AssetCountInProgressS : PX.Data.BQL.BqlString.Constant<AssetCountInProgressS> { public AssetCountInProgressS() : base(AssetCountInProgress) { } }
        public class BalancedS : PX.Data.BQL.BqlString.Constant<BalancedS> { public BalancedS() : base(Balanced) { } }
        public class InReviewS : PX.Data.BQL.BqlString.Constant<InReviewS> { public InReviewS() : base(InReview) { } }
        public class ReleasedS : PX.Data.BQL.BqlString.Constant<ReleasedS> { public ReleasedS() : base(Released) { } }
        public class CompletedS : PX.Data.BQL.BqlString.Constant<CompletedS> { public CompletedS() : base(Completed) { } }
        public class CancelledS : PX.Data.BQL.BqlString.Constant<CancelledS> { public CancelledS() : base(Cancelled) { } }

        /// <summary>
        /// PXStringListAttribute for the Status dropdown on the Asset Verification screen.
        /// </summary>
        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute()
                : base(
                new string[] { OnHold, Initial, AssetCountInProgress, Balanced, InReview, Released, Completed, Cancelled },
                new string[] { "On Hold", "Initialized", "Asset Count In progress", "Balanced", "In Review", "Released", "Completed", "Cancelled" }) { }
        }
    }
}
