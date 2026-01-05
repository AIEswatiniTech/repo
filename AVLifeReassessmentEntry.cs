using System;
using System.Collections;
using System.Linq;
using PX.Data;
using PX.Objects.FA;
using PX.Objects.EP;

namespace PPSAAssetValidation
{
    public class AVLifeReassessmentEntry : PXGraph<AVLifeReassessmentEntry, AVLifeReassessment>
    {
        // Data views
        public PXSelect<AVLifeReassessment> Document;

        [PXFilterable]
        public PXSelect<AVLifeReassessmentLine> Lines;

        [PXViewName(PX.Objects.EP.Messages.Approval)]
        public EPApprovalAutomation<AVLifeReassessmentLine, AVLifeReassessmentLine.approved, 
            AVLifeReassessmentLine.rejected, AVLifeReassessmentLine.hold, 
            AVLifeReassessmentApprovalSetup> Approval;

        protected virtual IEnumerable lines()
        {
            var doc = Document.Current;
            if (doc == null || doc.ReassessmentID == null)
                return new System.Collections.Generic.List<AVLifeReassessmentLine>();

            var state = GetPaginationState(doc.ReassessmentID.Value);
            
            // Load all filtered assets on initial load only
            if (state.AllFilteredAssets == null)
            {
                _currentPage = 0;
                LoadAllFilteredAssets(doc);
            }

            // If there are already cached rows in the view, just return them as-is
            // This prevents rebuilding when user is just editing fields
            if (Lines?.Cache != null)
            {
                var cachedRows = new System.Collections.Generic.List<AVLifeReassessmentLine>();
                foreach (AVLifeReassessmentLine c in Lines.Cache.Cached)
                {
                    if (c != null)
                    {
                        cachedRows.Add(c);
                    }
                }
                
                // If we have cached rows, return them without any rebuilding
                if (cachedRows.Count > 0)
                {
                    return cachedRows;
                }
            }

            // Only build rows if cache is empty (initial load or after filter change)
            var linesList = new System.Collections.Generic.List<AVLifeReassessmentLine>();
            
            if (_allFilteredAssets != null && _allFilteredAssets.Count > 0)
            {
                int startIdx = _currentPage * PAGE_SIZE;
                int endIdx = System.Math.Min(startIdx + PAGE_SIZE, _allFilteredAssets.Count);

                int displayLineNbr = startIdx + 1;
                for (int i = startIdx; i < endIdx; i++)
                {
                    int assetID = _allFilteredAssets[i].AssetID.Value;
                    
                    FixedAsset fa = PXSelect<FixedAsset,
                        Where<FixedAsset.assetID, Equal<Required<FixedAsset.assetID>>>>.Select(this, assetID);
                    
                    if (fa == null) continue;

                    FALocationHistory loc = null;
                    _locationCache.TryGetValue(fa.AssetID.Value, out loc);

                    var faExt = fa.GetExtension<FixedAssetExt>();

                    // Load FADetails for date and cost information
                    FADetails details = PXSelect<FADetails,
                        Where<FADetails.assetID, Equal<Required<FADetails.assetID>>>>.Select(this, fa.AssetID);

                    // Build fresh row using cache to avoid type casting issues
                    var newLine = (AVLifeReassessmentLine)Lines.Cache.CreateInstance();
                    newLine.ReassessmentID = doc.ReassessmentID;
                    newLine.LineID = displayLineNbr;
                    newLine.AssetID = fa.AssetID;
                    newLine.AssetCD = fa.AssetCD;
                    newLine.Description = fa.Description;
                    newLine.ClassID = fa.ClassID.HasValue && _classCache.ContainsKey(fa.ClassID.Value) ? _classCache[fa.ClassID.Value].AssetCD : null;
                    
                    // Use cache to set DateTime fields properly
                    Lines.Cache.SetValueExt<AVLifeReassessmentLine.receiptDate>(newLine, details?.ReceiptDate);
                    Lines.Cache.SetValueExt<AVLifeReassessmentLine.depreciateFromDate>(newLine, details?.DepreciateFromDate);
                    Lines.Cache.SetValueExt<AVLifeReassessmentLine.origAcquisitionCost>(newLine, details?.AcquisitionCost);
                    
                    newLine.Department = loc?.Department;
                    newLine.Status = fa.Status;
                    newLine.OrigUsefulLife = fa.UsefulLife;
                    newLine.RemainingLife = fa.UsefulLife;
                    newLine.LifeAdjustmentYears = 0m;
                    newLine.NewUsefulLife = 0m;
                    newLine.Comments = faExt?.UsrVerificationOutcome;
                    newLine.Condition = faExt?.UsrAssetCondition;

                    linesList.Add(newLine);
                    displayLineNbr++;
                }
            }

            return linesList;
        }

