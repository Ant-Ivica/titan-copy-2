// File path: components/modules/actions.js

export const SET_ACTIVITY_RIGHT = 'SET_ACTIVITY_RIGHT';
export const SET_CAN_MANAGE_TEQ = 'SET_CAN_MANAGE_TEQ';
export const SET_CAN_MANAGE_BEQ = 'SET_CAN_MANAGE_BEQ';
export const SET_HAS_ACCESS = 'SET_HAS_ACCESS';
export const SET_HAS_SUPER_ACCESS = 'SET_HAS_SUPER_ACCESS';
export const SET_ORDER_TO_INVALIDATE = 'SET_ORDER_TO_INVALIDATE';
export const SET_INVALID_BTN_ENABLE = 'SET_INVALID_BTN_ENABLE';
export const SET_LOGGED_TENANT = 'SET_LOGGED_TENANT';
export const SET_TOGGLING_TENANT = 'SET_TOGGLING_TENANT';
export const SET_DATE_FILTER_SELECTION = 'SET_DATE_FILTER_SELECTION';
export const SET_REFERENCE_NO_FILTER_SELECTION = 'SET_REFERENCE_NO_FILTER_SELECTION';
export const SET_FILTER_SECTION = 'SET_FILTER_SECTION';
export const SET_DISABLE_DATE = 'SET_DISABLE_DATE';
export const SET_FROM_DATE = 'SET_FROM_DATE';
export const SET_THROUGH_DATE = 'SET_THROUGH_DATE';
export const SET_BUSY = 'SET_BUSY';
export const SET_SERVICE_GRID_DATA = 'SET_SERVICE_GRID_DATA';
export const SET_FILTER_REFERENCE_NO_SECTION = 'SET_FILTER_REFERENCE_NO_SECTION';
export const SET_REFERENCE_NO = 'SET_REFERENCE_NO';
export const SET_BUSY_REF = 'SET_BUSY_REF';
export const SET_DISABLE_REFERENCE_NO = 'SET_DISABLE_REFERENCE_NO';
export const SET_SHOW_REF_NUM = 'SET_SHOW_REF_NUM';
export const SET_SHOW_DATES = 'SET_SHOW_DATES';

export function setActivityRight(activityRight) {
    return { type: SET_ACTIVITY_RIGHT, payload: activityRight };
}

export function setCanManageTeq(canManageTeq) {
    return { type: SET_CAN_MANAGE_TEQ, payload: canManageTeq };
}

export function setCanManageBeq(canManageBeq) {
    return { type: SET_CAN_MANAGE_BEQ, payload: canManageBeq };
}

export function setHasAccess(hasAccess) {
    return { type: SET_HAS_ACCESS, payload: hasAccess };
}

export function setHasSuperAccess(hasSuperAccess) {
    return { type: SET_HAS_SUPER_ACCESS, payload: hasSuperAccess };
}

export function setOrderToInvalidate(orderToInvalidate) {
    return { type: SET_ORDER_TO_INVALIDATE, payload: orderToInvalidate };
}

export function setInvalidBtnEnable(invalidBtnEnable) {
    return { type: SET_INVALID_BTN_ENABLE, payload: invalidBtnEnable };
}

export function setLoggedTenant(loggedTenant) {
    return { type: SET_LOGGED_TENANT, payload: loggedTenant };
}

export function setTogglingTenant(togglingTenant) {
    return { type: SET_TOGGLING_TENANT, payload: togglingTenant };
}

export function setDateFilterSelection(dateFilterSelection) {
    return { type: SET_DATE_FILTER_SELECTION, payload: dateFilterSelection };
}

export function setReferenceNoFilterSelection(referenceNoFilterSelection) {
    return { type: SET_REFERENCE_NO_FILTER_SELECTION, payload: referenceNoFilterSelection };
}

export function setFilterSection(filterSection) {
    return { type: SET_FILTER_SECTION, payload: filterSection };
}

export function setDisableDate(disableDate) {
    return { type: SET_DISABLE_DATE, payload: disableDate };
}

export function setFromDate(fromDate) {
    return { type: SET_FROM_DATE, payload: fromDate };
}

export function setThroughDate(throughDate) {
    return { type: SET_THROUGH_DATE, payload: throughDate };
}

export function setBusy(busy) {
    return { type: SET_BUSY, payload: busy };
}

export function setServiceGridData(serviceGridData) {
    return { type: SET_SERVICE_GRID_DATA, payload: serviceGridData };
}

export function setFilterReferenceNoSection(filterReferenceNoSection) {
    return { type: SET_FILTER_REFERENCE_NO_SECTION, payload: filterReferenceNoSection };
}

export function setReferenceNo(referenceNo) {
    return { type: SET_REFERENCE_NO, payload: referenceNo };
}

export function setBusyRef(busyRef) {
    return { type: SET_BUSY_REF, payload: busyRef };
}

export function setDisableReferenceNo(disableReferenceNo) {
    return { type: SET_DISABLE_REFERENCE_NO, payload: disableReferenceNo };
}

export function setShowRefNum(showRefNum) {
    return { type: SET_SHOW_REF_NUM, payload: showRefNum };
}

export function setShowDates(showDates) {
    return { type: SET_SHOW_DATES, payload: showDates };
}