using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data.WorkflowAPI;
using PX.Data;
using PX.Objects.CM;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.FA;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects;
using System.Collections.Generic;
using System;
using PPSAAssetValidation; // <-- IMPORTANT: Make sure namespace is correct

namespace PX.Objects.FA
{
    public class FixedAssetExt : PXCacheExtension<FixedAsset>
    {
        #region UsrLastVerificationDate
        [PXDBDate]
        [PXUIField(DisplayName = "Last Verification Date", Enabled = false)]
        public DateTime? UsrLastVerificationDate { get; set; }
        public abstract class usrLastVerificationDate : PX.Data.BQL.BqlDateTime.Field<usrLastVerificationDate> { }
        #endregion

        #region UsrVerificationOutcome
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Verification Outcome", Enabled = false)]
        public string UsrVerificationOutcome { get; set; }
        public abstract class usrVerificationOutcome : PX.Data.BQL.BqlString.Field<usrVerificationOutcome> { }
        #endregion

        #region UsrAssetCondition
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Asset Condition")]
        [AVAssetCondition.List]   // <-- Your global list attribute
        public string UsrAssetCondition { get; set; }
        public abstract class usrAssetCondition : PX.Data.BQL.BqlString.Field<usrAssetCondition> { }
        #endregion

        #region UsrMarkedForDisposal
        [PXDBBool]
        [PXUIField(DisplayName = "Marked for Disposal")]
        public bool? UsrMarkedForDisposal { get; set; }
        public abstract class usrMarkedForDisposal : PX.Data.BQL.BqlBool.Field<usrMarkedForDisposal> { }
        #endregion

        #region UsrRefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "Verification Ref. Nbr.", Enabled = false)]
        public string UsrRefNbr { get; set; }
        public abstract class usrRefNbr : PX.Data.BQL.BqlString.Field<usrRefNbr> { }
        #endregion
    }
}
