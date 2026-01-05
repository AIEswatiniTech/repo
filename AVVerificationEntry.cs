using System;
using System.Collections;
using PX.Data;
using PX.Objects.FA;

namespace PPSAAssetValidation
{
    public class AVVerificationEntry : PXGraph<AVVerificationEntry, AVVerification>
    {
        // Temporary debug toggle to force-enable editing in the grid.
        // Set to true to bypass status check and allow inline edits for testing.
        private const bool FORCE_ENABLE_EDITING_FOR_TEST = true;
        public PXSelect<AVVerification> MasterView;

        [PXFilterable]
        public PXSelect<AVVerificationLine> DetailsView;

        protected virtual IEnumerable detailsView()
        {
            var doc = MasterView.Current;
            if (doc == null || doc.VerificationID == null)
                return new System.Collections.Generic.List<AVVerificationLine>();

            var state = GetPaginationState(doc.VerificationID.Value);
            
            // Load all filtered assets on initial load only
            if (state.AllFilteredAssets == null)
            {
                _currentPage = 0;
                LoadAllFilteredAssets(doc);
            }

            // If there are already cached rows in the view, just return them as-is
            // This prevents rebuilding when user is just editing fields
            if (DetailsView?.Cache != null)
            {
                var cachedRows = new System.Collections.Generic.List<AVVerificationLine>();
                foreach (AVVerificationLine c in DetailsView.Cache.Cached)
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
            var lines = new System.Collections.Generic.List<AVVerificationLine>();
            
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

                    // Apply location-level filters
                    if (doc.Building != null && (loc?.BuildingID != doc.Building)) continue;
                    if (doc.Custodian != null && (loc?.EmployeeID != doc.Custodian)) continue;
                    if (!string.IsNullOrWhiteSpace(doc.Department) && (loc?.Department != doc.Department)) continue;

                    var faExt = fa.GetExtension<FixedAssetExt>();

                    // Build fresh row from FixedAsset
                    var newLine = new AVVerificationLine
                    {
                        VerificationID = doc.VerificationID,
                        LineID = displayLineNbr,
                        AssetID = fa.AssetID,
                        AssetStatus = fa.Status,
                        AssetDescription = fa.Description,
                        BranchID = fa.BranchID,
                        Building = loc?.BuildingID,
                        Floor = loc?.Floor,
                        Room = loc?.Room,
                        Custodian = loc?.EmployeeID,
                        Department = loc?.Department,
                        AssetClass = fa.ClassID,
                        Asset = fa.AssetCD,
                        AssetQuantity = fa.Qty ?? 1m,
                        ExpectedQty = fa.Qty ?? 1m,
                        CountQuantity = 0m,
                        Status = fa.Status,
                        Condition = faExt?.UsrAssetCondition,
                        MarkedForDisposal = faExt?.UsrMarkedForDisposal ?? false,
                        Comments = faExt?.UsrVerificationOutcome
                    };

                    lines.Add(newLine);
                    displayLineNbr++;
                }
            }

            return lines;
        }

        public new PXSave<AVVerification> Save;
        public new PXCancel<AVVerification> Cancel;
        public new PXInsert<AVVerification> Insert;
        public new PXDelete<AVVerification> Delete;
        public new PXFirst<AVVerification> First;
        public new PXPrevious<AVVerification> Previous;
        public new PXNext<AVVerification> Next;
        public new PXLast<AVVerification> Last;

        // Custom actions referenced in ASPX
        public PXAction<AVVerification> RemoveHold;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Remove Hold")]
        protected virtual void removeHold()
        {
            var doc = MasterView.Current;
            if (doc != null && doc.Status == AVVerificationStatus.OnHold)
            {
                doc.Status = AVVerificationStatus.AssetCountInProgress;
                MasterView.Update(doc);
                Save.Press();
            }
        }

        public PXAction<AVVerification> FirstPage;
        [PXButton]
        [PXUIField(DisplayName = "First Page")]
        protected virtual IEnumerable firstPage(PXAdapter adapter)
        {
            var doc = MasterView.Current;
            if (doc?.VerificationID == null) return adapter.Get();
            _currentPage = 0;
            return adapter.Get();
        }

        public PXAction<AVVerification> PreviousPage;
        [PXButton]
        [PXUIField(DisplayName = "Previous Page")]
        protected virtual IEnumerable previousPage(PXAdapter adapter)
        {
            var doc = MasterView.Current;
            if (doc?.VerificationID == null) return adapter.Get();
            if (_currentPage > 0) _currentPage--;
            return adapter.Get();
        }

