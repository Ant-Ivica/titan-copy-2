import React from 'react';
import ReportingComponent from './ps-reporting/psReportingTemplate'; // Corrected import path
interface RowData {
    ServiceRequestId: string;
    createddate: string;
    service: string;
    ApplicationId: string;
    CustomerId: string;
    ExternalRefNum: string;
    CustomerRefNum: string;
    InternalRefNum?: string;
    InternalRefId?: string;
}
interface ReportingProps {
    rows: RowData[];
    activeCustomerName: string;
    onRowSelectionChange: (selectedRowKeys: React.Key[], selectedRows: RowData[]) => void;
    loggedTenant: string;
    togglingTenant: string;
    title: string;
    hasAccess: boolean;
    inValidBtnEnable: boolean;
    orderToInvalidate: any[];
    activityright: string;
    canmanageteq: boolean;
    canmanagebeq: boolean;
    getUser: (response: any) => void;
    invalidateOrder: () => void;
    search: () => void;
    loadRFOrder: () => void;
    searchbyReferenceNo: () => void;
}
const PsReporting: React.FC<ReportingProps> = ({
    rows,
    activeCustomerName,
    onRowSelectionChange,
    loggedTenant,
    togglingTenant,
    title,
    hasAccess,
    inValidBtnEnable,
    orderToInvalidate,
    activityright,
    canmanageteq,
    canmanagebeq,
    getUser,
    invalidateOrder,
    search,
    loadRFOrder,
    searchbyReferenceNo
}) => {
    return (
        <div>
            <ReportingComponent
                loggedTenant={loggedTenant}
                togglingTenant={togglingTenant}
                title={title}
                hasAccess={hasAccess}
                inValidBtnEnable={inValidBtnEnable}
                orderToInvalidate={orderToInvalidate}
                activityright={activityright}
                canmanageteq={canmanageteq}
                canmanagebeq={canmanagebeq}
                getUser={getUser}
                invalidateOrder={invalidateOrder}
                search={search}
                loadRFOrder={loadRFOrder}
                searchbyReferenceNo={searchbyReferenceNo}
            />
        </div>
    );
};
export default PsReporting;