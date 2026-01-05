using System;
using PX.Data;

namespace PPSAAssetValidation
{
    /// <summary>
    /// Defines the list of available statuses for Asset Life Reassessment
    /// </summary>
    public class AVLifeReassessmentStatus
    {
        public const string OnHold = "HOLD";
        public const string PendingApproval = "PNDAPRV";
        public const string Approved = "APRVD";
        public const string Released = "RLSD";
        public const string Rejected = "RJCTD";
        public const string Cancelled = "CANCEL";

        // BQL Constants
        public class OnHoldS : PX.Data.BQL.BqlString.Constant<OnHoldS> { public OnHoldS() : base(OnHold) { } }
        public class PendingApprovalS : PX.Data.BQL.BqlString.Constant<PendingApprovalS> { public PendingApprovalS() : base(PendingApproval) { } }
        public class ApprovedS : PX.Data.BQL.BqlString.Constant<ApprovedS> { public ApprovedS() : base(Approved) { } }
        public class ReleasedS : PX.Data.BQL.BqlString.Constant<ReleasedS> { public ReleasedS() : base(Released) { } }
        public class RejectedS : PX.Data.BQL.BqlString.Constant<RejectedS> { public RejectedS() : base(Rejected) { } }
        public class CancelledS : PX.Data.BQL.BqlString.Constant<CancelledS> { public CancelledS() : base(Cancelled) { } }

        /// <summary>
        /// PXStringListAttribute for the Status dropdown
        /// </summary>
        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute()
                : base(
                new string[] { OnHold, PendingApproval, Approved, Released, Rejected, Cancelled },
                new string[] { "On Hold", "Pending Approval", "Approved", "Released", "Rejected", "Cancelled" }) { }
        }
    }
}