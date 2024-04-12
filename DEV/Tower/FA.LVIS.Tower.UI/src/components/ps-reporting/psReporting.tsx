 
import React, { useState, useEffect } from 'react';
import { Table, Button } from 'antd';
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
    loggedTenant: string; // Added to match the props of ReportingComponent
    togglingTenant: string; // Added to match the props of ReportingComponent
    title: string; // Added to match the props of ReportingComponent
    hasAccess: boolean; // Added to match the props of ReportingComponent
    inValidBtnEnable: boolean; // Added to match the props of ReportingComponent
    orderToInvalidate: any[]; // Added to match the props of ReportingComponent
    activityright: string; // Added to match the props of ReportingComponent
    canmanageteq: boolean; // Added to match the props of ReportingComponent
    canmanagebeq: boolean; // Added to match the props of ReportingComponent
    getUser: (response: any) => void; // Added to match the props of ReportingComponent
    invalidateOrder: () => void; // Added to match the props of ReportingComponent
    search: () => void; // Added to match the props of ReportingComponent
    loadRFOrder: () => void; // Added to match the props of ReportingComponent
    searchbyReferenceNo: () => void; // Added to match the props of ReportingComponent
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
    // State and handlers can be managed here if needed for additional functionality

    return (
        <div>
            {/* Utilize the imported ReportingComponent with all required props */}
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