        public PXAction<AVVerification> NextPage;
        [PXButton]
        [PXUIField(DisplayName = "Next Page")]
        protected virtual IEnumerable nextPage(PXAdapter adapter)
        {
            var doc = MasterView.Current;
            if (doc?.VerificationID == null) return adapter.Get();
            if (_currentPage < _totalPages - 1) _currentPage++;
            return adapter.Get();
        }

        public PXAction<AVVerification> LastPage;
        [PXButton]
        [PXUIField(DisplayName = "Last Page")]
        protected virtual IEnumerable lastPage(PXAdapter adapter)
        {
            var doc = MasterView.Current;
            if (doc?.VerificationID == null) return adapter.Get();
            _currentPage = _totalPages - 1;
            return adapter.Get();
        }

        public PXAction<AVVerification> Submit;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Submit")]
        protected virtual void submit()
        {
            var doc = MasterView.Current;
            if (doc != null && doc.Status == AVVerificationStatus.AssetCountInProgress)
            {
                doc.Status = AVVerificationStatus.Balanced;
                MasterView.Update(doc);
                Save.Press();
            }
        }

        public PXAction<AVVerification> CancelVerification;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Cancel")]
        protected virtual void cancelVerification()
        {
            var doc = MasterView.Current;
            if (doc != null)
            {
                doc.Status = AVVerificationStatus.Cancelled;
                MasterView.Update(doc);
                Save.Press();
            }
        }

        
        private const int PAGE_SIZE = 100;

        // Auto-generate the next RefNbr on insert
        protected virtual void AVVerification_RowInserting(PXCache cache, PXRowInsertingEventArgs e)
        {
            var row = e.Row as AVVerification;
            if (row != null && string.IsNullOrEmpty(row.RefNbr))
            {
                // Get the highest RefNbr from FixedAsset.UsrRefNbr (where actual verifications are tracked)
                int nextNumber = 1;
                
                var maxRefNbrRecord = PXSelectOrderBy<FixedAsset,
                    OrderBy<Desc<FixedAssetExt.usrRefNbr>>>.Select(this);
                
                foreach (PXResult<FixedAsset> result in maxRefNbrRecord)
                {
                    FixedAsset maxRefNbr = (FixedAsset)result;
                    if (maxRefNbr != null)
                    {
                        var maxExt = maxRefNbr.GetExtension<FixedAssetExt>();
                        if (maxExt != null && !string.IsNullOrEmpty(maxExt.UsrRefNbr))
                        {
                            if (int.TryParse(maxExt.UsrRefNbr, out int lastNum))
                            {
                                nextNumber = lastNum + 1;
                            }
                        }
                    }
                    break;
                }

                row.RefNbr = nextNumber.ToString("D4");
            }

            e.Cancel = true;
        }

        protected virtual void AVVerification_RowUpdating(PXCache cache, PXRowUpdatingEventArgs e)
        {
            e.Cancel = true;
        }

        // Persist detail line changes to FixedAsset when Save is clicked
        public override void Persist()
        {
            SyncDetailsToFixedAsset();
            base.Persist();
            
            // After save, clear the filtered assets cache so rows reload on next view refresh
            var doc = MasterView.Current;
            if (doc?.VerificationID != null)
            {
                var state = GetPaginationState(doc.VerificationID.Value);
                state.AllFilteredAssets = null;
                state.ClassCache = null;
                state.LocationCache = null;
            }
        }

        private void SyncDetailsToFixedAsset()
        {
            foreach (AVVerificationLine line in DetailsView.Cache.Updated)
            {
                if (line != null && line.AssetID != null)
                {
                    try
                    {
                        var updateParams = new System.Collections.Generic.List<PX.Data.PXDataFieldParam>();
                        updateParams.Add(new PX.Data.PXDataFieldAssign("UsrAssetCondition", line.Condition));
                        updateParams.Add(new PX.Data.PXDataFieldAssign("UsrMarkedForDisposal", line.MarkedForDisposal ?? false));
                        updateParams.Add(new PX.Data.PXDataFieldAssign("UsrVerificationOutcome", line.Comments));
                        updateParams.Add(new PX.Data.PXDataFieldRestrict<FixedAsset.assetID>(line.AssetID));

                        PX.Data.PXDatabase.Update<FixedAsset>(updateParams.ToArray());
                    }
                    catch (Exception ex)
                    {
                        PXTrace.WriteError($"Error syncing FixedAsset {line.AssetID}: {ex.Message}");
                    }
                }
            }
        }

        private static System.Collections.Generic.Dictionary<int, PaginationState> _paginationStates =
            new System.Collections.Generic.Dictionary<int, PaginationState>();

        private class PaginationState
        {
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public bool AssetsLoaded { get; set; }
            public bool FirstLoadDone { get; set; }
            public System.Collections.Generic.List<FixedAsset> AllFilteredAssets { get; set; }
            public System.Collections.Generic.Dictionary<int, FAClass> ClassCache { get; set; }
            public System.Collections.Generic.Dictionary<int, FALocationHistory> LocationCache { get; set; }
        }