        // Standard actions
        public new PXSave<AVLifeReassessment> Save;
        public new PXCancel<AVLifeReassessment> Cancel;
        public new PXInsert<AVLifeReassessment> Insert;
        public new PXDelete<AVLifeReassessment> Delete;
        public new PXFirst<AVLifeReassessment> First;
        public new PXPrevious<AVLifeReassessment> Previous;
        public new PXNext<AVLifeReassessment> Next;
        public new PXLast<AVLifeReassessment> Last;

        // Pagination actions
        public PXAction<AVLifeReassessment> FirstPage;
        public PXAction<AVLifeReassessment> PreviousPage;
        public PXAction<AVLifeReassessment> NextPage;
        public PXAction<AVLifeReassessment> LastPage;

        // Custom actions
        public PXAction<AVLifeReassessment> SubmitForApproval;
        public PXAction<AVLifeReassessment> Approve;
        public PXAction<AVLifeReassessment> Reject;
        public PXAction<AVLifeReassessment> Release;

        #region Actions

        [PXButton]
        [PXUIField(DisplayName = "First Page")]
        protected virtual IEnumerable firstPage(PXAdapter adapter)
        {
            var doc = Document.Current;
            if (doc?.ReassessmentID == null) return adapter.Get();
            _currentPage = 0;
            return adapter.Get();
        }

        [PXButton]
        [PXUIField(DisplayName = "Previous Page")]
        protected virtual IEnumerable previousPage(PXAdapter adapter)
        {
            var doc = Document.Current;
            if (doc?.ReassessmentID == null) return adapter.Get();
            if (_currentPage > 0) _currentPage--;
            return adapter.Get();
        }

        [PXButton]
        [PXUIField(DisplayName = "Next Page")]
        protected virtual IEnumerable nextPage(PXAdapter adapter)
        {
            var doc = Document.Current;
            if (doc?.ReassessmentID == null) return adapter.Get();
            if (_currentPage < _totalPages - 1) _currentPage++;
            return adapter.Get();
        }

