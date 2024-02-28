"use strict";

angular.module("psFramework").directive("psFramework", function () {
    return {
        transclude: true,
        scope: {
            title: '@',            
            iconFile: '@',
            currentuser: '@',
            tenantname: '@'
        },
        controller: "psFrameworkController",
        templateUrl: "ext-modules/psFramework/psFrameworkTemplate.html"
        
    };
});

//angular.module('psFramework')
//.directive('uiGridCustomCellSelect', ['$timeout', '$document', '$filter', 'rowSearcher', 'uiGridConstants', function ($timeout, $document, $filter, rowSearcher, uiGridConstants) {
//    return {
//        replace: true,
//        require: '^uiGrid',
//        scope: false,
//        controller: function () { },
//        compile: function () {
//            return {
//                pre: function ($scope, $elm, $attrs, uiGridCtrl) { },
//                post: function ($scope, $elm, $attrs, uiGridCtrl) {
//                    var _scope = $scope;
//                    var grid = uiGridCtrl.grid;


//                    // Data setup
//                    _scope.ugCustomSelect = {
//                        hiddenInput: angular.element('<input class="ui-grid-custom-selection-input" type="text" hidden/>').appendTo('body'),
//                        isDragging: false,
//                        selectedCells: [],
//                        cellArray: [],
//                        copyData: '',
//                        dragData: {
//                            startCell: {
//                                row: null,
//                                col: null
//                            },
//                            endCell: {
//                                row: null,
//                                col: null
//                            }
//                        }
//                    }

//                    // Bind events
//                    $timeout(function () {
//                        grid.element.on('mousedown', '.ui-grid-cell-contents', function (evt) {
//                            if (angular.element(evt.target).hasClass('ui-grid-cell-contents')) {
//                                var cellData = $(this).data().$scope;
//                                _scope.ugCustomSelect.isDragging = true;
//                                clearDragData();
//                                setStartCell(cellData.row, cellData.col);
//                                setSelectedStates();
//                            }
//                        });

//                        grid.element.on('mouseenter', '.ui-grid-cell-contents', function (evt) {
//                            if (_scope.ugCustomSelect.isDragging) {
//                                var cellData = $(this).data().$scope;
//                                setEndCell(cellData.row, cellData.col);
//                                setSelectedStates();
//                            }
//                        });

//                        angular.element('body').on('mouseup', function (evt) {
//                            if (_scope.ugCustomSelect.isDragging) {
//                                _scope.ugCustomSelect.isDragging = false;
//                                setSelectedStates();
//                            }
//                        });

//                        angular.element(document).on('keydown', function (e) {
//                            var cKey = 67;
//                            if (e.keyCode == cKey && e.ctrlKey && window.getSelection() + '' === '') {
//                                _scope.ugCustomSelect.hiddenInput.val(' ').focus().select();
//                                document.execCommand('copy');
//                                e.preventDefault();
//                            }
//                        });

//                        angular.element('body').on('copy', function (e) {
//                            var cbData,
//                                cbType;

//                            if (e.originalEvent.clipboardData) {
//                                cbData = e.originalEvent.clipboardData;
//                                cbType = 'text/plain';
//                            } else {
//                                cbData = window.clipboardData;
//                                cbType = 'Text';
//                            }

//                            if (cbData && (window.getSelection() + '' === '' || window.getSelection() + '' === ' ') && _scope.ugCustomSelect.copyData !== '') {
//                                cbData.setData(cbType, _scope.ugCustomSelect.copyData);
//                                setTimeout(function () {
//                                    /////alert('The selected grid cells have been copied to your clipboard');
//                                }, 100);
//                                e.preventDefault();
//                            }
//                        });

//                        grid.api.core.on.scrollBegin(_scope, function () {
//                            grid.element.addClass('ui-grid-custom-selected-scrolling');
//                        });

//                        grid.api.core.on.scrollEnd(_scope, function () {
//                            angular.element('.ui-grid-custom-selected').removeClass('ui-grid-custom-selected');
//                            var visibleCols = grid.renderContainers.body.renderedColumns;
//                            var visibleRows = grid.renderContainers.body.renderedRows;

//                            for (var ri = 0; ri < visibleRows.length; ri++) {
//                                var currentRow = visibleRows[ri];
//                                for (var ci = 0; ci < visibleCols.length; ci++) {
//                                    var currentCol = visibleCols[ci];
//                                    var rowCol = uiGridCtrl.cellNav.makeRowCol({ row: currentRow, col: currentCol });

//                                    if (inCellArray(rowCol)) {
//                                        getCellElem(currentCol, ri).find('.ui-grid-cell-contents').addClass('ui-grid-custom-selected');
//                                    }
//                                }
//                            }

//                            grid.element.removeClass('ui-grid-custom-selected-scrolling');
//                        });

//                        grid.api.core.on.filterChanged(_scope, clearDragData);
//                        grid.api.core.on.columnVisibilityChanged(_scope, clearDragData);
//                        grid.api.core.on.rowsVisibleChanged(_scope, clearDragData);
//                        grid.api.core.on.sortChanged(_scope, clearDragData);
//                    });

//                    // Functions
//                    function setStartCell(row, col) {
//                        _scope.ugCustomSelect.dragData.startCell.row = row;
//                        _scope.ugCustomSelect.dragData.startCell.col = col;
//                    }

//                    function setEndCell(row, col) {
//                        _scope.ugCustomSelect.dragData.endCell.row = row;
//                        _scope.ugCustomSelect.dragData.endCell.col = col;
//                    }