        private PaginationState GetPaginationState(int verificationID)
        {
            if (!_paginationStates.ContainsKey(verificationID))
            {
                _paginationStates[verificationID] = new PaginationState
                {
                    CurrentPage = 0,
                    TotalPages = 0,
                    AssetsLoaded = false,
                    FirstLoadDone = false,
                    AllFilteredAssets = null,
                    ClassCache = null,
                    LocationCache = null
                };
            }
            return _paginationStates[verificationID];
        }

        private void ClearPaginationCache(int verificationID)
        {
            if (_paginationStates.ContainsKey(verificationID))
            {
                _paginationStates.Remove(verificationID);
            }
        }

        private int _currentPage
        {
            get
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID == null) return 0;
                return GetPaginationState(doc.VerificationID.Value).CurrentPage;
            }
            set
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID != null)
                {
                    GetPaginationState(doc.VerificationID.Value).CurrentPage = value;
                }
            }
        }

        private int _totalPages
        {
            get
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID == null) return 0;
                return GetPaginationState(doc.VerificationID.Value).TotalPages;
            }
            set
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID != null)
                {
                    GetPaginationState(doc.VerificationID.Value).TotalPages = value;
                }
            }
        }

        private bool _assetsLoaded
        {
            get
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID == null) return false;
                return GetPaginationState(doc.VerificationID.Value).AssetsLoaded;
            }
            set
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID != null)
                {
                    GetPaginationState(doc.VerificationID.Value).AssetsLoaded = value;
                }
            }
        }

        private System.Collections.Generic.List<FixedAsset> _allFilteredAssets
        {
            get
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID == null) return null;
                return GetPaginationState(doc.VerificationID.Value).AllFilteredAssets;
            }
            set
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID != null)
                {
                    GetPaginationState(doc.VerificationID.Value).AllFilteredAssets = value;
                }
            }
        }

        private System.Collections.Generic.Dictionary<int, FAClass> _classCache
        {
            get
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID == null) return null;
                return GetPaginationState(doc.VerificationID.Value).ClassCache;
            }
            set
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID != null)
                {
                    GetPaginationState(doc.VerificationID.Value).ClassCache = value;
                }
            }
        }

        private System.Collections.Generic.Dictionary<int, FALocationHistory> _locationCache
        {
            get
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID == null) return null;
                return GetPaginationState(doc.VerificationID.Value).LocationCache;
            }
            set
            {
                var doc = MasterView.Current;
                if (doc?.VerificationID != null)
                {
                    GetPaginationState(doc.VerificationID.Value).LocationCache = value;
                }
            }
        }

        protected virtual void AVVerification_RowSelected(PXCache cache, PXRowSelectedEventArgs e)
        {
            var doc = e.Row as AVVerification;
            if (doc == null) return;

            PXUIFieldAttribute.SetEnabled<AVVerification.status>(cache, doc, true);
            PXUIFieldAttribute.SetEnabled<AVVerification.assetClassID>(cache, doc, true);
            PXUIFieldAttribute.SetEnabled<AVVerification.branchID>(cache, doc, true);
            PXUIFieldAttribute.SetEnabled<AVVerification.building>(cache, doc, true);
            PXUIFieldAttribute.SetEnabled<AVVerification.custodian>(cache, doc, true);
            PXUIFieldAttribute.SetEnabled<AVVerification.department>(cache, doc, true);

            RecalculateTotals();

            bool editable = FORCE_ENABLE_EDITING_FOR_TEST || doc.Status == AVVerificationStatus.AssetCountInProgress;
            DetailsView.AllowInsert = false;
            DetailsView.AllowDelete = false;
            DetailsView.AllowUpdate = editable;

            var detailCache = Caches[typeof(AVVerificationLine)];
            PXUIFieldAttribute.SetEnabled<AVVerificationLine.condition>(detailCache, null, editable);
            PXUIFieldAttribute.SetEnabled<AVVerificationLine.markedForDisposal>(detailCache, null, editable);
            PXUIFieldAttribute.SetEnabled<AVVerificationLine.comments>(detailCache, null, editable);
        }

        protected virtual void AVVerification_AssetStatusFilter_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
            if (_assetsLoaded) ReloadLines();
        }
        protected virtual void AVVerification_AssetClassID_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
            if (_assetsLoaded) ReloadLines();
        }
        protected virtual void AVVerification_BranchID_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
            if (_assetsLoaded) ReloadLines();
        }
        protected virtual void AVVerification_Building_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
            if (_assetsLoaded) ReloadLines();
        }
        protected virtual void AVVerification_Custodian_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
            if (_assetsLoaded) ReloadLines();
        }
        protected virtual void AVVerification_Department_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
            if (_assetsLoaded) ReloadLines();
        }

        private void ReloadLines()
        {
            var doc = MasterView.Current;
            if (doc == null || doc.VerificationID == null) return;

            foreach (AVVerificationLine ln in DetailsView.Select())
            {
                DetailsView.Delete(ln);
            }
            
            Caches[typeof(FixedAsset)].Clear();
            
            _currentPage = 0;
            _allFilteredAssets = null;
            _classCache = null;
            _locationCache = null;
            MasterView.Cache.SetValueExt<AVVerification.totalExpectedQty>(doc, 0m);
            MasterView.Cache.SetValueExt<AVVerification.totalActualQty>(doc, 0m);
            MasterView.Cache.SetValueExt<AVVerification.totalVarianceQty>(doc, 0m);
            
            LoadAllFilteredAssets(doc);
            RecalculateTotals();
        }

        private void LoadAllFilteredAssets(AVVerification doc)
        {
            if (doc == null) return;

            _allFilteredAssets = new System.Collections.Generic.List<FixedAsset>();

            if (!string.IsNullOrEmpty(doc.AssetStatusFilter))
            {
                foreach (PXResult<FixedAsset> res in PXSelectReadonly<FixedAsset,
                    Where<FixedAsset.status, Equal<Required<FixedAsset.status>>>,
                    OrderBy<Asc<FixedAsset.assetID>>>.Select(this, doc.AssetStatusFilter))
                {
                    var fa = (FixedAsset)res;
                    if ((doc.AssetClassID == null || fa.ClassID == doc.AssetClassID)
                        && (doc.BranchID == null || fa.BranchID == doc.BranchID))
                    {
                        _allFilteredAssets.Add(fa);
                    }
                }
            }
            else
            {
                foreach (PXResult<FixedAsset> res in PXSelectReadonly<FixedAsset,
                    Where<FixedAsset.status, NotEqual<FixedAssetStatus.disposed>>,
                    OrderBy<Asc<FixedAsset.assetID>>>.Select(this))
                {
                    var fa = (FixedAsset)res;
                    if ((doc.AssetClassID == null || fa.ClassID == doc.AssetClassID)
                        && (doc.BranchID == null || fa.BranchID == doc.BranchID))
                    {
                        _allFilteredAssets.Add(fa);
                    }
                }
            }

            if (_allFilteredAssets.Count == 0)
            {
                _totalPages = 0;
                MasterView.Cache.SetValueExt<AVVerification.lineCntr>(doc, 0);
                return;
            }

            _totalPages = (_allFilteredAssets.Count + PAGE_SIZE - 1) / PAGE_SIZE;

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

            _locationCache = new System.Collections.Generic.Dictionary<int, FALocationHistory>();
            PrefetchLocationHistoriesBatched(_allFilteredAssets);
        }

        public string GetPaginationInfo()
        {
            if (_allFilteredAssets == null || _allFilteredAssets.Count == 0)
                return "No assets";

            int startRow = _currentPage * PAGE_SIZE + 1;
            int endRow = System.Math.Min((_currentPage + 1) * PAGE_SIZE, _allFilteredAssets.Count);
            int currentPageNum = _currentPage + 1;

            return string.Format("Showing rows {0}-{1} of {2} | Page {3} of {4}",
                startRow, endRow, _allFilteredAssets.Count, currentPageNum, _totalPages);
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

        protected virtual void AVVerificationLine_CountQuantity_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
            var row = e.Row as AVVerificationLine;
            if (row == null) return;

            RecalculateTotals();
        }

        protected virtual void AVVerificationLine_RowUpdated(PXCache cache, PXRowUpdatedEventArgs e)
        {
            // Just track the change in memory - do NOT update database yet
            // Changes will be persisted only when Save is clicked
        }

        protected virtual void AVVerificationLine_RowPersisting(PXCache cache, PXRowPersistingEventArgs e)
        {
            e.Cancel = true;
        }

        private void RecalculateTotals()
        {
            var doc = MasterView.Current;
            if (doc == null) return;

            decimal totalExpected = 0m, totalActual = 0m;

            foreach (AVVerificationLine ln in DetailsView.Select())
            {
                totalExpected += ln.ExpectedQty ?? 0m;
                totalActual += ln.CountQuantity ?? 0m;
            }

            MasterView.Cache.SetValueExt<AVVerification.totalExpectedQty>(doc, totalExpected);
            MasterView.Cache.SetValueExt<AVVerification.totalActualQty>(doc, totalActual);
            MasterView.Cache.SetValueExt<AVVerification.totalVarianceQty>(doc, totalExpected - totalActual);
        }
    }
}