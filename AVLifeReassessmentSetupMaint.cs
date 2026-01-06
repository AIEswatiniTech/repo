using System;
using PX.Data;
using PX.Objects.EP;

namespace PPSAAssetValidation
{
    public class AVLifeReassessmentSetupMaint : PXGraph<AVLifeReassessmentSetupMaint>
    {
        public PXSave<AVLifeReassessmentSetup> Save;
        public PXCancel<AVLifeReassessmentSetup> Cancel;

        public PXSelect<AVLifeReassessmentSetup> Setup;
        public PXSelect<AVLifeReassessmentApprovalSetup,
            Where<AVLifeReassessmentApprovalSetup.setupID, Equal<Current<AVLifeReassessmentSetup.setupID>>>> ApprovalSetups;

        protected virtual void AVLifeReassessmentSetup_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
            AVLifeReassessmentSetup row = (AVLifeReassessmentSetup)e.Row;
            if (row == null) return;

            // When ApprovalsEnabled is toggled, update all approval setup records
            if (e.OldValue != null && e.NewValue != null && e.OldValue != e.NewValue)
            {
                bool enabledValue = (bool)e.NewValue;
                
                foreach (AVLifeReassessmentApprovalSetup approval in ApprovalSetups.Select())
                {
                    approval.IsActive = enabledValue;
                    ApprovalSetups.Update(approval);
                }
            }
        }
    }
}