//                    function clearDragData() {
//                        clearEndCell();
//                        clearStartCell();
//                        clearSelectedStates();
//                        _scope.ugCustomSelect.copyData = '';
//                    }

//                    function clearStartCell() {
//                        _scope.ugCustomSelect.dragData.startCell.row = null;
//                        _scope.ugCustomSelect.dragData.startCell.col = null;
//                    }

//                    function clearEndCell() {
//                        _scope.ugCustomSelect.dragData.endCell.row = null;
//                        _scope.ugCustomSelect.dragData.endCell.col = null;
//                    }

//                    // Sets selected styling based on start cell and end cell, including cells in between that range
//                    function setSelectedStates() {
//                        clearSelectedStates();
//                        var indexMap = createIndexMap(_scope.ugCustomSelect.dragData.startCell, _scope.ugCustomSelect.dragData.endCell);
//                        _scope.ugCustomSelect.selectedCells = getCellsWithIndexMap(indexMap);
//                        _scope.ugCustomSelect.cellArray = _scope.ugCustomSelect.selectedCells.map(function (item) {
//                            return item.rowCol;
//                        });

//                        for (var i = 0; i < _scope.ugCustomSelect.selectedCells.length; i++) {
//                            var currentCell = _scope.ugCustomSelect.selectedCells[i];
//                            currentCell.elem.find('.ui-grid-cell-contents').addClass('ui-grid-custom-selected');
//                        }

//                        _scope.ugCustomSelect.copyData = createCopyData(_scope.ugCustomSelect.selectedCells, (indexMap.col.end - indexMap.col.start) + 1);
//                    }

//                    // Clears selected state from any selected cells
//                    function clearSelectedStates() {
//                        angular.element('.ui-grid-custom-selected').removeClass('ui-grid-custom-selected');
//                        _scope.ugCustomSelect.selectedCells = [];
//                        _scope.ugCustomSelect.cellArray = [];
//                    }

//                    function createIndexMap(startCell, endCell) {
//                        var rowStart = grid.renderContainers.body.renderedRows.indexOf(_scope.ugCustomSelect.dragData.startCell.row),
//                            rowEnd = grid.renderContainers.body.renderedRows.indexOf(_scope.ugCustomSelect.dragData.endCell.row),
//                            colStart = grid.renderContainers.body.renderedColumns.indexOf(_scope.ugCustomSelect.dragData.startCell.col),
//                            colEnd = grid.renderContainers.body.renderedColumns.indexOf(_scope.ugCustomSelect.dragData.endCell.col)

//                        if (rowEnd === -1)
//                            rowEnd = rowStart;

//                        if (colEnd === -1)
//                            colEnd = colStart;

//                        return {
//                            row: {
//                                start: (rowStart < rowEnd) ? rowStart : rowEnd,
//                                end: (rowEnd > rowStart) ? rowEnd : rowStart
//                            },
//                            col: {
//                                start: (colStart < colEnd) ? colStart : colEnd,
//                                end: (colEnd > colStart) ? colEnd : colStart
//                            }
//                        };
//                    }

//                    function getCellsWithIndexMap(indexMap) {
//                        var visibleCols = grid.renderContainers.body.renderedColumns;
//                        var visibleRows = grid.renderContainers.body.renderedRows;
//                        var cellsArray = [];

//                        for (var ri = indexMap.row.start; ri <= indexMap.row.end; ri++) {
//                            var currentRow = visibleRows[ri];
//                            for (var ci = indexMap.col.start; ci <= indexMap.col.end; ci++) {
//                                var currentCol = visibleCols[ci];
//                                var rowCol = uiGridCtrl.cellNav.makeRowCol({ row: currentRow, col: currentCol });
//                                var cellElem = getCellElem(rowCol.col, ri);

//                                if (cellElem) {
//                                    cellsArray.push({
//                                        rowCol: rowCol,
//                                        elem: cellElem
//                                    });
//                                }
//                            }
//                        }

//                        return cellsArray;
//                    }

//                    function inCellArray(rowCol) {
//                        if (rowCol && rowCol.row && rowCol.col) {
//                            for (var i = 0; i < _scope.ugCustomSelect.cellArray.length; i++) {
//                                var currentCell = _scope.ugCustomSelect.cellArray[i];
//                                if (angular.equals(currentCell.col, rowCol.col) && angular.equals(currentCell.row.entity, rowCol.row.entity)) {
//                                    return true;
//                                }
//                            }
//                        }
//                        return false;
//                    }

//                    function getCellElem(col, rowIndex) {
//                        return (col && col.uid && typeof rowIndex == 'number') ? angular.element('#' + grid.id + '-' + rowIndex + '-' + col.uid + '-cell') : null;
//                    }

//                    function createCopyData(cells, numCols) {
//                        var copyData = '';

//                        for (var i = 0; i < cells.length; i++) {
//                            var currentCell = cells[i];
//                            var cellValue = grid.getCellDisplayValue(currentCell.rowCol.row, currentCell.rowCol.col);

//                            copyData += cellValue;

//                            if (i && (i + 1) % numCols === 0 && i !== cells.length - 1) {
//                                copyData += '\n';
//                            } else if (i !== cells.length - 1) {
//                                copyData += '\t';
//                            }
//                        }

//                        return copyData;
//                    }
//                }
//            };
//        }
//    };
//}]);


