using System;
using PX.Data;
using PX.Objects.EP;

namespace PPSAAssetValidation
{
    public class EPApprovalMapMaintExt : PXGraphExtension<EPApprovalMapMaint>
    {
        public static bool IsActive()
        {
            return true;
        }

        [PXOverride]
        public virtual void SetupEntityType()
        {
            // Call the original method
            Base.SetupEntityType();

            // Register our custom AVLifeReassessmentLine entity for approvals
            var entityTypeItem = new EPApprovalMapSetup
            {
                EntityType = typeof(AVLifeReassessmentLine).FullName,
                EntityTypeDisplay = "Asset Life Reassessment Line"
            };
        }
    }
}