        [PXButton]
        [PXUIField(DisplayName = "Last Page")]
        protected virtual IEnumerable lastPage(PXAdapter adapter)
        {
            var doc = Document.Current;
            if (doc?.ReassessmentID == null) return adapter.Get();
            _currentPage = _totalPages - 1;
            return adapter.Get();
        }

        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Submit for Approval", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable submitForApproval(PXAdapter adapter)
        {
            AVLifeReassessment doc = Document.Current;
            if (doc == null)
                return adapter.Get();

            if (doc.Status != AVLifeReassessmentStatus.OnHold)
            {
                throw new PXException("Only documents with 'On Hold' status can be submitted for approval.");
            }

            // At least one line required
            var lines = Lines.Select();
            AVLifeReassessmentLine firstLine = null;
            foreach (AVLifeReassessmentLine line in lines)
            {
                firstLine = line;
                break;
            }
            if (firstLine == null)
            {
                throw new PXException("At least one asset must be added before submission.");
            }

            Document.Cache.SetValueExt<AVLifeReassessment.status>(doc, AVLifeReassessmentStatus.PendingApproval);
            Persist();
            return adapter.Get();
        }

        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Approve", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable approve(PXAdapter adapter)
        {
            AVLifeReassessment doc = Document.Current;
            if (doc == null)
                return adapter.Get();

            if (doc.Status != AVLifeReassessmentStatus.PendingApproval)
            {
                throw new PXException("Only documents with 'Pending Approval' status can be approved.");
            }

            Document.Cache.SetValueExt<AVLifeReassessment.status>(doc, AVLifeReassessmentStatus.Approved);
            Persist();
            return adapter.Get();
        }

        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Reject", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable reject(PXAdapter adapter)
        {
            AVLifeReassessment doc = Document.Current;
            if (doc == null)
                return adapter.Get();

            if (doc.Status != AVLifeReassessmentStatus.PendingApproval)
            {
                throw new PXException("Only documents with 'Pending Approval' status can be rejected.");
            }

            Document.Cache.SetValueExt<AVLifeReassessment.status>(doc, AVLifeReassessmentStatus.Rejected);
            Persist();
            return adapter.Get();
        }

        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Release", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable release(PXAdapter adapter)
        {
            AVLifeReassessment doc = Document.Current;
            if (doc == null)
                return adapter.Get();

            if (doc.Status != AVLifeReassessmentStatus.Approved)
            {
                throw new PXException("Only approved documents can be released.");
            }

            Document.Cache.SetValueExt<AVLifeReassessment.status>(doc, AVLifeReassessmentStatus.Released);
            Persist();
            return adapter.Get();
        }

        #endregion

        #region Helper Methods

        #endregion

        #region Event Handlers

        protected virtual void AVLifeReassessment_RowSelected(PXCache cache, PXRowSelectedEventArgs e)
        {
            AVLifeReassessment doc = (AVLifeReassessment)e.Row;
            if (doc == null) return;

            bool isOnHold = doc.Status == AVLifeReassessmentStatus.OnHold;
            bool isPending = doc.Status == AVLifeReassessmentStatus.PendingApproval;
            bool isApproved = doc.Status == AVLifeReassessmentStatus.Approved;

            PXUIFieldAttribute.SetEnabled<AVLifeReassessment.reassessmentDate>(cache, doc, isOnHold);
            PXUIFieldAttribute.SetEnabled<AVLifeReassessment.description>(cache, doc, !isApproved);

            SubmitForApproval.SetEnabled(isOnHold);
            Approve.SetEnabled(isPending);
            Reject.SetEnabled(isPending);
            Release.SetEnabled(isApproved);

            // Grid editing controlled by approval status at line level (Hold field)
            // Only allow updates when document is in OnHold status
            Lines.AllowInsert = false;
            Lines.AllowDelete = false;
            Lines.AllowUpdate = isOnHold;

            // Only enable NewUsefulLife for editing when line is on Hold (not submitted for approval yet)
            var detailCache = Caches[typeof(AVLifeReassessmentLine)];
            foreach (AVLifeReassessmentLine line in Lines.Select())
            {
                bool isLineHeld = line.Hold == true;
                PXUIFieldAttribute.SetEnabled<AVLifeReassessmentLine.newUsefulLife>(detailCache, line, isLineHeld);
            }
        }

        protected virtual void AVLifeReassessmentLine_RowSelected(PXCache cache, PXRowSelectedEventArgs e)
        {
            // RowSelected events can stay minimal since field enabling is handled at document level
            AVLifeReassessmentLine line = (AVLifeReassessmentLine)e.Row;
            if (line == null) return;
        }

        protected virtual void AVLifeReassessmentLine_RowPersisting(PXCache cache, PXRowPersistingEventArgs e)
        {
            // Prevent automatic row persistence - let Persist() handle the updates
            e.Cancel = true;
        }

        #endregion

