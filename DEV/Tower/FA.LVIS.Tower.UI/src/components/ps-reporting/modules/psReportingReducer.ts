// File: components/modules/psReportingReducer.js

const initialState = {
    orderToInvalidate: [],
    inValidBtnEnable: true,
    loggedTenant: '',
    togglingTenant: '',
    activityright: '',
    canmanageteq: false,
    canmanagebeq: false,
    hasAccess: false,
    hasSuperAccess: false,
    Fromdate: '',
    ThroughDate: '',
    Busy: false,
    DateFilterSelection: [
        { title: 'Custom', value: '1' },
        { title: 'Last 90 Days', value: '90' },
        { title: 'Last 60 Days', value: '60' },
        { title: 'Last 30 Days', value: '30' },
        { title: 'Last 15 Days', value: '15' },
        { title: 'Last 7 Days', value: '7' },
        { title: '24 hrs', value: '24' },
        { title: 'Today', value: '0' }
    ],
    ReferencenoFilterSelection: [
        { title: '---Select---', value: '0' },
        { title: 'External Reference Number', value: '1' },
        { title: 'Internal Reference Number', value: '2' },
        { title: 'Customer Reference Number', value: '3' },
        { title: 'Internal Reference Id', value: '4' }
    ],
    FilterSection: '7',
    Disabledate: true,
    serviceGrid: {
        data: [],
        gridApi: null
    },
    modalInstance: null,
    ValidateError: false,
    FilterReferenceNoSection: '0',
    ReferenceNo: '',
    BusyRef: false,
    DisableReferenceNo: true,
    showrefnum: false,
    showdates: true,
    Tenant: '',
    mynumStyle: { color: '' },
    mydtStyle: { color: '#007acc' }
};

function psReportingReducer(state = initialState, action) {
    switch (action.type) {
        case 'SET_ORDER_TO_INVALIDATE':
            return {
                ...state,
                orderToInvalidate: action.payload
            };
        case 'SET_INVALID_BTN_ENABLE':
            return {
                ...state,
                inValidBtnEnable: action.payload
            };
        case 'SET_LOGGED_TENANT':
            return {
                ...state,
                loggedTenant: action.payload
            };
        case 'SET_TOGGLING_TENANT':
            return {
                ...state,
                togglingTenant: action.payload
            };
        case 'SET_ACTIVITY_RIGHT':
            return {
                ...state,
                activityright: action.payload
            };
        case 'SET_CAN_MANAGE_TEQ':
            return {
                ...state,
                canmanageteq: action.payload
            };
        case 'SET_CAN_MANAGE_BEQ':
            return {
                ...state,
                canmanagebeq: action.payload
            };
        case 'SET_HAS_ACCESS':
            return {
                ...state,
                hasAccess: action.payload
            };
        case 'SET_HAS_SUPER_ACCESS':
            return {
                ...state,
                hasSuperAccess: action.payload
            };
        case 'SET_FROM_DATE':
            return {
                ...state,
                Fromdate: action.payload
            };
        case 'SET_THROUGH_DATE':
            return {
                ...state,
                ThroughDate: action.payload
            };
        case 'SET_BUSY':
            return {
                ...state,
                Busy: action.payload
            };
        case 'SET_SERVICE_GRID_DATA':
            return {
                ...state,
                serviceGrid: {
                    ...state.serviceGrid,
                    data: action.payload
                }
            };
        case 'SET_VALIDATE_ERROR':
            return {
                ...state,
                ValidateError: action.payload
            };
        case 'SET_FILTER_REFERENCE_NO_SECTION':
            return {
                ...state,
                FilterReferenceNoSection: action.payload
            };
        case 'SET_REFERENCE_NO':
            return {
                ...state,
                ReferenceNo: action.payload
            };
        case 'SET_BUSY_REF':
            return {
                ...state,
                BusyRef: action.payload
            };
        case 'SET_DISABLE_REFERENCE_NO':
            return {
                ...state,
                DisableReferenceNo: action.payload
            };
        case 'SET_SHOW_REF_NUM':
            return {
                ...state,
                showrefnum: action.payload
            };
        case 'SET_SHOW_DATES':
            return {
                ...state,
                showdates: action.payload
            };
        case 'SET_TENANT':
            return {
                ...state,
                Tenant: action.payload
            };
        case 'SET_MY_NUM_STYLE':
            return {
                ...state,
                mynumStyle: action.payload
            };
        case 'SET_MY_DT_STYLE':
            return {
                ...state,
                mydtStyle: action.payload
            };
        default:
            return state;
    }
}

export default psReportingReducer;