        #region Pagination and Caching

        private const int PAGE_SIZE = 100;

        private static System.Collections.Generic.Dictionary<int, PaginationState> _paginationStates =
            new System.Collections.Generic.Dictionary<int, PaginationState>();

        private class PaginationState
        {
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public System.Collections.Generic.List<FixedAsset> AllFilteredAssets { get; set; }
            public System.Collections.Generic.Dictionary<int, FAClass> ClassCache { get; set; }
            public System.Collections.Generic.Dictionary<int, FALocationHistory> LocationCache { get; set; }
        }

        private PaginationState GetPaginationState(int reassessmentID)
        {
            if (!_paginationStates.ContainsKey(reassessmentID))
            {
                _paginationStates[reassessmentID] = new PaginationState
                {
                    CurrentPage = 0,
                    TotalPages = 0,
                    AllFilteredAssets = null,
                    ClassCache = new System.Collections.Generic.Dictionary<int, FAClass>(),
                    LocationCache = new System.Collections.Generic.Dictionary<int, FALocationHistory>()
                };
            }
            return _paginationStates[reassessmentID];
        }

        private int _currentPage
        {
            get
            {
                var doc = Document.Current;
                if (doc?.ReassessmentID == null) return 0;
                return GetPaginationState(doc.ReassessmentID.Value).CurrentPage;
            }
            set
            {
                var doc = Document.Current;
                if (doc?.ReassessmentID != null)
                {
                    GetPaginationState(doc.ReassessmentID.Value).CurrentPage = value;
                }
            }
        }

        private int _totalPages
        {
            get
            {
                var doc = Document.Current;
                if (doc?.ReassessmentID == null) return 0;
                return GetPaginationState(doc.ReassessmentID.Value).TotalPages;
            }
            set
            {
                var doc = Document.Current;
                if (doc?.ReassessmentID != null)
                {
                    GetPaginationState(doc.ReassessmentID.Value).TotalPages = value;
                }
            }
        }

        private System.Collections.Generic.List<FixedAsset> _allFilteredAssets
        {
            get
            {
                var doc = Document.Current;
                if (doc?.ReassessmentID == null) return null;
                return GetPaginationState(doc.ReassessmentID.Value).AllFilteredAssets;
            }
            set
            {
                var doc = Document.Current;
                if (doc?.ReassessmentID != null)
                {
                    GetPaginationState(doc.ReassessmentID.Value).AllFilteredAssets = value;
                }
            }
        }

        private System.Collections.Generic.Dictionary<int, FAClass> _classCache
        {
            get
            {
                var doc = Document.Current;
                if (doc?.ReassessmentID == null) return new System.Collections.Generic.Dictionary<int, FAClass>();
                return GetPaginationState(doc.ReassessmentID.Value).ClassCache;
            }
            set
            {
                var doc = Document.Current;
                if (doc?.ReassessmentID != null)
                {
                    GetPaginationState(doc.ReassessmentID.Value).ClassCache = value;
                }
            }
        }

        private System.Collections.Generic.Dictionary<int, FALocationHistory> _locationCache
        {
            get
            {
                var doc = Document.Current;
                if (doc?.ReassessmentID == null) return new System.Collections.Generic.Dictionary<int, FALocationHistory>();
                return GetPaginationState(doc.ReassessmentID.Value).LocationCache;
            }
            set
            {
                var doc = Document.Current;
                if (doc?.ReassessmentID != null)
                {
                    GetPaginationState(doc.ReassessmentID.Value).LocationCache = value;
                }
            }
        }

        private void LoadAllFilteredAssets(AVLifeReassessment doc)
        {
            if (doc == null) return;

            _allFilteredAssets = new System.Collections.Generic.List<FixedAsset>();

            // Load all active (non-disposed) fixed assets
            foreach (PXResult<FixedAsset> res in PXSelectReadonly<FixedAsset,
                Where<FixedAsset.status, NotEqual<FixedAssetStatus.disposed>>,
                OrderBy<Asc<FixedAsset.assetID>>>.Select(this))
            {
                var fa = (FixedAsset)res;
                _allFilteredAssets.Add(fa);
            }

            if (_allFilteredAssets.Count == 0)
            {
                _totalPages = 0;
                return;
            }

            _totalPages = (_allFilteredAssets.Count + PAGE_SIZE - 1) / PAGE_SIZE;

            // Build class cache
            _classCache = new System.Collections.Generic.Dictionary<int, FAClass>();
            var classIds = new System.Collections.Generic.HashSet<int>();
            foreach (var fa in _allFilteredAssets)
                if (fa.ClassID.HasValue) classIds.Add(fa.ClassID.Value);
            foreach (var classId in classIds)
            {
                var faClass = PXSelect<FAClass, Where<FAClass.classID, Equal<Required<FAClass.classID>>>>.Select(this, classId);
                if (faClass != null)
                    _classCache[classId] = faClass;
            }

            // Prefetch location histories
            _locationCache = new System.Collections.Generic.Dictionary<int, FALocationHistory>();
            PrefetchLocationHistoriesBatched(_allFilteredAssets);
        }

        private void PrefetchLocationHistoriesBatched(System.Collections.Generic.List<FixedAsset> assets)
        {
            if (assets == null || assets.Count == 0) return;

            int batchSize = 50;
            for (int i = 0; i < assets.Count; i += batchSize)
            {
                int endIdx = System.Math.Min(i + batchSize, assets.Count);
                var assetIdsBatch = new int?[endIdx - i];
                for (int j = i; j < endIdx; j++)
                {
                    assetIdsBatch[j - i] = assets[j].AssetID;
                }

                foreach (PXResult<FALocationHistory> res in PXSelectReadonly<FALocationHistory,
                    Where<FALocationHistory.assetID, In<Required<FALocationHistory.assetID>>>>.Select(this, assetIdsBatch))
                {
                    var loc = (FALocationHistory)res;
                    if (!_locationCache.ContainsKey(loc.AssetID.Value))
                    {
                        _locationCache[loc.AssetID.Value] = loc;
                    }
                }
            }
        }

        public override void Persist()
        {
            // Only sync to FixedAsset when line is approved
            SyncApprovedLinesToFixedAsset();
            base.Persist();
            
            // After save, clear the filtered assets cache so rows reload on next view refresh
            var doc = Document.Current;
            if (doc?.ReassessmentID != null)
            {
                var state = GetPaginationState(doc.ReassessmentID.Value);
                state.AllFilteredAssets = null;
                state.ClassCache = null;
                state.LocationCache = null;
            }
        }

        private void SyncApprovedLinesToFixedAsset()
        {
            // Sync only APPROVED lines with NewUsefulLife changes to FixedAsset
            foreach (AVLifeReassessmentLine line in Lines.Select())
            {
                if (line == null || line.AssetID == null)
                    continue;

                // Only sync if line is approved
                if (line.Approved != true)
                {
                    continue;
                }

                // Only update if NewUsefulLife has a value
                if (line.NewUsefulLife == null || line.NewUsefulLife <= 0m)
                {
                    continue;
                }

                try
                {
                    var updateParams = new System.Collections.Generic.List<PX.Data.PXDataFieldParam>();
                    updateParams.Add(new PX.Data.PXDataFieldAssign<FixedAsset.usefulLife>(line.NewUsefulLife.Value));
                    updateParams.Add(new PX.Data.PXDataFieldRestrict<FixedAsset.assetID>(line.AssetID));

                    PX.Data.PXDatabase.Update<FixedAsset>(updateParams.ToArray());
                }
                catch (Exception ex)
                {
                    PXTrace.WriteError($"Error syncing approved FixedAsset {line.AssetID}: {ex.Message}");
                }
            }
        }

        #endregion
    }
}
